/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Mapper                                  *
*  Type     : BudgetExplorerQueryMapper                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetExplorerQuery DTO structures to BudgetExplorerCommand instances.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Maps BudgetExplorerQuery DTO structures to BudgetExplorerCommand instances.</summary>
  static internal class BudgetExplorerQueryMapper {

    static internal BudgetExplorerCommand Map(BudgetExplorerQuery query) {
      return new BudgetExplorerCommand {
        Budget = Budget.Parse(query.BudgetUID),
        GroupBy = MapToGroupBy(query.GroupBy),
        FilteredBy = MapToFilteredBy(query.FilteredBy),
      };
    }

    #region Helpers

    static private FixedList<BudgetSegmentFilter> MapToFilteredBy(BudgetSegmentQuery[] segmentFilter) {
      var list = new List<BudgetSegmentFilter>(segmentFilter.Length);

      foreach (var item in segmentFilter) {
        var filter = new BudgetSegmentFilter {
           SegmentType = BudgetAccountSegmentType.Parse(item.SegmentTypeUID),
           SegmentItems = item.SegmentItems.Select(x => BudgetAccountSegment.Parse(x))
                                           .ToFixedList()
        };
      }

      return list.ToFixedList();
    }


    static private FixedList<BudgetAccountSegmentType> MapToGroupBy(string[] segmentTypes) {
      var list = new List<BudgetAccountSegmentType>(segmentTypes.Length);

      foreach (var segmentTypeUID in segmentTypes) {
        var segmentType = BudgetAccountSegmentType.Parse(segmentTypeUID);

        list.Add(segmentType);
      }

      return list.ToFixedList();
    }

    #endregion Helpers

  }  // class BudgetExplorerQueryMapper

}  // namespace Empiria.Budgeting.Explorer.Adapters
