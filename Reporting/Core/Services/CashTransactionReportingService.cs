/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : CashTransactionReportingService               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate cash transactions reports.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Reporting {

  /// <summary>Provides services used to generate cash transactions reports.</summary>
  public class CashTransactionReportingService : Service {

    #region Constructors and parsers

    private CashTransactionReportingService() {
      // no-op
    }

    static public CashTransactionReportingService ServiceInteractor() {
      return Service.CreateInstance<CashTransactionReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportAccountsAnalysisToExcel(FixedList<CashEntryDto> entries) {
      Assertion.Require(entries, nameof(entries));

      var templateUID = $"{this.GetType().Name}.ExportAccountsAnalysisToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashAccountsAnalysisToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(entries);

      return excelFile.ToFileDto();
    }


    public FileDto ExportTransactionToPdf(CashTransactionHolderDto transaction) {
      Assertion.Require(transaction, nameof(transaction));

      var templateUID = $"{this.GetType().Name}.ExportTransactionToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashTransactionVoucherBuilder(templateConfig);

      return exporter.CreateVoucher(transaction);
    }


    public FileDto ExportTransactionsToExcel(FixedList<CashTransactionHolderDto> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      var templateUID = $"{this.GetType().Name}.ExportTransactionsToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashTransactionToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public FileDto ExportTransactionsTotalsToExcel(FixedList<CashTransactionHolderDto> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      var templateUID = $"{this.GetType().Name}.ExportTransactionsTotalsToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashTransactionTotalsToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }



    public FileDto ExportTransactionsEntriesToExcel(FixedList<CashEntryExtendedDto> entries) {
      Assertion.Require(entries, nameof(entries));

      var templateUID = $"{this.GetType().Name}.ExportEntriesToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashEntriesToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(entries);

      return excelFile.ToFileDto();
    }

    #endregion Services

  } // class CashTransactionReportingService

} // namespace Empiria.CashFlow.Reporting
