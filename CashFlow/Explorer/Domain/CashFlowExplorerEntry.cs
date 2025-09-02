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

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Holds a cash flow dynamic explorer entry</summary>
  public class CashFlowExplorerEntry {

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
        return Account.Parent.Description;
      }
    }

    public string StandardAccountNo {
      get {
        if (CashAccountId <= 0) {
          return CashAccountNo;
        }
        return Account.StandardAccount.StdAcctNo;
      }
    }

    public string Program {
      get {
        if (CashAccountId <= 0) {
          return "N/D";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("program"),
                                                    Account.StandardAccount.StdAcctNo.Substring(0, 2));

        return ((INamedEntity) segment).Name;
      }
    }


    public string Subprogram {
      get {
        if (CashAccountId <= 0) {
          return "N/D";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("subprogram"),
                                                   Account.StandardAccount.StdAcctNo.Substring(3, 2));

        return ((INamedEntity) segment).Name;
      }
    }


    public string FinancingSource {
      get {
        if (CashAccountId <= 0) {
          return "N/D";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("financingSource"),
                                                   Account.StandardAccount.StdAcctNo.Substring(12, 2));

        return ((INamedEntity) segment).Name;
      }
    }

    public string OperationType {
      get {
        if (CashAccountId <= 0) {
          return "N/D";
        }
        var segment = StandardAccountSegment.Parse(StandardAccountCategory.ParseWithNamedKey("operationType"),
                                                   Account.StandardAccount.StdAcctNo.Substring(15, 2));

        return ((INamedEntity) segment).Name;
      }
    }

    public string CurrencyCode {
      get; internal set;
    }

    public decimal Inflows {
      get; private set;
    }

    public decimal Outflows {
      get; private set;
    }


    internal void Sum(CashLedgerTotalEntryDto entry) {
      Inflows += entry.Debit;
      Outflows += entry.Credit;
    }

    private FinancialAccount Account {
      get {
        if (CashAccountId <= 0) {
          return FinancialAccount.Empty;
        }
        return FinancialAccount.Parse(CashAccountId);
      }
    }

  }  // class CashFlowExplorerEntry

}  // namespace Empiria.CashFlow.Explorer
