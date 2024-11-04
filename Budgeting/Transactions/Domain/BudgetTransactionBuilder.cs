/* Empiria Financial *****************************************************************************************
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
      _transaction = CreateTransaction();

      CreateEntries();

      return _transaction;
    }

    #region Helpers

    private void CreateEntries() {
      foreach (var item in _payable.Items) {
        var fields = CreateEntryFields(item, BalanceColumn.Available, false);

        _transaction.AddEntry(fields);

        fields = CreateEntryFields(item, BalanceColumn.Commited, true);

        _transaction.AddEntry(fields);
      }
    }


    static private BudgetEntryFields CreateEntryFields(IPayableEntityItem item,
                                                       BalanceColumn balanceColumn,
                                                       bool deposit) {
      return new BudgetEntryFields {
        BudgetAccountUID = item.BudgetAccount.UID,
        BalanceColumnUID = balanceColumn.UID,
        Description = item.Description,
        ProductUID = item.Product.UID,
        CurrencyUID = item.Currency.UID,
        Deposit = deposit ? item.Total : 0,
        Withdrawal = deposit ? 0: item.Total,
      };
    }


    private BudgetTransaction CreateTransaction() {

      var transactionType = BudgetTransactionType.Parse(_fields.TransactionTypeUID);

      var budget = Budget.Parse(_fields.BaseBudgetUID);

      var transaction = new BudgetTransaction(transactionType, budget);

      transaction.Update(_fields);

      return transaction;
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions
