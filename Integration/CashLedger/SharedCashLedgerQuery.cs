/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Query DTO                            *
*  Type     : SharedCashLedgerQuery                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve cash ledger transactions.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.Financial.Integration.CashLedger {

  /// <summary>Enumerates the cash account status in a cash ledger transaction entry.</summary>
  public enum SharedCashAccountStatus {

    CashAccountWaiting = -2,

    NoCashAccount = -1,

    CashAccountPending = 0,

    WithCashAccount = 1,

    All = 255

  }  // enum SharedCashAccountStatus


  /// <summary>Input query DTO used to retrieve cash ledger transactions.</summary>
  public class SharedCashLedgerQuery {

    public DateTime FromAccountingDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToAccountingDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string Keywords {
      get; set;
    } = string.Empty;


    public TransactionStatus TransactionStatus {
      get; set;
    } = TransactionStatus.All;


    public SharedCashAccountStatus CashAccountStatus {
      get; set;
    } = SharedCashAccountStatus.All;


    public string Concept {
      get; set;
    } = string.Empty;


    public string AccountingLedgerUID {
      get; set;
    } = string.Empty;


    public string[] CashAccounts {
      get; set;
    } = new string[0];


    public DateTime FromRecordingDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToRecordingDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string[] VoucherAccounts {
      get; set;
    } = new string[0];


    public string[] SubledgerAccounts {
      get; set;
    } = new string[0];


    public string[] VerificationNumbers {
      get; set;
    } = new string[0];


    public string TransactionTypeUID {
      get; set;
    } = string.Empty;


    public string VoucherTypeUID {
      get; set;
    } = string.Empty;


    public string SourceUID {
      get; set;
    } = string.Empty;


    public string OrderBy {
      get; set;
    } = string.Empty;


    public int PageSize {
      get; set;
    } = 10000;

  }  // class SharedCashLedgerQuery

}  // namespace Empiria.Financial.Integration.CashLedger
