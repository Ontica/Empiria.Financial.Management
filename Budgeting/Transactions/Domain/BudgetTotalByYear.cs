/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Information holder                      *
*  Type     : BudgetEntryByYear                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a budget transaction entry with values for a whole year.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Holds a budget transaction entry with values for a whole year.</summary>
  public class BudgetTotalByYear {

    public BudgetTotalByYear(Budget budget, int year, BalanceColumn column, FixedList<BudgetEntry> entries) {
      Assertion.Require(budget, nameof(budget));
      Assertion.Require(column, nameof(column));
      Assertion.Require(entries, nameof(entries));
      Assertion.Require(entries.Count > 0, "entries must not be an empty list.");

      Budget = budget;
      BalanceColumn = column;
      Year = year;

      Entries = entries;
    }

    #region Properties


    public Budget Budget {
      get;
    }

    public BalanceColumn BalanceColumn {
      get;
    }

    public int Year {
      get;
    }

    public FixedList<BudgetEntry> Entries {
      get; private set;
    }

    public decimal Total {
      get {
        return Entries.Sum(x => x.Amount);
      }
    }

    #endregion Properties

    #region Methods

    public decimal GetTotalForMonth(int month) {
      return Entries.FindAll(x => x.Month == month)
                    .Sum(x => x.Amount);
    }

    #endregion Methods


  }  // BudgetEntryByYear

}  // namespace Empiria.Budgeting.Transactions
