/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Output DTO                           *
*  Type     : CashEntryDescriptor                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger transaction entries for use in lists.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO used to retrieve cash ledger transaction entries for use in lists.</summary>
  public class CashEntryDescriptor : BaseCashTransactionEntryDto {

    public long TransactionId {
      get; set;
    }

    public string TransactionNumber {
      get; set;
    }

    public string TransactionLedgerName {
      get; set;
    }

    public string TransactionConcept {
      get; set;
    }

    public DateTime TransactionAccountingDate {
      get; set;
    }

    public DateTime TransactionRecordingDate {
      get; set;
    }

  }  // class CashEntryDescriptor

}  // namespace Empiria.CashFlow.CashLedger.Adapters
