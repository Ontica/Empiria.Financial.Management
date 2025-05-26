/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashFlowProjectionsController                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update cash flow projections.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.StateEnums;
using Empiria.Storage;
using Empiria.WebApi;

using Empiria.CashFlow.Projections.Adapters;
using Empiria.CashFlow.Projections.UseCases;

namespace Empiria.CashFlow.Projections.WebApi {

  /// <summary>Web API used to retrieve and update cash flow projections.</summary>
  public class CashFlowProjectionsController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v1/cash-flow/projections")]
    public SingleObjectModel CreateProjection([FromBody] CashFlowProjectionFields fields) {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        CashFlowProjectionHolderDto projection = usecases.CreateProjection(fields);

        return new SingleObjectModel(base.Request, projection);
      }
    }


    [HttpDelete]
    [Route("v1/cash-flow/projections/{projectionUID:guid}")]
    public NoDataModel DeleteOrCancelProjection([FromUri] string projectionUID) {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteOrCancelProjection(projectionUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/projections/operation-sources")]
    public SingleObjectModel GetOperationSources() {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> operationSources = usecases.GetOperationSources();

        return new SingleObjectModel(base.Request, operationSources);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/projections/{projectionUID:guid}")]
    public SingleObjectModel GetProjection([FromUri] string projectionUID) {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        CashFlowProjectionHolderDto projection = usecases.GetProjection(projectionUID);

        return new SingleObjectModel(base.Request, projection);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/projections/{projectionUID:guid}/print")]
    public SingleObjectModel PrintProjection([FromUri] string projectionUID) {

      FileDto file = new FileDto(FileType.Pdf,
        $"https://bnodpycgcyf-b.banobras.gob.mx/pyc/output.files/budgeting.transactions/" +
        $"cedula.presupuestal.2025-GC-AUT-00710.2025.05.23-04.20.08.pdf");

      return new SingleObjectModel(base.Request, file);
    }


    [HttpPost]
    [Route("v1/cash-flow/projections/search")]
    public CollectionModel SearchProjections([FromBody] CashFlowProjectionsQuery query) {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        FixedList<CashFlowProjectionDescriptorDto> projections = usecases.SearchProjections(query);

        return new CollectionModel(base.Request, projections);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/projections/parties")]
    public CollectionModel SearchProjectionsParties([FromBody] TransactionPartiesQuery query) {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> parties = usecases.SearchProjectionsParties(query);

        return new CollectionModel(base.Request, parties);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/cash-flow/projections/{projectionUID:guid}")]
    public SingleObjectModel UpdateProjection([FromUri] string projectionUID,
                                              [FromBody] CashFlowProjectionFields fields) {

      using (var usecases = CashFlowProjectionUseCases.UseCaseInteractor()) {
        CashFlowProjectionHolderDto projection = usecases.UpdateProjection(projectionUID, fields);

        return new SingleObjectModel(base.Request, projection);
      }
    }

    #endregion Query web apis

  }  // class CashFlowProjectionsController

}  // namespace Empiria.CashFlow.Projections.WebApi
