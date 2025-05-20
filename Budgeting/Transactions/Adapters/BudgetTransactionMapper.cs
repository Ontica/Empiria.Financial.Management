/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetTransactionMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetTransaction instances to data transfer objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History.Services;
using Empiria.StateEnums;

using Empiria.Budgeting.Adapters;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps BudgetTransaction instances to data transfer objects.</summary>
  static public class BudgetTransactionMapper {

    #region Public mappers

    static internal BudgetTransactionHolderDto Map(BudgetTransaction transaction) {
      var byYearTransaction = new BudgetTransactionByYear(transaction);

      return new BudgetTransactionHolderDto {
        Transaction = MapTransaction(transaction),
        Entries = BudgetEntryMapper.MapToDescriptor(transaction.Entries),
        GroupedEntries = new BudgetEntriesByYearTableDto(byYearTransaction.GetEntries()),
        Documents = DocumentServices.GetAllEntityDocuments(transaction),
        History = HistoryServices.GetEntityHistory(transaction),
        Actions = MapActions(transaction.Rules)
      };
    }


    static internal FixedList<BudgetTypeForEditionDto> MapBudgetTypesForEdition(FixedList<BudgetType> budgetTypes) {
      return budgetTypes.Select(x => MapBudgetTypeForEdition(x))
                        .ToFixedList();
    }


    static public BudgetTransactionDescriptorDto MapToDescriptor(BudgetTransaction transaction) {
      return new BudgetTransactionDescriptorDto {
        UID = transaction.UID,
        TransactionTypeName = transaction.BudgetTransactionType.DisplayName,
        BudgetTypeName = transaction.BaseBudget.BudgetType.DisplayName,
        BudgetName = transaction.BaseBudget.Name,
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        Total = transaction.GetTotal(),
        OperationSourceName = transaction.OperationSource.Name,
        BasePartyName = transaction.BaseParty.Name,
        RecordingDate = transaction.RecordingDate,
        RecordedBy = transaction.RecordedBy.Name,
        RequestedDate = transaction.RequestedDate,
        RequestedBy = transaction.RequestedBy.Name,
        AuthorizationDate = transaction.AuthorizationDate,
        AuthorizedBy = transaction.AuthorizedBy.Name,
        ApplicationDate = transaction.ApplicationDate,
        AppliedBy = transaction.AppliedBy.Name,
        StatusName = transaction.Status.GetName(),
      };
    }


    static public FixedList<BudgetTransactionDescriptorDto> MapToDescriptor(FixedList<BudgetTransaction> transactions) {
      return transactions.Select(x => MapToDescriptor(x)).ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private BudgetTransactionActions MapActions(BudgetTransactionRules rules) {
      return new BudgetTransactionActions {
        CanAuthorize = rules.CanAuthorize,
        CanClose = rules.CanClose,
        CanDelete = rules.CanDelete,
        CanEditDocuments = rules.CanEditDocuments,
        CanReject = rules.CanReject,
        CanSendToAuthorization = rules.CanSendToAuthorization,
        CanUpdate = rules.CanUpdate
      };
    }


    static private BudgetTypeForEditionDto MapBudgetTypeForEdition(BudgetType budgetType) {
      return new BudgetTypeForEditionDto {
        UID = budgetType.UID,
        Name = budgetType.DisplayName,
        Multiyear = budgetType.Multiyear,
        Budgets = MapBudgetsForEdition(Budget.GetList(budgetType).FindAll(x => x.EditionAllowed))
      };
    }


    static private FixedList<BudgetForEditionDto> MapBudgetsForEdition(FixedList<Budget> budgets) {
      return budgets.Select(x => MapBudgetForEdition(x))
                    .ToFixedList();
    }


    static private BudgetForEditionDto MapBudgetForEdition(Budget budget) {
      return new BudgetForEditionDto {
         UID = budget.UID,
         Name = budget.Name,
         Year = budget.Year,
         Type = budget.BudgetType.MapToNamedEntity(),
         TransactionTypes = MapTransactionTypes(budget),
         SegmentTypes = BudgetSegmentTypesMapper.Map(budget.BudgetType.SegmentTypes),
      };
    }


    static private FixedList<TransactionTypeForEditionDto> MapTransactionTypes(Budget budget) {
      var principal = ExecutionServer.CurrentPrincipal;

      return budget.AvailableTransactionTypes.Select(x => BudgetTransactionType.Parse(x.UID))
                                             .ToFixedList()
                                             .FindAll(x => !x.IsProtected ||
                                                           principal.IsInRole("budget-manager") ||
                                                           principal.IsInRole("budget-authorizer"))
                                             .Select(x => MapTransactionTypeForEdition(BudgetTransactionType.Parse(x.UID)))
                                             .ToFixedList();
    }


    static private TransactionTypeForEditionDto MapTransactionTypeForEdition(BudgetTransactionType txnType) {
      return new TransactionTypeForEditionDto {
        UID = txnType.UID,
        Name = txnType.DisplayName,
        OperationSources = txnType.OperationSources.MapToNamedEntityList(),
        RelatedDocumentTypes = txnType.RelatedDocumentTypes.MapToNamedEntityList(),
        EntriesRules = MapTransactionTypeEntriesRules(txnType)
      };
    }


    static private TransactionTypeEntriesRulesDto MapTransactionTypeEntriesRules(BudgetTransactionType txnType) {
      return new TransactionTypeEntriesRulesDto {
        BalanceColumns = txnType.BalanceColumns.MapToNamedEntityList(),
        SelectProduct = txnType.SelectProduct,
        Years = txnType.AvailableYears
      };
    }


    static private BudgetTransactionDto MapTransaction(BudgetTransaction transaction) {
      return new BudgetTransactionDto {
        UID = transaction.UID,
        TransactionType = MapTransactionTypeForEdition(transaction.BudgetTransactionType),
        TransactionNo = transaction.TransactionNo,
        BudgetType = transaction.BaseBudget.BudgetType.MapToNamedEntity(),
        Budget = transaction.BaseBudget.MapToNamedEntity(),
        Description = transaction.Description,
        Justification = transaction.Justification,
        OperationSource = transaction.OperationSource.MapToNamedEntity(),
        BaseParty = transaction.BaseParty.MapToNamedEntity(),
        BaseEntityType = transaction.HasEntity ?
            transaction.GetEntity().GetEmpiriaType().MapToNamedEntity() : NamedEntityDto.Empty,
        BaseEntity = transaction.HasEntity ?
            ((INamedEntity) transaction.GetEntity()).MapToNamedEntity() : NamedEntityDto.Empty,
        Total = transaction.GetTotal(),
        RecordingDate = transaction.RequestedDate,
        RecordedBy = transaction.RecordedBy.MapToNamedEntity(),
        RequestedDate = transaction.RequestedDate,
        RequestedBy = transaction.RequestedBy.MapToNamedEntity(),
        AuthorizationDate = transaction.AuthorizationDate,
        AuthorizedBy = transaction.AuthorizedBy.MapToNamedEntity(),
        ApplicationDate = transaction.ApplicationDate,
        AppliedBy = transaction.AppliedBy.MapToNamedEntity(),
        Status = transaction.Status.MapToNamedEntity()
      };
    }

    #endregion Helpers

  }  // class BudgetTransactionMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
