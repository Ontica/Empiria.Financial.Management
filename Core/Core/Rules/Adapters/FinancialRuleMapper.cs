/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Mapper                                *
*  Type     : FinancialRuleMapper                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial rules.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

namespace Empiria.Financial.Rules.Adapters {

  /// <summary>Provides financial rules mapping methods.</summary>
  static internal class FinancialRuleMapper {

    static public DynamicDto<FinancialRuleDto> Map(FinancialRuleCategory category,
                                                   FixedList<FinancialRule> rules) {

      rules.Sort((x, y) => x.DebitAccount.CompareTo(y.DebitAccount));

      var dtos = rules.Select(rule => new FinancialRuleDto {
        UID = rule.UID,
        Description = rule.Description,
        DebitAccount = rule.DebitAccount,
        CreditAccount = rule.CreditAccount,
        DebitConcept = rule.DebitConcept,
        CreditConcept = rule.CreditConcept,
      }).ToFixedList();

      return new DynamicDto<FinancialRuleDto>(category.GetDataColumns(), dtos);
    }

  }  // class FinancialRuleMapper

}  // namespace Empiria.Financial.Rules.Adapters
