/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : CashFlowReportingService                      License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate cash flow reports.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Reporting {

  /// <summary>Provides services used to generate cash flow reports.</summary>
  public class CashFlowReportingService : Service {

    #region Constructors and parsers

    private CashFlowReportingService() {
      // no-op
    }

    static public CashFlowReportingService ServiceInteractor() {
      return Service.CreateInstance<CashFlowReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportCashFlowExplorerToExcel(CashFlowExplorerResultDto explorerResult) {
      Assertion.Require(explorerResult, nameof(explorerResult));

      var templateUID = $"{this.GetType().Name}.ExportCashExplorerToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashFlowExplorerToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(explorerResult);

      return excelFile.ToFileDto();
    }

    #endregion Services

  } // class CashFlowReportingService

} // namespace Empiria.CashFlow.Reporting
