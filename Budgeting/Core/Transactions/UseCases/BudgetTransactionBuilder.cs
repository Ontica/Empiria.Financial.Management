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

using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services used to build a budget transaction using data from another transaction.</summary>
  public class BudgetTransactionBuilder {

    private readonly IBudgetable _budgetable;
    private readonly OperationSource _operationSource;
    private readonly DateTime _applicationDate;
    private readonly decimal _exchangeRate;

    public BudgetTransactionBuilder(IBudgetable budgetable,
                                    OperationSource operationSource,
                                    DateTime applicationDate,
                                    decimal? exchangeRate = null) {

      Assertion.Require(budgetable, nameof(budgetable));
      Assertion.Require(operationSource, nameof(operationSource));

      if (exchangeRate.HasValue) {
        Assertion.Require(exchangeRate > decimal.Zero, "El tipo de cambio debe ser mayor a cero.");
      } else {
        exchangeRate = budgetable.Data.ExchangeRate;
      }

      _budgetable = budgetable;
      _operationSource = operationSource;
      _applicationDate = applicationDate;
      _exchangeRate = exchangeRate.Value;
    }


    public Budget BaseBudget {
      get {
        return (Budget) _budgetable.Data.BaseBudget;
      }
    }


    public BudgetTransaction Build(BudgetOperationType operationType,
                                   BudgetTransaction previousTransaction) {
      Assertion.Require(operationType, nameof(operationType));
      Assertion.Require(previousTransaction, nameof(previousTransaction));

      Assertion.Require(previousTransaction.BaseBudget.Equals(BaseBudget),
            "El presupuesto de la transacción no coincide con el presupuesto de la transacción previa.");

      Assertion.Require(_applicationDate >= previousTransaction.ApplicationDate,
            "La fecha de aplicación de la transacción no puede ser menor a " +
            "la fecha de aplicación de la transacción previa.");

      BudgetTransactionFields fields = BuildTransactionFields(operationType, previousTransaction);

      BudgetTransaction transaction = BuildTransaction(operationType, fields);

      BuildEntries(transaction, previousTransaction);

      Assertion.Require(transaction.Entries.Count > 0,
                        "No es posible generar la transacción presupuestal debido " +
                        "a que la orden de compra o requisición no cuenta " +
                        "con conceptos pendientes de autorizar.");

      return transaction;
    }

    #region Helpers

    private void BuildEntries(BudgetTransaction transaction, BudgetTransaction baseTransaction) {

      BalanceColumn depositColumn = transaction.OperationType.DepositColumn();

      BalanceColumn withdrawalColumn = baseTransaction.OperationType.DepositColumn();

      foreach (var entry in baseTransaction.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                                 x.BalanceColumn.Equals(withdrawalColumn))) {

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


    private BudgetTransaction BuildTransaction(BudgetOperationType operationType, BudgetTransactionFields fields) {

      var transactionType = BudgetTransactionType.GetFor(BaseBudget.BudgetType, operationType);

      var transaction = new BudgetTransaction(transactionType, BaseBudget, _budgetable);

      transaction.Update(fields);

      return transaction;
    }


    private BudgetTransactionFields BuildTransactionFields(BudgetOperationType operationType,
                                                           BudgetTransaction previousTransaction) {

      if (operationType == BudgetOperationType.Exercise) {

        return new BudgetTransactionFields {
          BasePartyUID = previousTransaction.BaseParty.UID,
          OperationSourceUID = _operationSource.UID,
          CurrencyUID = previousTransaction.Currency.UID,
          ExchangeRate = _exchangeRate,
          ApplicationDate = _applicationDate,
          Description = previousTransaction.Description,
          Justification = previousTransaction.Justification
        };

      } else {

        return new BudgetTransactionFields {
          BasePartyUID = _budgetable.Data.RequestedBy.UID,
          OperationSourceUID = _operationSource.UID,
          CurrencyUID = _budgetable.Data.Currency.UID,
          ExchangeRate = _exchangeRate,
          ApplicationDate = _applicationDate,
          Description = _budgetable.Data.Description,
          Justification = _budgetable.Data.Justification,
          RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        };
      }
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions
