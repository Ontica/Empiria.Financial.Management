/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetTransactionReportingService             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate budget transactions reports.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;
using Empiria.Office;

namespace Empiria.Financial.Reporting {

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

    public FileDto ExportTransactionToPdf(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      return new FileDto(FileType.Pdf,
                         FileTemplateConfig.GeneratedFilesBaseUrl + "/budgeting.transactions/" + "cedula.pdf");
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

} // namespace Empiria.Financial.Reporting
