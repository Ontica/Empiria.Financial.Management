/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashFlowProjectionEntriesController          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update cash flow projection entries.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.CashFlow.Projections.Adapters;
using Empiria.CashFlow.Projections.UseCases;

namespace Empiria.CashFlow.Projections.WebApi {

  /// <summary>Web API used to retrieve and update cash flow projection entries.</summary>
  public class CashFlowProjectionEntriesController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries")]
    public SingleObjectModel CreateProjectionEntry([FromUri] string projectionUID,
                                                   [FromBody] CashFlowProjectionEntryFields fields) {

      fields.ProjectionUID = projectionUID;

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        CashFlowProjectionEntryDto projectionEntry = usecases.CreateProjectionEntry(fields);

        return new SingleObjectModel(base.Request, projectionEntry);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/{projectionEntryUID:guid}")]
    public SingleObjectModel GetProjectionEntry([FromUri] string projectionUID,
                                                [FromUri] string projectionEntryUID) {

      var fields = new CashFlowProjectionEntryFields {
        UID = projectionEntryUID,
        ProjectionUID = projectionUID,
      };

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        CashFlowProjectionEntryDto projectionEntry = usecases.GetProjectionEntry(fields);

        return new SingleObjectModel(base.Request, projectionEntry);
      }
    }


    [HttpDelete]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/{projectionEntryUID:guid}")]
    public NoDataModel RemoveProjectionEntry([FromUri] string projectionUID,
                                             [FromUri] string projectionEntryUID) {

      var fields = new CashFlowProjectionEntryFields {
        UID = projectionEntryUID,
        ProjectionUID = projectionUID,
      };

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveProjectionEntry(fields);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/{projectionEntryUID:guid}")]
    public SingleObjectModel UpdateProjectionEntry([FromUri] string projectionUID,
                                                   [FromUri] string projectionEntryUID,
                                                   [FromBody] CashFlowProjectionEntryFields fields) {

      fields.UID = projectionEntryUID;
      fields.ProjectionUID = projectionUID;

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        CashFlowProjectionEntryDto projectionEntry = usecases.UpdateProjectionEntry(fields);

        return new SingleObjectModel(base.Request, projectionEntry);
      }
    }

    #endregion Web Apis

  }  // class CashFlowProjectionEntriesController

}  // namespace Empiria.CashFlow.Projections.WebApi
