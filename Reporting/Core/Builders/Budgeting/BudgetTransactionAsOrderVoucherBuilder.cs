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

using Empiria.Orders;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  internal class BudgetTransactionAsOrderVoucherBuilder {

    private readonly BudgetTransaction _txn;
    private readonly Order _order;

    private readonly FileTemplateConfig _templateConfig;
    private readonly string _htmlTemplate;

    public BudgetTransactionAsOrderVoucherBuilder(BudgetTransaction transaction,
                                                  FileTemplateConfig templateConfig) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(templateConfig, nameof(templateConfig));

      _txn = transaction;
      _order = transaction.GetEntity() as Order;

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
      html = BuildTotals(html);
      html = BuildFooter(html);

      return html.ToString();
    }


    private StringBuilder BuildEntries(StringBuilder html) {
      string TEMPLATE = GetEntriesTemplate();

      var entriesHtml = new StringBuilder();

      foreach (var entry in _order.GetItems<OrderItem>()) {
        var entryHtml = new StringBuilder(TEMPLATE.Replace("{{BUDGET_ACCOUNT_CODE}}",
                                          entry.BudgetAccount.Code));

        var budgetEntry = _txn.Entries.Find(x => entry.Id == x.EntityId && x.ControlNo.Length != 0);

        string controlNo = string.Empty;
        string budgetProgram = string.Empty;

        if (budgetEntry != null) {
          controlNo = budgetEntry.ControlNo;
          budgetProgram = budgetEntry.BudgetProgram.Code;
        }

        entryHtml.Replace("{{BUDGET_ACCOUNT_CODE}}", entry.BudgetAccount.Code);
        entryHtml.Replace("{{PRODUCT_CODE}}", entry.ProductCode);
        entryHtml.Replace("{{DESCRIPTION}}", entry.Description);
        entryHtml.Replace("{{ORIGIN_COUNTRY}}", entry.OriginCountry.CountryISOCode);
        entryHtml.Replace("{{CONTROL_NO}}", controlNo);
        entryHtml.Replace("{{PROGRAM}}", budgetProgram);
        entryHtml.Replace("{{YEAR}}", entry.Budget.Year.ToString());
        entryHtml.Replace("{{PRODUCT_UNIT}}", entry.ProductUnit.Name);
        entryHtml.Replace("{{QUANTITY}}", entry.Quantity.ToString("C2"));
        entryHtml.Replace("{{UNIT_PRICE}}", entry.UnitPrice.ToString("C2"));
        entryHtml.Replace("{{TOTAL}}", entry.Subtotal.ToString("C2"));

        entriesHtml.Append(entryHtml);
      }

      return ReplaceEntriesTemplate(html, entriesHtml);
    }


    private StringBuilder BuildFooter(StringBuilder html) {
      var footerHtml = new StringBuilder();

      html.Replace("{{JUSTIFICATION}}", _order.Justification);
      html.Replace("{{OBSERVATIONS}}", _order.Observations);
      html.Replace("{{GUARANTEE_NOTES}}", _order.GuaranteeNotes);
      html.Replace("{{PENALTY_NOTES}}", _order.PenaltyNotes);
      html.Replace("{{DELIVERY_NOTES}}", _order.DeliveryNotes);

      if (_txn.AuthorizedBy.IsEmptyInstance) {
        html.Replace("{{SECURITY_CODE}}", "<span class='warning'>PENDIENTE DE AUTORIZAR</span>");
        html.Replace("{{SECURITY_SEAL}}", "<span class='warning'>ESTE DOCUMENTO NO TIENE VALIDEZ OFICIAL</span>");

        return html;
      }

      var seal = $"{_txn.TransactionNo}|{_order.OrderNo}|{_txn.Justification}|{_txn.GetTotal()}|{_order.Taxes}|{_order.Items}";

      seal = Security.Cryptographer.Encrypt(Security.EncryptionMode.Standard, seal);

      seal = EmpiriaString.DivideLongString(seal, 95, "<br/>");

      html.Replace("{{SECURITY_CODE}}", _txn.TransactionNo.GetHashCode().ToString("00000000"));
      html.Replace("{{SECURITY_SEAL}}", seal);

      return html;
    }


    private StringBuilder BuildHeader(StringBuilder html) {

      string title = _templateConfig.Title;

      if (_txn.IsMultiYear) {
        title = $"{title} (plurianual)";
      }
      if (_txn.AuthorizedBy.IsEmptyInstance) {
        title = $"<span class='warning'>{title}</span>";
      }


      html.Replace("{{SYSTEM.DATETIME}}", $"Impresión: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
      html.Replace("{{REPORT.TITLE}}", title);
      html.Replace("{{TRANSACTION_NO}}", _txn.TransactionNo);
      html.Replace("{{ORDER_NAME}}", _order.Name);
      html.Replace("{{ORDER_NO}}", $"{_order.OrderNo} {(_txn.IsMultiYear ? "(Plurianual)" : string.Empty)}");
      html.Replace("{{REQUESTED_BY}}", ((INamedEntity) _order.RequestedBy).Name);
      html.Replace("{{RECORDING_TIME}}", _order.RequestedTime.ToString("dd/MMM/yyyy"));

      if (ExecutionServer.IsMinOrMaxDate(_order.StartDate)) {
        html.Replace("{{REQUIRED_TIME}}", _order.RequestedTime.ToString("dd/MMM/yyyy"));
      } else if (_order.StartDate == _order.EndDate) {
        html.Replace("{{REQUIRED_TIME}}", EmpiriaString.DateLongString(_order.StartDate));
      } else {
        html.Replace("{{REQUIRED_TIME}}", $"Del {EmpiriaString.DateLongString(_order.StartDate)} " +
                                          $"al {EmpiriaString.DateLongString(_order.EndDate)}");
      }

      html.Replace("{{ESTIMATED_MONTHS}}", _order.EstimatedMonths.ToString("00"));

      return html;
    }


    private StringBuilder BuildTotals(StringBuilder html) {
      string TEMPLATE = GetTotalsTemplate();

      var totalsHtml = new StringBuilder();

      foreach (var entry in _order.Taxes.GetList()) {
        var totalHtml = new StringBuilder(TEMPLATE.Replace("{{TOTAL_TYPE}}", entry.TaxType.Name));

        totalHtml.Replace("{{TOTAL}}", entry.Total.ToString("C2"));

        totalsHtml.Append(totalHtml);
      }

      var orderTotallHtml = new StringBuilder(TEMPLATE.Replace("{{TOTAL_TYPE}}", "Total"));

      orderTotallHtml.Replace("{{TOTAL}}", _order.GetTotal().ToString("C2"));

      totalsHtml.Append(orderTotallHtml);

      return ReplaceTotalsTemplate(html, totalsHtml);
    }


    private string GetEntriesTemplate() {
      int startIndex = _htmlTemplate.IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.START}}");
      int endIndex = _htmlTemplate.IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.END}}");

      var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

      return template.Replace("{{TRANSACTION_ENTRY.TEMPLATE.START}}", string.Empty);
    }


    private string GetTotalsTemplate() {
      int startIndex = _htmlTemplate.IndexOf("{{TRANSACTION_TOTALS.TEMPLATE.START}}");
      int endIndex = _htmlTemplate.IndexOf("{{TRANSACTION_TOTALS.TEMPLATE.END}}");

      var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

      return template.Replace("{{TRANSACTION_TOTALS.TEMPLATE.START}}", string.Empty);
    }


    private StringBuilder ReplaceEntriesTemplate(StringBuilder html,
                                                 StringBuilder entriesHtml) {
      int startIndex = html.ToString().IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.START}}");
      int endIndex = html.ToString().IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.END}}");

      html.Remove(startIndex, endIndex - startIndex);

      return html.Replace("{{TRANSACTION_ENTRY.TEMPLATE.END}}", entriesHtml.ToString());
    }


    private StringBuilder ReplaceTotalsTemplate(StringBuilder html, StringBuilder totalsHtml) {
      int startIndex = html.ToString().IndexOf("{{TRANSACTION_TOTALS.TEMPLATE.START}}");
      int endIndex = html.ToString().IndexOf("{{TRANSACTION_TOTALS.TEMPLATE.END}}");

      html.Remove(startIndex, endIndex - startIndex);

      return html.Replace("{{TRANSACTION_TOTALS.TEMPLATE.END}}", totalsHtml.ToString());
    }

    #endregion Builders

    #region Helpers

    private string GetFileName(string filename) {
      return Path.Combine(FileTemplateConfig.GenerationStoragePath + "/budgeting.transactions/", filename);
    }


    private string GetVoucherPdfFileName() {
      return $"suficiencia.presupuestal.{_txn.TransactionNo}.{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.pdf";
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
      return new FileDto(FileType.Pdf, $"{FileTemplateConfig.GeneratedFilesBaseUrl}/budgeting.transactions/{filename}");
    }

    #endregion Helpers

  }  // class BudgetTransactionAsOrderVoucherBuilder

}  // namespace Empiria.Budgeting.Reporting 
