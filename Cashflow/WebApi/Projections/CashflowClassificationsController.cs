/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                          Component : Web Api                               *
*  Assembly : Empiria.Cashflow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashflowClassificationsController            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve cashflow projections classifications.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Projects;

namespace Empiria.Cashflow.Projections.WebApi {

  /// <summary>Web API used to retrieve cashflow projections classifications. </summary>
  public class CashflowClassificationsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cashflow/projections/classifications")]
    public CollectionModel GetClassifications() {

      FixedList<NamedEntityDto> classifications = FinancialProjectCategory.GetList()
                                                                          .MapToNamedEntityList();

      return new CollectionModel(this.Request, classifications);
    }

    #endregion Query web apis

  }  // class CashflowClassificationsController

}  // namespace Empiria.Cashflow.Projections.WebApi
