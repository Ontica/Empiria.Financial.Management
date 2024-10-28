/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetTransactionMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetTransaction instances to data transfer objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps BudgetTransaction instances to data transfer objects.</summary>
  static internal class BudgetTransactionMapper {

    #region Mappers

    static internal FixedList<BudgetTransactionDto> Map(FixedList<BudgetTransaction> transactions) {
      return transactions.Select(x => Map(x)).ToFixedList();
    }


    static internal BudgetTransactionDto Map(BudgetTransaction transaction) {
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
        Status = transaction.Status.MapToNamedEntity(),
      };
    }


    static internal FixedList<BudgetTransactionDescriptorDto> MapToDescriptor(FixedList<BudgetTransaction> transactions) {
      return transactions.Select(x => MapToDescriptor(x)).ToFixedList();
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

    #endregion Mappers

  }  // class BudgetTransactionMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
