/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : CashTransactionVoucherBuilder                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a Pdf file with a voucher for a cash transaction.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;
using System.Text;

using Empiria.Office;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.Financial.Reporting {

  /// <summary>Builds a Pdf file with a voucher for a cash transaction.</summary>
  internal class CashTransactionVoucherBuilder {

    private readonly FileTemplateConfig _templateConfig;
    private readonly string _htmlTemplate;

    public CashTransactionVoucherBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
      _htmlTemplate = File.ReadAllText(_templateConfig.TemplateFullPath);
    }


    internal FileDto CreateVoucher(CashTransactionHolderDto transaction) {
      Assertion.Require(transaction, nameof(transaction));

      string filename = GetVoucherPdfFileName(transaction);

      string html = BuildVoucherHtml(transaction);

      SaveHtmlAsPdf(html, filename);

      return ToFileDto(filename);
    }

    #region Builders

    private string BuildVoucherHtml(CashTransactionHolderDto transaction) {
      StringBuilder html = new StringBuilder(_htmlTemplate);

      html = BuildHeader(html, transaction.Transaction);
      html = BuildEntries(html, transaction);

      return html.ToString();
    }


    private StringBuilder BuildEntries(StringBuilder html, CashTransactionHolderDto txn) {
      string TEMPLATE = GetEntriesTemplate();

      var entriesHtml = new StringBuilder();

      foreach (var entry in txn.Entries) {
        var entryHtml = new StringBuilder(TEMPLATE.Replace("{{ACCOUNT.NUMBER}}", entry.AccountNumber));

        entryHtml.Replace("{{ACCOUNT.NAME}}", entry.AccountName);
        entryHtml.Replace("{{SECTOR.CODE}}", entry.SectorCode);
        entryHtml.Replace("{{SUBLEDGER_ACCOUNT.NUMBER}}", entry.SubledgerAccountNumber != string.Empty ? entry.SubledgerAccountNumber : "No aplica");
        entryHtml.Replace("{{SUBLEDGER_ACCOUNT.NAME}}", entry.SubledgerAccountName);
        entryHtml.Replace("{{VERIFICATION_NUMBER}}", entry.VerificationNumber != string.Empty ? entry.VerificationNumber : "-");
        entryHtml.Replace("{{RESPONSIBILITY_AREA.CODE}}", entry.ResponsibilityAreaCode);
        entryHtml.Replace("{{CURRENCY.CODE}}", entry.CurrencyName);
        entryHtml.Replace("{{EXCHANGE.RATE}}", entry.ExchangeRate.ToString("C2"));
        entryHtml.Replace("{{DEBIT.AMOUNT}}", entry.Debit.ToString("C2"));
        entryHtml.Replace("{{CREDIT.AMOUNT}}", entry.Credit.ToString("C2"));
        entryHtml.Replace("{{CASH_ACCOUNT.NAME}}", entry.CashAccount.Name);
        entryHtml.Replace("{{CUENTA_SISTEMA_LEGADO}}", entry.CuentaSistemaLegado);

        entriesHtml.Append(entryHtml);
      }

      return ReplaceEntriesTemplate(html, entriesHtml);
    }


    private StringBuilder BuildHeader(StringBuilder html, CashTransactionDescriptor txn) {
      bool IS_PENDING = txn.Status == "Pending";
      var TITLE = IS_PENDING ? $"<span class='warning'> {_templateConfig.Title.ToUpper()} PENDIENTE </span>" : _templateConfig.Title;

      html.Replace("{{SYSTEM.DATETIME}}", $"Impresión: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
      html.Replace("{{REPORT.TITLE}}", TITLE);
      html.Replace("{{NUMBER}}", txn.Number);
      html.Replace("{{ID}}", txn.Id.ToString());
      html.Replace("{{CONCEPT}}", txn.Concept);
      html.Replace("{{LEDGER.NAME}}", txn.LedgerName);
      html.Replace("{{VOUCHER_TYPE.NAME}}", txn.VoucherTypeName);
      html.Replace("{{TRANSACTION_TYPE.NAME}}", txn.TransactionTypeName);
      html.Replace("{{SOURCE.NAME}}", txn.SourceName);
      html.Replace("{{ACCOUNTING_DATE}}", txn.AccountingDate.ToString("dd/MMM/yyyy"));
      html.Replace("{{ELABORATED_BY}}", txn.ElaboratedBy);
      html.Replace("{{RECORDING_DATE}}", txn.RecordingDate.ToString("dd/MMM/yyyy"));
      html.Replace("{{AUTHORIZED_BY}}", txn.AuthorizedBy);

      return html;
    }


    private string GetEntriesTemplate() {
      int startIndex = _htmlTemplate.IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.START}}");
      int endIndex = _htmlTemplate.IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.END}}");

      var template = _htmlTemplate.Substring(startIndex, endIndex - startIndex);

      return template.Replace("{{TRANSACTION_ENTRY.TEMPLATE.START}}", string.Empty);
    }


    private StringBuilder ReplaceEntriesTemplate(StringBuilder html,
                                                 StringBuilder entriesHtml) {
      int startIndex = html.ToString().IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.START}}");
      int endIndex = html.ToString().IndexOf("{{TRANSACTION_ENTRY.TEMPLATE.END}}");

      html.Remove(startIndex, endIndex - startIndex);

      return html.Replace("{{TRANSACTION_ENTRY.TEMPLATE.END}}", entriesHtml.ToString());
    }

    #endregion Builders

    #region Helpers

    private string GetFileName(string filename) {
      return Path.Combine(FileTemplateConfig.GenerationStoragePath + "/cash.transactions/", filename);
    }


    private string GetVoucherPdfFileName(CashTransactionHolderDto txn) {
      return $"cedula.flujo.efectivo.{txn.Transaction.Number}.{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.pdf";
    }


    private void SaveHtmlAsPdf(string html, string filename) {
      string fullpath = GetFileName(filename);

      var pdfConverter = new HtmlToPdfConverter();

      var options = new PdfConverterOptions {
        BaseUri = FileTemplateConfig.TemplatesStoragePath,
      };

      pdfConverter.Convert(html, fullpath, options);
    }


    private FileDto ToFileDto(string filename) {
      return new FileDto(FileType.Pdf, FileTemplateConfig.GeneratedFilesBaseUrl + "/cash.transactions/" + filename);
    }

    #endregion Helpers

  } // class CashTransactionVoucherBuilder

} // namespace Empiria.Financial.Reporting
