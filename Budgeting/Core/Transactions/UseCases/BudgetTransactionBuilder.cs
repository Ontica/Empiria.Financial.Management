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


    public BudgetTransaction Build(BudgetOperationType operationType) {
      Assertion.Require(operationType, nameof(operationType));

      BudgetTransactionFields fields = BuildTransactionFields(operationType, BudgetTransaction.Empty);

      BudgetTransaction transaction = BuildTransaction(operationType, fields);

      BuildEntries(transaction, BudgetTransaction.Empty);

      Assertion.Require(transaction.Entries.Count > 0,
                        "No es posible generar la transacción presupuestal debido " +
                        "a que la orden de compra o requisición no cuenta " +
                        "con conceptos pendientes de autorizar.");

      return transaction;
    }


    public BudgetTransaction Build(BudgetOperationType operationType, BudgetTransaction previousTransaction) {

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

    private void BuildEntries(BudgetTransaction transaction, BudgetTransaction previousTransaction) {

      if (transaction.OperationType == BudgetOperationType.Exercise) {

        BuildEntriesFromTransaction(transaction, previousTransaction);

      } else if (!previousTransaction.IsEmptyInstance) {

        BuildEntriesFromBudgetable(transaction, previousTransaction);

      } else {

        BuildEntriesFromBudgetable(transaction);

      }
    }


    private void BuildEntriesFromBudgetable(BudgetTransaction transaction) {

      BalanceColumn depositColumn = transaction.OperationType.DepositColumn();

      BalanceColumn withdrawalColumn = transaction.OperationType.DefaultWithdrawalColumn();

      foreach (var entry in _budgetable.Items) {

        BudgetEntry newEntry = BuildEntry(transaction, entry, _applicationDate, depositColumn, true);

        transaction.AddEntry(newEntry);

        newEntry = BuildEntry(transaction, entry, _applicationDate, withdrawalColumn, false);

        transaction.AddEntry(newEntry);
      }

    }


    private void BuildEntriesFromBudgetable(BudgetTransaction transaction, BudgetTransaction previousTransaction) {

      BalanceColumn depositColumn = transaction.OperationType.DepositColumn();

      BalanceColumn withdrawalColumn = previousTransaction.OperationType.DepositColumn();

      foreach (var entry in _budgetable.Items) {

        var previousEntry = previousTransaction.Entries
                                               .Find(x => x.Equals(entry.BudgetEntry) &&
                                                          x.BalanceColumn.Equals(withdrawalColumn) &&
                                                          x.Deposit > 0 && x.NotAdjustment);

        Assertion.Require(previousEntry, "No se encontró una entrada previa correspondiente.");

        BudgetEntry newEntry = BuildEntry(transaction, entry, _applicationDate, depositColumn, true);

        transaction.AddEntry(newEntry);

        DateTime withdrawalDate = SameYearMonth(_applicationDate, previousEntry.Date) ? _applicationDate : previousEntry.Date;

        newEntry = BuildEntry(transaction, entry, withdrawalDate, withdrawalColumn, false);

        transaction.AddEntry(newEntry);

        if (SameYearMonth(_applicationDate, withdrawalDate)) {
          continue;
        }

        newEntry = previousEntry.CloneFor(transaction, withdrawalDate, BalanceColumn.Reduced, true, true);

        transaction.AddEntry(newEntry);

        newEntry = previousEntry.CloneFor(transaction, _applicationDate, BalanceColumn.Expanded, true, true);

        transaction.AddEntry(newEntry);
      }

    }


    private void BuildEntriesFromTransaction(BudgetTransaction transaction, BudgetTransaction previousTransaction) {

      BalanceColumn depositColumn = transaction.OperationType.DepositColumn();

      BalanceColumn withdrawalColumn = previousTransaction.OperationType.DepositColumn();

      foreach (var entry in previousTransaction.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                                     x.BalanceColumn.Equals(withdrawalColumn))) {

        BudgetEntry newEntry = entry.CloneFor(transaction, _applicationDate, depositColumn, true);

        transaction.AddEntry(newEntry);

        DateTime withdrawalDate = SameYearMonth(_applicationDate, entry.Date) ? _applicationDate : entry.Date;

        newEntry = entry.CloneFor(transaction, withdrawalDate, withdrawalColumn, false);

        transaction.AddEntry(newEntry);

        if (SameYearMonth(_applicationDate, withdrawalDate)) {
          continue;
        }

        newEntry = entry.CloneFor(transaction, withdrawalDate, BalanceColumn.Reduced, true, true);

        transaction.AddEntry(newEntry);

        newEntry = entry.CloneFor(transaction, _applicationDate, BalanceColumn.Expanded, true, true);

        transaction.AddEntry(newEntry);
      }
    }


    private BudgetEntry BuildEntry(BudgetTransaction transaction, BudgetableItemData entry, DateTime budgetingDate,
                                   BalanceColumn depositColumn, bool isDeposit) {

      entry.BudgetingDate = budgetingDate;
      entry.ExchangeRate = _exchangeRate;

      BudgetEntry newEntry = transaction.AddEntry(entry, depositColumn, isDeposit);

      return newEntry;
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


    static private bool SameYearMonth(DateTime date1, DateTime date2) {
      return date1.Year == date2.Year && date1.Month == date2.Month;
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions
