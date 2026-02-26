/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Builder                                 *
*  Type     : BudgetExplorer                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Retrieves budget information bases on a query returning a dynamic result data structure.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;

using Empiria.Budgeting.Explorer.Data;

namespace Empiria.Budgeting.Explorer {

  public enum BudgetExplorerGroupBy {

    AREA_PARTIDA,

    PARTIDA

  } // enum BudgetExplorerGroupBy



  /// <summary>Retrieves budget information bases on a query returning a dynamic result data structure.</summary>
  internal class BudgetExplorer {

    private readonly BudgetExplorerCommand _command;

    internal BudgetExplorer(BudgetExplorerCommand query) {
      Assertion.Require(query, nameof(query));

      _command = query;
    }

    internal BudgetExplorerResult Execute() {

      return new BudgetExplorerResult {
        Command = _command,
        Columns = BuildColumns(),
        Entries = BuildEntries()
      };
    }

    #region Helpers

    private FixedList<BudgetExplorerEntry> BuildEntries() {
      FixedList<BudgetDataInColumns> budgetData = GetBudgetData();

      budgetData = FilterBudgetData(budgetData);

      return GroupBudgetData(budgetData);
    }

    private FixedList<DataTableColumn> BuildColumns() {
      var columns = new List<DataTableColumn> {
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
      };

      if (_command.GroupBy == BudgetExplorerGroupBy.AREA_PARTIDA) {
        columns.Insert(0, new DataTableColumn("organizationalUnitName", "Área", "string"));
        columns.Insert(1, new DataTableColumn("budgetAccountName", "Partida", "string"));

        if (_command.ReportType == Adapters.BudgetReportType.SaldosOperacion) {
          columns.Insert(2, new DataTableColumn("monthName", "Mes", "text"));
        }

      } else if (_command.GroupBy == BudgetExplorerGroupBy.PARTIDA) {
        columns.Insert(0, new DataTableColumn("budgetAccountName", "Partida", "string"));

        if (_command.ReportType == Adapters.BudgetReportType.SaldosOperacion) {
          columns.Insert(1, new DataTableColumn("monthName", "Mes", "text"));
        }
      }

      return columns.ToFixedList();
    }


    private FixedList<BudgetDataInColumns> FilterBudgetData(FixedList<BudgetDataInColumns> budgetData) {
      if (_command.OrganizationalUnits.Count == 0) {
        return budgetData;

      } else {
        return budgetData.FindAll(x => _command.OrganizationalUnits.Contains(x.BudgetAccount.OrganizationalUnit));
      }
    }


    private FixedList<BudgetDataInColumns> GetBudgetData() {
      switch (_command.ReportType) {
        case Adapters.BudgetReportType.ByColumn:
          return BudgetExplorerDataService.GetBudgetDataInMultipleColumns(_command.Budget);
        case Adapters.BudgetReportType.SaldosOperacion:
          return BudgetExplorerDataService.GetBudgetDataInMultipleColumnsByMonth(_command.Budget);
        default:
          return BudgetExplorerDataService.GetBudgetDataInMultipleColumns(_command.Budget);
      }
    }


    private FixedList<BudgetExplorerEntry> GroupBudgetData(FixedList<BudgetDataInColumns> budgetData) {
      return budgetData.GroupBy(GroupByFunction())
                       .Select(x => TransformToEntry(x.ToFixedList()))
                       .OrderBy(x => SortByFunction(x))
                       .ToFixedList();
    }


    private string SortByFunction(BudgetExplorerEntry entry) {
      switch (_command.GroupBy) {
        case BudgetExplorerGroupBy.AREA_PARTIDA:
          return $"{entry.BudgetAccount.OrganizationalUnit.Code}{entry.BudgetAccount.Code}{entry.Month:00}";
        case BudgetExplorerGroupBy.PARTIDA:
          return $"{entry.BudgetAccount.Code}{entry.Month:00}";

        default:
          return $"{entry.BudgetAccount.OrganizationalUnit.Code}{entry.BudgetAccount.Code}{entry.Month:00}";
      }
    }


    private Func<BudgetDataInColumns, object> GroupByFunction() {

      if (_command.ReportType == Adapters.BudgetReportType.SaldosOperacion) {

        switch (_command.GroupBy) {

          case BudgetExplorerGroupBy.AREA_PARTIDA:
            return x => new { x.Month, x.BudgetAccount.OrganizationalUnit, x.BudgetAccount.StandardAccount };

          case BudgetExplorerGroupBy.PARTIDA:
            return x => new { x.Month, x.BudgetAccount.StandardAccount };

          default:
            return x => new { x.Month, x.BudgetAccount.OrganizationalUnit, x.BudgetAccount.StandardAccount };
        }
      }

      switch (_command.GroupBy) {
        case BudgetExplorerGroupBy.AREA_PARTIDA:
          return x => new { x.BudgetAccount.OrganizationalUnit, x.BudgetAccount.StandardAccount };

        case BudgetExplorerGroupBy.PARTIDA:
          return x => new { x.BudgetAccount.StandardAccount };

        default:
          return x => new { x.BudgetAccount.OrganizationalUnit, x.BudgetAccount.StandardAccount };
      }

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

  }  // class BudgetExplorer

}  // namespace Empiria.Budgeting.Explorer
