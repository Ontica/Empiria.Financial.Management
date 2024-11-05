/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Dynamic Columns Output DTO              *
*  Type     : BudgetExplorerResultDto                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic columns output DTO with budget information.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Dynamic columns output DTO with budget information.</summary>
  public class BudgetExplorerResultDto {

    public BudgetExplorerQuery Query {
      get; internal set;
    }

    public FixedList<DataTableColumn> Columns {
      get; internal set;
    }

    public FixedList<DynamicBudgetExplorerEntryDto> Entries {
      get; internal set;
    } = new FixedList<DynamicBudgetExplorerEntryDto>();


  }  // class BudgetExplorerResultDto

}  // namespace Empiria.Budgeting.Explorer.Adapters
