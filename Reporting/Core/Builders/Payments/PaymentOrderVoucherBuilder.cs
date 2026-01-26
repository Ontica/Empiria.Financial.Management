/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Html builder                         *
*  Type     : BudgetTransactionAsOrderVoucherBuilder        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a PDF file with a budget transaction with its associated order data.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Text;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Billing;
using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;
using Empiria.Orders;
using Empiria.Procurement.Suppliers;

namespace Empiria.Payments.Reporting {

  internal class PaymentOrderVoucherBuilder : HtmlBuilder {

    private readonly PaymentOrder _paymentOrder;
    private readonly BudgetTransaction _budgetTxn;
    private readonly Order _order;

    private readonly FixedList<Bill> _bills;

    public PaymentOrderVoucherBuilder(PaymentOrder paymentOrder,
                                      FileTemplateConfig templateConfig) : base(templateConfig) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      _paymentOrder = paymentOrder;

      _order = _paymentOrder.PayableEntity as Order;

      _bills = Bill.GetListFor(_paymentOrder.PayableEntity);

      _budgetTxn = GetApplicableBudgetTransaction();
    }

    #region Builders

    protected override void Build() {

      BuildHeader();
      BuildEntries();
      BuildBills();
      BuildTotals();
      BuildFooter();

      SignIf(_paymentOrder.Payed);
    }

    private void BuildBills() {
      string TEMPLATE = GetSection("BILLS.TEMPLATE");

      var entriesHtml = new StringBuilder();

      foreach (var bill in _bills) {
        var entryHtml = new StringBuilder(TEMPLATE);

        entryHtml.Replace("{{BILL_NO}}", BuildBillNumber(bill));
        entryHtml.Replace("{{BILL_TYPE}}", BuildBillType(bill));
        entryHtml.Replace("{{DESCRIPTION}}", bill.Name);
        entryHtml.Replace("{{ISSUE_DATE}}", bill.IssueDate.ToString("dd/MMM/yyyy"));
        entryHtml.Replace("{{POSTING_DATE}}", bill.PostingTime.ToString("dd/MMM/yyyy"));
        entryHtml.Replace("{{SUBTOTAL}}", bill.Subtotal.ToString("C2"));
        entryHtml.Replace("{{DISCOUNT}}", bill.Discount.ToString("C2"));
        entryHtml.Replace("{{TOTAL}}", (bill.Subtotal - bill.Discount).ToString("C2"));

        entriesHtml.Append(entryHtml);
      }

      SetSection("BILLS.TEMPLATE", entriesHtml.ToString());
    }


    private void BuildEntries() {

      if (_order.BudgetType == BudgetType.None) {

        Set("{{BUDGET_ENTRIES_TABLE_TITLE}}", "ESTE PAGO NO ESTÁ RELACIONADO CON EL CONTROL PRESUPUESTAL");
        Hide("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

        return;

      } else if (_budgetTxn.IsEmptyInstance) {

        SetWarning("{{BUDGET_ENTRIES_TABLE_TITLE}}", "ESTE PAGO AÚN NO CUENTA CON APROBACIÓN PRESUPUESTAL");
        Hide("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

        return;

      }

      SetIf("{{BUDGET_ENTRIES_TABLE_TITLE}}", _budgetTxn.IsClosed,
                                              $"CONTROL PRESUPUESTAL [{_budgetTxn.TransactionNo}]",
                                              Warning("LA APROBACIÓN PRESUPUESTAL ESTÁ EN PROCESO"));

      Remove("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

      string TEMPLATE = base.GetSection("ORDER_ENTRY.TEMPLATE");

      var entriesHtml = new StringBuilder();

      FixedList<OrderItem> orderItems = _order.GetItems<OrderItem>()
                                              .Sort((x, y) => $"{x.BudgetAccount.Code}{x.BudgetAccount.OrganizationalUnit.Code}"
                                                             .CompareTo($"{y.BudgetAccount.Code}{y.BudgetAccount.OrganizationalUnit.Code}"));

      foreach (var orderEntry in orderItems) {

        var budgetEntry = _budgetTxn.Entries.Find(x => orderEntry.BudgetEntry.Id == x.Id);

        if (budgetEntry == null) {
          continue;
        }

        var entryHtml = new StringBuilder(TEMPLATE);

        entryHtml.Replace("{{BUDGET_ACCOUNT_CODE}}", budgetEntry.BudgetAccount.Code);
        entryHtml.Replace("{{BUDGET_ACCOUNT_PARTY}}", budgetEntry.BudgetAccount.OrganizationalUnit.Code);

        entryHtml.Replace("{{DESCRIPTION}}", orderEntry.Description);
        entryHtml.Replace("{{ORIGIN_COUNTRY}}", orderEntry.OriginCountry.CountryISOCode);
        entryHtml.Replace("{{CONTROL_NO}}", budgetEntry.ControlNo);
        entryHtml.Replace("{{PROGRAM}}", budgetEntry.BudgetProgram.Code);
        entryHtml.Replace("{{PRODUCT_UNIT}}", orderEntry.ProductUnit.Name);
        entryHtml.Replace("{{SUBTOTAL}}", (orderEntry.Subtotal + orderEntry.DiscountsTotal).ToString("C2"));
        entryHtml.Replace("{{DISCOUNT}}", orderEntry.Discount.ToString("C2"));
        entryHtml.Replace("{{PENALTIES}}", orderEntry.PenaltyDiscount.ToString("C2"));
        entryHtml.Replace("{{TOTAL}}", budgetEntry.Amount.ToString("C2"));

        entriesHtml.Append(entryHtml);
      }

      SetSection("ORDER_ENTRY.TEMPLATE", entriesHtml.ToString());
    }


    private void BuildFooter() {
      HideIf("{{CLASS_HIDE_SIGN}}", _paymentOrder.Total <= 4000000m);
    }


    private void BuildHeader() {

      string title = "Relación de pagos";

      if (!_paymentOrder.Payed) {
        title = $"{title} [{_paymentOrder.Status.GetName()}]";
      }

      SetIf("{{REPORT.TITLE}}", _paymentOrder.Payed, title, Warning(title));

      Set("{{PAYMENT.PAYMENT_ORDER_NO}}", _paymentOrder.PaymentOrderNo);
      Set("{{PAYMENT.REQUESTED_BY}}", $"({_paymentOrder.RequestedBy.Code}) {_paymentOrder.RequestedBy.Name}");

      Set("{{PAYMENT.PAYMENT_TYPE}}", BuildPaymentType());

      var payTo = (Supplier) _paymentOrder.PayTo;

      Set("{{PAYMENT.PAY_TO}}", $"{payTo.Name} ({payTo.SubledgerAccount})");

      if (_paymentOrder.Payed) {
        Set("{{PAYMENT.PAYMENT_METHOD}}",
                $"{_paymentOrder.PaymentMethod.Name} {Space(4)} {Normal("Fecha y hora:")} " +
                $"{_paymentOrder.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm")}" +
                $"{Space(3)}{_paymentOrder.LastPaymentInstruction.BrokerInstructionNo}");
      } else {
        Set("{{PAYMENT.PAYMENT_METHOD}}", $"{_paymentOrder.PaymentMethod.Name}");
        Set("Total pagado", "Total a pagar");
      }

      Set("{{PAYMENT.BUDGET}}", _paymentOrder.PayableEntity.Budget.Name);
      Set("{{PAYMENT.DESCRIPTION}}", _paymentOrder.Description);

      Set("{{PAYMENT.TOTAL}}", _paymentOrder.Total);
      Set("{{PAYMENT.CURRENCY}}", _paymentOrder.Currency.ISOCode);

      if (!_paymentOrder.PaymentAccount.IsEmptyInstance) {
        Set("{{PAYMENT.INSTITUTION}}", _paymentOrder.PaymentAccount.Institution.Name);
        Set("{{PAYMENT.ACCOUNT_NO}}", $"{Space(4)} {Normal("No:")} {_paymentOrder.PaymentAccount.AccountNo}");
        SetIf("{{PAYMENT.REFERENCE_NUMBER}}", _paymentOrder.ReferenceNumber.Length != 0,
                                            $"{Space(2)} {Normal("Referencia:")} {_paymentOrder.ReferenceNumber}");
      } else {
        Set("{{PAYMENT.INSTITUTION}}", "No aplica");
        Remove("{{PAYMENT.ACCOUNT_NO}}");
        Remove("{{PAYMENT.REFERENCE_NUMBER}}");
      }

      Set("{{REQUISITION.NO}}", _order.Requisition.OrderNo);
      Set("{{REQUISITION.TOTAL}}", _order.Requisition.Total);
      Set("{{REQUISITION.BALANCE}}", _order.Requisition.Total - _order.Total);

      if (!_order.Contract.IsEmptyInstance) {
        Set("{{CONTRACT.NO}}", _order.Contract.OrderNo);
        Set("{{CONTRACT.PERIOD}}", _order.Contract.StartDate.ToString("dd/MMM/yyyy") + " - " +
                                   _order.Contract.EndDate.ToString("dd/MMM/yyyy"));
        Set("{{CONTRACT.TOTAL}}", _order.Contract.Total);
        Set("{{CONTRACT.BALANCE}}", _order.Contract.Total - _order.Total);
      } else {
        Set("{{CONTRACT.NO}}", "No aplica");
        Set("{{CONTRACT.PERIOD}}", "N/A");
        Set("{{CONTRACT.TOTAL}}", "N/A");
        Set("{{CONTRACT.BALANCE}}", "N/A");
      }

      if (!_paymentOrder.Debtor.IsEmptyInstance) {
        var debtor = (Supplier) _paymentOrder.Debtor;

        Set("{{PAYMENT.DEBTOR}}", $"{debtor.Name} ({debtor.SubledgerAccount})");
      } else {
        Set("{{PAYMENT.DEBTOR}}", "No aplica");
      }

      BuildBudgetRequests();


      SetIf("{{PAYMENT.ACCOUNTING_VOUCHER}}", _paymentOrder.AccountingVoucher.Length == 0,
                                              Warning("Pendiente de registrar"), _paymentOrder.AccountingVoucher);

      Set("{{ORDER.ORDER_NO}}", _order.OrderNo);
      Set("{{ORDER.NAME}}", _order.Name);
      Set("{{ORDER.JUSTIFICATION}}", _order.Justification);
    }


    private void BuildTotals() {

      Set("{{ORDER.SUBTOTAL}}", _order.Subtotal + _order.Items.DiscountsTotal);
      Set("{{ORDER.DISCOUNT}}", _order.Items.Discount);
      Set("{{ORDER.PENALTIES}}", _order.Items.Penalties);
      Set("{{ORDER.TOTAL}}", _order.Subtotal);

      var billsTotals = new BillsTotals(_bills);

      Set("{{BILLS.SUBTOTAL}}", billsTotals.Subtotal.ToString("C2"));

      string TEMPLATE = base.GetSection("TAXES.TEMPLATE");

      var totalsHtml = new StringBuilder();

      foreach (var taxItem in billsTotals.TaxItems) {

        var totalHtml = TEMPLATE.Replace("{{TAX_TYPE}}", taxItem.TaxName);

        totalHtml = totalHtml.Replace("{{TAX_TOTAL}}", taxItem.Total.ToString("C2"));

        totalsHtml.Append(totalHtml);
      }

      SetSection("TAXES.TEMPLATE", totalsHtml.ToString());

      Set("{{BILLS.TOTAL}}", billsTotals.Total);
    }

    #endregion Builders

    #region Helpers

    private string BuildBillNumber(Bill bill) {
      string serial = string.Empty;

      if (bill.SchemaData.Serie.Length != 0) {
        serial = Normal("Serie: ") + bill.SchemaData.Serie;
      }
      if (bill.SchemaData.Folio.Length != 0) {
        if (serial.Length != 0) {
          serial += Space(2);
        }
        serial += Normal("Folio: ") + bill.SchemaData.Folio;
      }

      if (serial.Length != 0) {
        serial = $"<br />{serial}";
      }

      return $"{bill.BillNo}{serial}";
    }


    private string BuildBillType(Bill bill) {
      string billType = bill.BillCategory.Name;

      if (bill.BillCategory.IsCFDI && bill.PaymentMethod.Length != 0) {
        billType += $"<br/>{Strong(bill.PaymentMethod)}";
      }

      return billType;
    }


    private void BuildBudgetRequests() {
      if (_budgetTxn.IsEmptyInstance) {
        Set("{{PAYMENT.BUDGET_REQUESTS}}", "No aplica");
        Set("{{PAYMENT.BUDGET_EXCERCISE}}", "No aplica");
      }

      var bdgRequests = BudgetTransaction.GetRelatedTo(_budgetTxn)
                                         .FindAll(x => x.OperationType == BudgetOperationType.Request)
                                         .Select(x => x.TransactionNo)
                                         .ToFixedList();

      Set("{{PAYMENT.BUDGET_REQUESTS}}", string.Join(", ", bdgRequests));

      bdgRequests = BudgetTransaction.GetRelatedTo(_budgetTxn)
                                     .FindAll(x => x.OperationType == BudgetOperationType.Exercise)
                                     .Select(x => x.TransactionNo)
                                     .ToFixedList();


      Set("{{PAYMENT.BUDGET_EXCERCISE}}", bdgRequests.Count == 0 ?
                                          Warning("Pendiente de registrar") : bdgRequests[0]);
    }


    private string BuildPaymentType() {
      string paymentType = _paymentOrder.PaymentType.Name;

      if (!_order.Category.IsEmptyInstance) {
        paymentType += $" &nbsp; / {_order.Category.Name}";
      } else if (!_order.Contract.IsEmptyInstance) {
        paymentType += $" &nbsp; / {_order.Contract.Category.Name}";
      } else {
        paymentType += $" &nbsp; / {_order.Requisition.Category.Name}";
      }

      return paymentType;
    }

    private BudgetTransaction GetApplicableBudgetTransaction() {

      if (_order.BudgetType == BudgetType.None) {
        return BudgetTransaction.Empty;
      }

      BudgetTransaction txn = _paymentOrder.TryGetApprovedBudget();

      if (txn != null) {
        return txn;
      }

      txn = BudgetTransaction.GetFor(_order)
                             .FindLast(x => x.OperationType == BudgetOperationType.Commit);

      if (txn != null) {
        return txn;
      }

      return BudgetTransaction.Empty;
    }

    #endregion Helpers

  }  // class PaymentOrderVoucherBuilder

}  // namespace Empiria.Payments.Reporting 
