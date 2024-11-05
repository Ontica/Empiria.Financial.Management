/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Output DTO                              *
*  Type     : DynamicBudgetExplorerEntryDto              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with budget explorer entry information.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Output DTO with budget explorer entry information.</summary>
  public class DynamicBudgetExplorerEntryDto {

    public int Year {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public string BudgetAccount {
      get; internal set;
    }

    public string Currency {
      get; internal set;
    }

    public decimal Planned {
      get; internal set;
    }

    public decimal Authorized {
      get; internal set;
    }

    public decimal Expanded {
      get; internal set;
    }

    public decimal Reduced {
      get; internal set;
    }

    public decimal Modified {
      get; internal set;
    }

    public decimal Available {
      get; internal set;
    }

    public decimal Commited {
      get; internal set;
    }

    public decimal ToPay {
      get; internal set;
    }

    public decimal Excercised {
      get; internal set;
    }

    public decimal ToExercise {
      get; internal set;
    }

  } // class DynamicBudgetExplorerEntryDto

}  // namespace Empiria.Budgeting.Explorer.Adapters
