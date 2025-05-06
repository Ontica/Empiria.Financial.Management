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

    public DynamicBudgetExplorerEntryDto(BudgetExplorerEntry entry) : base(entry) {
      OrganizationalUnitName = entry.OrganizationalUnit.FullName;
      BudgetAccountName = entry.BudgetAccount.Name;
      Capitulo = entry.BudgetAccount.BaseSegment.Parent.FullName;
      CurrencyCode = entry.Currency.ISOCode;
    }

    public string OrganizationalUnitName {
      get; private set;
    }

    public string BudgetAccountName {
      get; private set;
    }

    public string CurrencyCode {
      get; private set;
    }

    public string Capitulo {
      get; private set;
    }

  } // class DynamicBudgetExplorerEntryDto

}  // namespace Empiria.Budgeting.Explorer.Adapters
