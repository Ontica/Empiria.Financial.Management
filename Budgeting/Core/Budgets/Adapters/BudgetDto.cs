/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Output DTO                              *
*  Type     : BudgetDto                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for Budget instances.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Output DTO for Budget instances.</summary>
  public class BudgetDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public NamedEntityDto Type {
      get; internal set;
    }

    public FixedList<NamedEntityDto> TransactionTypes {
      get; internal set;
    }

  }  // class BudgetDto

}  // namespace Empiria.Budgeting.Adapters
