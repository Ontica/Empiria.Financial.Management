/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Interface adapters                      *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ChartOfAccountsMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for charts of accounts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Mapping methods for charts of accounts.</summary>
  static public class ChartOfAccountsMapper {

    static internal ChartOfAccountsDto Map(ChartOfAccounts chartOfAccounts) {
      return new ChartOfAccountsDto {
        UID = chartOfAccounts.UID,
        Name = chartOfAccounts.Name,
        AccountTypes = chartOfAccounts.FinancialAccountTypes.MapToNamedEntityList(),
        Accounts = StandardAccountMapper.MapToDescriptor(chartOfAccounts.GetStandardAccounts())
      };
    }


    static internal ChartOfAccountsDto Map(ChartOfAccounts chartOfAccounts,
                                           FixedList<StandardAccount> stdAccounts) {
      return new ChartOfAccountsDto {
        UID = chartOfAccounts.UID,
        Name = chartOfAccounts.Name,
        AccountTypes = chartOfAccounts.FinancialAccountTypes.MapToNamedEntityList(),
        Accounts = StandardAccountMapper.MapToDescriptor(stdAccounts)
      };
    }


    static internal FixedList<ChartOfAccountsDefinitionDto> Map(FixedList<ChartOfAccounts> chartsOfAccounts) {
      return chartsOfAccounts.Select(x => MapToDefinition(x))
                             .ToFixedList();

    }


    static private ChartOfAccountsDefinitionDto MapToDefinition(ChartOfAccounts chartOfAccounts) {
      AutoGrouping autoGroupingConfig = chartOfAccounts.AutoGrouping;

      return new ChartOfAccountsDefinitionDto {
        UID = chartOfAccounts.UID,
        Name = chartOfAccounts.Name,
        AccountTypes = chartOfAccounts.FinancialAccountTypes.MapToNamedEntityList(),
        AutoGrouping = new AutoGroupingDto {
          Applies = !autoGroupingConfig.IsEmptyInstance,
          FinancialConceptGroup = autoGroupingConfig.FinancialConceptGroup.MapToNamedEntity(),
          StandardAccountCategories = autoGroupingConfig.StandardAccountCategories.MapToNamedEntityList(),
        },
        ShowAccounts = chartOfAccounts.ShowAccounts,
        ShowOrgUnits = chartOfAccounts.ShowOrgUnits,
      };
    }

  }  // class ChartOfAccountsMapper

}  // namespace Empiria.Financial.Adapters
