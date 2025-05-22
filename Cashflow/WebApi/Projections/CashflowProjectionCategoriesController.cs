/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                          Component : Web Api                               *
*  Assembly : Empiria.Cashflow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashflowClassificationsController            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve cashflow projection's categories.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Projects;

namespace Empiria.Cashflow.Projections.WebApi {

  /// <summary>Web API used to retrieve cashflow projection's categories.</summary>
  public class CashflowProjectionCategoriesController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cashflow/projections/categories")]
    public CollectionModel GetCategories() {

      FixedList<NamedEntityDto> categories =CashflowProjectionCategory.GetList()
                                                                      .MapToNamedEntityList();

      return new CollectionModel(this.Request, categories);
    }

    #endregion Query web apis

  }  // class CashflowProjectionCategoriesController

}  // namespace Empiria.Cashflow.Projections.WebApi
