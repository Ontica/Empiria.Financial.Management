/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetEntryByYearMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps budget transaction entries with values for a whole year.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Maps budget transaction entries with values for a whole year.</summary>
  static public class BudgetEntryByYearMapper {

    #region Mappers

    static internal BudgetEntryByYearDto Map(BudgetEntryByYear entry) {
      return new BudgetEntryByYearDto {
        UID = entry.UID,
        TransactionUID = entry.Transaction.UID,
        BalanceColumn = entry.BalanceColumn.MapToNamedEntity(),
        BudgetAccount = entry.BudgetAccount.MapToNamedEntity(),
        Party = entry.BudgetAccount.OrganizationalUnit.MapToNamedEntity(),
        Product = entry.Product.MapToNamedEntity(),
        Description = entry.Description,
        ProductUnit = entry.ProductUnit.MapToNamedEntity(),
        Justification = entry.Justification,
        Project = entry.Project.MapToNamedEntity(),
        Year = entry.Year,
        Currency = entry.Currency.MapToNamedEntity(),
        Amounts = MapAmounts(entry.Entries),
      };
    }

    #endregion Mappers

    #region Helpers

    static private FixedList<BudgetMonthEntryDto> MapAmounts(FixedList<BudgetEntry> entries) {
      return entries.Select(x => Map(x))
                    .ToFixedList();
    }

    static private BudgetMonthEntryDto Map(BudgetEntry entry) {
      return new BudgetMonthEntryDto {
        BudgetEntryUID = entry.UID,
        Month = entry.Month,
        ProductQty = entry.ProductQty,
        Amount = entry.Amount,
      };
    }

    #endregion Helpers

  }  // class BudgetEntryByYearMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
