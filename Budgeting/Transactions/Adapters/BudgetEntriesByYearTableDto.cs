/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Dynamic Output DTO                      *
*  Type     : BudgetEntriesByYearTableDto                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic table output DTO with budget entries information.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Dynamic table output DTO with budget entries information.</summary>
  public class BudgetEntriesByYearTableDto {

    private readonly FixedList<BudgetEntryByYear> _entries;

    internal BudgetEntriesByYearTableDto(FixedList<BudgetEntryByYear> entries) {
      _entries = entries;
    }

    public FixedList<DataTableColumn> Columns {
      get {
        return BuildColumns();
      }
    }


    public FixedList<BudgetEntryByYearDynamicDto> Entries {
      get {
        return BuildEntriesAndTotals();
      }
    }


    private FixedList<DataTableColumn> BuildColumns() {
      var columns = new List<DataTableColumn> {
        new DataTableColumn("itemDescription", "Partida presupuestal", "text-italic"),
        new DataTableColumn("budgetProgram", "Programa", "text-no-wrap"),
      };

      if (_entries.SelectDistinctFlat(x => x.Entries.Select(y => y.Year)).Count() >= 2) {
        columns.Add(new DataTableColumn("year", "Año", "text-nowrap"));
      }

      if (_entries.SelectDistinctFlat(x => x.Entries.Select(y => y.BalanceColumn)).Count() >= 2) {
        columns.Add(new DataTableColumn("balanceColumn", "Movimiento", "text-nowrap"));
      }

      columns.Add(new DataTableColumn("total", "Total", "decimal"));

      FixedList<int> months = _entries.SelectDistinctFlat(x => x.Entries.Select(y => y.Month))
                                      .Sort((x, y) => x.CompareTo(y));

      foreach (int month in months) {
        columns.Add(new DataTableColumn($"month_{month}",
                                        EmpiriaString.MonthName(month).Substring(0, 3), "decimal"));
      }

      return columns.ToFixedList();
    }

    private FixedList<BudgetEntryByYearDynamicDto> BuildEntries() {
      return _entries.Select(x => new BudgetEntryByYearDynamicDto(x))
                    .ToFixedList()
                    .Sort((x, y) => x.BudgetAccount.CompareTo(y.BudgetAccount));
    }


    private FixedList<BudgetEntryByYearDynamicDto> BuildEntriesAndTotals() {
      FixedList<BudgetEntryByYearDynamicDto> entries = BuildEntries();

      var list = new List<BudgetEntryByYearDynamicDto>(entries);

      FixedList<BudgetEntryByYearDynamicDto> totals = BuildTotals();

      list.AddRange(totals);

      return list.ToFixedList();
    }


    private FixedList<BudgetEntryByYearDynamicDto> BuildTotals() {

      var entries = BuildEntries();

      if (entries.Count() == 1) {
        return new FixedList<BudgetEntryByYearDynamicDto>();
      }

      var groups = entries.GroupBy(x => $"{x.Year}|{x.BalanceColumn}");

      var totals = new List<BudgetEntryByYearDynamicDto>(groups.Count());

      foreach (var group in groups) {

        var total = new BudgetEntryByYearDynamicDto(group.First(), group.ToFixedList());

        totals.Add(total);
      }

      return totals.ToFixedList();
    }

  }  // class BudgetEntriesByYearTableDto



  /// <summary>Dynamic fields DTO that holds a year budget entry with months in columns.</summary>
  public class BudgetEntryByYearDynamicDto : DynamicFields {

    internal BudgetEntryByYearDynamicDto(BudgetEntryByYearDynamicDto pivot,
                                         FixedList<BudgetEntryByYearDynamicDto> fields) {
      UID = $"{pivot.Year}|{pivot.BalanceColumn}";
      ItemType = DataTableEntryType.Total.ToString();
      ItemDescription = $"Presupuesto {pivot.BalanceColumn} {pivot.Year}";
      BalanceColumn = pivot.BalanceColumn;
      Year = pivot.Year;

      decimal total = 0m;
      for (int i = 1; i <= 12; i++) {
        decimal amount = fields.Sum(x => x.GetTotalField($"Month_{i}"));
        if (amount != 0) {
          base.SetTotalField($"Month_{i}", amount);
        }
        total += amount;
      }
      base.SetTotalField("Total", total);
    }


    internal BudgetEntryByYearDynamicDto(BudgetEntryByYear entry) {
      UID = entry.UID;
      ItemType = DataTableEntryType.Entry.ToString();
      ItemDescription = entry.BudgetAccount.Name;
      TransactionUID = entry.Transaction.UID;
      BalanceColumn = entry.BalanceColumn.Name;
      BudgetAccount = entry.BudgetAccount.Name;
      BudgetProgram = entry.BudgetAccount.BudgetProgram;
      Product = entry.Product.Name;
      Description = entry.Description;
      ProductUnit = entry.ProductUnit.Name;
      Justification = entry.Justification;
      Project = entry.Project.Name;
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
      base.SetTotalField("Total", total);
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


    public string TransactionUID {
      get;
    } = string.Empty;


    public string BudgetAccount {
      get;
    } = string.Empty;


    public string BudgetProgram {
      get; private set;
    } = string.Empty;


    public string BalanceColumn {
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


    public string Project {
      get;
    } = string.Empty;


    public int Year {
      get;
    }

    public string Currency {
      get;
    } = string.Empty;


    public BudgetEntryDtoType EntryType {
      get;
    } = BudgetEntryDtoType.Annually;


    public override IEnumerable<string> GetDynamicMemberNames() {
      var members = new List<string> {
        "UID",
        "TransactionUID",
        "ItemType",
        "ItemDescription",
        "BudgetAccount",
        "BudgetProgram",
        "BalanceColumn",
        "Product",
        "Description",
        "ProductUnit",
        "Justification",
        "Project",
        "Year",
        "Currency",
        "EntryType",
      };

      members.AddRange(base.GetDynamicMemberNames());

      return members;
    }

  }  // BudgetEntryByYearDynamicDto

}  // namespace Empiria.Budgeting.Transactions.Adapters
