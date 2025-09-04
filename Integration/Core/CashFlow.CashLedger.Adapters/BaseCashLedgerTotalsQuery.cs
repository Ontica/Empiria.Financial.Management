/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Query DTO                               *
*  Type     : BaseCashLedgerTotalsQuery                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Base input query DTO used to retrieve cash ledger totals.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Base input query DTO used to retrieve cash ledger totals.</summary>
  public class BaseCashLedgerTotalsQuery {

    public string AccountingLedgerUID {
      get; set;
    } = string.Empty;


    public DateTime FromAccountingDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToAccountingDate {
      get; set;
    } = ExecutionServer.DateMinValue;

  }  // class BaseCashLedgerTotalsQuery

}  // namespace Empiria.CashFlow.CashLedger.Adapters
