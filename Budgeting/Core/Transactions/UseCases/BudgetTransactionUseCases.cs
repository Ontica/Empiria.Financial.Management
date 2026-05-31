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

using Empiria.Financial;

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


    public FixedList<BudgetTypeForEditionDto> GetBudgetTypesForTransactionEdition(TransactionStage stage) {

      FixedList<Budget> budgets = Budget.GetList()
                                        .FindAll(x => x.AvailableTransactionTypes.Count > 0);

      var principal = ExecutionServer.CurrentPrincipal;

      FixedList<BudgetTransactionType> txnTypes = budgets.SelectDistinctFlat(x => x.AvailableTransactionTypes)
                                                         .FindAll(x => x.ManualEdition)
                                                         .FindAll(x => !x.IsProtected ||
                                                                       principal.IsInRole("budget-manager") ||
                                                                       principal.IsInRole("budget-authorizer"));

      if (stage == TransactionStage.Planning) {
        txnTypes = txnTypes.FindAll(x => x.OperationType == BudgetOperationType.Plan);
      }

      return BudgetTransactionMapper.MapBudgetTransactionTypesForEdition(txnTypes);
    }


    public FixedList<NamedEntityDto> GetOperationSources() {
      return OperationSource.GetList()
                            .MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetOrgUnitsForTransactionEdition(string budgetUID,
                                                                      string transactionTypeUID,
                                                                      TransactionStage stage) {

      Assertion.Require(budgetUID, nameof(budgetUID));
      Assertion.Require(transactionTypeUID, nameof(transactionTypeUID));

      var party = Party.ParseWithContact(ExecutionServer.CurrentContact);

      var orgUnits = Accountability.GetCommissionersFor<OrganizationalUnit>(party, "budgeting");

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

      var searcher = new BudgetAccountSearcher(query);

      FixedList<BudgetAccount> accounts = searcher.Search();

      if (query.GetTransactionType().OperationType != BudgetOperationType.Plan) {
        return BudgetAccountMapper.Map(accounts);
      }

      FixedList<StandardAccount> availableAccounts = searcher.SearchAvailable();

      return BudgetAccountMapper.Map(accounts, availableAccounts, query.GetBaseParty());
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
