/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetTransactionAsOrderVoucherBuilder        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a PDF file with a budget transaction with its associated order data.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;
using System.Text;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Financial;
using Empiria.Billing;

using Empiria.Orders;

using Empiria.Budgeting.Transactions;

namespace Empiria.Payments.Reporting {

  internal class PaymentOrderVoucherBuilder {

    private readonly PaymentOrder _paymentOrder;
    private readonly BudgetTransaction _budgetTxn;
    private readonly Order _order;


    private readonly FileTemplateConfig _templateConfig;
    private readonly string _htmlTemplate;
    private readonly FixedList<Bill> _bills;

    public PaymentOrderVoucherBuilder(PaymentOrder paymentOrder,
                                      FileTemplateConfig templateConfig) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(templateConfig, nameof(templateConfig));

      _paymentOrder = paymentOrder;

      _budgetTxn = paymentOrder.TryGetApprovedBudget();

      if (_budgetTxn == null) {
        Assertion.RequireFail("La solicitud de pago no cuenta con la autorización presupuestal de pago.");
        return;
      }

      _order = _budgetTxn.GetEntity() as Order;

      _bills = Bill.GetListFor((IPayableEntity) _order);

      _templateConfig = templateConfig;
      _htmlTemplate = File.ReadAllText(_templateConfig.TemplateFullPath);
    }


    internal FileDto CreateVoucher() {
      string filename = GetVoucherPdfFileName();

      string html = BuildVoucherHtml();

      SaveHtmlAsPdf(html, filename);

      return ToFileDto(filename);
    }

    #region Builders

    private string BuildVoucherHtml() {
      StringBuilder html = new StringBuilder(_htmlTemplate);

      html = BuildHeader(html);
      html = BuildEntries(html);
      html = BuildBills(html);
      html = BuildTotals(html);
      html = BuildFooter(html);

      return html.ToString();
    }


    private StringBuilder BuildBills(StringBuilder html) {
      string TEMPLATE = GetBillsTemplate();

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

      return ReplaceBillsTemplate(html, entriesHtml);
    }


    private StringBuilder BuildEntries(StringBuilder html) {
      string TEMPLATE = GetEntriesTemplate();

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
        entryHtml.Replace("{{PRODUCT_CODE}}", orderEntry.ProductCode);
        entryHtml.Replace("{{DESCRIPTION}}", orderEntry.Description);
        entryHtml.Replace("{{ORIGIN_COUNTRY}}", orderEntry.OriginCountry.CountryISOCode);
        entryHtml.Replace("{{CONTROL_NO}}", budgetEntry.ControlNo);
        entryHtml.Replace("{{PROGRAM}}", budgetEntry.BudgetProgram.Code);
        entryHtml.Replace("{{YEAR}}", orderEntry.Budget.Year.ToString());
        entryHtml.Replace("{{PRODUCT_UNIT}}", orderEntry.ProductUnit.Name);
        entryHtml.Replace("{{QUANTITY}}", budgetEntry.Currency.ISOCode);
        entryHtml.Replace("{{UNIT_PRICE}}", budgetEntry.RequestedAmount.ToString("C2"));
        entryHtml.Replace("{{TOTAL}}", budgetEntry.Amount.ToString("C2"));

        entriesHtml.Append(entryHtml);
      }

      return ReplaceEntriesTemplate(html, entriesHtml);
    }


    private StringBuilder BuildFooter(StringBuilder html) {
      var footerHtml = new StringBuilder();

      if (_budgetTxn.AuthorizedBy.IsEmptyInstance) {
        html.Replace("{{SECURITY_CODE}}", "<span class='warning'>PENDIENTE DE AUTORIZAR</span>");
        html.Replace("{{SECURITY_SEAL}}", "<span class='warning'>ESTE DOCUMENTO NO TIENE VALIDEZ OFICIAL</span>");

        return html;
      }

      var seal = $"{_budgetTxn.TransactionNo}|{_order.OrderNo}|{_budgetTxn.Justification}|" +
                 $"{_budgetTxn.GetTotal()}|{_order.Taxes}|{_order.Items}|" +
                 $"{_bills.SelectDistinct(x => x.BillNo)}";

      seal = Security.Cryptographer.Encrypt(Security.EncryptionMode.Standard, seal);

      seal = EmpiriaString.DivideLongString(seal, 95, "<br/>");

      html.Replace("{{SECURITY_CODE}}", _paymentOrder.PaymentOrderNo.GetHashCode().ToString("00000000"));
      html.Replace("{{SECURITY_SEAL}}", seal);

      return html;
    }


    private StringBuilder BuildHeader(StringBuilder html) {

      string title = _paymentOrder.PaymentType.Name;

      bool isClosed = _paymentOrder.Status == PaymentOrderStatus.Payed;

      if (!isClosed) {
        title = $"{title} [{_paymentOrder.Status.GetName()}])";
      }
      if (!isClosed) {
        title = $"<span class='warning'>{title}</span>";
      }

      html.Replace("{{SYSTEM.DATETIME}}", $"Impresión: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
      html.Replace("{{REPORT.TITLE}}", title);

      html.Replace("{{PAYMENT.PAYMENT_ORDER_NO}}", _paymentOrder.PaymentOrderNo);
      html.Replace("{{PAYMENT.PAYMENT_ORDER_TYPE}}", _paymentOrder.RequestedBy.Name);

      html.Replace("{{PAYMENT.PAY_TO}}", _paymentOrder.PayTo.Name);
      html.Replace("{{PAYMENT.PAYMENT_METHOD}}", _paymentOrder.PaymentMethod.Name);
      html.Replace("{{PAYMENT.BUDGET}}", _paymentOrder.PayableEntity.Budget.Name);
      html.Replace("{{PAYMENT.DESCRIPTION}}", _paymentOrder.Description);

      html.Replace("{{PAYMENT.TOTAL}}", _paymentOrder.Total.ToString("C2"));
      html.Replace("{{PAYMENT.CURRENCY}}", _paymentOrder.Currency.ISOCode);

      if (!_paymentOrder.PaymentAccount.IsEmptyInstance) {
        html.Replace("{{PAYMENT.INSTITUTION}}", _paymentOrder.PaymentAccount.Institution.Name);
        html.Replace("{{PAYMENT.ACCOUNT_NO}}", $"Cuenta: {_paymentOrder.PaymentAccount.AccountNo}");
        html.Replace("{{PAYMENT.REFERENCE_NUMBER}}", _paymentOrder.ReferenceNumber.Length == 0 ?
                                                          string.Empty : $"Referencia: {_paymentOrder.ReferenceNumber}");
      } else {
        html.Replace("{{PAYMENT.INSTITUTION}}", "No aplica");
        html.Replace("{{PAYMENT.ACCOUNT_NO}}", string.Empty);
        html.Replace("{{PAYMENT.REFERENCE_NUMBER}}", string.Empty);
      }

      html.Replace("{{PAYMENT.DEBTOR}}", _paymentOrder.Debtor.IsEmptyInstance ? "No aplica" : _paymentOrder.Debtor.Name);

      html.Replace("{{PAYMENT.OBSERVATIONS}}", _paymentOrder.Observations);

      html.Replace("{{ORDER.ORDER_NO}}", _order.OrderNo);
      html.Replace("{{ORDER.IS_FOR_MULTIPLE_BENEFICIARIES}}", _order.IsForMultipleBeneficiaries ? "Sí" : "No");
      html.Replace("{{ORDER.REQUESTED_BY}}", _paymentOrder.PostedBy.Name);
      html.Replace("{{ORDER.JUSTIFICATION}}", _order.Justification);

      return html;
    }


    private StringBuilder BuildTotals(StringBuilder html) {

      html.Replace("{{ORDER.SUBTOTAL}}", _order.Subtotal.ToString("C2"));

      var billsTotals = new BillsTotals(_bills);

      html.Replace("{{BILLS.SUBTOTAL}}", billsTotals.Subtotal.ToString("C2"));

      string TEMPLATE = GetBillTotalsTemplate();

      var totalsHtml = new StringBuilder();

      foreach (var taxItem in billsTotals.TaxItems) {

        var totalHtml = new StringBuilder(TEMPLATE.Replace("{{TAX_TYPE}}", taxItem.TaxName));

        totalHtml.Replace("{{TAX_TOTAL}}", taxItem.Total.ToString("C2"));

        totalsHtml.Append(totalHtml);
      }

      html.Replace("{{BILLS.TOTAL}}", billsTotals.Total.ToString("C2"));

      return ReplaceBillTotalsTemplate(html, totalsHtml);
    }


    private string GetBillsTemplate() {
      int startIndex = _htmlTemplate.IndexOf("{{BILLS.TEMPLATE.START}}");
      int endIndex = _htmlTemplate.IndexOf("{{BILLS.TEMPLATE.END}}");

      var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

      return template.Replace("{{BILLS.TEMPLATE.START}}", string.Empty);
    }


    private string GetEntriesTemplate() {
      int startIndex = _htmlTemplate.IndexOf("{{ORDER_ENTRY.TEMPLATE.START}}");
      int endIndex = _htmlTemplate.IndexOf("{{ORDER_ENTRY.TEMPLATE.END}}");

      var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

      return template.Replace("{{ORDER_ENTRY.TEMPLATE.START}}", string.Empty);
    }


    private string GetBillTotalsTemplate() {
      int startIndex = _htmlTemplate.IndexOf("{{TAXES.TEMPLATE.START}}");
      int endIndex = _htmlTemplate.IndexOf("{{TAXES.TEMPLATE.END}}");

      var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

      return template.Replace("{{TAXES.TEMPLATE.START}}", string.Empty);
    }


    private StringBuilder ReplaceBillsTemplate(StringBuilder html,
                                               StringBuilder billsHtml) {
      int startIndex = html.ToString().IndexOf("{{BILLS.TEMPLATE.START}}");
      int endIndex = html.ToString().IndexOf("{{BILLS.TEMPLATE.END}}");

      html.Remove(startIndex, endIndex - startIndex);

      return html.Replace("{{BILLS.TEMPLATE.END}}", billsHtml.ToString());
    }


    private StringBuilder ReplaceEntriesTemplate(StringBuilder html,
                                                 StringBuilder entriesHtml) {
      int startIndex = html.ToString().IndexOf("{{ORDER_ENTRY.TEMPLATE.START}}");
      int endIndex = html.ToString().IndexOf("{{ORDER_ENTRY.TEMPLATE.END}}");

      html.Remove(startIndex, endIndex - startIndex);

      return html.Replace("{{ORDER_ENTRY.TEMPLATE.END}}", entriesHtml.ToString());
    }


    private StringBuilder ReplaceBillTotalsTemplate(StringBuilder html, StringBuilder totalsHtml) {
      int startIndex = html.ToString().IndexOf("{{TAXES.TEMPLATE.START}}");
      int endIndex = html.ToString().IndexOf("{{TAXES.TEMPLATE.END}}");

      html.Remove(startIndex, endIndex - startIndex);

      return html.Replace("{{TAXES.TEMPLATE.END}}", totalsHtml.ToString());
    }

    #endregion Builders

    #region Helpers

    private string GetFileName(string filename) {
      return Path.Combine(FileTemplateConfig.GenerationStoragePath + "/payment.transactions/", filename);
    }


    private string GetVoucherPdfFileName() {
      return $"cedula.pago.{_paymentOrder.PaymentOrderNo}.{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.pdf";
    }


    private void SaveHtmlAsPdf(string html, string filename) {
      string fullpath = GetFileName(filename);

      var pdfConverter = new HtmlToPdfConverter();

      var options = new PdfConverterOptions {
        BaseUri = FileTemplateConfig.TemplatesStoragePath,
        Landscape = true
      };

      pdfConverter.Convert(html, fullpath, options);
    }


    private FileDto ToFileDto(string filename) {
      return new FileDto(FileType.Pdf, $"{FileTemplateConfig.GeneratedFilesBaseUrl}/payment.transactions/{filename}");
    }

    #endregion Helpers

  }  // class PaymentOrderVoucherBuilder

}  // namespace Empiria.Payments.Reporting 
