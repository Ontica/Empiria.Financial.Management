/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetAccountMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for budget accounts.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.StateEnums;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Mapping methods for budget accounts.</summary>
  static internal class BudgetAccountMapper {

    static internal FixedList<BudgetAccountDto> Map(FixedList<BudgetAccount> accounts) {
      return accounts.Select(x => Map(x))
                     .ToFixedList();
    }


    static internal FixedList<BudgetAccountDto> Map(FixedList<BudgetAccount> accounts,
                                                    FixedList<BudgetAccountSegment> unassignedSegments) {
      FixedList<BudgetAccountDto> mappedAccounts = accounts.Select(x => Map(x))
                                                           .ToFixedList();

      FixedList<BudgetAccountDto> mappedSegments = unassignedSegments.Select(x => Map(x))
                                                                     .ToFixedList();

      return mappedAccounts.Concat(mappedSegments)
                           .ToFixedList()
                           .Sort((x, y) => x.Code.CompareTo(y.Code));
    }

    #region Helpers

    static private BudgetAccountDto Map(BudgetAccount account) {
      return new BudgetAccountDto {
        UID = account.UID,
        BaseSegmentUID = account.BaseSegment.UID,
        Code = account.Code,
        Name = $"[{account.Code}] {account.Name} (No asignada)",
        Type = account.BudgetAccountType.MapToNamedEntity(),
        OrganizationalUnit = account.OrganizationalUnit.MapToNamedEntity(),
        Status = account.Status.MapToDto(),
        IsAssigned = true
      };
    }


    static private BudgetAccountDto Map(BudgetAccountSegment segment) {
      return new BudgetAccountDto {
        UID = string.Empty,
        BaseSegmentUID = segment.UID,
        Code = segment.Code,
        Name = segment.Name,
        Type = segment.BudgetSegmentType.MapToNamedEntity(),
        OrganizationalUnit = NamedEntityDto.Empty,
        Status = segment.Status.MapToDto(),
        IsAssigned = false
      };
    }

    #endregion Helpers

  }  // class BudgetAccountMapper

}  // namespace Empiria.Budgeting.Adapters
