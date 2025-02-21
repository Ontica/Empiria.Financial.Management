/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetTransactionMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetTransaction instances to data transfer objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;
using Empiria.Parties;

using Empiria.Budgeting.Adapters;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps BudgetTransaction instances to data transfer objects.</summary>
  static public class BudgetTransactionMapper {

    #region Public mappers

    static internal BudgetTransactionHolderDto Map(BudgetTransaction transaction) {
      return new BudgetTransactionHolderDto {
        Transaction = MapTransaction(transaction),
        Entries = BudgetEntryMapper.MapToDescriptor(transaction.Entries),
        Documents = DocumentServices.GetEntityDocuments(transaction),
        History = HistoryServices.GetEntityHistory(transaction),
        Actions = MapActions(transaction)
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
        OperationSourceName = transaction.OperationSource.Name,
        BasePartyName = transaction.BaseParty.Name,
        ApplicationDate = transaction.ApplicationDate,
        RequestedDate = transaction.RequestedTime,
        StatusName = transaction.Status.GetName(),
      };
    }


    static public FixedList<BudgetTransactionDescriptorDto> MapToDescriptor(FixedList<BudgetTransaction> transactions) {
      return transactions.Select(x => MapToDescriptor(x)).ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private BudgetTransactionActions MapActions(BudgetTransaction transaction) {
      bool canAuthorize = transaction.Status == BudgetTransactionStatus.OnAuthorization ||
                          transaction.Status == BudgetTransactionStatus.Pending;

      return new BudgetTransactionActions {
        CanAuthorize = canAuthorize,
        CanReject = canAuthorize,
        CanDelete = transaction.Status == BudgetTransactionStatus.Pending,
        CanUpdate = canAuthorize,
        CanEditDocuments = true
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
         TransactionTypes = MapTransactionTypes(BudgetTransactionType.GetList(budget.BudgetType)),
         SegmentTypes = BudgetSegmentTypesMapper.Map(budget.BudgetType.SegmentTypes),
      };
    }


    static private FixedList<TransactionTypeForEditionDto> MapTransactionTypes(FixedList<BudgetTransactionType> budgetTransactionTypes) {
      return budgetTransactionTypes.Select(x => MapTransactionTypeForEdition(x))
                                   .ToFixedList();
    }


    static private TransactionTypeForEditionDto MapTransactionTypeForEdition(BudgetTransactionType x) {
      return new TransactionTypeForEditionDto {
        UID = x.UID,
        Name = x.DisplayName,
        OperationSources = OperationSource.GetList().MapToNamedEntityList(),
        RelatedDocumentTypes = x.RelatedDocumentTypes.MapToNamedEntityList()
      };
    }


    static private BudgetTransactionDto MapTransaction(BudgetTransaction transaction) {
      return new BudgetTransactionDto {
        UID = transaction.UID,
        TransactionType = transaction.BudgetTransactionType.MapToNamedEntity(),
        BudgetType = transaction.BaseBudget.BudgetType.MapToNamedEntity(),
        Budget = transaction.BaseBudget.MapToNamedEntity(),
        TransactionNo = transaction.TransactionNo,
        Description = transaction.Description,
        OperationSource = transaction.OperationSource.MapToNamedEntity(),
        BaseParty = transaction.BaseParty.MapToNamedEntity(),
        ApplicationDate = transaction.ApplicationDate,
        RequestedDate = transaction.RequestedTime,
        Status = transaction.Status.MapToNamedEntity()
      };
    }

    #endregion Helpers

  }  // class BudgetTransactionMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
