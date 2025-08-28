/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Enumeration                          *
*  Type     : CashAccountStatus                             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Enumerates the cash account status in a cash ledger transaction entry.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Enumerates the cash account status in a cash ledger transaction entry.</summary>
  public enum CashAccountStatus {

    CashAccountWaiting = -2,

    NoCashAccount = -1,

    CashAccountPending = 0,

    WithCashAccount = 1,

    FalsePositives = -3,

    All = 255

  }  // enum CashAccountStatus

}  // namespace Empiria.CashFlow.CashLedger.Adapters
