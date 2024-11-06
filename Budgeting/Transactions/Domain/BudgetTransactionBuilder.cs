﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Service provider                        *
*  Type     : BudgetTransactionBuilder                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Builds a budget transaction using a IPayableEntity instance.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Builds a budget transaction using a IPayableEntity instance.</summary>
  internal class BudgetTransactionBuilder {

    private readonly IPayableEntity _payable;
    private readonly BudgetTransactionFields _fields;
    private BudgetTransaction _transaction;

    public BudgetTransactionBuilder(IPayableEntity payable, BudgetTransactionFields fields) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(fields, nameof(fields));

      _payable = payable;
      _fields = fields;
    }


    internal BudgetTransaction Build() {
      _transaction = BuildTransaction();

      BuildEntries();

      return _transaction;
    }

    #region Helpers

    private void BuildDoubleEntries(IPayableEntityItem item,
                                    BalanceColumn depositColumn,
                                    BalanceColumn withdrawalColumn) {

      var fields = BuildEntryFields(item, depositColumn, true);

      _transaction.AddEntry(fields);

      fields = BuildEntryFields(item, withdrawalColumn, false);

      _transaction.AddEntry(fields);
    }


    private void BuildEntries() {
      foreach (var item in _payable.Items) {

        if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.ComprometerGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Commited, BalanceColumn.Available);
        } else if (_transaction.BudgetTransactionType.Equals(BudgetTransactionType.EjercerGastoCorriente)) {
          BuildDoubleEntries(item, BalanceColumn.Exercised, BalanceColumn.Commited);
        }
      }
    }


    static private BudgetEntryFields BuildEntryFields(IPayableEntityItem item,
                                                      BalanceColumn balanceColumn,
                                                      bool isDeposit) {
      return new BudgetEntryFields {
        BudgetAccountUID = item.BudgetAccount.UID,
        BalanceColumnUID = balanceColumn.UID,
        Description = item.Description,
        ProductUID = item.Product.UID,
        CurrencyUID = item.Currency.UID,
        Deposit = isDeposit ? item.Total : 0,
        Withdrawal = isDeposit ? 0: item.Total,
      };
    }


    private BudgetTransaction BuildTransaction() {

      var transactionType = BudgetTransactionType.Parse(_fields.TransactionTypeUID);

      var budget = Budget.Parse(_fields.BaseBudgetUID);

      var transaction = new BudgetTransaction(transactionType, budget);

      transaction.Update(_fields);

      return transaction;
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions