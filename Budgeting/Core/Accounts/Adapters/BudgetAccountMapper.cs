/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetAccountMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for budget accounts.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Mapping methods for budget accounts.</summary>
  static internal class BudgetAccountMapper {

    static internal FixedList<BudgetAccountDto> Map(FixedList<BudgetAccount> accounts) {

      return accounts.Select(x => Map(x))
                     .ToFixedList();
    }


    static internal FixedList<BudgetAccountDto> Map(FixedList<BudgetAccount> currentAccounts,
                                                    FixedList<StandardAccount> availableAccounts,
                                                    OrganizationalUnit orgUnit) {

      FixedList<BudgetAccountDto> current = currentAccounts.Select(x => Map(x))
                                                          .ToFixedList();

      FixedList<BudgetAccountDto> available = availableAccounts.Select(x => Map(x, orgUnit))
                                                               .ToFixedList();

      return FixedList<BudgetAccountDto>.Merge(current, available)
                                        .Sort((x, y) => x.Code.CompareTo(y.Code));
    }

    #region Helpers

    static private BudgetAccountDto Map(BudgetAccount account) {
      return new BudgetAccountDto {
        UID = account.UID,
        BaseSegmentUID = account.StandardAccount.UID,
        Code = account.Code,
        Name = account.Name,
        Type = account.BudgetAccountType.MapToNamedEntity(),
        OrganizationalUnit = account.OrganizationalUnit.MapToNamedEntity(),
        Status = account.Status.MapToDto(),
        IsAssigned = true
      };
    }


    static private BudgetAccountDto Map(StandardAccount account, OrganizationalUnit orgUnit) {
      return new BudgetAccountDto {
        UID = string.Empty,
        BaseSegmentUID = account.UID,
        Code = account.StdAcctNo,
        Name = $"{account.Name} (No utilizada)",
        Type = account.StandardAccountType.MapToNamedEntity(),
        OrganizationalUnit = orgUnit.MapToNamedEntity(),
        Status = account.Status.MapToDto(),
        IsAssigned = false
      };
    }

    #endregion Helpers

  }  // class BudgetAccountMapper

}  // namespace Empiria.Budgeting.Adapters
