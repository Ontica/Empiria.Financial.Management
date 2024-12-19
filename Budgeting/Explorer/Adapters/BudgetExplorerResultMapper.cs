/* Empiria Financial *****************************************************************************************
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
      return new DynamicBudgetExplorerEntryDto(entry);
    }

    #endregion Helpers

  }  // class BudgetExplorerResultMapper

}  // namespace Empiria.Budgeting.Explorer.Adapters
