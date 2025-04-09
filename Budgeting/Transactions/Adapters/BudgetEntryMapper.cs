/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetEntryMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetEntry instances to data transfer objects.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

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
        TransactionUID = entry.BudgetTransaction.UID,
        BudgetAccount = entry.BudgetAccount.MapToNamedEntity(),
        Product = entry.Product.MapToNamedEntity(),
        ProductUnit = entry.ProductUnit.MapToNamedEntity(),
        ProductQty = entry.ProductQty,
        Project = entry.Project.MapToNamedEntity(),
        Party = entry.Party.MapToNamedEntity(),
        Description = entry.Description,
        Justification = entry.Justification,
        Year = entry.Year,
        Month = new NamedEntityDto(entry.Month.ToString(), entry.MonthName),
        Day = entry.Day,
        Currency = entry.Currency.MapToNamedEntity(),
        BalanceColumn = entry.BalanceColumn.MapToNamedEntity(),
        OriginalAmount = entry.OriginalAmount,
        Amount = entry.Amount,
        ExchangeRate = entry.ExchangeRate,
        Status = entry.Status.MapToNamedEntity(),
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
