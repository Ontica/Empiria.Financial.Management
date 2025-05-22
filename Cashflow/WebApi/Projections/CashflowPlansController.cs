/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query web api controller              *
*  Type     : CashFlowPlansController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve cash flow plans.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.CashFlow.Projections.WebApi {

  /// <summary>Web API used to retrieve cash flow plans.</summary>
  public class CashFlowPlansController : WebApiController {

    #region Command web apis

    [HttpGet]
    [Route("v1/cash-flow/projections/plans")]
    public CollectionModel GetPlans() {

      FixedList<NamedEntityDto> plans = CashFlowPlan.GetList()
                                                    .MapToNamedEntityList();

      return new CollectionModel(this.Request, plans);
    }

    #endregion Command web apis

  }  // class CashFlowPlansController

}  // namespace Empiria.CashFlow.Projections.WebApi
