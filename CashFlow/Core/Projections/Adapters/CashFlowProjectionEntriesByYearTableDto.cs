/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Dynamic Output DTO                      *
*  Type     : CashFlowProjectionEntriesByYearTableDto    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic table output DTO with cash flow projection's entries grouped by year.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;

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

      columns.Add(new DataTableColumn("inflowAmount", "Ingresos", "decimal"));
      columns.Add(new DataTableColumn("outflowAmount", "Egresos", "decimal"));

      FixedList<int> months = _entries.SelectDistinctFlat(x => x.Entries.Select(y => y.Month))
                                      .Sort((x, y) => x.CompareTo(y));

      foreach (int month in months) {
        columns.Add(new DataTableColumn($"month_{month}",
                                        EmpiriaString.MonthName(month).Substring(0, 3), "decimal"));
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

      return list.ToFixedList();
    }


    private FixedList<CashFlowProjectionEntryByYearDynamicDto> BuildTotals() {

      var entries = BuildEntries();

      if (entries.Count() == 1) {
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
        decimal inflows = fields.FindAll(x => x.IsInflowAccount).Sum(x => x.GetTotalField($"Month_{i}"));
        decimal outflows = fields.FindAll(x => !x.IsInflowAccount).Sum(x => x.GetTotalField($"Month_{i}"));
        if (inflows != 0 || outflows != 0) {
          base.SetTotalField($"Month_{i}", inflows - outflows);
        }
      }

      decimal inflowTotal = fields.FindAll(x => x.IsInflowAccount).Sum(x => x.GetTotalField("InflowAmount"));
      base.SetTotalField("InflowAmount", inflowTotal);

      decimal outflowTotal = fields.FindAll(x => !x.IsInflowAccount).Sum(x => x.GetTotalField("OutflowAmount"));
      base.SetTotalField("OutflowAmount", outflowTotal);
    }


    internal CashFlowProjectionEntryByYearDynamicDto(CashFlowProjectionEntryByYear entry) {
      UID = entry.UID;
      ItemType = DataTableEntryType.Entry.ToString();
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
          base.SetTotalField($"Month_{i}", amount);
        }
        total += amount;
      }

      if (IsInflowAccount) {
        base.SetTotalField("InflowAmount", total);
      } else {
        base.SetTotalField("OutflowAmount", total);
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
