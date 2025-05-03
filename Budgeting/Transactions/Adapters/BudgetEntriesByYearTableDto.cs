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
        return GetColumns();
      }
    }


    public FixedList<BudgetEntryByYearDynamicDto> Entries {
      get {
        return _entries.Select(x => new BudgetEntryByYearDynamicDto(x))
                       .ToFixedList()
                       .Sort((x, y) => x.BudgetAccount.CompareTo(y.BudgetAccount));
      }
    }

    private FixedList<DataTableColumn> GetColumns() {
      var columns = new List<DataTableColumn> {
        new DataTableColumn("budgetAccount", "Partida presupuestal", "text-link"),
        new DataTableColumn("year", "Año", "text-nowrap"),
        new DataTableColumn("balanceColumn", "Movimiento", "text-nowrap")
      };

      FixedList<int> months = _entries.SelectDistinctFlat(x => x.Entries.Select(y => y.Month))
                                      .Sort((x, y) => x.CompareTo(y));

      foreach (int month in months) {
        columns.Add(new DataTableColumn($"month_{month}",
                                        EmpiriaString.MonthName(month).Substring(0, 3), "decimal"));
      }

      return columns.ToFixedList();
    }

  }  // class BudgetEntriesByYearTableDto



  /// <summary>Dynamic fields DTO that holds a year budget entry with months in columns.</summary>
  public class BudgetEntryByYearDynamicDto : DynamicFields {

    internal BudgetEntryByYearDynamicDto(BudgetEntryByYear entry) {
      UID = entry.UID;
      TransactionUID = entry.Transaction.UID;
      BalanceColumn = entry.BalanceColumn.Name;
      BudgetAccount = entry.BudgetAccount.Name;
      Product = entry.Product.Name;
      Description = entry.Description;
      ProductUnit = entry.ProductUnit.Name;
      Justification = entry.Justification;
      Project = entry.Project.Name;
      Year = entry.Year;
      Currency = entry.Currency.ISOCode;

      for (int i = 1; i <= 12; i++) {
        decimal amount = entry.GetAmountForMonth(i);
        if (amount != 0) {
          base.SetTotalField($"Month_{i}", amount);
        }
      }
    }

    public string UID {
      get;
    }

    public BudgetEntryDtoType EntryType {
      get;
    } = BudgetEntryDtoType.Annually;


    public string TransactionUID {
      get;
    }

    public string BalanceColumn {
      get;
    }

    public string BudgetAccount {
      get;
    }

    public string Product {
      get;
    }

    public string Description {
      get;
    }

    public string ProductUnit {
      get;
    }

    public string Justification {
      get;
    }

    public string Project {
      get;
    }

    public int Year {
      get;
    }

    public string Currency {
      get;
    }

    public override IEnumerable<string> GetDynamicMemberNames() {
      var members = new List<string> {
        "UID",
        "TransactionUID",
        "BalanceColumn",
        "BudgetAccount",
        "Product",
        "Description",
        "ProductUnit",
        "Justification",
        "Project",
        "Year",
        "Currency"
      };

      members.AddRange(base.GetDynamicMemberNames());

      return members;
    }

  }  // BudgetEntryByYearDynamicDto

}  // namespace Empiria.Budgeting.Transactions.Adapters
