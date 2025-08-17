/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Adpaters Layer                          *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Input fields                            *
*  Type     : SharedCashEntryFields                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to update cash ledger transaction entries.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.FinancialAccounting.CashLedger.Adapters {

  /// <summary>Input fields used to update cash ledger transaction entries.</summary>
  public class SharedCashEntryFields {

    public long EntryId {
      get; set;
    }

    public long CashAccountId {
      get; set;
    }

    public long TransactionId {
      get; set;
    }

  }  // class SharedCashEntryFields

}  // namespace Empiria.FinancialAccounting.CashLedger.Adapters
