/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetTransactionByYearVoucherBuilder         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a PDF file with a voucher for a budget transaction with year months in columns.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;
using System.Text;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds a PDF file with a voucher for a budget transaction with year months in columns.</summary>
  internal class BudgetTransactionByYearVoucherBuilder {

    private readonly FileTemplateConfig _templateConfig;
    private readonly string _htmlTemplate;

    public BudgetTransactionByYearVoucherBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
      _htmlTemplate = File.ReadAllText(_templateConfig.TemplateFullPath);
    }


    internal FileDto CreateVoucher(BudgetTransactionByYear transaction) {
      Assertion.Require(transaction, nameof(transaction));

      string filename = GetVoucherPdfFileName(transaction);

      string html = BuildVoucherHtml(transaction);

      SaveHtmlAsPdf(html, filename);

      return ToFileDto(filename);
    }

    #region Builders

    private string BuildVoucherHtml(BudgetTransactionByYear transaction) {
      StringBuilder html = new StringBuilder(_htmlTemplate);

      html = BuildHeader(html, transaction.Transaction);
      html = BuildEntries(html, transaction);
      html = BuildTotals(html, transaction);

      return html.ToString();
    }


    private StringBuilder BuildEntries(StringBuilder html, BudgetTransactionByYear txn) {
      string TEMPLATE = GetEntriesTemplate();

      var entriesHtml = new StringBuilder();

      foreach (var entry in txn.GetEntries()) {
        var entryHtml = new StringBuilder(TEMPLATE.Replace("{{BUDGET_ACCOUNT.CODE}}",
                                          entry.BudgetAccount.BaseSegment.Code));

        entryHtml.Replace("{{BUDGET_ACCOUNT.NAME}}", entry.BudgetAccount.BaseSegment.Name);
        entryHtml.Replace("{{BUDGET_PROGRAM.CODE}}", entry.BudgetAccount.BudgetProgram);

        entryHtml.Replace("{{YEAR}}", entry.Year.ToString());
        entryHtml.Replace("{{BALANCE_COLUMN}}", entry.BalanceColumn.Name);

        entryHtml.Replace("{{TOTAL}}", entry.Total.ToString("C2"));
        entryHtml.Replace("{{AMOUNT_ENE}}", entry.GetAmountForMonth(1).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_FEB}}", entry.GetAmountForMonth(2).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_MAR}}", entry.GetAmountForMonth(3).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_ABR}}", entry.GetAmountForMonth(4).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_MAY}}", entry.GetAmountForMonth(5).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_JUN}}", entry.GetAmountForMonth(6).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_JUL}}", entry.GetAmountForMonth(7).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_AGO}}", entry.GetAmountForMonth(8).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_SEP}}", entry.GetAmountForMonth(9).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_OCT}}", entry.GetAmountForMonth(10).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_NOV}}", entry.GetAmountForMonth(11).ToString("C2"));
        entryHtml.Replace("{{AMOUNT_DIC}}", entry.GetAmountForMonth(12).ToString("C2"));

        entriesHtml.Append(entryHtml);
      }

      return ReplaceEntriesTemplate(html, entriesHtml);
    }


    private StringBuilder BuildHeader(StringBuilder html, BudgetTransaction txn) {
      const string NO_VALID = "<span class='warning'> CÉDULA PRESUPUESTAL PENDIENTE DE AUTORIZAR </span>";

      html.Replace("{{SYSTEM.DATETIME}}", $"Impresión: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
      html.Replace("{{REPORT.TITLE}}",
                    txn.AuthorizedBy.IsEmptyInstance ? NO_VALID : _templateConfig.Title);
      html.Replace("{{TRANSACTION_NUMBER}}", txn.TransactionNo);
      html.Replace("{{TRANSACTION_TYPE.NAME}}", txn.BudgetTransactionType.DisplayName);
      html.Replace("{{BASE_PARTY.NAME}}", txn.BaseParty.Name);
      html.Replace("{{BUDGET.NAME}}", txn.BaseBudget.Name);
      html.Replace("{{BASE_ENTITY_TYPE.NAME}}", "No aplica");
      html.Replace("{{BASE_ENTITY}}", "No aplica");
      html.Replace("{{RECORDED_BY}}", txn.RecordedBy.Name);
      html.Replace("{{RECORDING_DATE}}", txn.RecordingDate.ToString("dd/MMM/yyyy"));
      html.Replace("{{AUTHORIZED_BY}}", txn.AuthorizedBy.Name);
      html.Replace("{{AUTHORIZATION_DATE}}",
                    txn.AuthorizedBy.IsEmptyInstance ? NO_VALID : txn.AuthorizationDate.ToString("dd/MMM/yyyy"));

      return html;
    }


    private StringBuilder BuildTotals(StringBuilder html, BudgetTransactionByYear txn) {
      string TEMPLATE = GetTotalsTemplate();

      var totalsHtml = new StringBuilder();

      foreach (var entry in txn.GetTotals()) {
        var totalHtml = new StringBuilder(TEMPLATE.Replace("{{TITLE}}", $"Presupuesto {entry.Budget.Name}"));

        totalHtml.Replace("{{YEAR}}", entry.Year.ToString());
        totalHtml.Replace("{{BALANCE_COLUMN}}", entry.BalanceColumn.Name);

        totalHtml.Replace("{{TOTAL}}", entry.Total.ToString("C2"));
        totalHtml.Replace("{{TOTAL_ENE}}", entry.GetTotalForMonth(1).ToString("C2"));
        totalHtml.Replace("{{TOTAL_FEB}}", entry.GetTotalForMonth(2).ToString("C2"));
        totalHtml.Replace("{{TOTAL_MAR}}", entry.GetTotalForMonth(3).ToString("C2"));
        totalHtml.Replace("{{TOTAL_ABR}}", entry.GetTotalForMonth(4).ToString("C2"));
        totalHtml.Replace("{{TOTAL_MAY}}", entry.GetTotalForMonth(5).ToString("C2"));
        totalHtml.Replace("{{TOTAL_JUN}}", entry.GetTotalForMonth(6).ToString("C2"));
        totalHtml.Replace("{{TOTAL_JUL}}", entry.GetTotalForMonth(7).ToString("C2"));
        totalHtml.Replace("{{TOTAL_AGO}}", entry.GetTotalForMonth(8).ToString("C2"));
        totalHtml.Replace("{{TOTAL_SEP}}", entry.GetTotalForMonth(9).ToString("C2"));
        totalHtml.Replace("{{TOTAL_OCT}}", entry.GetTotalForMonth(10).ToString("C2"));
        totalHtml.Replace("{{TOTAL_NOV}}", entry.GetTotalForMonth(11).ToString("C2"));
        totalHtml.Replace("{{TOTAL_DIC}}", entry.GetTotalForMonth(12).ToString("C2"));

        totalsHtml.Append(totalHtml);
      }

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
      return Path.Combine($"{FileTemplateConfig.GenerationStoragePath}/budgeting.transactions/", filename);
    }


    private string GetVoucherPdfFileName(BudgetTransactionByYear txn) {
      return $"cedula.presupuestal.{txn.Transaction.TransactionNo}.{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.pdf";
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

  } // class BudgetTransactionByYearVoucherBuilder

} // namespace Empiria.Budgeting.Reporting
