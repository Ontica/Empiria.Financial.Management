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


    public BudgetTransaction Build(BudgetOperationType operationType, FixedList<BudgetEntry> previousEntries) {
      Assertion.Require(operationType, nameof(operationType));

      if (operationType == BudgetOperationType.Request) {
        return BuildBudgetRequestTransaction();
      }

      return BuildTransaction(operationType, previousEntries);
    }

    #region Transaction builders

    private BudgetTransaction BuildBudgetRequestTransaction() {
      BudgetTransactionFields fields = BuildTransactionFields(BudgetOperationType.Request);

      BudgetTransaction transaction = BuildTransaction(BudgetOperationType.Request, fields);

      BuildBudgetRequestEntries(transaction);

      Assertion.Require(transaction.Entries.Count > 0,
                        "No es posible generar la transacción presupuestal debido " +
                        "a que la requisición no cuenta con conceptos pendientes de autorizar.");

      return transaction;
    }


    private BudgetTransaction BuildTransaction(BudgetOperationType operationType, FixedList<BudgetEntry> previousEntries) {

      Assertion.Require(operationType, nameof(operationType));

      BudgetTransactionFields fields = BuildTransactionFields(operationType);

      BudgetTransaction transaction = BuildTransaction(operationType, fields);

      BuildEntriesFromBudgetable(transaction, previousEntries);

      Assertion.Require(transaction.Entries.Count > 0,
          "No es posible generar la transacción presupuestal debido " +
          "a que la orden de compra o requisición no cuenta " +
          "con conceptos pendientes de autorizar.");

      return transaction;
    }


    private BudgetTransaction BuildTransaction(BudgetOperationType operationType, BudgetTransactionFields fields) {

      var transactionType = BudgetTransactionType.GetFor(BaseBudget.BudgetType, operationType);

      var transaction = new BudgetTransaction(transactionType, BaseBudget, _budgetable);

      transaction.Update(fields);

      return transaction;
    }


    private BudgetTransactionFields BuildTransactionFields(BudgetOperationType operationType) {

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

    #endregion Transaction builders

    #region Entries builders

    private void BuildBudgetRequestEntries(BudgetTransaction transaction) {

      BalanceColumn depositColumn = transaction.OperationType.DepositColumn();

      BalanceColumn withdrawalColumn = transaction.OperationType.DefaultWithdrawalColumn();

      foreach (var entry in _budgetable.Items) {

        BudgetEntry newEntry = BuildEntry(transaction, BudgetEntry.Empty, entry,
                                          entry.BudgetingDate, depositColumn, true);

        transaction.AddEntry(newEntry);

        newEntry = BuildEntry(transaction, BudgetEntry.Empty, entry,
                              entry.BudgetingDate, withdrawalColumn, false);

        transaction.AddEntry(newEntry);
      }
    }


    private void BuildEntriesFromBudgetable(BudgetTransaction transaction, FixedList<BudgetEntry> previousEntries) {

      BalanceColumn depositColumn = transaction.OperationType.DepositColumn();

      previousEntries = previousEntries.FindAll(x => x.Deposit > 0 && x.NotAdjustment);

      foreach (var budgetableItem in _budgetable.Items) {

        var previousEntry = previousEntries.Find(x => budgetableItem.HasRelatedBudgetableItem &&
                                                      x.EntityId == budgetableItem.RelatedBudgetableItem.Id &&
                                                      x.EntityTypeId == budgetableItem.RelatedBudgetableItem.GetEmpiriaType().Id);

        if (previousEntry == null) {
          previousEntry = previousEntries.Find(x => x.EntityId == budgetableItem.BudgetableItem.Id &&
                                                    x.EntityTypeId == budgetableItem.BudgetableItem.GetEmpiriaType().Id);
        }

        Assertion.Require(previousEntry, "No se encontró una entrada previa correspondiente.");

        BudgetEntry newEntry = BuildEntry(transaction, previousEntry, budgetableItem, _applicationDate, depositColumn, true);

        transaction.AddEntry(newEntry);

        DateTime withdrawalDate = SameYearMonth(_applicationDate, previousEntry.Date) ? _applicationDate : previousEntry.Date;

        newEntry = BuildEntry(transaction, previousEntry, budgetableItem,
                              withdrawalDate, previousEntry.BalanceColumn, false);

        transaction.AddEntry(newEntry);

        if (!SameYearMonth(_applicationDate, withdrawalDate)) {
          BuildMonthAdjustmentEntries(previousEntry, transaction, withdrawalDate, budgetableItem.CurrencyAmount);
        }

        if (previousEntry.ExchangeRate != _exchangeRate) {
          BuildExchangeRateAdjustmentEntries(previousEntry, transaction, budgetableItem.CurrencyAmount);
        }

      }  // foreach

    }


    private BudgetEntry BuildEntry(BudgetTransaction transaction, BudgetEntry previousEntry,
                                   BudgetableItemData entry, DateTime budgetingDate,
                                   BalanceColumn balanceColumn, bool isDeposit) {

      entry.BudgetingDate = budgetingDate;
      entry.ExchangeRate = _exchangeRate;
      entry.PreviousBudgetEntry = previousEntry;

      BudgetEntry newEntry = new BudgetEntry(transaction, entry, balanceColumn, isDeposit);

      return newEntry;
    }


    private BudgetEntry BuildEntryForAdjustment(BudgetEntry previousEntry, BudgetTransaction transaction,
                                                DateTime date, BalanceColumn balanceColumn, decimal currencyAmount) {

      BudgetEntry newEntry = previousEntry.CloneFor(transaction, date, balanceColumn, true, true);

      newEntry.SetAmount(currencyAmount, _exchangeRate);

      return newEntry;
    }


    private void BuildExchangeRateAdjustmentEntries(BudgetEntry previousEntry, BudgetTransaction transaction,
                                                    decimal currencyAmount) {
      decimal difference = previousEntry.ExchangeRate - _exchangeRate;

      BudgetEntry newEntry;

      if (difference > 0) {
        newEntry = previousEntry.CloneFor(transaction, _applicationDate, BalanceColumn.Expanded, true, true);
      } else if (difference < 0) {
        newEntry = previousEntry.CloneFor(transaction, _applicationDate, BalanceColumn.Reduced, true, true);
      } else {
        return;
      }

      newEntry.SetAmount(currencyAmount, difference);

      newEntry.SetDescription($"Ajuste por tipo de cambio. Tipo de cambio previo: {previousEntry.ExchangeRate:C2}, " +
                              $"nuevo tipo de cambio: {_exchangeRate:C2}.");

      transaction.AddEntry(newEntry);
    }


    private void BuildMonthAdjustmentEntries(BudgetEntry previousEntry, BudgetTransaction transaction,
                                             DateTime withdrawalDate, decimal currencyAmount) {

      BudgetEntry newEntry = BuildEntryForAdjustment(previousEntry, transaction, withdrawalDate,
                                                     BalanceColumn.Reduced, currencyAmount);

      transaction.AddEntry(newEntry);

      newEntry = BuildEntryForAdjustment(previousEntry, transaction, _applicationDate,
                                         BalanceColumn.Expanded, currencyAmount);

      transaction.AddEntry(newEntry);
    }

    #endregion Entries builders

    #region Helpers

    static private bool SameYearMonth(DateTime date1, DateTime date2) {
      return date1.Year == date2.Year && date1.Month == date2.Month;
    }

    #endregion Helpers

  }  // class BudgetTransactionBuilder

}  // namespace Empiria.Budgeting.Transactions
