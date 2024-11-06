/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Information Holder                      *
*  Type     : BudgetExplorerResult                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds the dynamic result of a budget explorer execution.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Holds the dynamic result of a budget explorer execution.</summary>
  internal class BudgetExplorerResult {

    public BudgetExplorerCommand Command {
      get;
      internal set;
    }

    public FixedList<DataTableColumn> Columns {
      get; internal set;
    }

    public FixedList<BudgetExplorerEntry> Entries {
      get; internal set;
    }

  }  // class BudgetExplorerResult

}  // namespace Empiria.Budgeting.Explorer
