/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Mapper                                  *
*  Type     : BudgetExplorerQueryMapper                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetExplorerQuery DTO structures to BudgetExplorerCommand instances.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Maps BudgetExplorerQuery DTO structures to BudgetExplorerCommand instances.</summary>
  static internal class BudgetExplorerQueryMapper {

    static internal BudgetExplorerCommand Map(BudgetExplorerQuery query) {
      return new BudgetExplorerCommand {
        ReportType = query.ReportType,
        Budget = Budget.Parse(query.BudgetUID),
        OrganizationalUnits = MapToOrganizationalUnits(query.BaseParties),
        BudgetAccounts = query.BudgetAccounts,
        GroupBy = query.GroupByColumn
      };
    }

    #region Helpers

    static private FixedList<OrganizationalUnit> MapToOrganizationalUnits(string[] baseParties) {
      return baseParties.ToFixedList()
                        .Select(x => OrganizationalUnit.Parse(x))
                        .ToFixedList();
    }

    #endregion Helpers

  }  // class BudgetExplorerQueryMapper

}  // namespace Empiria.Budgeting.Explorer.Adapters
