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
  public class DynamicBudgetExplorerEntryDto : BudgetExplorerEntry {

    public string OrganizationalUnitName {
      get; internal set;
    }

    public string BudgetAccountName {
      get; internal set;
    }

    public string CurrencyCode {
      get; internal set;
    }

    public string Capitulo {
      get;
      internal set;
    }
  } // class DynamicBudgetExplorerEntryDto

}  // namespace Empiria.Budgeting.Explorer.Adapters
