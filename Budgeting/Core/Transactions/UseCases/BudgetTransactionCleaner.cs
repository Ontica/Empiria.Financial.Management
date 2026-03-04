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
using System.Linq;
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

        Assertion.Require(previousEntry, $"No se encontró una entrada previa correspondiente: " +
                                         $"{transaction.TransactionNo} {transaction.Id} / {budgetableItem.BudgetableItem.Id} in" +
                                         $"{string.Join(",", previousEntries.Select(x => x.Id))}");

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


    public FixedList<BudgetEntry> CreateCrossedAccountsAdjustEntries(BudgetTransaction approvePaymentTxn,
                                                                     BudgetTransaction commitTxn) {

      var newEntries = new List<BudgetEntry>();

      foreach (var commitEntry in commitTxn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                                 x.BalanceColumn == BalanceColumn.Commited)) {

        var paymentEntries = approvePaymentTxn.Entries.FindAll(x => x.Withdrawal > 0 && x.NotAdjustment && x.BalanceColumn == BalanceColumn.Commited &&
                                                                    x.BudgetAccount.StandardAccount.Id == commitEntry.BudgetAccount.StandardAccount.Id &&
                                                                    x.ControlNo.StartsWith(commitEntry.ControlNo));

        if (paymentEntries.Count == 0) {
          continue;
        }

        DateTime withdrawalDate = SameYearMonth(approvePaymentTxn.ApplicationDate, commitEntry.Date) ? approvePaymentTxn.ApplicationDate : commitEntry.Date;

        if (approvePaymentTxn.Entries.Any(x => x.BalanceColumn == BalanceColumn.Reduced && x.Date == withdrawalDate &&
                                               x.CurrencyAmount == paymentEntries.Sum(y => y.CurrencyAmount))) {
          continue;
        }

        decimal reducedAmount = 0;

        foreach (var entry in paymentEntries) {

          if (entry.BudgetAccount.Equals(commitEntry.BudgetAccount) && !SameYearMonth(withdrawalDate, entry.Date)) {
            EmpiriaLog.Debug($"Caso meses cruzados que no debería ocurrir en multiáreas {approvePaymentTxn.TransactionNo}");

            return new FixedList<BudgetEntry>();
          }

          if (entry.BudgetAccount.Equals(commitEntry.BudgetAccount) && SameYearMonth(withdrawalDate, entry.Date)) {
            continue;
          }

          var newEntry = BuildEntryForAdjustment(entry, approvePaymentTxn, approvePaymentTxn.ApplicationDate,
                                                 BalanceColumn.Expanded, entry.CurrencyAmount);

          newEntries.Add(newEntry);
          approvePaymentTxn.AddAdjustmentEntry(newEntry);

          reducedAmount += entry.CurrencyAmount;

          if (entry.BudgetAccount.Distinct(commitEntry.BudgetAccount)) {
            entry.SetAccount(commitEntry.BudgetAccount);
            entry.SetDate(withdrawalDate);

            newEntries.Add(entry);
          }

        }  // foreach entry

        if (reducedAmount == 0) {
          continue;
        }

        var reducedEntry = BuildEntryForAdjustment(commitEntry, approvePaymentTxn, withdrawalDate,
                                                   BalanceColumn.Reduced, reducedAmount);

        newEntries.Add(reducedEntry);
        approvePaymentTxn.AddAdjustmentEntry(reducedEntry);


      } // foreach commitEntry

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
