/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Output DTO                              *
*  Type     : BudgetEntryDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used for budget entries.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Output DTO used for budget entries.</summary>
  public class BudgetEntryDto {

    public string UID {
      get; internal set;
    }

    public BudgetEntryDtoType EntryType {
      get;
    } = BudgetEntryDtoType.Monthly;


    public string TransactionUID {
      get; internal set;
    }

    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public NamedEntityDto ProductUnit {
      get; internal set;
    }

    public decimal ProductQty {
      get; set;
    }

    public NamedEntityDto Project {
      get; internal set;
    }

    public NamedEntityDto Party {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public NamedEntityDto Month {
      get; internal set;
    }

    public int Day {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public NamedEntityDto BalanceColumn {
      get; internal set;
    }

    public decimal RequestedAmount {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

    public decimal ExchangeRate {
      get; internal set;
    } = 1m;

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // BudgetEntryDto



  /// <summary>Output DTO used to display budget entries in a list.</summary>
  public class BudgetEntryDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string BudgetAccountCode {
      get; internal set;
    }

    public string BudgetAccountName {
      get; internal set;
    }

    public string Program {
      get; internal set;
    }

    public string ProductCode {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string ControlNo {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public string MonthName {
      get; internal set;
    }

    public int Day {
      get; internal set;
    }

    public string BalanceColumn {
      get; internal set;
    }

    public decimal Deposit {
      get; internal set;
    }

    public decimal Withdrawal {
      get; internal set;
    }

    public string ItemType {
      get; internal set;
    }

  }  // BudgetEntryDescriptorDto

}  // namespace Empiria.Budgeting.Transactions.Adapters
