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
        var entryHtml = new StringBuilder(TEMPLATE.Replace("{{BILL_NO}}", bill.BillNo));

        entryHtml.Replace("{{BILL_TYPE}}", bill.BillCategory.Name);
        entryHtml.Replace("{{DESCRIPTION}}", bill.Name);
        entryHtml.Replace("{{ISSUE_DATE}}", bill.IssueDate.ToString("dd/MMM/yyyy"));
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

      if (_budgetTxn.IsClosed) {
        Set("{{BUDGET_ENTRIES_TABLE_TITLE}}", $"CONTROL PRESUPUESTAL [{_budgetTxn.TransactionNo}]");
      } else {
        SetWarning("{{BUDGET_ENTRIES_TABLE_TITLE}}", "LA APROBACIÓN PRESUPUESTAL ESTÁ EN PROCESO");
      }

      Remove("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

      string TEMPLATE = base.GetSection("ORDER_ENTRY.TEMPLATE");

      var entriesHtml = new StringBuilder();

      foreach (var orderEntry in _order.GetItems<OrderItem>()) {
        var entryHtml = new StringBuilder(TEMPLATE.Replace("{{BUDGET_ACCOUNT_CODE}}",
                                          orderEntry.BudgetAccount.Code));

        var budgetEntry = _budgetTxn.Entries.Find(x => orderEntry.Id == x.EntityId &&
                                                       orderEntry.GetEmpiriaType().Id == x.EntityTypeId);

        if (budgetEntry == null) {
          continue;
        }

        entryHtml.Replace("{{BUDGET_ACCOUNT_PARTY}}", orderEntry.BudgetAccount.OrganizationalUnit.Code);
        entryHtml.Replace("{{BUDGET_ACCOUNT_CODE}}", orderEntry.BudgetAccount.Code);
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

      string title = _paymentOrder.PaymentType.Name;

      if (!_paymentOrder.Payed) {
        title = $"{title} [{_paymentOrder.Status.GetName()}])";
      }
      if (!_paymentOrder.Payed) {
        title = Warning(title);
      }

      Set("{{REPORT.TITLE}}", title);

      Set("{{PAYMENT.PAYMENT_ORDER_NO}}", _paymentOrder.PaymentOrderNo);
      Set("{{PAYMENT.PAYMENT_ORDER_TYPE}}", _paymentOrder.RequestedBy.Name);

      Set("{{PAYMENT.PAY_TO}}", _paymentOrder.PayTo.Name);

      if (_paymentOrder.Payed) {
        Set("{{PAYMENT.PAYMENT_METHOD}}",
                $"{_paymentOrder.PaymentMethod.Name} {Space(4)} {Normal("Fecha y hora:")} " +
                $"{_paymentOrder.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm")}" +
                $"{Space(3)}{_paymentOrder.LastPaymentInstruction.BrokerInstructionNo}" +
                $"");
      } else {
        Set("{{PAYMENT.PAYMENT_METHOD}}", $"{_paymentOrder.PaymentMethod.Name}");
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

      Set("{{PAYMENT.DEBTOR}}", _paymentOrder.Debtor.IsEmptyInstance ? "No aplica" : _paymentOrder.Debtor.Name);
      Set("{{PAYMENT.OBSERVATIONS}}", _paymentOrder.Observations);

      if (!_budgetTxn.IsEmptyInstance) {
        var bdgRequests = BudgetTransaction.GetRelatedTo(_budgetTxn)
                                           .FindAll(x => x.BudgetTransactionType.OperationType == BudgetOperationType.Request)
                                           .Select(x => x.TransactionNo);

        Set("{{PAYMENT.BUDGET_REQUESTS}}", string.Join(", ", bdgRequests));
      } else {

        Set("{{PAYMENT.BUDGET_REQUESTS}}", "No aplica");
      }

      Set("{{PAYMENT.ACCOUNTING_VOUCHER}}", _paymentOrder.AccountingVoucher);

      Set("{{ORDER.ORDER_NO}}", $"{_order.OrderNo} ({_order.OrderType.DisplayName})");
      Set("{{ORDER.DESCRIPTION}}", _order.Name);


      if (!_paymentOrder.Payed) {
        Set("Total pagado", "Total a pagar");
      }
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

    private BudgetTransaction GetApplicableBudgetTransaction() {

      if (_order.BudgetType == BudgetType.None) {
        return BudgetTransaction.Empty;
      }

      BudgetTransaction txn = _paymentOrder.TryGetApprovedBudget();

      if (txn != null) {
        return txn;
      }

      txn = BudgetTransaction.GetFor(_order)
                             .FindLast(x => x.BudgetTransactionType.OperationType == BudgetOperationType.Commit);

      if (txn != null) {
        return txn;
      }

      return BudgetTransaction.Empty;
    }

    #endregion Helpers

  }  // class PaymentOrderVoucherBuilder

}  // namespace Empiria.Payments.Reporting 
