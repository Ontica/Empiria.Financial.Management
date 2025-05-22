/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashFlowClassificationsController            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve cash flow projections classifications.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Projects;

namespace Empiria.CashFlow.Projections.WebApi {

  /// <summary>Web API used to retrieve cash flow projections classifications. </summary>
  public class CashFlowClassificationsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cash-flow/projections/classifications")]
    public CollectionModel GetClassifications() {

      FixedList<NamedEntityDto> classifications = FinancialProjectCategory.GetList()
                                                                          .MapToNamedEntityList();

      return new CollectionModel(this.Request, classifications);
    }

    #endregion Query web apis

  }  // class CashFlowClassificationsController

}  // namespace Empiria.CashFlow.Projections.WebApi
