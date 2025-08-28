/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Query DTO                            *
*  Type     : BaseCashLedgerQuery                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve cash ledger transactions.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Input query DTO used to retrieve cash ledger transactions.</summary>
  public class BaseCashLedgerQuery {

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


    public CashAccountStatus CashAccountStatus {
      get; set;
    } = CashAccountStatus.All;


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

  }  // class BaseCashLedgerQuery

}  // namespace Empiria.CashFlow.CashLedger.Adapters
