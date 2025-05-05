/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Information holder                      *
*  Type     : BudgetEntryByYear                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a budget transaction entry with values for a whole year.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Products;
using Empiria.Projects;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Holds a budget transaction entry with values for a whole year.</summary>
  public class BudgetEntryByYear {

    public BudgetEntryByYear(string entryByYearUID, FixedList<BudgetEntry> entries) {
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));
      Assertion.Require(entries, nameof(entries));
      Assertion.Require(entries.Count > 0, "entries must not be an empty list.");

      string[] parts = entryByYearUID.Split('|');

      Transaction = BudgetTransaction.Parse(parts[0]);
      BalanceColumn = BalanceColumn.Parse(parts[1]);
      BudgetAccount = BudgetAccount.Parse(parts[2]);
      Product = Product.Parse(parts[3]);
      ProductUnit = ProductUnit.Parse(parts[4]);
      Project = Project.Parse(parts[5]);
      Currency = Currency.Parse(parts[6]);
      Year = int.Parse(parts[7]);

      Entries = entries;
    }


    static internal string BuildUID(BudgetEntry entry) {
      return $"{entry.BudgetTransaction.UID}|{entry.BalanceColumn.UID}|{entry.BudgetAccount.UID}|" +
             $"{entry.Product.UID}|{entry.ProductUnit.UID}|{entry.Project.UID}|{entry.Currency.UID}|{entry.Year}";
    }

    #region Properties

    public string UID {
      get {
        return $"{Transaction.UID}|{BalanceColumn.UID}|{BudgetAccount.UID}|{Product.UID}|" +
               $"{ProductUnit.UID}|{Project.UID}|{Currency.UID}|{Year}";
      }
    }

    public BudgetTransaction Transaction {
      get;
    }

    public BalanceColumn BalanceColumn {
      get;
    }

    public BudgetAccount BudgetAccount {
      get;
    }

    public Product Product {
      get;
    }

    public string Description {
      get;
    } = string.Empty;


    public ProductUnit ProductUnit {
      get; private set;
    }

    public string Justification {
      get; private set;
    } = string.Empty;


    public Project Project {
      get;
    }

    public int Year {
      get;
    }

    public Currency Currency {
      get;
    }

    public FixedList<BudgetEntry> Entries {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal decimal GetAmountForMonth(int month) {
      BudgetEntry entry = Entries.Find(x => x.Month == month);

      if (entry != null) {
        return entry.Amount;
      } else {
        return 0;
      }
    }

    #endregion Methods

  }  // BudgetEntryByYear

}  // namespace Empiria.Budgeting.Transactions
