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
using Empiria.WebApi;

using Empiria.CashFlow.Projections.Adapters;
using Empiria.CashFlow.Projections.UseCases;

namespace Empiria.CashFlow.Projections.WebApi {

  /// <summary>Web API used to retrieve and update cash flow projections.</summary>
  public class CashFlowProjectionsController : WebApiController {

    #region Query web apis

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

    #endregion Query web apis

  }  // class CashFlowProjectionsController

}  // namespace Empiria.CashFlow.Projections.WebApi
