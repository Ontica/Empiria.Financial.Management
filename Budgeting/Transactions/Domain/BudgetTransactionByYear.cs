/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Structurer                              *
*  Type     : BudgetTransactionByYear                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a budget transaction with its entries and amounts grouped for a whole year.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Financial;
using Empiria.Products;
using Empiria.Projects;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Represents a budget transaction with its entries and amounts grouped for a whole year.</summary>
  public class BudgetTransactionByYear {

    #region Constructors and parsers

    public BudgetTransactionByYear(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(!transaction.IsEmptyInstance, "Transaction can't be the empty instance.");
      Assertion.Require(!transaction.IsNew, "Transaction can't be a new instance.");

      Transaction = transaction;
    }

    #endregion Constructors and parsers

    #region Properties

    public BudgetTransaction Transaction {
      get;
    }

    #endregion Properties

    #region Methods

    internal FixedList<BudgetEntry> GetBudgetEntries(string entryByYearUID) {
      string[] parts = entryByYearUID.Split('|');

      var fields = new BudgetEntryByYearFields {
        BalanceColumnUID = parts[0],
        BudgetAccountUID = parts[1],
        ProductUID = parts[2],
        ProjectUID = parts[3],
        CurrencyUID = parts[4],
        Year = int.Parse(parts[5])
      };

      return GetBudgetEntries(fields);
    }


    internal FixedList<BudgetEntry> GetBudgetEntries(BudgetEntryByYearFields fields) {
      var column = FieldPatcher.PatchField(fields.BalanceColumnUID, BalanceColumn.Empty);
      var account = FieldPatcher.PatchField(fields.BudgetAccountUID, BudgetAccount.Empty);
      var product = FieldPatcher.PatchField(fields.ProductUID, Product.Empty);
      var project = FieldPatcher.PatchField(fields.ProjectUID, Project.Empty);
      var currency = FieldPatcher.PatchField(fields.CurrencyUID, Currency.Default);
      var year = fields.Year > 0 ? fields.Year : Transaction.BaseBudget.Year;

      FixedList<BudgetEntry> entries = Transaction.Entries.FindAll(x => x.BalanceColumn.Equals(column) &&
                                                                        x.BudgetAccount.Equals(account) &&
                                                                        x.Project.Equals(project) &&
                                                                        x.Product.Equals(product) &&
                                                                        x.Currency.Equals(currency) &&
                                                                        x.Year == year);

      return entries.Sort((x, y) => x.Month.CompareTo(y.Month));
    }


    internal FixedList<BudgetEntry> GetNewBudgetEntries(BudgetEntryByYearFields fields) {
      var list = new List<BudgetEntry>(12);

      foreach (var amount in fields.Amounts) {
        if (amount.Amount == 0) {
          continue;
        }
        var entryFields = new BudgetEntryFields {
          Amount = amount.Amount,
          BalanceColumnUID = fields.BalanceColumnUID,
          BudgetAccountUID = fields.BudgetAccountUID,
          CurrencyUID = fields.CurrencyUID,
          ProjectUID = fields.ProjectUID,
          ProductUID = fields.ProductUID,
          Description = fields.Description,
          Justification = fields.Justification,
          OriginalAmount = amount.Amount
        };

        var entry = new BudgetEntry(Transaction, fields.Year, amount.Month);

        entry.Update(entryFields);

        list.Add(entry);
      }

      return list.ToFixedList();
    }

    #endregion Methods

  }  // class BudgetTransactionByYear

}  // namespace Empiria.Budgeting.Transactions
