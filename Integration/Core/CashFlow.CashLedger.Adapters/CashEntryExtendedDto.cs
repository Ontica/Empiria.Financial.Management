/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.Core.dll        Pattern   : Output DTO                           *
*  Type     : CashEntryExtendedDto                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger entries with transaction information.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO used to retrieve cash ledger entries with transaction information.</summary>
  public class CashEntryExtendedDto : BaseCashEntryDto {

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

  }  // class CashEntryExtendedDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
