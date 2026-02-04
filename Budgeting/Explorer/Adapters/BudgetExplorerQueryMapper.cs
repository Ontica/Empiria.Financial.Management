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

using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Maps BudgetExplorerQuery DTO structures to BudgetExplorerCommand instances.</summary>
  static internal class BudgetExplorerQueryMapper {

    static internal BudgetExplorerCommand Map(BudgetExplorerQuery query) {
      return new BudgetExplorerCommand {
        Budget = Budget.Parse(query.BudgetUID),
        OrganizationalUnit = MapToOrganizationalUnit(query.BasePartyUID),
        GroupBy = MapToGroupBy(query.GroupBy),
        FilteredBy = MapToFilteredBy(query.FilteredBy),
      };
    }

    #region Helpers

    static private FixedList<BudgetSegmentFilter> MapToFilteredBy(BudgetSegmentQuery[] segmentFilter) {
      var list = new List<BudgetSegmentFilter>(segmentFilter.Length);

      foreach (var item in segmentFilter) {
        var filter = new BudgetSegmentFilter {
          SegmentType = StandardAccountCategory.Parse(item.SegmentTypeUID),
          SegmentItems = item.SegmentItems.Select(x => StandardAccount.Parse(x))
                                           .ToFixedList()
        };
      }

      return list.ToFixedList();
    }


    static private FixedList<StandardAccountCategory> MapToGroupBy(string[] segmentTypes) {
      var list = new List<StandardAccountCategory>(segmentTypes.Length);

      foreach (var segmentTypeUID in segmentTypes) {
        var segmentType = StandardAccountCategory.Parse(segmentTypeUID);

        list.Add(segmentType);
      }

      return list.ToFixedList();
    }


    static private OrganizationalUnit MapToOrganizationalUnit(string basePartyUID) {
      if (basePartyUID.Length == 0) {
        return OrganizationalUnit.Empty;
      }
      return OrganizationalUnit.Parse(basePartyUID);
    }

    #endregion Helpers

  }  // class BudgetExplorerQueryMapper

}  // namespace Empiria.Budgeting.Explorer.Adapters
