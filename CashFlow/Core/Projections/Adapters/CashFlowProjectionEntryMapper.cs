/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Mapper                                  *
*  Type     : CashFlowProjectionEntryMapper              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetEntry instances to data transfer objects.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;
using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Describes a cash flow projection entry type.</summary>
  public enum CashFlowProjectionEntryDtoType {

    Monthly,

    Annually,

  }  // enum CashFlowProjectionEntryDtoType



  /// <summary>Maps CashFlowProjectionEntry instances to data transfer objects.</summary>
  static internal class CashFlowProjectionEntryMapper {

    #region Mappers

    static internal FixedList<CashFlowProjectionEntryDto> Map(FixedList<CashFlowProjectionEntry> entries) {
      return entries.Select(x => Map(x))
                    .ToFixedList();
    }


    static internal CashFlowProjectionEntryDto Map(CashFlowProjectionEntry entry) {
      return new CashFlowProjectionEntryDto {
        UID = entry.UID,
        ProjectionUID = entry.Projection.UID,
        CashFlowAccount = entry.CashFlowAccount.MapToNamedEntity(),
        Product = entry.Product.MapToNamedEntity(),
        ProductUnit = entry.ProductUnit.MapToNamedEntity(),
        ProductQty = entry.ProductQty,
        Description = entry.Description,
        Justification = entry.Justification,
        Year = entry.Year,
        Month = new NamedEntityDto(entry.Month.ToString(), entry.MonthName),
        Currency = entry.Currency.MapToNamedEntity(),
        ProjectionColumn = entry.ProjectionColumn.MapToNamedEntity(),
        OriginalAmount = entry.OriginalAmount,
        Amount = entry.Amount,
        ExchangeRate = entry.ExchangeRate,
        Status = entry.Status.MapToNamedEntity()
      };
    }


    static internal FixedList<CashFlowProjectionEntryDescriptor> MapToDescriptor(FixedList<CashFlowProjectionEntry> entries) {
      var list = new List<CashFlowProjectionEntryDescriptor>(entries.Count + 1);

      entries.Sort((x, y) => $"{x.CashFlowAccount.AccountNo}.{x.Year}.{x.Month.ToString("00")}"
                             .CompareTo($"{y.CashFlowAccount.AccountNo}.{y.Year}.{y.Month.ToString("00")}"));

      list.AddRange(entries.Select(x => MapToDescriptor(x)));

      list.AddRange(GetTotalEntries(entries));

      return list.ToFixedList();
    }

    #endregion Mappers

    #region Helpers

    static private FixedList<CashFlowProjectionEntryDescriptor> GetTotalEntries(FixedList<CashFlowProjectionEntry> entries) {
      var groups = entries.GroupBy(x => $"{x.Year}|{x.ProjectionColumn.Name}");

      var list = new List<CashFlowProjectionEntryDescriptor>(groups.Count());

      foreach (var group in groups) {
        var dto = MapToDescriptorTotals(group.First(), group.ToFixedList());
        list.Add(dto);
      }

      return list.ToFixedList();
    }


    static private CashFlowProjectionEntryDescriptor MapToDescriptor(CashFlowProjectionEntry entry) {
      return new CashFlowProjectionEntryDescriptor {
        UID = entry.UID,
        CashFlowAccountCode = entry.CashFlowAccount.AccountNo,
        CashFlowAccountName = entry.CashFlowAccount.Name,
        Year = entry.Year,
        Month = entry.Month,
        MonthName = entry.MonthName,
        ProjectionColumn = entry.ProjectionColumn.Name,
        InflowAmount = entry.InflowAmount,
        OutflowAmount = entry.OutflowAmount,
        ItemType = DataTableEntryType.Entry.ToString()
      };
    }


    static private CashFlowProjectionEntryDescriptor MapToDescriptorTotals(CashFlowProjectionEntry pivot,
                                                                           FixedList<CashFlowProjectionEntry> items) {
      return new CashFlowProjectionEntryDescriptor {
        UID = $"{pivot.Year}|{pivot.ProjectionColumn.Name}",
        CashFlowAccountCode = string.Empty,
        CashFlowAccountName = $"{pivot.Projection.Plan.Name} {pivot.ProjectionColumn.Name}",
        Year = pivot.Year,
        ItemType = DataTableEntryType.Total.ToString(),
        ProjectionColumn = pivot.ProjectionColumn.Name,
        InflowAmount = items.Sum(x => x.InflowAmount),
        OutflowAmount = items.Sum(x => x.OutflowAmount)
      };
    }

    #endregion Helpers

  }  // class CashFlowProjectionEntryMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
