/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Output DTO                              *
*  Type     : CashTransactionAnalysisEntry               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that holds a result entry of a cash leger transaction analysis.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO that holds a result entry of a cash leger transaction analysis.</summary>
  public class CashTransactionAnalysisEntry {

    public string EntryLabel {
      get; internal set;
    }

    public string Currency {
      get; internal set;
    }

    public int TotalEntries {
      get; internal set;
    }

    public decimal Debits {
      get; internal set;
    }

    public decimal Credits {
      get; internal set;
    }

    public decimal Difference {
      get {
        return Debits - Credits;
      }
    }

  }  // class CashTransactionAnalysisEntry

}  // namespace Empiria.CashFlow.CashLedger.Adapters
