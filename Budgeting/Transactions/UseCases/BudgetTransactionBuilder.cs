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

    public BudgetTransactionBuilder(IPayableEntity payable, BudgetTransactionFields fields) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(fields, nameof(fields));

      _payable = payable;
      _fields = fields;
    }


    internal BudgetTransaction Build() {
      BudgetTransaction transaction = CreateTransaction();

      return transaction;
    }

    #region Helpers

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
