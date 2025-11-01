/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialRuleUseCases                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial rules.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;
using Empiria.Services;

using Empiria.Financial.Rules.Adapters;

namespace Empiria.Financial.Rules.UseCases {

  /// <summary>Provides use cases for update and retrieve financial rules.</summary>
  public class FinancialRuleUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialRuleUseCases() {
      // no-op
    }

    static public FinancialRuleUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialRuleUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public FinancialRuleDto CreateRule(FinancialRuleFields fields) {
      Assertion.Require(fields, nameof(fields));

      var category = FinancialRuleCategory.Parse(fields.CategoryUID);

      FinancialRule rule = category.AddRule(fields);

      rule.Save();

      return FinancialRuleMapper.Map(rule);
    }


    public FinancialRuleDto DeleteRule(string ruleUID) {
      Assertion.Require(ruleUID, nameof(ruleUID));

      var rule = FinancialRule.Parse(ruleUID);

      rule.Category.RemoveRule(rule);

      rule.Save();

      return FinancialRuleMapper.Map(rule);
    }


    public FixedList<NamedEntityDto> GetCategories() {
      return FinancialRuleCategory.GetList()
                                  .MapToNamedEntityList();
    }


    public FinancialRuleDto GetRule(string ruleUID) {
      Assertion.Require(ruleUID, nameof(ruleUID));

      var rule = FinancialRule.Parse(ruleUID);

      return FinancialRuleMapper.Map(rule);
    }


    public DynamicDto<FinancialRuleDescriptor> SearchRules(FinancialRuleQuery query) {
      Assertion.Require(query, nameof(query));
      Assertion.Require(query.CategoryUID, nameof(query.CategoryUID));

      var category = FinancialRuleCategory.Parse(query.CategoryUID);

      FixedList<FinancialRule> rules = query.Execute();

      return FinancialRuleMapper.Map(category, rules);
    }


    public FinancialRuleDto UpdateRule(FinancialRuleFields fields) {
      Assertion.Require(fields, nameof(fields));

      var rule = FinancialRule.Parse(fields.UID);

      rule.Category.UpdateRule(rule, fields);

      rule.Save();

      return FinancialRuleMapper.Map(rule);
    }

    #endregion Use cases

  }  // class FinancialRuleUseCases

}  // namespace Empiria.Financial.Rules.UseCases
