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
using Empiria.Financial;
using Empiria.History;
using Empiria.StateEnums;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Budgeting.Adapters;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps BudgetTransaction instances to data transfer objects.</summary>
  static public class BudgetTransactionMapper {

    #region Public mappers

    static internal BudgetTransactionHolderDto Map(BudgetTransaction transaction) {
      var byYearTransaction = new BudgetTransactionByYear(transaction);

      FixedList<Bill> bills = FixedList<Bill>.Empty;

      FixedList<DocumentDto> documents = DocumentServices.GetAllEntityDocuments(transaction);

      if (transaction.HasEntity) {
        var payableEntities = transaction.GetEntity().GetPayableEntities();

        bills = Bill.GetListFor(payableEntities);

        var payableDocs = DocumentServices.GetAllEntityDocuments((BaseObject) transaction.GetEntity());

        if (payableDocs.Count != 0) {
          documents = FixedList<DocumentDto>.Merge(documents, payableDocs);
        }
      }

      var relatedTxns = BudgetTransaction.GetRelatedTo(transaction);

      return new BudgetTransactionHolderDto {
        Transaction = MapTransaction(transaction),
        Entries = BudgetEntryMapper.MapToDescriptor(transaction.Entries),
        GroupedEntries = new BudgetEntriesByYearTableDto(byYearTransaction.GetEntries()),
        Taxes = MapTaxes(transaction),
        Bills = BillMapper.MapToBillStructure(bills),
        RelatedTransactions = MapToDescriptor(relatedTxns),
        Documents = documents,
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
        TransactionTypeName = transaction.TransactionType.DisplayName,
        BudgetTypeName = transaction.BaseBudget.BudgetType.DisplayName,
        BudgetName = transaction.BaseBudget.Name,
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        Total = transaction.GetTotal(),
        BaseEntityNo = transaction.HasEntity ? transaction.GetEntity().EntityNo : string.Empty,
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
        RejectedReason = transaction.RejectedReason
      };
    }


    static public FixedList<BudgetTransactionDescriptorDto> MapToDescriptor(FixedList<BudgetTransaction> transactions) {
      return transactions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private BudgetTransactionActions MapActions(BudgetTransactionRules rules) {
      return new BudgetTransactionActions {
        CanAuthorize = rules.CanAuthorize,
        CanCancel = rules.CanCancel,
        CanClose = rules.CanClose,
        CanDelete = rules.CanDelete,
        CanEditDocuments = rules.CanEditDocuments,
        CanReject = rules.CanReject,
        CanSendToAuthorization = rules.CanSendToAuthorization,
        CanUpdate = rules.CanUpdate
      };
    }


    static private BudgetTypeForEditionDto MapBudgetTypeForEdition(BudgetType budgetType) {

      var budgets = Budget.GetList(budgetType)
                          .FindAll(x => x.AvailableTransactionTypes.Contains(y => y.ManualEdition));

      return new BudgetTypeForEditionDto {
        UID = budgetType.UID,
        Name = budgetType.DisplayName,
        Multiyear = budgetType.Multiyear,
        Budgets = MapBudgetsForEdition(budgets)
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
        SegmentTypes = BudgetSegmentTypesMapper.Map(budget.BudgetType.StdAccountCategories),
      };
    }


    static private FixedList<TaxEntryDto> MapTaxes(BudgetTransaction transaction) {
      if (!transaction.HasEntity) {
        return FixedList<TaxEntryDto>.Empty;
      }
      FixedList<ITaxEntry> taxes = transaction.GetEntity().Taxes;

      return taxes.Select(x => MapTaxes(x))
                  .ToFixedList();
    }


    static private TaxEntryDto MapTaxes(ITaxEntry taxEntry) {
      return new TaxEntryDto {
        TaxType = taxEntry.TaxType.MapToNamedEntity(),
        Total = taxEntry.Total
      };
    }


    static private FixedList<TransactionTypeForEditionDto> MapTransactionTypes(Budget budget) {
      var principal = ExecutionServer.CurrentPrincipal;

      return budget.AvailableTransactionTypes.Select(x => BudgetTransactionType.Parse(x.UID))
                                             .ToFixedList()
                                             .FindAll(x => x.ManualEdition)
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
        AskForAllowsOverdrafts = txnType.AllowsOverdrafts,
        ManualEdition = txnType.ManualEdition,
        OperationSources = txnType.OperationSources.MapToNamedEntityList(),
        RelatedDocumentTypes = txnType.RelatedDocumentTypes.MapToNamedEntityList(),
        EntriesRules = MapTransactionTypeEntriesRules(txnType)
      };
    }


    static private TransactionTypeEntriesRulesDto MapTransactionTypeEntriesRules(BudgetTransactionType txnType) {
      return new TransactionTypeEntriesRulesDto {
        BalanceColumns = txnType.BalanceColumns.MapToNamedEntityList(),
        SelectProduct = txnType.SelectProduct,
        SelectParty = txnType.SelectParty,
        Years = txnType.AvailableYears
      };
    }


    static private BudgetTransactionDto MapTransaction(BudgetTransaction transaction) {
      return new BudgetTransactionDto {
        UID = transaction.UID,
        TransactionType = MapTransactionTypeForEdition(transaction.TransactionType),
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
            transaction.GetEntity().MapToNamedEntity() : NamedEntityDto.Empty,
        BaseEntityNo = transaction.HasEntity ? transaction.GetEntity().EntityNo : string.Empty,
        AllowsOverdrafts = transaction.AllowsOverdrafts,
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
