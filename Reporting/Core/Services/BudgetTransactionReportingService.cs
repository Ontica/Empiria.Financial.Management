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
      return Service.CreateInstance<BudgetTransactionReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportTransactionToPdf(BudgetTransactionByYear transaction) {
      Assertion.Require(transaction, nameof(transaction));

      var templateUID = $"{this.GetType().Name}.ExportTransactionToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetTransactionVoucherBuilder(templateConfig);

      return exporter.CreateVoucher(transaction);
    }


    public FileDto ExportTransactionEntriesToExcel(FixedList<BudgetTransactionByYear> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      var templateUID = $"{this.GetType().Name}.ExportTransactionEntriesToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetTransactionEntriesToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }

    #endregion Services

  } // class BudgetTransactionReportingService

} // namespace Empiria.Budgeting.Reporting
