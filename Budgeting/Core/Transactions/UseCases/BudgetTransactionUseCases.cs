/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionUseCases                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve budget transactions.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.HumanResources;
using Empiria.Parties;
using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Budgeting.Transactions.UseCases {

  /// <summary>Use cases used to retrieve budget transactions.</summary>
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


    public FixedList<BudgetTypeForEditionDto> GetBudgetTypesForTransactionEdition() {
      FixedList<Budget> budgets = Budget.GetList()
                                        .FindAll(x => x.EditionAllowed)
                                        .FindAll(x => x.AvailableTransactionTypes.Contains(y => y.ManualEdition));

      return BudgetTransactionMapper.MapBudgetTypesForEdition(budgets.SelectDistinct(x => x.BudgetType));
    }


    public FixedList<NamedEntityDto> GetOperationSources() {
      return OperationSource.GetList()
                            .MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetOrgUnitsForTransactionEdition(string budgetUID,
                                                                      string transactionTypeUID) {

      Assertion.Require(budgetUID, nameof(budgetUID));
      Assertion.Require(transactionTypeUID, nameof(transactionTypeUID));

      var party = Party.ParseWithContact(ExecutionServer.CurrentContact);

      var orgUnits =
        Accountability.GetCommissionersFor<OrganizationalUnit>(party, "budgeting",
                                                               BudgetTransactionRules.ACQUISITION_MANAGER);

      return orgUnits.Select(x => new NamedEntityDto(x.UID, x.FullName))
                     .ToFixedList();
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


    public FixedList<BudgetAccountDto> SearchBudgetAccounts(BudgetAccountsQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureValid();

      var searcher = new BudgetAccountSearcher(query.GetBaseBudget().BudgetType, query.Keywords);

      FixedList<BudgetAccount> accounts = searcher.Search(query.GetBaseParty(),
                                                          query.GetTransactionType().BudgetAccountsFilter);

      return BudgetAccountMapper.Map(accounts);
    }


    public FixedList<BudgetTransactionDescriptorDto> SearchTransactions(BudgetTransactionsQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<BudgetTransaction> transactions = query.Execute();

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }


    public FixedList<NamedEntityDto> SearchTransactionsParties(TransactionPartiesQuery query) {
      var persons = BaseObject.GetList<Person>();

      return persons.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class BudgetTransactionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
