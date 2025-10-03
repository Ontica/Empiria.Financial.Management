/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Explorer                           Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Web Api Controller              *
*  Type     : CashFlowReportsController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrieve cash flow related reports.                                      *
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

  /// <summary>Query web API used to retrieve cash flow related reports.</summary>
  public class CashFlowReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v1/cash-flows/reports/ConceptAnalytic")]  // ToDo: remove this route in future versions
    [Route("v1/cash-flow/reports/ConceptAnalytic")]
    public async Task<SingleObjectModel> ConceptAnalytics([FromBody] CashFlowReportQuery query) {

      query.ReportType = CashFlowReportType.ConceptAnalytic;

      using (var usecases = CashFlowReportsUseCases.UseCaseInteractor()) {
        var concepts = await usecases.ConceptsAnalytics(query);

        return new SingleObjectModel(base.Request, concepts);
      }
    }


    [HttpPost]
    [Route("v1/cash-flows/reports/ConceptAnalytic/export")]  // ToDo: remove this route in future versions
    [Route("v1/cash-flow/reports/ConceptAnalytic/export")]
    public async Task<SingleObjectModel> ExportConceptAnalytics([FromBody] CashFlowReportQuery query) {

      query.ReportType = CashFlowReportType.ConceptAnalytic;

      using (var usecases = CashFlowReportsUseCases.UseCaseInteractor()) {
        var concepts = await usecases.ConceptsAnalytics(query);

        var reportingService = CashFlowReportingService.ServiceInteractor();

        FileDto report = reportingService.ExportConceptsAnalyticsToExcel(concepts);

        base.SetOperation($"Se exportó el reporte analítico de por concepto de movimientos a Excel.");

        return new SingleObjectModel(base.Request, report);
      }
    }

    #endregion Web apis

  }  // class CashFlowReportsController

}  // namespace Empiria.CashFlow.Explorer.WebApi
