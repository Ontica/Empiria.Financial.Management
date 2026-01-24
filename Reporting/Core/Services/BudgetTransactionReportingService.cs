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

    public FileDto ExportGroupedEntriesToExcel(FixedList<BudgetTransactionByYear> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      var templateUID = $"{GetType().Name}.ExportGroupedEntriesToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetGroupedEntriesToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public FileDto ExportUngroupedEntriesToExcel(FixedList<BudgetTransaction> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      var templateUID = $"{GetType().Name}.ExportUngroupedEntriesToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetUngroupedEntriesToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public FileDto ExportTransactionVoucherToPdf(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      switch (transaction.BudgetTransactionType.OperationType) {

        case BudgetOperationType.Authorize:
        case BudgetOperationType.Expand:
        case BudgetOperationType.Modify:
        case BudgetOperationType.Plan:
          return ExportTransactionByYearToPdf(transaction);

        case BudgetOperationType.Request:
        case BudgetOperationType.Commit:
          return ExportBudgetRequestToPdf(transaction);

        case BudgetOperationType.ApprovePayment:
        case BudgetOperationType.Exercise:
          return ExportBudgetRequestToPdf(transaction);

        default:
          throw Assertion.EnsureNoReachThisCode($"Unsupported budget operation type: " +
                                                $"{transaction.BudgetTransactionType.OperationType}");
      }
    }

    #endregion Services

    #region Helpers

    private FileDto ExportBudgetRequestToPdf(BudgetTransaction transaction) {
      var templateUID = $"{GetType().Name}.ExportBudgetRequestToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetRequestVoucherBuilder(transaction, templateConfig);

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
