/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Builder                                 *
*  Type     : BudgetExplorer                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Retrives budget information bases on a query returning a dynamic result data structure.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;
using Empiria.Parties;

using Empiria.Budgeting.Explorer.Data;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Retrives budget information bases on a query returning a dynamic result data structure.</summary>
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

      if (_command.GroupBy.Contains(x => x.Id == 3520)) {
        return UngroupedBudgetData(budgetData);
      }
      return GroupBudgetData(budgetData);
    }


    private FixedList<DataTableColumn> BuildColumns() {
      var columns = new List<DataTableColumn>();

      if (AREA) {
        columns.Add(new DataTableColumn("organizationalUnitName", "Área", "text"));
      } else if (AREA_CAPITULO) {
        columns.Add(new DataTableColumn("organizationalUnitName", "Área", "text"));
        columns.Add(new DataTableColumn("capitulo", "Capítulo", "text"));
      } else if (AREA_PARTIDA) {
        columns.Add(new DataTableColumn("organizationalUnitName", "Área", "text"));
        columns.Add(new DataTableColumn("budgetAccountName", "Partida", "text"));
      } else if (CAPITULO) {
        columns.Add(new DataTableColumn("capitulo", "Capítulo", "text"));
      } else if (CAPITULO_AREA) {
        columns.Add(new DataTableColumn("capitulo", "Capítulo", "text"));
        columns.Add(new DataTableColumn("organizationalUnitName", "Área", "text"));
      } else if (PARTIDA) {
        columns.Add(new DataTableColumn("budgetAccountName", "Partida", "text"));
      } else if (PARTIDA_AREA) {
        columns.Add(new DataTableColumn("budgetAccountName", "Partida", "text"));
        columns.Add(new DataTableColumn("organizationalUnitName", "Área", "text"));
      } else {
        columns.Add(new DataTableColumn("organizationalUnitName", "Área", "text"));
        columns.Add(new DataTableColumn("budgetAccountName", "Partida", "text"));
      }

      columns.Add(new DataTableColumn("planned", "Planeado", "decimal"));
      columns.Add(new DataTableColumn("authorized", "Autorizado", "decimal"));
      columns.Add(new DataTableColumn("expanded", "Ampliaciones", "decimal"));
      columns.Add(new DataTableColumn("reduced", "Reducciones", "decimal"));
      columns.Add(new DataTableColumn("modified", "Modificado", "decimal"));
      columns.Add(new DataTableColumn("requested", "Apartado", "decimal"));
      columns.Add(new DataTableColumn("commited", "Comprometido", "decimal"));
      columns.Add(new DataTableColumn("toPay", "Por pagar", "decimal"));
      columns.Add(new DataTableColumn("excercised", "Ejercido", "decimal"));
      columns.Add(new DataTableColumn("toExercise", "Por ejercer", "decimal"));
      columns.Add(new DataTableColumn("available", "Disponible", "decimal"));

      return columns.ToFixedList();
    }


    private FixedList<BudgetDataInColumns> GetBudgetData() {
      return BudgetExplorerDataService.GetBugetDataInMultipleColumns();
    }


    private FixedList<BudgetExplorerEntry> UngroupedBudgetData(FixedList<BudgetDataInColumns> budgetData) {
      return budgetData.Select(x => TransformToFullEntry(x))
                       .OrderBy(x => SortByFunction(x))
                       .ToFixedList();
    }


    private FixedList<BudgetExplorerEntry> GroupBudgetData(FixedList<BudgetDataInColumns> budgetData) {
      return budgetData.GroupBy(GroupByFunction())
                       .Select(x => TransformToEntry(x.ToFixedList()))
                       .OrderBy(x => SortByFunction(x))
                       .ToFixedList();
    }


    private string SortByFunction(BudgetExplorerEntry entry) {
      if (AREA) {
        return entry.BudgetAccount.Segment_1.Code;
      }
      if (AREA_CAPITULO) {
        return $"{entry.BudgetAccount.Segment_1.Code}{entry.BudgetAccount.Segment_2.Parent.Code}";
      }
      if (AREA_PARTIDA) {
        return $"{entry.BudgetAccount.Segment_1.Code}{entry.BudgetAccount.Segment_2.Code}";
      }
      if (CAPITULO) {
        return entry.BudgetAccount.Segment_2.Parent.Code;
      }
      if (CAPITULO_AREA) {
        return $"{entry.BudgetAccount.Segment_2.Parent.Code}{entry.BudgetAccount.Segment_1.Code}";
      }
      if (PARTIDA) {
        return entry.BudgetAccount.Segment_2.Code;
      }
      if (PARTIDA_AREA) {
        return $"{entry.BudgetAccount.Segment_2.Code}{entry.BudgetAccount.Segment_1.Code}";
      }

      return $"{entry.BudgetAccount.Segment_1.Code}{entry.BudgetAccount.Segment_2.Code}";
    }


    private Func<BudgetDataInColumns, object> GroupByFunction() {
      if (AREA) {
        return x => new { x.BudgetAccount.Segment_1 };
      }
      if (AREA_CAPITULO) {
        return x => new { x.BudgetAccount.Segment_1, x.BudgetAccount.Segment_2.Parent };
      }
      if (AREA_PARTIDA) {
        return x => new { x.BudgetAccount.Segment_1, x.BudgetAccount.Segment_2 };
      }
      if (CAPITULO) {
        return x => new { x.BudgetAccount.Segment_2.Parent };
      }
      if (CAPITULO_AREA) {
        return x => new { x.BudgetAccount.Segment_2.Parent, x.BudgetAccount.Segment_1 };
      }
      if (PARTIDA) {
        return x => new { x.BudgetAccount.Segment_2 };
      }
      if (PARTIDA_AREA) {
        return x => new { x.BudgetAccount.Segment_2, x.BudgetAccount.Segment_1 };
      }

      return x => new { x.BudgetAccount.Segment_1, x.BudgetAccount.Segment_2 };
    }


    private bool AREA {
      get {
        if (_command.GroupBy.Count == 1 && _command.GroupBy[0].Id == 3523) {
          return true;
        }
        return false;
      }
    }


    private bool AREA_PARTIDA {
      get {
        if (_command.GroupBy.Count == 2 && _command.GroupBy[0].Id == 3523 && _command.GroupBy[1].Id == 3526) {
          return true;
        }
        return false;
      }
    }


    private bool AREA_CAPITULO {
      get {
        if (_command.GroupBy.Count == 2 && _command.GroupBy[0].Id == 3523 && _command.GroupBy[1].Id == 3524) {
          return true;
        }
        return false;
      }
    }


    private bool CAPITULO {
      get {
        if (_command.GroupBy.Count == 1 && _command.GroupBy[0].Id == 3524) {
          return true;
        }
        return false;
      }
    }

    private bool PARTIDA {
      get {
        if (_command.GroupBy.Count == 1 && _command.GroupBy[0].Id == 3526) {
          return true;
        }
        return false;
      }
    }

    private bool PARTIDA_AREA {
      get {
        if (_command.GroupBy.Count == 2 && _command.GroupBy[0].Id == 3526 && _command.GroupBy[1].Id == 3523) {
          return true;
        }
        return false;
      }
    }


    private bool CAPITULO_AREA {
      get {
        if (_command.GroupBy.Count == 2 && _command.GroupBy[0].Id == 3524 && _command.GroupBy[1].Id == 3523) {
          return true;
        }
        return false;
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


    private BudgetExplorerEntry TransformToFullEntry(BudgetDataInColumns sourceData) {
      return new BudgetExplorerEntry(sourceData, true);
    }

    #endregion Helpers

  }  // class BudgetExplorer

}  // namespace Empiria.Budgeting.Explorer
