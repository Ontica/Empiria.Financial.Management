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

using Empiria.Parties;

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
          goto NextDeposit;
        }

        var available = GetAvailableBudget(account, month);

        if (available >= needed) {
          BudgetEntry entry = BuildEntry(account, BalanceColumn.Available, month, -needed);

          entries.Add(entry);

          goto NextDeposit;

        } else if (available > 0) {
          BudgetEntry entry = BuildEntry(account, BalanceColumn.Available, month, -available);

          entries.Add(entry);

          needed = needed - available;
        }

        if (needed <= 0) {
          goto NextDeposit;
        }

        for (int monthIndex = 1; monthIndex < month; monthIndex++) {

          available = GetAvailableBudget(account, monthIndex);

          if (available >= needed) {

            var reduceExpand = BuildReduceExpandEntries(account, monthIndex, account, month, needed);

            entries.AddRange(reduceExpand);

            goto NextDeposit;

          } else if (available > 0) {

            var reduceExpand = BuildReduceExpandEntries(account, monthIndex, account, month, available);

            entries.AddRange(reduceExpand);

            needed = needed - available;
          }
        }

        if (needed <= 0) {
          goto NextDeposit;
        }

        for (int monthIndex = 12; monthIndex > month; monthIndex--) {

          available = GetAvailableBudget(account, monthIndex);

          if (available >= needed) {

            var reduceExpand = BuildReduceExpandEntries(account, monthIndex, account, month, needed);

            entries.AddRange(reduceExpand);

            goto NextDeposit;

          } else if (available > 0) {

            var reduceExpand = BuildReduceExpandEntries(account, monthIndex, account, month, available);

            entries.AddRange(reduceExpand);

            needed = needed - available;
          }
        }

        if (needed <= 0) {
          goto NextDeposit;
        }

        var globalAcount = TryGetGlobalBudgetAccount(account);

        if (globalAcount == null) {
          goto NextDeposit;
        }

        available = GetAvailableBudget(globalAcount, month);

        if (available >= needed) {

          var reduceExpand = BuildReduceExpandEntries(globalAcount, month, account, month, needed);

          entries.AddRange(reduceExpand);

          goto NextDeposit;

        } else if (available > 0) {

          var reduceExpand = BuildReduceExpandEntries(globalAcount, month, account, month, available);

          entries.AddRange(reduceExpand);

          needed = needed - available;
        }

        if (needed <= 0) {
          goto NextDeposit;
        }

        for (int monthIndex = 1; monthIndex < month; monthIndex++) {

          available = GetAvailableBudget(globalAcount, monthIndex);

          if (available >= needed) {

            var reduceExpand = BuildReduceExpandEntries(globalAcount, monthIndex, account, month, needed);

            entries.AddRange(reduceExpand);

            goto NextDeposit;

          } else if (available > 0) {

            var reduceExpand = BuildReduceExpandEntries(globalAcount, monthIndex, account, month, available);

            entries.AddRange(reduceExpand);

            needed = needed - available;
          }
        }

        if (needed <= 0) {
          goto NextDeposit;
        }

        for (int monthIndex = 12; monthIndex > month; monthIndex--) {

          available = GetAvailableBudget(globalAcount, monthIndex);

          if (available >= needed) {

            var reduceExpand = BuildReduceExpandEntries(globalAcount, monthIndex, account, month, needed);

            entries.AddRange(reduceExpand);

            goto NextDeposit;

          } else if (available > 0) {

            var reduceExpand = BuildReduceExpandEntries(globalAcount, monthIndex, account, month, available);

            entries.AddRange(reduceExpand);

            needed = needed - available;
          }
        }

NextDeposit:
        ;
      }  // foreach

      return entries.ToFixedList();
    }


    private BudgetEntry[] BuildReduceExpandEntries(BudgetAccount reduceAccount, int reduceMonth,
                                                   BudgetAccount expandAccount, int expandMonth,
                                                   decimal amount) {

      BudgetEntry reduce = BuildEntry(reduceAccount, BalanceColumn.Reduced, reduceMonth, amount);

      BudgetEntry expand = BuildEntry(expandAccount, BalanceColumn.Expanded, expandMonth, amount);

      return new[] { reduce, expand };
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


    private decimal GetAvailableBudget(BudgetAccount account, int month) {
      return _availableBudget.FindAll(x => x.BudgetAccount.Equals(account) &&
                                           x.Month == month)
                             .Sum(x => x.Available);
    }


    private BudgetAccount TryGetGlobalBudgetAccount(BudgetAccount account) {
      var globalOrgUnit = OrganizationalUnit.TryParseWithID("901200");

      return BudgetAccount.TryParse(globalOrgUnit, account.AccountNo);
    }

    #endregion Helpers

  }  // class BudgetTransactionBalancer

}  // namespace Empiria.Budgeting.Transactions
