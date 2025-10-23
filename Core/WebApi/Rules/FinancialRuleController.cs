/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialRuleController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update financial rules.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Rules.UseCases;

namespace Empiria.Financial.Rules.WebApi {

  /// <summary>Web API used to retrive and update financial rules.</summary>
  public class FinancialRuleController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/financial-rules/types")]
    public CollectionModel GetFinancialRuleCategories() {

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> categories = usecases.GetCategories();

        return new CollectionModel(base.Request, categories);
      }
    }

    #endregion Query web apis

  }  // class FinancialRuleController

}  // namespace Empiria.Financial.Rules.WebApi
