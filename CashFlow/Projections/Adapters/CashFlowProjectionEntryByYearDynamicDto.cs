/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Dynamic Output DTO                      *
*  Type     : CashFlowProjectionEntryByYearDynamicDto    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic output DTO with a yearly cash flow projection entry with months in columns.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Dynamic output DTO with a yearly cash flow projection entry with months in columns.</summary>
  public class CashFlowProjectionEntryByYearDynamicDto : DynamicFields {

    internal CashFlowProjectionEntryByYearDynamicDto(CashFlowProjectionEntryByYearDynamicDto pivot,
                                                     FixedList<CashFlowProjectionEntryByYearDynamicDto> fields) {
      UID = $"{pivot.Year}|{pivot.ProjectionColumn}";
      ItemType = DataTableEntryType.Total.ToString();
      ItemDescription = $"{pivot.CashFlowPlanName} {pivot.ProjectionColumn}";
      ProjectionColumn = pivot.ProjectionColumn;
      Year = pivot.Year;

      for (int i = 1; i <= 12; i++) {
        decimal inflows = fields.FindAll(x => x.IsInflowAccount)
                                .Sum(x => decimal.Parse(x.GetField($"Month_{i}", "0")));

        decimal outflows = fields.FindAll(x => !x.IsInflowAccount)
                                 .Sum(x => decimal.Parse(x.GetField($"Month_{i}", "0")));

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


    internal CashFlowProjectionEntryByYearDynamicDto(string description, DataTableEntryType entryType,
                                                     decimal[] decimals, string format = "N0") {
      UID = string.Empty;
      ItemType = entryType.ToString();
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
          base.SetField($"Month_{i + 1}", decimals[i].ToString(format));
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

  }  // class CashFlowProjectionEntryByYearDynamicDto

}  // namespace Empiria.CashFlow.Projections.Adapters
