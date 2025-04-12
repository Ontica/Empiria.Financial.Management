/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetEntryByYearEditionUseCases           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to edit budget transactions entries by a whole year.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

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

    public BudgetEntryByYearDto CreateBudgetEntryByYear(string budgetTransactionUID,
                                                        BudgetEntryByYearFields fields) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(fields, nameof(fields));

      // fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      // var entry = transaction.AddEntry(fields);

      transaction.Save();

      throw new NotImplementedException();

      // return BudgetEntryByYearMapper.Map(entry);
    }


    public BudgetEntryByYearDto GetBudgetEntryByYear(string budgetTransactionUID, string entryByYearUID) {
      throw new NotImplementedException();
    }


    public BudgetEntryByYearDto RemoveBudgetEntryByYear(string budgetTransactionUID, string entryByYearUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      var budgetEntry = transaction.GetEntry(entryByYearUID);

      transaction.RemoveEntry(budgetEntry);

      transaction.Save();

      return BudgetEntryByYearMapper.Map(budgetEntry);
    }


    public BudgetEntryByYearDto UpdateBudgetEntryByYear(string budgetTransactionUID,
                                                        string entryByYearUID,
                                                        BudgetEntryByYearFields fields) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));
      Assertion.Require(fields, nameof(fields));

      // fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      var budgetEntry = transaction.GetEntry(entryByYearUID);

      // budgetEntry.Update(fields);

      budgetEntry.Save();

      return BudgetEntryByYearMapper.Map(budgetEntry);
    }

    #endregion Use cases

  }  // class BudgetEntryByYearEditionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
