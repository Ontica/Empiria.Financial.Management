/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Services Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Builder                                 *
*  Type     : BudgetTransactionValidator                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to validate a budget transaction.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

namespace Empiria.Budgeting.Transactions {


  public class AvailableBudgetEntry {

    internal AvailableBudgetEntry(BudgetEntry budgetEntry, decimal availableAmount) {
      BudgetEntry = budgetEntry;
      AvailableAmount = availableAmount;
    }


    public BudgetEntry BudgetEntry {
      get;
    }

    public decimal AvailableAmount {
      get;
    }

  }   // class AvailableBudgetEntry


  /// <summary>Provides services used to validate a budget transaction.</summary>
  public class BudgetTransactionValidator {

    private readonly BudgetTransaction _transaction;
    private readonly FixedList<BudgetTransaction> _relatedTransactions;

    public BudgetTransactionValidator(BudgetTransaction transaction) {

      Assertion.Require(transaction, nameof(transaction));

      Assertion.Require(transaction.Entries.Count > 0,
                        "Transaction has no entries.");

      //Assertion.Require(transaction.Entries.SelectDistinct(x => x.Year).Count == 1,
      //                  "Transaction can not be multiyear.");

      _transaction = transaction;

      _relatedTransactions = BudgetTransaction.GetRelatedTo(transaction)
                                              .FindAll(x => x.InProcess || x.IsClosed);
    }


    public decimal AvailableAmount() {

      var entries = _relatedTransactions.SelectFlat(x => x.Entries)
                                        .FindAll(x => (x.BalanceColumn.Equals(BalanceColumn.Requested) ||
                                                       x.BalanceColumn.Equals(BalanceColumn.Commited)) &&
                                                      !x.IsAdjustment && x.NoRejected);

      return entries.Sum(x => x.Deposit - x.Withdrawal);
    }


    public decimal AvailableAmount(BalanceColumn balanceColumn) {

      var entries = _relatedTransactions.SelectFlat(x => x.Entries)
                                        .FindAll(x => x.BalanceColumn.Equals(balanceColumn) &&
                                                      !x.IsAdjustment && x.NoRejected);

      return entries.Sum(x => x.Deposit - x.Withdrawal);
    }


    public FixedList<AvailableBudgetEntry> AvailableBudgetEntries() {

      var groupedEntries = _relatedTransactions.SelectFlat(x => x.Entries)
                                               .FindAll(x => x.BalanceColumn.Equals(_transaction.OperationType.DepositColumn()) &&
                                                            !x.IsAdjustment && x.NoRejected)
                                               .GroupBy(x => new { x.BudgetAccount, ControlNo = x.ControlNo.Split('/')[0] });

      var availableBudgetEntries = new List<AvailableBudgetEntry>();

      foreach (var entry in groupedEntries) {

        decimal availableAmount = entry.Sum(x => x.Deposit - x.Withdrawal);

        if (availableAmount <= 0) {
          continue;
        }

        var txnEntry = _transaction.Entries.Find(x => x.BalanceColumn.Equals(_transaction.OperationType.DepositColumn()) &&
                                                      x.Deposit > 0 &&
                                                      x.BudgetAccount.Equals(entry.Key.BudgetAccount) &&
                                                      x.ControlNo.Equals(entry.Key.ControlNo) &&
                                                      !x.IsAdjustment && x.NoRejected);

        if (txnEntry == null) {
          continue;
        }

        var availableEntry = new AvailableBudgetEntry(txnEntry, availableAmount);

        availableBudgetEntries.Add(availableEntry);
      }

      return availableBudgetEntries.ToFixedList();
    }


    public void EnsureHasAvailableBudget() {

      var deposits = _transaction.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                  x.BudgetAccount.StandardAccount.RoleType != Financial.AccountRoleType.Control)
                                         .GroupBy(x => new { x.BudgetAccount });


      foreach (var deposit in deposits) {

        BudgetAccount account = deposit.Key.BudgetAccount;

        decimal requiredAmount = deposit.Sum(x => x.Deposit);

        decimal availableBudget = GetTotal(_transaction.OperationType.DefaultWithdrawalColumn(), account);

        if (requiredAmount > availableBudget) {
          Assertion.RequireFail($"No hay presupuesto comprometido disponible para la partida {account.AccountNo}: " +
                                $"Solicitado {requiredAmount:C2}, Disponible {(availableBudget):C2}");
        }
      }
    }

    #region Helpers

    private decimal GetTotal(BalanceColumn balanceColumn, BudgetAccount account) {
      return _relatedTransactions.SelectFlat(x => x.Entries)
                                 .FindAll(x => x.BudgetAccount.Equals(account) &&
                                               x.BalanceColumn.Equals(balanceColumn) &&
                                               !x.IsAdjustment)
                                 .Sum(x => x.Deposit - x.Withdrawal);
    }


    private decimal GetDepositsTotal(BalanceColumn balanceColumn, BudgetAccount account) {
      return _relatedTransactions.SelectFlat(x => x.Entries)
                                 .FindAll(x => x.BudgetAccount.Equals(account) &&
                                               x.BalanceColumn.Equals(balanceColumn) &&
                                               x.Deposit > 0 && !x.IsAdjustment)
                                 .Sum(x => x.Deposit);
    }


    private decimal GetWithdrawalsTotal(BalanceColumn balanceColumn, BudgetAccount account) {
      return _relatedTransactions.SelectFlat(x => x.Entries)
                                 .FindAll(x => x.BudgetAccount.Equals(account) &&
                                               x.BalanceColumn.Equals(balanceColumn) &&
                                               x.Withdrawal > 0 && !x.IsAdjustment)
                                 .Sum(x => x.Withdrawal);
    }

    #endregion Helpers

  }  // class BudgetTransactionValidator

}  // namespace Empiria.Budgeting.Transactions
