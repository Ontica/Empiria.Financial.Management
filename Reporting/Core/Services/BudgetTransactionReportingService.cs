/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetTransactionReportingService             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate budget transactions reports.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Storage;
using Empiria.Office;

using Empiria.Financial;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Provides services used to generate budget transactions reports.</summary>
  public class BudgetTransactionReportingService : Service {

    #region Constructors and parsers

    private BudgetTransactionReportingService() {
      // no-op
    }

    static public BudgetTransactionReportingService ServiceInteractor() {
      return CreateInstance<BudgetTransactionReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportTransactionVoucherToPdf(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      if (transaction.HasEntity && transaction.GetEntity() is IBudgetable budgetable) {
        return ExportTransactionAsOrderToPdf(transaction);
      } else {

        return ExportTransactionByYearToPdf(transaction);
      }
    }


    public FileDto ExportTransactionEntriesToExcel(FixedList<BudgetTransactionByYear> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      var templateUID = $"{GetType().Name}.ExportTransactionEntriesToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetTransactionEntriesToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }

    #endregion Services

    #region Helpers

    private FileDto ExportTransactionAsOrderToPdf(BudgetTransaction transaction) {
      var templateUID = $"{GetType().Name}.ExportTransactionAsOrderToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetTransactionAsOrderVoucherBuilder(transaction, templateConfig);

      return exporter.CreateVoucher();
    }


    private FileDto ExportTransactionByYearToPdf(BudgetTransaction transaction) {

      var templateUID = $"{GetType().Name}.ExportTransactionByYearToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetTransactionByYearVoucherBuilder(templateConfig);

      var byYearTransaction = new BudgetTransactionByYear(transaction);

      return exporter.CreateVoucher(byYearTransaction);
    }

    #endregion Helpers

  } // class BudgetTransactionReportingService

} // namespace Empiria.Budgeting.Reporting
