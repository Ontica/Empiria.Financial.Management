/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                          Component : Web Api                               *
*  Assembly : Empiria.Cashflow.WebApi.dll                  Pattern   : Query web api controller              *
*  Type     : CashflowPlansController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve cashflow plans.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.Cashflow.Projections.WebApi {

  /// <summary>Web API used to retrieve cashflow plans.</summary>
  public class CashflowPlansController : WebApiController {

    #region Command web apis

    [HttpGet]
    [Route("v1/cashflow/projections/plans")]
    public CollectionModel GetPlans() {

      FixedList<NamedEntityDto> plans = CashflowPlan.GetList()
                                                    .MapToNamedEntityList();

      return new CollectionModel(this.Request, plans);
    }

    #endregion Command web apis

  }  // class CashflowPlansController

}  // namespace Empiria.Cashflow.Projections.WebApi
