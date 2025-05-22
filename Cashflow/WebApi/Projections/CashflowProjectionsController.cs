/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                          Component : Web Api                               *
*  Assembly : Empiria.Cashflow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashflowProjectionsController                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update cashflow projections.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.StateEnums;
using Empiria.WebApi;

using Empiria.Cashflow.Projections.Adapters;
using Empiria.Cashflow.Projections.UseCases;

namespace Empiria.Cashflow.Projections.WebApi {

  /// <summary>Web API used to retrieve and update cashflow projections.</summary>
  public class CashflowProjectionsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cashflow/projections/operation-sources")]
    public SingleObjectModel GetOperationSources() {

      using (var usecases = CashflowProjectionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> operationSources = usecases.GetOperationSources();

        return new SingleObjectModel(base.Request, operationSources);
      }
    }


    [HttpGet]
    [Route("v1/cashflow/projections/{projectionUID:guid}")]
    public SingleObjectModel GetProjection([FromUri] string projectionUID) {

      using (var usecases = CashflowProjectionUseCases.UseCaseInteractor()) {
        CashflowProjectionHolderDto projection = usecases.GetProjection(projectionUID);

        return new SingleObjectModel(base.Request, projection);
      }
    }


    [HttpPost]
    [Route("v1/cashflow/projections/search")]
    public CollectionModel SearchProjections([FromBody] CashflowProjectionsQuery query) {

      using (var usecases = CashflowProjectionUseCases.UseCaseInteractor()) {
        FixedList<CashflowProjectionDescriptorDto> projections = usecases.SearchProjections(query);

        return new CollectionModel(base.Request, projections);
      }
    }


    [HttpPost]
    [Route("v1/cashflow/projections/parties")]
    public CollectionModel SearchProjectionsParties([FromBody] TransactionPartiesQuery query) {

      using (var usecases = CashflowProjectionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> parties = usecases.SearchProjectionsParties(query);

        return new CollectionModel(base.Request, parties);
      }
    }

    #endregion Query web apis

  }  // class CashflowProjectionsController

}  // namespace Empiria.Cashflow.Projections.WebApi
