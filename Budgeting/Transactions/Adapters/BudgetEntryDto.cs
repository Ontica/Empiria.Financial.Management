/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Output DTO                              *
*  Type     : BudgetEntryDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used for budget entries.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Output DTO used for budget entries.</summary>
  public class BudgetEntryDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto BudgetType {
      get; internal set;
    }

    public NamedEntityDto TransactionType {
      get; internal set;
    }

    public string TransactionNo {
      get; internal set;
    }

    public NamedEntityDto Budget {
      get; internal set;
    }

    public NamedEntityDto BaseParty {
      get; internal set;
    }

    public NamedEntityDto OperationSource {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // BudgetEntryDto



  /// <summary>Output DTO used to display budget entries in a lists.</summary>
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

  }  // BudgetTransactionDescriptorDto

}  // namespace Empiria.Budgeting.Transactions.Adapters
