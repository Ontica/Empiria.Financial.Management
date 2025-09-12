/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Explorer                           Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Web Api Controller              *
*  Type     : CashFlowExplorerController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive cash flow explorer information.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.CashFlow.Explorer.Adapters;
using Empiria.CashFlow.Explorer.UseCases;

using Empiria.CashFlow.Reporting;

namespace Empiria.CashFlow.Explorer.WebApi {

  /// <summary>Query web API used to retrive cash flow explorer information.</summary>
  public class CashFlowExplorerController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v1/cash-flow/explorer")]
    public async Task<SingleObjectModel> ExploreCashFlow([FromBody] CashFlowExplorerQuery query) {

      switch (query.ReportType) {
        case CashFlowReportType.CashFlow:
          var cashflow = await GetExplorerData<CashFlowExplorerEntry>(query);

          return new SingleObjectModel(base.Request, cashflow);

        case CashFlowReportType.ConceptAnalytic:
          var concepts = await GetExplorerData<ConceptAnalyticsDto>(query);

          return new SingleObjectModel(base.Request, concepts);

        default:
          throw Assertion.EnsureNoReachThisCode($"El reporte {query.ReportType} no está disponible.");
      }
    }

    [HttpPost]
    [Route("v1/cash-flow/export")]
    public async Task<SingleObjectModel> ExportCashFlowToExcel([FromBody] CashFlowExplorerQuery query) {

      var reportingService = CashFlowReportingService.ServiceInteractor();

      FileDto report;

      switch (query.ReportType) {
        case CashFlowReportType.CashFlow:
          var cashflow = await GetExplorerData<CashFlowExplorerEntry>(query);

          report = reportingService.ExportCashFlowExplorerToExcel(cashflow);

          break;

        case CashFlowReportType.ConceptAnalytic:
          var concepts = await GetExplorerData<ConceptAnalyticsDto>(query);

          report = reportingService.ExportConceptsAnalyticsToExcel(concepts);

          break;

        default:
          throw Assertion.EnsureNoReachThisCode($"El reporte {query.ReportType} no está disponible.");
      }

      base.SetOperation($"Se exportó el reporte de flujo de efectivo a Excel.");

      return new SingleObjectModel(base.Request, report);
    }

    #endregion Web apis

    #region Helpers

    private async Task<DynamicDto<T>> GetExplorerData<T>(CashFlowExplorerQuery query) {
      using (var usecases = CashFlowExplorerUseCases.UseCaseInteractor()) {
        switch (query.ReportType) {
          case CashFlowReportType.CashFlow:
            return (DynamicDto<T>) (object) await usecases.ExploreCashFlow(query);

          case CashFlowReportType.ConceptAnalytic:
            return (DynamicDto<T>) (object) await usecases.ConceptsAnalytics(query);

          default:
            throw Assertion.EnsureNoReachThisCode($"El reporte {query.ReportType} no está disponible.");
        }
      }
    }

    #endregion Helpers

  }  // class CashFlowExplorerController

}  // namespace Empiria.CashFlow.Explorer.WebApi
