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

      BudgetEntryByYearFields fields = BudgetTransactionByYear.BuildFields(entryByYearUID);

      Transaction = BudgetTransaction.Parse(fields.TransactionUID);
      BalanceColumn = BalanceColumn.Parse(fields.BalanceColumnUID);
      BudgetAccount = BudgetAccount.Parse(fields.BudgetAccountUID);
      Product = Product.Parse(fields.ProductUID);
      ProductUnit = ProductUnit.Parse(fields.ProductUnitUID);
      Project = Project.Parse(fields.ProjectUID);
      Currency = Currency.Parse(fields.CurrencyUID);
      Year = fields.Year;

      Entries = entries;
    }

    #region Properties

    public string UID {
      get {
        return BudgetTransactionByYear.BuildUID(this);
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
      get {
        var modes = Entries.GetModes(x => x.Justification);

        return modes.Count == 1 ? modes[0] : string.Empty;
      }
    }


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
