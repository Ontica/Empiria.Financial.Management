/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Query DTO                               *
*  Type     : BudgetExplorerQuery                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve budget information.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Input query DTO used to retrieve budget information.</summary>
  public class BudgetExplorerQuery {

    public string UID {
      get; set;
    }

    public string Code {
      get; set;
    }

    public string Name {
      get; set;
    }

    public string Description {
      get; set;
    }

    public BudgetSegmentTypeDto Type {
      get; set;
    }

    public BudgetAccountSegmentDto Parent {
      get; set;
    }

    public FixedList<BudgetAccountSegmentDto> Children {
      get; set;
    }

  }  // class BudgetExplorerQuery

}  // namespace Empiria.Budgeting.Adapters
