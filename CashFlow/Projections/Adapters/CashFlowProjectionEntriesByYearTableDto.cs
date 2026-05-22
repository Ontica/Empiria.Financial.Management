/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Dynamic Output DTO                      *
*  Type     : CashFlowProjectionEntriesByYearTableDto    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic table output DTO with cash flow projection's entries grouped by year.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;
using Empiria.Financial;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Dynamic table output DTO with cash flow projection's entries grouped by year.</summary>
  public class CashFlowProjectionEntriesByYearTableDto {

    private readonly CashFlowProjectionByYear _byYearProjection;
    private readonly FixedList<CashFlowProjectionEntryByYear> _entries;

    internal CashFlowProjectionEntriesByYearTableDto(CashFlowProjectionByYear byYearProjection) {
      Assertion.Require(byYearProjection, nameof(byYearProjection));

      _byYearProjection = byYearProjection;
      _entries = byYearProjection.GetEntries();
    }

    public FixedList<DataTableColumn> Columns {
      get {
        var columns = new List<DataTableColumn> {
          new DataTableColumn("itemDescription", "Concepto financiero", "text-italic"),
          new DataTableColumn("year", "Año", "text-nowrap")
        };

        if (_entries.SelectDistinctFlat(x => x.Entries.Select(y => y.ProjectionColumn)).Count() >= 2) {

          columns.Add(new DataTableColumn("projectionColumn", "Movimiento", "text-nowrap"));
        }

        columns.Add(new DataTableColumn("currency", "Mon", "text"));
        columns.Add(new DataTableColumn("inflowAmount", "Ingresos", "text") { Align = "right" });
        columns.Add(new DataTableColumn("outflowAmount", "Egresos", "text") { Align = "right" });

        FixedList<int> months = _byYearProjection.GetMonths();

        foreach (int month in months) {

          var columnName = EmpiriaString.MonthName(month)
                                        .Substring(0, 3);

          var monthColumn = new DataTableColumn($"month_{month}", columnName, "text") { Align = "right" };

          columns.Add(monthColumn);
        }

        return columns.ToFixedList();
      }
    }


    public FixedList<CashFlowProjectionEntryByYearDynamicDto> Entries {
      get {
        FixedList<CashFlowProjectionEntryByYearDynamicDto> entries = BuildEntries();

        var list = new List<CashFlowProjectionEntryByYearDynamicDto>(entries);

        FixedList<CashFlowProjectionEntryByYearDynamicDto> totals = BuildTotals();

        list.AddRange(totals);

        if (_byYearProjection.HasForeignCurrencies()) {

          list.AddRange(BuildExchangeRates());
        }

        if (_byYearProjection.HasCalculationVariables()) {

          list.AddRange(BuildCalculationVariables());
        }

        if (_byYearProjection.HasForeignCurrencies()) {

          list.Add(BuildMXNTotal());
        }

        return list.ToFixedList();
      }
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildEntries() {
      return _entries.Select(x => new CashFlowProjectionEntryByYearDynamicDto(x))
                     .ToFixedList()
                     .Sort((x, y) => x.CashFlowAccount.CompareTo(y.CashFlowAccount));
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildTotals() {

      FixedList<CashFlowProjectionEntryByYearDynamicDto> entries = BuildEntries();

      if (entries.Count() == 0) {
        return new FixedList<CashFlowProjectionEntryByYearDynamicDto>();
      }

      var groups = entries.GroupBy(x => $"{x.Year}|{x.ProjectionColumn}");

      var totals = new List<CashFlowProjectionEntryByYearDynamicDto>(groups.Count());

      foreach (var group in groups) {

        var total = new CashFlowProjectionEntryByYearDynamicDto(group.First(), group.ToFixedList());

        totals.Add(total);
      }

      return totals.ToFixedList();
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildCalculationVariables() {
      return new FixedList<CashFlowProjectionEntryByYearDynamicDto>();
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildExchangeRates() {

      FixedList<(Currency, decimal[])> exchangeRates = _byYearProjection.GetExchangeRates(_entries[0].Year);

      if (exchangeRates.Count() == 0) {
        return new FixedList<CashFlowProjectionEntryByYearDynamicDto>();
      }

      var tableEntries = new List<CashFlowProjectionEntryByYearDynamicDto>();

      foreach (var exchangeRate in exchangeRates) {

        var tableEntry = new CashFlowProjectionEntryByYearDynamicDto($"Tipo de cambio {exchangeRate.Item1.ISOCode}",
                                                                     DataTableEntryType.Group1, exchangeRate.Item2, "N6");

        tableEntries.Add(tableEntry);
      }

      return tableEntries.ToFixedList();
    }


    private CashFlowProjectionEntryByYearDynamicDto BuildMXNTotal() {

      decimal[] mxnTotals = _byYearProjection.GetMXNTotals(_entries[0].Year);

      var dynDto = new CashFlowProjectionEntryByYearDynamicDto("Total en pesos mexicanos",
                                                               DataTableEntryType.Summary, mxnTotals);

      dynDto.SetField("InflowAmount", _byYearProjection.InflowsTotal.ToString("N0"));
      dynDto.SetField("OutflowAmount", _byYearProjection.OutflowsTotal.ToString("N0"));

      return dynDto;
    }

  }  // class CashFlowProjectionEntriesByYearTableDto

}  // namespace Empiria.CashFlow.Projections.Adapters
