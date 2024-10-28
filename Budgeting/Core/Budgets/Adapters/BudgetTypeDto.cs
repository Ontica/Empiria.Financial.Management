/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Output DTO                              *
*  Type     : BudgetTypeDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for BudgetType instances.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Output DTO for BudgetType instances.</summary>
  public class BudgetTypeDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<BudgetDto> Budgets {
      get; internal set;
    }

    public FixedList<BudgetSegmentTypeDto> SegmentTypes {
      get; internal set;
    }

    public FixedList<NamedEntityDto> TransactionTypes {
      get; internal set;
    }

  }  // class BudgetTypeDto

}  // namespace Empiria.Budgeting.Adapters
