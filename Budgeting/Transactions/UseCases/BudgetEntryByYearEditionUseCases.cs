/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetEntryByYearEditionUseCases           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to edit budget transactions entries by a whole year.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Budgeting.Transactions.UseCases {

  /// <summary>Use cases used to edit budget transactions entries by a whole year.</summary>
  public class BudgetEntryByYearEditionUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetEntryByYearEditionUseCases() {
      // no-op
    }

    static public BudgetEntryByYearEditionUseCases UseCaseInteractor() {
      return CreateInstance<BudgetEntryByYearEditionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BudgetEntryByYearDto CreateBudgetEntryByYear(BudgetEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(fields.TransactionUID);

      var byYearTransaction = new BudgetTransactionByYear(transaction);

      FixedList<BudgetEntry> entries = byYearTransaction.GetBudgetEntries(fields);

      transaction.UpdateEntries(entries);

      entries = byYearTransaction.GetNewBudgetEntries(fields);

      transaction.AddEntries(entries);

      transaction.Save();

      var entryByYear = new BudgetEntryByYear(byYearTransaction.BuildUID(entries[0]), entries);

      return BudgetEntryByYearMapper.Map(entryByYear);
    }


    public BudgetEntryByYearDto GetBudgetEntryByYear(string transactionUID, string entryByYearUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));

      var transaction = BudgetTransaction.Parse(transactionUID);

      var byYearTransaction = new BudgetTransactionByYear(transaction);

      FixedList<BudgetEntry> entries = byYearTransaction.GetBudgetEntries(entryByYearUID);

      var entryByYear = new BudgetEntryByYear(entryByYearUID, entries);

      return BudgetEntryByYearMapper.Map(entryByYear);
    }


    public void RemoveBudgetEntryByYear(string transactionUID, string entryByYearUID) {
      Assertion.Require(transactionUID, nameof(transactionUID));
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));

      var transaction = BudgetTransaction.Parse(transactionUID);

      var byYearTransaction = new BudgetTransactionByYear(transaction);

      FixedList<BudgetEntry> entries = byYearTransaction.GetBudgetEntries(entryByYearUID);

      transaction.RemoveEntries(entries);

      transaction.Save();
    }


    public BudgetEntryByYearDto UpdateBudgetEntryByYear(BudgetEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(fields.TransactionUID);

      var byYearTransaction = new BudgetTransactionByYear(transaction);

      FixedList<BudgetEntry> entries = byYearTransaction.GetBudgetEntries(fields);

      transaction.UpdateEntries(entries);

      transaction.Save();

      var entryByYear = new BudgetEntryByYear(fields.UID, entries);

      return BudgetEntryByYearMapper.Map(entryByYear);
    }

    #endregion Use cases

  }  // class BudgetEntryByYearEditionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
