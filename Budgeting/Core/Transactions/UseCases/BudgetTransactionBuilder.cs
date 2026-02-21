/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Services Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Builder                                 *
*  Type     : BudgetTransactionBuilder                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to build a budget transaction using data from another transaction.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services used to build a budget transaction using data from another transaction.</summary>
  public class BudgetTransactionBuilder {

    private readonly BudgetTransaction _baseTransaction;
    private readonly BudgetOperationType _operationType;
    private readonly OperationSource _operationSource;
    private readonly DateTime _applicationDate;
    private readonly BudgetTransactionType _transactionType;

    public BudgetTransactionBuilder(BudgetTransaction baseTransaction,
                                    BudgetOperationType operationType,
                                    OperationSource operationSource,
                                    DateTime applicationDate) {

      Assertion.Require(baseTransaction, nameof(baseTransaction));
      Assertion.Require(operationType, nameof(operationType));
      Assertion.Require(operationSource, nameof(operationSource));
      Assertion.Require(applicationDate >= baseTransaction.ApplicationDate,
        "La fecha de aplicación debe ser igual o posterior a la fecha de aplicación de la transacción base.");

      _baseTransaction = baseTransaction;
      _operationType = operationType;
      _operationSource = operationSource;
      _applicationDate = applicationDate;
      _transactionType = BudgetTransactionType.GetFor(baseTransaction.BaseBudget.BudgetType, operationType);
    }


    public BudgetTransaction Build() {

      BudgetTransaction transaction = BuildTransaction();

      BuildEntries(transaction);

      Assertion.Require(transaction.Entries.Count > 0,
                        "No es posible generar la transacción presupuestal debido " +
                        "a que la orden de compra o requisición no cuenta " +
                        "con conceptos pendientes de autorizar.");

      return transaction;
    }

    #region Helpers

    private void BuildEntries(BudgetTransaction transaction) {

      BalanceColumn depositColumn = _operationType.DepositColumn();

      BalanceColumn withdrawalColumn = _baseTransaction.OperationType.DepositColumn();

      foreach (var entry in _baseTransaction.Entries.FindAll(x => x.Deposit > 0 && x.BalanceColumn.Equals(withdrawalColumn))) {

        BudgetEntry newEntry = entry.CloneFor(transaction, _applicationDate, depositColumn, true);

        transaction.AddEntry(newEntry);

        DateTime withdrawalDate = (entry.Year == _applicationDate.Year && entry.Month == _applicationDate.Month)
                                      ? _applicationDate : entry.Date;

        newEntry = entry.CloneFor(transaction, withdrawalDate, withdrawalColumn, false);

        transaction.AddEntry(newEntry);

        if (withdrawalDate == _applicationDate) {
          continue;
        }

        newEntry = entry.CloneFor(transaction, withdrawalDate, BalanceColumn.Reduced, true, true);

        transaction.AddEntry(newEntry);

        newEntry = entry.CloneFor(transaction, _applicationDate, BalanceColumn.Expanded, true, true);

        transaction.AddEntry(newEntry);

      }
    }


    private BudgetTransaction BuildTransaction() {

      BudgetTransactionFields fields = BuildTransactionFields();

      var budget = Budget.Parse(fields.BaseBudgetUID);

      var bdgTxnType = BudgetTransactionType.GetFor(budget.BudgetType, _operationType);

      var transaction = new BudgetTransaction(bdgTxnType, budget, _baseTransaction.GetEntity());

      transaction.Update(fields);

      return transaction;
    }


    private BudgetTransactionFields BuildTransactionFields() {

      return new BudgetTransactionFields {
        TransactionTypeUID = _transactionType.UID,
        Justification = _baseTransaction.Justification,
        Description = _baseTransaction.Description,
        BaseBudgetUID = _baseTransaction.BaseBudget.UID,
        BasePartyUID = _baseTransaction.BaseParty.UID,
        CurrencyUID = _baseTransaction.Currency.UID,
        ExchangeRate = _baseTransaction.ExchangeRate,
        OperationSourceUID = _operationSource.UID,
        ApplicationDate = _applicationDate
      };
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions
