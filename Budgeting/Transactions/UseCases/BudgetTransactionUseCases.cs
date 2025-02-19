/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionUseCases                   License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Use cases used to retrive budget transactions.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;
using Empiria.Services;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions.UseCases {

  /// <summary>Use cases used to retrive budget transactions.</summary>
  public class BudgetTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetTransactionUseCases() {
      // no-op
    }

    static public BudgetTransactionUseCases UseCaseInteractor() {
      return CreateInstance<BudgetTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BudgetEntryDto GetBudgetEntry(string budgetTransactionUID, string budgetEntryUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(budgetEntryUID, nameof(budgetEntryUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      BudgetEntry budgetEntry = transaction.GetEntry(budgetEntryUID);

      return BudgetEntryMapper.Map(budgetEntry);
    }


    public FixedList<BudgetTypeForEditionDto> GetBudgetTypesForEdition() {
      FixedList<Budget> budgets = Budget.GetList()
                                        .FindAll(x => x.EditionAllowed);

      return BudgetTransactionMapper.MapBudgetTypesForEdition(budgets.SelectDistinct(x => x.BudgetType));
    }


    public FixedList<NamedEntityDto> GetOperationSources() {
      return OperationSource.GetList()
                            .MapToNamedEntityList();
    }


    public BudgetTransactionHolderDto GetTransaction(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      return BudgetTransactionMapper.Map(transaction);
    }


    public FixedList<NamedEntityDto> GetTransactionTypes(string budgetTypeUID) {
      Assertion.Require(budgetTypeUID, nameof(budgetTypeUID));

      var budgetType = BudgetType.Parse(budgetTypeUID);

      return BudgetTransactionType.GetList(budgetType)
                                  .MapToNamedEntityList();
    }


    public FixedList<BudgetTransactionDescriptorDto> SearchTransactions(BudgetTransactionsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      FixedList<BudgetTransaction> transactions = BudgetTransactionDataService.SearchTransactions(filter, sort);

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }


    public FixedList<NamedEntityDto> SearchTransactionsParties(TransactionPartiesQuery query) {
      var persons = BaseObject.GetList<Person>();

      return persons.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class BudgetTransactionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
