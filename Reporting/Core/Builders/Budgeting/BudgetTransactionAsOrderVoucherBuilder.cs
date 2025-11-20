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
      //html = BuildEntries(html);
      //html = BuildTotals(html);
      //html = BuildFooter(html);

      return html.ToString();
    }


    private StringBuilder BuildEntries(StringBuilder html) {
      string TEMPLATE = GetEntriesTemplate();

      var entriesHtml = new StringBuilder();

      return ReplaceEntriesTemplate(html, entriesHtml);
    }


    private StringBuilder BuildFooter(StringBuilder html) {
      var totalsHtml = new StringBuilder();

      return ReplaceTotalsTemplate(html, totalsHtml);
    }

    private StringBuilder BuildHeader(StringBuilder html) {
      const string NO_VALID = "<span class='warning'> SUFICIENCIA PRESUPUESTAL PENDIENTE DE AUTORIZAR </span>";

      html.Replace("{{SYSTEM.DATETIME}}", $"Impresión: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
      html.Replace("{{REPORT.TITLE}}",
                    _txn.AuthorizedBy.IsEmptyInstance ? NO_VALID : _templateConfig.Title);
      html.Replace("{{TRANSACTION_NUMBER}}", _txn.TransactionNo);
      html.Replace("{{TRANSACTION_TYPE.NAME}}", _txn.BudgetTransactionType.DisplayName);
      html.Replace("{{BASE_PARTY.NAME}}", _txn.BaseParty.Name);
      html.Replace("{{BUDGET.NAME}}", _txn.BaseBudget.Name);
      html.Replace("{{BASE_ENTITY_TYPE.NAME}}", "No aplica");
      html.Replace("{{BASE_ENTITY}}", "No aplica");
      html.Replace("{{RECORDED_BY}}", _txn.RecordedBy.Name);
      html.Replace("{{RECORDING_DATE}}", _txn.RecordingDate.ToString("dd/MMM/yyyy"));
      html.Replace("{{AUTHORIZED_BY}}", _txn.AuthorizedBy.Name);
      html.Replace("{{AUTHORIZATION_DATE}}",
                    _txn.AuthorizedBy.IsEmptyInstance ? NO_VALID : _txn.AuthorizationDate.ToString("dd/MMM/yyyy"));

      return html;
    }


    private StringBuilder BuildTotals(StringBuilder html) {
      string TEMPLATE = GetTotalsTemplate();

      var totalsHtml = new StringBuilder();

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
