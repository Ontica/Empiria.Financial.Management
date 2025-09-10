/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Query DTO                               *
*  Type     : CashFlowExplorerQuery                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve cash flow explorer information.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Explorer.Adapters {

  public enum CashFlowReportType {

    AccountTotals,

    CashFlow,

    ConceptAnalytic,

    ConceptDetail,

    ConceptTotals,

    ProjectTotals,

    None

  }  // CashFlowReportType



  /// <summary>Input query DTO used to retrieve cash flow explorer information.</summary>
  public class CashFlowExplorerQuery : BaseCashLedgerTotalsQuery {

    public CashFlowReportType ReportType {
      get; set;
    } = CashFlowReportType.None;


    public string ProgramUID {
      get; set;
    } = string.Empty;


    public string SubprogramUID {
      get; set;
    } = string.Empty;


    public string FinancingSourceUID {
      get; set;
    } = string.Empty;


    public string OperationTypeUID {
      get; set;
    } = string.Empty;


    public string PartyUID {
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
