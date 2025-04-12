/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Imput Fields                            *
*  Type     : BudgetEntryByYearFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create and update a budget entries for a whole year.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Input fields used to create and update a budget entries for a whole year.</summary>
  public class BudgetEntryByYearFields {

    public string BalanceColumnUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string ProductUnitUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public int Year {
      get; internal set;
    }

    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public BudgetMonthEntryFields[] Amounts {
      get; set;
    }

  }  // BudgetEntryByYearFields



  /// <summary>Input fields used to create and update a budget entry amounts for a month.</summary>
  public class BudgetMonthEntryFields {

    public string BudgetEntryUID {
      get; set;
    }

    public int Month {
      get; set;
    }

    public decimal ProductQty {
      get; set;
    }

    public decimal Amount {
      get; set;
    }

  }  // class BudgetMonthEntryFields

}  // namespace Empiria.Budgeting.Transactions
