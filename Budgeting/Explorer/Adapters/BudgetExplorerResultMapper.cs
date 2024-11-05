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

    static internal BudgetExplorerResultDto Map(BudgetExplorerResult result) {
      return new BudgetExplorerResultDto {
        Query = result.Query,
        Columns = result.Columns,
        Entries = Map(result.Entries)
      };
    }

    #region Helpers

    static private FixedList<DynamicBudgetExplorerEntryDto> Map(FixedList<BudgetDataInColumns> entries) {
      return entries.Select(x => Map(x))
                    .ToFixedList();
    }

    static private DynamicBudgetExplorerEntryDto Map(BudgetDataInColumns entry) {
      return new DynamicBudgetExplorerEntryDto {
        Authorized = entry.Authorized,
        Available = entry.Available,
        BudgetAccount = entry.BudgetAccount.Name,
        Commited = entry.Commited,
        Currency = entry.Currency.ISOCode,
        Excercised = entry.Excercised,
        Expanded = entry.Expanded,
        Modified = entry.Modified,
        Month = entry.Month,
        Planned = entry.Planned,
        Reduced = entry.Reduced,
        ToExercise = entry.ToExercise,
        ToPay = entry.ToPay,
        Year = entry.Year,
      };
    }

    #endregion Helpers

  }  // class BudgetExplorerResultMapper

}  // namespace Empiria.Budgeting.Explorer.Adapters
