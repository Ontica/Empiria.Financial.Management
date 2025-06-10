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

    #region Single entry web apis

    [HttpPost]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/calculate")]
    public CollectionModel CalculateProjectionEntries([FromUri] string projectionUID) {

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        FixedList<CashFlowProjectionEntryDto> entries = usecases.CalculateProjectionEntries(projectionUID);

        return new CollectionModel(base.Request, entries);
      }
    }


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

    #endregion Single entry web apis

    #region By year entries web apis

    [HttpPost]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/create-annually")]
    public SingleObjectModel CreateProjectionEntriesByYear([FromUri] string projectionUID,
                                                           [FromBody] CashFlowProjectionEntryByYearFields fields) {

      fields.ProjectionUID = projectionUID;

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        CashFlowProjectionEntryByYearDto entryByYear = usecases.CreateProjectionEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/{entryByYearUID}/get-annually")]
    public SingleObjectModel GetProjectionEntriesByYear([FromUri] string projectionUID,
                                                        [FromUri] string entryByYearUID) {

      var fields = new CashFlowProjectionEntryByYearFields {
        UID = entryByYearUID,
        ProjectionUID = projectionUID
      };

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        CashFlowProjectionEntryByYearDto entryByYear = usecases.GetProjectionEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpDelete]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/{entryByYearUID}/remove-annually")]
    public NoDataModel RemoveProjectionEntriesByYear([FromUri] string projectionUID,
                                                     [FromUri] string entryByYearUID) {

      var fields = new CashFlowProjectionEntryByYearFields {
        UID = entryByYearUID,
        ProjectionUID = projectionUID
      };

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {

        usecases.RemoveProjectionEntryByYear(fields);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/entries/{entryByYearUID}/update-annually")]
    public SingleObjectModel UpdateProjectionEntriesByYear([FromUri] string projectionUID,
                                                           [FromUri] string entryByYearUID,
                                                           [FromBody] CashFlowProjectionEntryByYearFields fields) {

      fields.UID = entryByYearUID;
      fields.ProjectionUID = projectionUID;

      using (var usecases = CashFlowProjectionEntriesUseCases.UseCaseInteractor()) {
        CashFlowProjectionEntryByYearDto entryByYear = usecases.UpdateProjectionEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }

    #endregion By year entries web apis

  }  // class CashFlowProjectionEntriesController

}  // namespace Empiria.CashFlow.Projections.WebApi
