/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Input fields                            *
*  Type     : CashEntryFields                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to update cash ledger transaction entries.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Input fields used to update cash ledger transaction entries.</summary>
  public class CashEntryFields {

    public long TransactionId {
      get; set;
    }

    public long EntryId {
      get; set;
    }

    public int CashAccountId {
      get; set;
    }

    public string CashAccountNo {
      get; set;
    }

    public string AppliedRule {
      get; set;
    }

    public int UserId {
      get; set;
    }

  }  // class CashEntryFields

}  // namespace Empiria.CashFlow.CashLedger.Adapters
