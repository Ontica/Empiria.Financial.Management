/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetTransactionMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetTransaction instances to data transfer objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

using Empiria.Documents.Services;
using Empiria.History.Services;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps BudgetTransaction instances to data transfer objects.</summary>
  static internal class BudgetTransactionMapper {

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

    static internal FixedList<BudgetTransactionDescriptorDto> MapToDescriptor(FixedList<BudgetTransaction> transactions) {
      return transactions.Select(x => MapToDescriptor(x)).ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private BudgetTransactionActions MapActions(BudgetTransaction transaction) {
      return new BudgetTransactionActions {
        CanAuthorize = true,
        CanEditDocuments = true
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


    static private BudgetTransactionDescriptorDto MapToDescriptor(BudgetTransaction transaction) {
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

    #endregion Helpers

  }  // class BudgetTransactionMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
