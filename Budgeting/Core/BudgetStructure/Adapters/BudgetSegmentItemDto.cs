/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Output DTO                              *
*  Type     : BudgetSegmentItemDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for budget segment items.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Output DTO for budget segment items.</summary>
  public class BudgetSegmentItemDto {

    public string UID {
      get; internal set;
    }

    public string Code {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public BudgetSegmentTypeDto Type {
      get; internal set;
    }

    public BudgetSegmentItemDto Parent {
      get; internal set;
    }

    public FixedList<BudgetSegmentItemDto> Children {
      get; internal set;
    }

  }  // class BudgetSegmentItemDto

}  // namespace Empiria.Budgeting.Adapters
