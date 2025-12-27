/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Query DTO                               *
*  Type     : BudgetExplorerCommand                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Command information used by the BudgetExplorer.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Command information used by the BudgetExplorer.</summary>
  internal class BudgetExplorerCommand {

    internal Budget Budget {
      get; set;
    }

    internal FixedList<StandardAccountCategory> GroupBy {
      get; set;
    }

    internal FixedList<BudgetSegmentFilter> FilteredBy {
      get; set;
    }

  }  // class BudgetExplorerCommand



  /// <summary>Specifies a filter for budget segments.</summary>
  internal class BudgetSegmentFilter {

    internal StandardAccountCategory SegmentType {
      get; set;
    }

    internal FixedList<StandardAccount> SegmentItems {
      get; set;
    }

  }  // class BudgetSegmentFilter

}  // namespace Empiria.Budgeting.Explorer
