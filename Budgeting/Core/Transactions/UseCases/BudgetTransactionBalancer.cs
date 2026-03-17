/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Services Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Builder                                 *
*  Type     : BudgetTransactionBalancer                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to create entries for a budget transaction in order to balance it.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Budgeting.Explorer;
using Empiria.Budgeting.Explorer.Adapters;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services used to create entries for a budget transaction in order to balance it.</summary>
  public class BudgetTransactionBalancer {

    private readonly BudgetTransaction _transaction;
    private readonly FixedList<BudgetDataInColumns> _availableBudget;

    public BudgetTransactionBalancer(BudgetTransaction transaction) {

      Assertion.Require(transaction, nameof(transaction));

      Assertion.Require(transaction.Entries.Count > 0,
                        "Transaction has no entries.");

      Assertion.Require(transaction.Entries.SelectDistinct(x => x.Year).Count == 1,
                        "Transaction can not be multiyear.");

      _transaction = transaction;

      _availableBudget = GetAvailableBudget();
    }


    public FixedList<BudgetEntry> BuildBalanceEntries() {
      var deposits = _transaction.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment)
                                         .GroupBy(x => new { x.BudgetAccount, x.Month });

      var withdrawals = _transaction.Entries.FindAll(x => x.Withdrawal > 0);

      var entries = new List<BudgetEntry>(_transaction.Entries.Count);

      foreach (var deposit in deposits) {

        var account = deposit.Key.BudgetAccount;
        var month = deposit.Key.Month;

        var withdrawalAmount = withdrawals.FindAll(x => x.BudgetAccount.Equals(account))
                                          .Sum(x => x.Withdrawal);

        var needed = deposit.Sum(x => x.Deposit) - withdrawalAmount;

        if (needed == 0) {
          continue;
        }

        var available = _availableBudget.FindAll(x => x.BudgetAccount.Equals(account) &&
                                                      x.Month == month)
                                        .Sum(x => x.Available);

        if (available >= needed) {
          BudgetEntry entry = BuildEntry(account, BalanceColumn.Available, month, -needed);

          entries.Add(entry);

          continue;

        } else if (available > 0) {
          BudgetEntry entry = BuildEntry(account, BalanceColumn.Available, month, -available);

          entries.Add(entry);

          needed = needed - available;
        }

        for (int monthIndex = 12; monthIndex > month; monthIndex--) {
          available = _availableBudget.FindAll(x => x.BudgetAccount.Equals(account) &&
                                                    x.Month == monthIndex)
                                      .Sum(x => x.Available);

          if (available >= needed) {

            BudgetEntry entry = BuildEntry(account, BalanceColumn.Reduced, monthIndex, needed);

            entries.Add(entry);

            var entry2 = BuildEntry(account, BalanceColumn.Expanded, month, needed);

            entries.Add(entry2);

            break;

          } else if (available > 0) {

            BudgetEntry entry = BuildEntry(account, BalanceColumn.Reduced, monthIndex, available);

            entries.Add(entry);

            var entry2 = BuildEntry(account, BalanceColumn.Expanded, month, available);

            entries.Add(entry2);

            needed = needed - available;
          }
        }

      }  // foreach

      return entries.ToFixedList();
    }

    #region Helpers

    private BudgetEntry BuildEntry(BudgetAccount account, BalanceColumn balanceColumn,
                                  int month, decimal amount) {

      var entry = new BudgetEntry(_transaction, account, month,
                                  balanceColumn, amount, true);

      return entry;
    }


    private FixedList<BudgetDataInColumns> GetAvailableBudget() {
      var query = new AvailableBudgetQuery {
        Year = _transaction.Entries.First().Year,
        Budget = _transaction.BaseBudget
      };

      var available = new AvailableBudgetBuilder(query);

      return available.Build();
    }

    #endregion Helpers

  }  // class BudgetTransactionBalancer

}  // namespace Empiria.Budgeting.Transactions
