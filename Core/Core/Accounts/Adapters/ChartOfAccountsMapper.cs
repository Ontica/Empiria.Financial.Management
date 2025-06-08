/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Mapper class                            *
*  Type     : AccountsChartMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for accounts charts and their contents.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Adapters {

  /// <summary>Mapping methods for accounts charts and their contents.</summary>
  static public class ChartOfAccountsMapper {

    static internal ChartOfAccountsDto Map(ChartOfAccounts chartOfAccounts) {
      return new ChartOfAccountsDto {
        UID = chartOfAccounts.UID,
        Name = chartOfAccounts.Name,
        Accounts = MapToStdAccountDescriptor(chartOfAccounts.GetStandardAccounts())
      };
    }


    static internal ChartOfAccountsDto Map(ChartOfAccounts chartOfAccounts,
                                           FixedList<StandardAccount> stdAccounts) {
      return new ChartOfAccountsDto {
        UID = chartOfAccounts.UID,
        Name = chartOfAccounts.Name,
        Accounts = MapToStdAccountDescriptor(stdAccounts)
      };
    }

    #region Helpers

    static private StandardAccountDescriptor MapToStdAccountDescriptor(StandardAccount account) {
      return new StandardAccountDescriptor {
        UID = account.UID,
        Description = account.Description,
        FullName = account.FullName,
        Number = account.StdAcctNo,
        TypeName = account.Category.Name,
        RoleType = account.RoleType,
        DebtorCreditorType = account.DebtorCreditorType,
        Level = account.Level,
        IsLastLevel = account.IsLastLevel,
        StatusName = account.Status.GetName(),
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        Obsolete = false,
      };
    }

    static private FixedList<StandardAccountDescriptor> MapToStdAccountDescriptor(FixedList<StandardAccount> list) {
      return list.Select((x) => MapToStdAccountDescriptor(x))
                 .ToFixedList();
    }

    #endregion Helpers

  }  // class AccountsChartMapper

}  // namespace Empiria.FinancialAccounting.Adapters
