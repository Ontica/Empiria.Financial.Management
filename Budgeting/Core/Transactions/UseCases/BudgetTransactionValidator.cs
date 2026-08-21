/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Services Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Builder                                 *
*  Type     : BudgetTransactionValidator                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to validate a budget transaction.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services used to validate a budget transaction.</summary>
  public class BudgetTransactionValidator {

    private readonly BudgetTransaction _transaction;
    private readonly FixedList<BudgetTransaction> _relatedTransactions;

    public BudgetTransactionValidator(BudgetTransaction transaction) {

      Assertion.Require(transaction, nameof(transaction));

      Assertion.Require(transaction.Entries.Count > 0,
                        "Transaction has no entries.");

      Assertion.Require(transaction.Entries.SelectDistinct(x => x.Year).Count == 1,
                        "Transaction can not be multiyear.");

      _transaction = transaction;

      _relatedTransactions = BudgetTransaction.GetRelatedTo(transaction)
                                              .FindAll(x => x.InProcess || x.IsClosed);
    }


    public decimal AvailableAmount() {

      var deposits = _transaction.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                       x.BudgetAccount.StandardAccount.RoleType != Financial.AccountRoleType.Control)
                                         .GroupBy(x => new { x.BudgetAccount });

      decimal availableAmount = 0;

      foreach (var deposit in deposits) {

        BudgetAccount account = deposit.Key.BudgetAccount;

        decimal depositsTotal = GetDepositsTotal(_transaction.OperationType.DepositColumn(), account);

        decimal withdrawalsTotal = GetWithdrawalsTotal(_transaction.OperationType.DefaultWithdrawalColumn(), account);

        decimal availableBudget = depositsTotal - withdrawalsTotal;

        availableAmount += availableBudget;
      }

      return availableAmount;
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
                                $"Disponible {(availableBudget):C2}, Solicitado {requiredAmount:C2}");
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
