/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Services Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Builder                                 *
*  Type     : BudgetTransactionCleaner                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to clean budget transactions.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Financial;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services used to clean budget transactions.</summary>
  public class BudgetTransactionCleaner {


    public FixedList<BudgetEntry> CreateAdjustMonthsEntries(BudgetTransaction transaction,
                                                            FixedList<BudgetEntry> previousEntries) {

      IBudgetable budgetable = transaction.GetEntity();
      DateTime applicationDate = transaction.ApplicationDate;

      Assertion.Require(!transaction.Entries.Contains(x => x.BalanceColumn == BalanceColumn.Expanded || x.BalanceColumn == BalanceColumn.Reduced),
                        $"Transaction {transaction.TransactionNo} already contains adjustment entries.");

      previousEntries = previousEntries.FindAll(x => x.Deposit > 0 && x.NotAdjustment);

      var newEntries = new List<BudgetEntry>();

      foreach (var budgetableItem in budgetable.Items) {

        var previousEntry = previousEntries.Find(x => budgetableItem.HasRelatedBudgetableItem &&
                                                      x.EntityId == budgetableItem.RelatedBudgetableItem.Id &&
                                                      x.EntityTypeId == budgetableItem.RelatedBudgetableItem.GetEmpiriaType().Id);

        if (previousEntry == null) {
          previousEntry = previousEntries.Find(x => x.EntityId == budgetableItem.BudgetableItem.Id &&
                                                    x.EntityTypeId == budgetableItem.BudgetableItem.GetEmpiriaType().Id);
        }

        Assertion.Require(previousEntry, "No se encontró una entrada previa correspondiente.");

        DateTime withdrawalDate = SameYearMonth(applicationDate, previousEntry.Date) ? applicationDate : previousEntry.Date;

        if (SameYearMonth(applicationDate, withdrawalDate)) {
          continue;
        }

        BudgetEntry newEntry = BuildEntryForAdjustment(previousEntry, transaction, withdrawalDate,
                                                       BalanceColumn.Reduced, budgetableItem.CurrencyAmount);

        newEntries.Add(newEntry);
        transaction.AddAdjustmentEntry(newEntry);

        newEntry = BuildEntryForAdjustment(previousEntry, transaction, applicationDate,
                                           BalanceColumn.Expanded, budgetableItem.CurrencyAmount);

        newEntries.Add(newEntry);
        transaction.AddAdjustmentEntry(newEntry);
      }

      return newEntries.ToFixedList();
    }


    #region Entries builders

    private BudgetEntry BuildEntryForAdjustment(BudgetEntry previousEntry, BudgetTransaction transaction,
                                                DateTime date, BalanceColumn balanceColumn, decimal currencyAmount) {

      BudgetEntry newEntry = previousEntry.CloneFor(transaction, date, balanceColumn, true, true);

      newEntry.SetAmount(currencyAmount, previousEntry.ExchangeRate);

      return newEntry;
    }


    #endregion Entries builders

    #region Helpers

    static private bool SameYearMonth(DateTime date1, DateTime date2) {
      return date1.Year == date2.Year && date1.Month == date2.Month;
    }

    #endregion Helpers

  }  // class BudgetTransactionCleaner

}  // namespace Empiria.Budgeting.Transactions
