/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Information Holder                      *
*  Type     : CashFlowExplorerEntry                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a cash flow dynamic explorer entry.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Holds a cash flow dynamic explorer entry</summary>
  public class CashFlowExplorerEntry {

    public string CashAccountNo {
      get; internal set;
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

  }  // class CashFlowExplorerEntry

}  // namespace Empiria.CashFlow.Explorer
