/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashFlowProjectionCategoriesController       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve cash flow projection's categories.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.CashFlow.Projections.WebApi {

  /// <summary>Web API used to retrieve cash flow projection's categories.</summary>
  public class CashFlowProjectionCategoriesController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cash-flow/projections/categories")]
    public CollectionModel GetCategories() {

      FixedList<NamedEntityDto> categories = CashFlowProjectionCategory.GetList()
                                                                       .MapToNamedEntityList();

      return new CollectionModel(this.Request, categories);
    }

    #endregion Query web apis

  }  // class CashFlowProjectionCategoriesController

}  // namespace Empiria.CashFlow.Projections.WebApi
