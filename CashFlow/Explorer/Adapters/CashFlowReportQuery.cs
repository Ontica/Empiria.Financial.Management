/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Query DTO                               *
*  Type     : CashFlowReportQuery                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve cash flow related reports.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.CashFlow.Explorer.Adapters {

  public enum CashFlowReportType {

    ConceptAnalytic,

    ConceptDetail,

    None

  }  // CashFlowReportType



  /// <summary>Input query DTO used to retrieve cash flow related reports information.</summary>
  public class CashFlowReportQuery : IQuery {

    public CashFlowReportType ReportType {
      get; set;
    } = CashFlowReportType.None;


    public DateTime FromDate {
      get; set;
    } = DateTime.MaxValue;


    public DateTime ToDate {
      get; set;
    } = DateTime.MaxValue;


    public string Keywords {
      get; set;
    } = string.Empty;


    public string AccountingLedgerUID {
      get; set;
    } = string.Empty;


    public string[] Accounts {
      get; set;
    } = new string[0];


    public string[] SubledgerAccounts {
      get; set;
    } = new string[0];


    public string OperationTypeUID {
      get; set;
    } = string.Empty;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public string ProgramUID {
      get; set;
    } = string.Empty;


    public string SubprogramUID {
      get; set;
    } = string.Empty;


    public string FinancingSourceUID {
      get; set;
    } = string.Empty;


    public string ProjectTypeUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string FinancialAccountUID {
      get; set;
    } = string.Empty;

  }  // class CashFlowExplorerQuery

}  // namespace Empiria.CashFlow.Explorer.Adapters
