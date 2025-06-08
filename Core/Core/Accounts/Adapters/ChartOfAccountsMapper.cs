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
        Accounts = StandardAccountMapper.MapToDescriptor(chartOfAccounts.GetStandardAccounts())
      };
    }


    static internal ChartOfAccountsDto Map(ChartOfAccounts chartOfAccounts,
                                           FixedList<StandardAccount> stdAccounts) {
      return new ChartOfAccountsDto {
        UID = chartOfAccounts.UID,
        Name = chartOfAccounts.Name,
        Accounts = StandardAccountMapper.MapToDescriptor(stdAccounts)
      };
    }

  }  // class ChartOfAccountsMapper

}  // namespace Empiria.FinancialAccounting.Adapters
