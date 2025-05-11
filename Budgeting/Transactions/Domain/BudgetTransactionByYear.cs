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
using System.Linq;

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

    internal string BuildUID(BudgetEntry entry) {
      return $"{entry.BudgetTransaction.UID}|{entry.BalanceColumn.UID}|{entry.BudgetAccount.UID}|" +
             $"{entry.Product.UID}|{entry.ProductUnit.UID}|{entry.Project.UID}|{entry.Currency.UID}|{entry.Year}";
    }


    internal FixedList<BudgetEntry> CreateBudgetEntries(BudgetEntryByYearFields fields) {
      return GetNewBudgetEntries(fields).ToFixedList();
    }


    internal FixedList<BudgetEntry> GetBudgetEntries(string entryByYearUID) {
      string[] parts = entryByYearUID.Split('|');

      var fields = new BudgetEntryByYearFields {
        UID = entryByYearUID,
        TransactionUID = parts[0],
        BalanceColumnUID = parts[1],
        BudgetAccountUID = parts[2],
        ProductUID = parts[3],
        ProductUnitUID = parts[4],
        ProjectUID = parts[5],
        CurrencyUID = parts[6],
        Year = int.Parse(parts[7])
      };

      Assertion.Ensure(fields.TransactionUID == Transaction.UID, "Transaction UID mismatch.");

      return GetBudgetEntries(fields);
    }


    internal FixedList<BudgetEntry> GetBudgetEntries(BudgetEntryByYearFields fields) {

      var column = FieldPatcher.PatchField(fields.BalanceColumnUID, BalanceColumn.Empty);
      var account = FieldPatcher.PatchField(fields.BudgetAccountUID, BudgetAccount.Empty);
      var product = FieldPatcher.PatchField(fields.ProductUID, Product.Empty);
      var productUnit = FieldPatcher.PatchField(fields.ProductUnitUID, ProductUnit.Empty);
      var project = FieldPatcher.PatchField(fields.ProjectUID, Project.Empty);
      var currency = FieldPatcher.PatchField(fields.CurrencyUID, Transaction.BaseBudget.BudgetType.Currency);
      var year = fields.Year;

      FixedList<BudgetEntry> entries = Transaction.Entries.FindAll(x => x.BalanceColumn.Equals(column) &&
                                                                        x.BudgetAccount.Equals(account) &&
                                                                        x.Product.Equals(product) &&
                                                                        x.ProductUnit.Equals(productUnit) &&
                                                                        x.Project.Equals(project) &&
                                                                        x.Currency.Equals(currency) &&
                                                                        x.Year == year);

      return entries.Sort((x, y) => x.Month.CompareTo(y.Month));
    }


    public FixedList<BudgetEntryByYear> GetEntries() {
      var groups = Transaction.Entries.GroupBy(x => BudgetEntryByYear.BuildUID(x));

      var list = new List<BudgetEntryByYear>(groups.Count());

      foreach (var group in groups) {
        var item = new BudgetEntryByYear(group.Key, group.ToFixedList());

        list.Add(item);
      }

      return list.ToFixedList();
    }


    internal FixedList<BudgetEntry> GetUpdatedBudgetEntries(BudgetEntryByYearFields fields) {
      FixedList<BudgetEntry> currentEntries = GetBudgetEntries(fields.UID);

      var updatedBudgetEntries = new List<BudgetEntry>(12);

      updatedBudgetEntries.AddRange(GetNewBudgetEntries(fields));
      updatedBudgetEntries.AddRange(GetChangedBudgetEntries(fields));
      updatedBudgetEntries.AddRange(GetDeletedBudgetEntries(fields));

      return updatedBudgetEntries.ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private List<BudgetEntry> GetDeletedBudgetEntries(BudgetEntryByYearFields fields) {
      var list = new List<BudgetEntry>(12);

      var currentEntries = GetBudgetEntries(fields);

      var deletedEntries = currentEntries.FindAll(x => !fields.Amounts.ToFixedList().Contains(y => x.Month == y.Month));

      foreach (var entry in deletedEntries) {
        entry.Delete();

        list.Add(entry);
      }

      return list;
    }


    private List<BudgetEntry> GetChangedBudgetEntries(BudgetEntryByYearFields fields) {
      var list = new List<BudgetEntry>(12);

      var currentEntries = GetBudgetEntries(fields);

      var changedEntries = fields.Amounts.ToFixedList()
                                  .FindAll(x => x.Amount != 0 && x.BudgetEntryUID.Length != 0 &&
                                                currentEntries.Contains(y => y.UID == x.BudgetEntryUID));

      foreach (var amount in changedEntries) {
        BudgetEntryFields entryFields = TransformToBudgetEntryFields(fields, amount);

        var entry = currentEntries.Find(x => x.UID == amount.BudgetEntryUID);

        entry.Update(entryFields);

        list.Add(entry);
      }

      return list;
    }


    private List<BudgetEntry> GetNewBudgetEntries(BudgetEntryByYearFields fields) {
      var list = new List<BudgetEntry>(12);

      var currentEntries = GetBudgetEntries(fields);

      var newEntries = fields.Amounts.ToFixedList()
                                     .FindAll(x => x.Amount != 0 &&
                                                   !currentEntries.Contains(y => y.UID == x.BudgetEntryUID));

      foreach (var amount in newEntries) {
        BudgetEntryFields entryFields = TransformToBudgetEntryFields(fields, amount);

        var entry = new BudgetEntry(Transaction, fields.Year, amount.Month);

        entry.Update(entryFields);

        list.Add(entry);
      }

      return list;
    }


    private BudgetEntryFields TransformToBudgetEntryFields(BudgetEntryByYearFields fields, BudgetMonthEntryFields amount) {
      return new BudgetEntryFields {
        BalanceColumnUID = fields.BalanceColumnUID,
        BudgetAccountUID = fields.BudgetAccountUID,
        CurrencyUID = fields.CurrencyUID,
        ProjectUID = fields.ProjectUID,
        ProductUID = fields.ProductUID,
        ProductUnitUID = fields.ProductUnitUID,

        Description = fields.Description,
        Justification = fields.Justification,

        Amount = amount.Amount,
        OriginalAmount = amount.Amount,
        ProductQty = amount.ProductQty,
      };
    }

    #endregion Helpers

  }  // class BudgetTransactionByYear

}  // namespace Empiria.Budgeting.Transactions
