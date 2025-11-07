/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Information Holder                      *
*  Type     : CashFlowExplorerEntry                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a cash flow dynamic explorer entry.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Financial.Concepts;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Holds a cash flow dynamic explorer entry</summary>
  public class CashFlowExplorerEntry {

    public string CashFlowAcct_01 {
      get {
        return MainClassification.GetLevel(1).Name;
      }
    }

    public string CashFlowAcct_02 {
      get {
        return MainClassification.GetLevel(2).Name;
      }
    }

    public string CashFlowAcct_03 {
      get {
        return MainClassification.GetLevel(3).Name;
      }
    }

    public string CashFlowAcct_04 {
      get {
        return MainClassification.GetLevel(4).Name;
      }
    }

    public string CashFlowAcct_05 {
      get {
        return MainClassification.GetLevel(5).Name;
      }
    }

    public string CashFlowAcct_06 {
      get {
        return MainClassification.GetLevel(6).Name;
      }
    }

    public int CashAccountId {
      get; internal set;
    }

    public string CashAccountNo {
      get; internal set;
    }

    public string ConceptDescription {
      get {
        if (CashAccountId <= 0) {
          return CashAccountNo;
        }
        return CashAccount.Parent.Description;
      }
    }

    public string StandardAccountNo {
      get {
        if (CashAccountId <= 0) {
          return CashAccountNo;
        }
        return CashAccount.StandardAccount.StdAcctNo;
      }
    }

    public string OrganizationalUnit {
      get {
        if (CashAccountId <= 0) {
          return "No aplica";
        }
        return CashAccount.OrganizationalUnit.FullName;
      }
    }

    public string Program {
      get {
        if (CashAccountId <= 0) {
          return "No aplica";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("program"),
                                                   CashAccount.StandardAccount.StdAcctNo.Substring(0, 2));

        return ((INamedEntity) segment).Name;
      }
    }


    public string Subprogram {
      get {
        if (CashAccountId <= 0) {
          return "No aplica";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("subprogram"),
                                                   CashAccount.StandardAccount.StdAcctNo.Substring(3, 2));

        return ((INamedEntity) segment).Name;
      }
    }


    public string FinancingSource {
      get {
        if (CashAccountId <= 0) {
          return "No aplica";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("financingSource"),
                                                   CashAccount.StandardAccount.StdAcctNo.Substring(12, 2));

        return ((INamedEntity) segment).Name;
      }
    }

    public string OperationType {
      get {
        if (CashAccountId <= 0) {
          return "No aplica";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("operationType"),
                                                   CashAccount.StandardAccount.StdAcctNo.Substring(15, 2));

        return ((INamedEntity) segment).Name;
      }
    }

    public string CurrencyCode {
      get; internal set;
    }

    public decimal TotalPlanned {
      get; internal set;
    } = EmpiriaMath.GetRandom(100000, 99999999);


    public decimal TotalAuthorized {
      get; internal set;
    } = EmpiriaMath.GetRandom(100000, 99999999);


    public decimal PeriodAuthorized {
      get; internal set;
    } = EmpiriaMath.GetRandom(100000 / 12, 99999999 / 12);


    public decimal YtdAuthorized {
      get; internal set;
    } = EmpiriaMath.GetRandom(100000 * 7 / 12, 99999999 * 7 / 12);


    public decimal PeriodTotal {
      get {
        switch (CurrencyCode) {
          case "MXN":
            return PeriodTotalOriginalCurrency;

          case "UDI":
            return PeriodTotalOriginalCurrency * 8.525333m;

          case "USD":
            return PeriodTotalOriginalCurrency * 18.868000m;

          case "EUR":
            return PeriodTotalOriginalCurrency * 21.596310m;

          case "JPY":
            return PeriodTotalOriginalCurrency * 0.125470m;

          default:
            return PeriodTotalOriginalCurrency;
        }
      }
    }


    public decimal PeriodTotalOriginalCurrency {
      get {
        if (MainClassification.ConceptNo.StartsWith("1")) {
          return Inflows;
        } else if (MainClassification.ConceptNo.StartsWith("2")) {
          return Outflows;
        } else {
          return Inflows - Outflows;
        }
      }
    }


    public decimal YtdTotal {
      get {
        return EmpiriaMath.GetRandom(100000 * 6 / 12, 99999999 * 6 / 12) + PeriodTotal;
      }
    }


    public decimal PeriodDifference {
      get {
        return PeriodTotal - PeriodAuthorized;
      }
    }


    public decimal YtdDifference {
      get {
        return YtdTotal - YtdAuthorized;
      }
    }

    public decimal Inflows {
      get; internal set;
    }

    public decimal Outflows {
      get; private set;
    }

    internal void Sum(CashAccountTotalDto entry) {
      Inflows += entry.Inflows;
      Outflows += entry.Outflows;
    }

    internal FinancialAccount CashAccount {
      get {
        if (CashAccountId <= 0) {
          return FinancialAccount.Empty;
        }
        return FinancialAccount.Parse(CashAccountId);
      }
    }

    internal FinancialConcept MainClassification {
      get {
        if (CashAccountId <= 0) {
          return FinancialConcept.Empty;
        }
        return CashAccount.StandardAccount.MainClassification;
      }
    }

  }  // class CashFlowExplorerEntry

}  // namespace Empiria.CashFlow.Explorer
