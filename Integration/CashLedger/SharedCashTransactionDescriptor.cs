/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Output DTO                           *
*  Type     : SharedCashTransactionDescriptor               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger transactions.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Integration.CashLedger {

  /// <summary>Output DTO used to retrieve cash ledger transactions for use in lists.</summary>
  public class SharedCashTransactionDescriptor {

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

    public string Status {
      get; set;
    }

    public string StatusName {
      get; set;
    }

  }  // class SharedCashTransactionDescriptor

}  // namespace Empiria.Financial.Integration.CashLedger
