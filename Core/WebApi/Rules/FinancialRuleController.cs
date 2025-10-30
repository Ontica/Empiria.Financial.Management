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

using Empiria.DynamicData;
using Empiria.WebApi;

using Empiria.Financial.Rules.Adapters;
using Empiria.Financial.Rules.UseCases;

namespace Empiria.Financial.Rules.WebApi {

  /// <summary>Web API used to retrive and update financial rules.</summary>
  public class FinancialRuleController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/financial-rules/categories")]
    public CollectionModel GetCategories() {

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> categories = usecases.GetCategories();

        return new CollectionModel(base.Request, categories);
      }
    }


    [HttpGet]
    [Route("v3/financial-rules/{ruleUID:guid}")]
    public SingleObjectModel GetRule([FromUri] string ruleUID) {

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        FinancialRuleDto rule = usecases.GetRule(ruleUID);

        return new SingleObjectModel(base.Request, rule);
      }
    }


    [HttpPost]
    [Route("v3/financial-rules/categories/{categoryUID:guid}")]
    public SingleObjectModel SearchRules([FromUri] string categoryUID,
                                         [FromUri] FinancialRuleQuery query) {

      query.CategoryUID = categoryUID;

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        DynamicDto<FinancialRuleDescriptor> rules = usecases.SearchRules(query);

        return new SingleObjectModel(base.Request, rules);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v3/financial-rules")]
    public SingleObjectModel CreateRule([FromBody] FinancialRuleFields fields) {

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        FinancialRuleDto rule = usecases.CreateRule(fields);

        return new SingleObjectModel(base.Request, rule);
      }
    }


    [HttpDelete]
    [Route("v3/financial-rules/{ruleUID:guid}")]
    public NoDataModel DeleteRule([FromUri] string ruleUID) {

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        _ = usecases.DeleteRule(ruleUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v3/financial-rules/{ruleUID:guid}")]
    public SingleObjectModel UpdateRule([FromUri] string ruleUID,
                                        [FromBody] FinancialRuleFields fields) {

      fields.UID = ruleUID;

      using (var usecases = FinancialRuleUseCases.UseCaseInteractor()) {

        FinancialRuleDto rule = usecases.UpdateRule(fields);

        return new SingleObjectModel(base.Request, rule);
      }
    }

    #endregion Command web apis

  }  // class FinancialRuleController

}  // namespace Empiria.Financial.Rules.WebApi
