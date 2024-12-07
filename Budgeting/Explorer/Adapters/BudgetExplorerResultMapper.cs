﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Mapper                                  *
*  Type     : BudgetExplorerResultMapper                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps a BudgetExplorerResult instance to its output DTO.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Maps a BudgetExplorerResult instance to its output DTO.</summary>
  static internal class BudgetExplorerResultMapper {

    static internal BudgetExplorerResultDto Map(BudgetExplorerQuery query, BudgetExplorerResult result) {
      Assertion.Require(query, nameof(query));
      Assertion.Require(result, nameof(result));

      return new BudgetExplorerResultDto {
        Query = query,
        Columns = result.Columns,
        Entries = Map(result.Entries)
      };
    }

    #region Helpers

    static private FixedList<DynamicBudgetExplorerEntryDto> Map(FixedList<BudgetExplorerEntry> entries) {
      return entries.Select(x => Map(x))
                    .ToFixedList();
    }

    static private DynamicBudgetExplorerEntryDto Map(BudgetExplorerEntry entry) {
      return new DynamicBudgetExplorerEntryDto {
        OrganizationalUnitName = entry.OrganizationalUnit.FullName,
        BudgetAccountName = entry.BudgetAccount.Name,
        Capitulo = entry.BudgetAccount.Segment_2.Parent.FullName,
        Year = entry.Year,
        Month = entry.Month,
        CurrencyCode = entry.Currency.ISOCode,
        Planned = entry.Planned,
        Authorized = entry.Authorized,
        Expanded = entry.Expanded,
        Reduced = entry.Reduced,
        Modified = entry.Modified,
        Available = entry.Available,
        Commited = entry.Commited,
        ToPay = entry.ToPay,
        Excercised = entry.Excercised,
        ToExercise = entry.ToExercise
      };
    }

    #endregion Helpers

  }  // class BudgetExplorerResultMapper

}  // namespace Empiria.Budgeting.Explorer.Adapters
