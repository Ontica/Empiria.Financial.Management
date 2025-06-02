/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Mapper                                  *
*  Type     : CashFlowProjectionEntryByYearMapper        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps cash flow projection's entries with values for a whole year.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Maps cash flow projection's entries with values for a whole year.</summary>
  static public class CashFlowProjectionEntryByYearMapper {

    #region Mappers

    static internal CashFlowProjectionEntryByYearDto Map(CashFlowProjectionEntryByYear entry) {
      return new CashFlowProjectionEntryByYearDto {
        UID = entry.UID,
        ProjectionUID = entry.Projection.UID,
        ProjectionColumn = entry.ProjectionColumn.MapToNamedEntity(),
        CashFlowAccount = entry.CashFlowAccount.MapToNamedEntity(),
        Product = entry.Product.MapToNamedEntity(),
        Description = entry.Description,
        ProductUnit = entry.ProductUnit.MapToNamedEntity(),
        Justification = entry.Justification,
        Year = entry.Year,
        Currency = entry.Currency.MapToNamedEntity(),
        Amounts = MapAmounts(entry.Entries),
      };
    }

    #endregion Mappers

    #region Helpers

    static private FixedList<CashFlowProjectionMonthEntryDto> MapAmounts(FixedList<CashFlowProjectionEntry> entries) {
      return entries.Select(x => Map(x))
                    .ToFixedList();
    }

    static private CashFlowProjectionMonthEntryDto Map(CashFlowProjectionEntry entry) {
      return new CashFlowProjectionMonthEntryDto {
        BudgetEntryUID = entry.UID,
        Month = entry.Month,
        ProductQty = entry.ProductQty,
        Amount = entry.Amount,
      };
    }

    #endregion Helpers

  }  // class CashFlowProjectionEntryByYearMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
