/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetEntryMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetEntry instances to data transfer objects.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps BudgetEntry instances to data transfer objects.</summary>
  static internal class BudgetEntryMapper {

    #region Mappers

    static internal FixedList<BudgetEntryDto> Map(FixedList<BudgetEntry> entries) {
      return entries.Select(x => Map(x)).ToFixedList();
    }


    static internal BudgetEntryDto Map(BudgetEntry entry) {
      return new BudgetEntryDto {
        UID = entry.UID,
        //TransactionType = transaction.BudgetTransactionType.MapToNamedEntity(),
        //BudgetType = transaction.BaseBudget.BudgetType.MapToNamedEntity(),
        //Budget = transaction.BaseBudget.MapToNamedEntity(),
        //TransactionNo = transaction.TransactionNo,
        //Description = transaction.Description,
        //OperationSource = transaction.OperationSource.MapToNamedEntity(),
        //BaseParty = transaction.BaseParty.MapToNamedEntity(),
        //ApplicationDate = transaction.ApplicationDate,
        //RequestedDate = transaction.RequestedTime,
        //Status = transaction.Status.MapToNamedEntity(),
      };
    }


    static internal FixedList<BudgetEntryDescriptorDto> MapToDescriptor(FixedList<BudgetEntry> entries) {
      return entries.Select(x => MapToDescriptor(x)).ToFixedList();
    }


    static private BudgetEntryDescriptorDto MapToDescriptor(BudgetEntry entry) {
      return new BudgetEntryDescriptorDto {
        UID = entry.UID,
        BudgetAccountCode = entry.BudgetAccount.Code,
        BudgetAccountName = entry.BudgetAccount.Name,
        Year = entry.Year,
        Month = entry.Month,
        MonthName = entry.MonthName,
        Day = entry.Day,
        BalanceColumn = entry.BalanceColumn.Name,
        Deposit = entry.Deposit,
        Withdrawal = entry.Withdrawal,
      };
    }

    #endregion Mappers

  }  // class BudgetEntryMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
