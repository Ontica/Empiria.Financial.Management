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

using Empiria.CashFlow.Reporting;

using Empiria.CashFlow.Explorer.Adapters;
using Empiria.CashFlow.Explorer.UseCases;

namespace Empiria.CashFlow.Explorer.WebApi {

  /// <summary>Query web API used to retrive cash flow explorer information.</summary>
  public class CashFlowExplorerController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v1/cash-flow/explorer")]
    public async Task<SingleObjectModel> ExploreCashFlow([FromBody] CashFlowExplorerQuery query) {

      using (var usecases = CashFlowExplorerUseCases.UseCaseInteractor()) {

        switch (query.ReportType) {

          case CashFlowExplorerReportType.CashFlowConceptsReport:
            var cashFlowConcepts = await usecases.GetCashFlowConcepts(query);

            base.SetOperation($"Se consultó el explorador de flujo de efectivo por conceptos.");

            return new SingleObjectModel(base.Request, cashFlowConcepts);

          case CashFlowExplorerReportType.CashFlowReport:

            var cashFlowTotals = await usecases.GetCashFlowTotals(query);

            base.SetOperation($"Se consultó el reporte ejecutivo de flujo de efectivo.");

            return new SingleObjectModel(base.Request, cashFlowTotals);

          default:
            throw Assertion.EnsureNoReachThisCode($"Unhandled report type {query.ReportType}");
        }
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/export")]
    public async Task<SingleObjectModel> ExportCashFlowToExcel([FromBody] CashFlowExplorerQuery query) {

      using (var usecases = CashFlowExplorerUseCases.UseCaseInteractor()) {
        var cashflow = await usecases.GetCashFlowConcepts(query);

        var reportingService = CashFlowReportingService.ServiceInteractor();

        FileDto report = reportingService.ExportCashFlowExplorerToExcel(cashflow);

        base.SetOperation($"Se exportó el reporte de flujo de efectivo a Excel.");

        return new SingleObjectModel(base.Request, report);
      }
    }

    #endregion Web apis

  }  // class CashFlowExplorerController

}  // namespace Empiria.CashFlow.Explorer.WebApi
