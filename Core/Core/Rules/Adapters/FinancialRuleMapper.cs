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
using Empiria.StateEnums;

namespace Empiria.Financial.Rules.Adapters {

  /// <summary>Provides financial rules mapping methods.</summary>
  static internal class FinancialRuleMapper {

    static public DynamicDto<FinancialRuleDescriptor> Map(FinancialRuleCategory category,
                                                   FixedList<FinancialRule> rules) {

      rules.Sort((x, y) => x.DebitAccount.CompareTo(y.DebitAccount));

      var dtos = rules.Select(rule => new FinancialRuleDescriptor {
        UID = rule.UID,
        DebitAccount = rule.DebitAccount,
        CreditAccount = rule.CreditAccount,
        DebitConcept = rule.DebitConcept,
        CreditConcept = rule.CreditConcept,
        Description = rule.Description,
        StartDate = rule.StartDate,
        EndDate = rule.EndDate,
        StatusName = rule.Status.GetName()
      }).ToFixedList();

      return new DynamicDto<FinancialRuleDescriptor>(category.GetDataColumns(), dtos);
    }


    static internal FinancialRuleDto Map(FinancialRule rule) {
      return new FinancialRuleDto {
        UID = rule.UID,
        CategoryUID = rule.Category.UID,
        DebitAccount = rule.DebitAccount,
        CreditAccount = rule.CreditAccount,
        DebitConcept = rule.DebitConcept,
        CreditConcept = rule.CreditConcept,
        Description = rule.Description,
        StartDate = rule.StartDate,
        EndDate = rule.EndDate,
        Status = rule.Status.MapToDto()
      };
    }

  }  // class FinancialRuleMapper

}  // namespace Empiria.Financial.Rules.Adapters
