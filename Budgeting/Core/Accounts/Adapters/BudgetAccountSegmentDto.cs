/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Output DTO                              *
*  Type     : BudgetAccountSegmentDto                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for a budget account segment.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Output DTO for a budget account segment.</summary>
  public class BudgetAccountSegmentDto {

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

    public BudgetAccountSegmentDto Parent {
      get; internal set;
    }

    public FixedList<BudgetAccountSegmentDto> Children {
      get; internal set;
    }

  }  // class BudgetAccountSegmentDto

}  // namespace Empiria.Budgeting.Adapters
