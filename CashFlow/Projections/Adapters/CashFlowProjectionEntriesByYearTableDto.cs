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

    private readonly FixedList<CashFlowProjectionEntryByYear> _entries;

    internal CashFlowProjectionEntriesByYearTableDto(FixedList<CashFlowProjectionEntryByYear> entries) {
      _entries = entries;
    }

    public FixedList<DataTableColumn> Columns {
      get {
        return BuildColumns();
      }
    }


    public FixedList<CashFlowProjectionEntryByYearDynamicDto> Entries {
      get {
        return BuildEntriesAndTotals();
      }
    }


    private FixedList<DataTableColumn> BuildColumns() {
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

      FixedList<int> months = _entries.SelectDistinctFlat(x => x.Entries.Select(y => y.Month))
                                      .Sort((x, y) => x.CompareTo(y));

      foreach (int month in months) {
        columns.Add(new DataTableColumn($"month_{month}",
                                        EmpiriaString.MonthName(month).Substring(0, 3), "text") {
          Align = "right"
        });
      }

      return columns.ToFixedList();
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildEntries() {
      return _entries.Select(x => new CashFlowProjectionEntryByYearDynamicDto(x))
                    .ToFixedList()
                    .Sort((x, y) => x.CashFlowAccount.CompareTo(y.CashFlowAccount));
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildEntriesAndTotals() {
      FixedList<CashFlowProjectionEntryByYearDynamicDto> entries = BuildEntries();

      var list = new List<CashFlowProjectionEntryByYearDynamicDto>(entries);

      FixedList<CashFlowProjectionEntryByYearDynamicDto> totals = BuildTotals();

      list.AddRange(totals);

      list.AddRange(BuildVariables());

      return list.ToFixedList();
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


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildVariables() {
      var currencies = _entries.SelectDistinct(x => x.Currency).FindAll(x => x.Distinct(Currency.Default));

      if (currencies.Count() == 0) {
        return new FixedList<CashFlowProjectionEntryByYearDynamicDto>();
      }

      var variables = new List<CashFlowProjectionEntryByYearDynamicDto>(currencies.Count());

      foreach (var currency in currencies) {
        decimal[] exchangeRates = FinancialVariables.GetExchangeRates(_entries[0].Year, currency);

        var currencyVariable = new CashFlowProjectionEntryByYearDynamicDto($"Tipo de cambio {currency.ISOCode}",
                                                                           exchangeRates);

        variables.Add(currencyVariable);
      }

      return variables.ToFixedList();
    }


  }  // class CashFlowProjectionEntriesByYearTableDto


  /// <summary>Dynamic fields DTO that holds a yearly cash flow projection entry with months in columns.</summary>
  public class CashFlowProjectionEntryByYearDynamicDto : DynamicFields {

    internal CashFlowProjectionEntryByYearDynamicDto(CashFlowProjectionEntryByYearDynamicDto pivot,
                                                     FixedList<CashFlowProjectionEntryByYearDynamicDto> fields) {
      UID = $"{pivot.Year}|{pivot.ProjectionColumn}";
      ItemType = DataTableEntryType.Total.ToString();
      ItemDescription = $"{pivot.CashFlowPlanName} {pivot.ProjectionColumn}";
      ProjectionColumn = pivot.ProjectionColumn;
      Year = pivot.Year;

      for (int i = 1; i <= 12; i++) {
        decimal inflows = fields.FindAll(x => x.IsInflowAccount).Sum(x => decimal.Parse(x.GetField($"Month_{i}", "0")));
        decimal outflows = fields.FindAll(x => !x.IsInflowAccount).Sum(x => decimal.Parse(x.GetField($"Month_{i}", "0")));
        if (inflows != 0 || outflows != 0) {
          base.SetField($"Month_{i}", (inflows - outflows).ToString("N0"));
        }
      }

      Currency = pivot.Currency;

      decimal inflowTotal = fields.FindAll(x => x.IsInflowAccount)
                                  .Sum(x => decimal.Parse(x.GetField("InflowAmount", "0")));

      base.SetField("InflowAmount", inflowTotal.ToString("N0"));

      decimal outflowTotal = fields.FindAll(x => !x.IsInflowAccount)
                                   .Sum(x => decimal.Parse(x.GetField("OutflowAmount", "0")));

      base.SetField("OutflowAmount", outflowTotal.ToString("N0"));
    }


    internal CashFlowProjectionEntryByYearDynamicDto(CashFlowProjectionEntryByYear entry) {
      UID = entry.UID;
      ItemType = DataTableEntryType.Group1.ToString();
      ItemDescription = ((INamedEntity) entry.CashFlowAccount).Name;
      ProjectionUID = entry.Projection.UID;
      CashFlowPlanName = entry.Projection.Plan.Name;
      ProjectionColumn = entry.ProjectionColumn.Name;
      CashFlowAccount = ((INamedEntity) entry.CashFlowAccount).Name;
      IsInflowAccount = entry.CashFlowAccount.IsInflowAccount;
      Product = entry.Product.Name;
      Description = entry.Description;
      ProductUnit = entry.ProductUnit.Name;
      Justification = entry.Justification;
      Year = entry.Year;
      Currency = entry.Currency.ISOCode;

      decimal total = 0m;
      for (int i = 1; i <= 12; i++) {
        decimal amount = entry.GetAmountForMonth(i);
        if (amount != 0) {
          base.SetField($"Month_{i}", amount.ToString("N0"));
        }
        total += amount;
      }

      if (IsInflowAccount) {
        base.SetField("InflowAmount", total.ToString("N0"));
      } else {
        base.SetField("OutflowAmount", total.ToString("N0"));
      }
    }

    public CashFlowProjectionEntryByYearDynamicDto(string description, decimal[] decimals) {
      UID = string.Empty;
      ItemType = DataTableEntryType.Entry.ToString();
      ItemDescription = description;
      ProjectionUID = string.Empty;
      CashFlowPlanName = string.Empty;
      ProjectionColumn = string.Empty;
      CashFlowAccount = string.Empty;
      IsInflowAccount = false;
      Product = string.Empty;
      Description = string.Empty;
      ProductUnit = string.Empty;
      Justification = string.Empty;
      Year = 2027;
      Currency = Financial.Currency.Default.ISOCode;

      for (int i = 0; i < decimals.Length && i < 12; i++) {
        if (decimals[i] != 0) {
          base.SetField($"Month_{i + 1}", decimals[i].ToString("N6"));
        }
      }
    }

    public string UID {
      get;
    }

    public string ItemType {
      get;
    }

    public string ItemDescription {
      get;
    } = string.Empty;


    public string ProjectionUID {
      get;
    } = string.Empty;


    public string CashFlowPlanName {
      get;
    }


    public string CashFlowAccount {
      get;
    } = string.Empty;


    public bool IsInflowAccount {
      get;
    }


    public string ProjectionColumn {
      get;
    } = string.Empty;


    public string Product {
      get;
    } = string.Empty;


    public string Description {
      get;
    } = string.Empty;


    public string ProductUnit {
      get;
    } = string.Empty;


    public string Justification {
      get;
    } = string.Empty;


    public int Year {
      get;
    }

    public string Currency {
      get;
    } = string.Empty;


    public CashFlowProjectionEntryDtoType EntryType {
      get;
    } = CashFlowProjectionEntryDtoType.Annually;

    public override IEnumerable<string> GetDynamicMemberNames() {
      var members = new List<string> {
        "UID",
        "ProjectionUID",
        "ItemType",
        "ItemDescription",
        "CashFlowAccount",
        "ProjectionColumn",
        "Product",
        "Description",
        "ProductUnit",
        "Justification",
        "Year",
        "Currency",
        "EntryType",
      };

      members.AddRange(base.GetDynamicMemberNames());

      return members;
    }

  }  // CashFlowProjectionEntryByYearDynamicDto

}  // namespace Empiria.CashFlow.Projections.Adapters
