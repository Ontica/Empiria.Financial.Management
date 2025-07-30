/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Output DTO                              *
*  Type     : CashTransactionDescriptor                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger transactions for use in lists.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO used to retrieve cash ledger transactions for use in lists.</summary>
  public class CashTransactionDescriptor {

    public long Id {
      get; set;
    }

    public string Number {
      get; set;
    }

    public string LedgerName {
      get; set;
    }

    public string Concept {
      get; set;
    }

    public string TransactionTypeName {
      get; set;
    }

    public string VoucherTypeName {
      get; set;
    }

    public string SourceName {
      get; set;
    }

    public DateTime AccountingDate {
      get; set;
    }

    public DateTime RecordingDate {
      get; set;
    }

    public string ElaboratedBy {
      get; set;
    }

    public string StatusName {
      get; set;
    }

  }  // class CashTransactionDescriptor

}  // namespace Empiria.CashFlow.CashLedger.Adapters
