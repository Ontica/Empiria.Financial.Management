/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Builder                                 *
*  Type     : BudgetBreakdown                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Builds budget information for a budget account break down by months.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;
using Empiria.Parties;

using Empiria.Budgeting.Explorer.Data;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Builds budget information for a budget account break down by months.</summary>
  internal class BudgetBreakdown {

    private readonly BudgetExplorerCommand _command;

    internal BudgetBreakdown(BudgetExplorerCommand query) {
      Assertion.Require(query, nameof(query));

      _command = query;
    }


    internal BudgetExplorerResult Execute(OrganizationalUnit orgUnit,
                                          BudgetAccount budgetAccount) {
      return new BudgetExplorerResult {
        Command = _command,
        Columns = BuildColumns(),
        Entries = BuildEntries(orgUnit, budgetAccount)
      };
    }


    #region Helpers

    private FixedList<BudgetExplorerEntry> BuildEntries(OrganizationalUnit orgUnit, BudgetAccount budgetAccount) {
      FixedList<BudgetDataInColumns> budgetData = GetBudgetData()
                                                 .FindAll(x => x.BudgetAccount.Equals(budgetAccount) &&
                                                               x.BudgetAccount.OrganizationalUnit.Equals(orgUnit));

      return GroupBudgetData(budgetData);
    }


    private FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn> {
        new DataTableColumn("monthName", "Mes", "text"),
        new DataTableColumn("planned", "Planeado", "decimal"),
        new DataTableColumn("authorized", "Autorizado", "decimal"),
        new DataTableColumn("expanded", "Ampliaciones", "decimal"),
        new DataTableColumn("reduced", "Reducciones", "decimal"),
        new DataTableColumn("modified", "Modificado", "decimal"),
        new DataTableColumn("requested", "Apartado", "decimal"),
        new DataTableColumn("commited", "Comprometido", "decimal"),
        new DataTableColumn("toPay", "Por pagar", "decimal"),
        new DataTableColumn("excercised", "Ejercido", "decimal"),
        new DataTableColumn("toExercise", "Por ejercer", "decimal"),
        new DataTableColumn("available", "Disponible", "decimal")
      }.ToFixedList();
    }


    private FixedList<BudgetDataInColumns> GetBudgetData() {
      return BudgetExplorerDataService.GetBudgetDataInMultipleColumnsByMonth(_command.Budget);
    }


    private FixedList<BudgetExplorerEntry> GroupBudgetData(FixedList<BudgetDataInColumns> budgetData) {
      return budgetData.GroupBy(x => x.Month)
                       .Select(x => TransformToEntry(x.ToFixedList()))
                       .OrderBy(x => x.Month)
                       .ToFixedList();
    }


    private BudgetExplorerEntry TransformToEntry(FixedList<BudgetDataInColumns> groupedEntries) {
      BudgetDataInColumns baseData = groupedEntries[0];

      var entry = new BudgetExplorerEntry(baseData, true);

      for (int i = 1; i < groupedEntries.Count; i++) {
        BudgetExplorerEntry sourceDataAsEntry = TransformToEntry(groupedEntries[i]);

        entry.Sum(sourceDataAsEntry);
      }

      return entry;
    }

    private BudgetExplorerEntry TransformToEntry(BudgetDataInColumns sourceData) {
      return new BudgetExplorerEntry(sourceData, false);
    }

    #endregion Helpers

  }  // class BudgetBreakdown

}  // namespace Empiria.Budgeting.Explorer
