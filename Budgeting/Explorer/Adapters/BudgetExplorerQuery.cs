/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Query DTO                               *
*  Type     : BudgetExplorerQuery                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to explore budget information.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Enumeration values that describe budget reports.</summary>
  public enum BudgetReportType {

    ByColumn,

    MonthlyAvailability,

    SaldosOperacion,

  }  // enum BudgetReportType



  /// <summary>Input query DTO used to explore budget information.</summary>
  public class BudgetExplorerQuery {

    public BudgetReportType ReportType {
      get; set;
    } = BudgetReportType.ByColumn;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string[] BaseParties {
      get; set;
    } = new string[0];


    public string[] BudgetAccounts {
      get; set;
    } = new string[0];


    public BudgetExplorerGroupBy GroupByColumn {
      get; set;
    } = BudgetExplorerGroupBy.AREA_PARTIDA;

  }  // class BudgetExplorerQuery

}  // namespace Empiria.Budgeting.Explorer.Adapters
