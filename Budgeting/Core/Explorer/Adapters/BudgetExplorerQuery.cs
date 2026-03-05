/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Query DTO                               *
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



  public class ExplorerBreakdownQuery {

    public BudgetExplorerQuery Query {
      get; set;
    }

    public BreakdownSubQuery SubQuery {
      get; set;
    }

    public BreakdownEntry Entry {
      get; set;
    }

  }


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


  public class BreakdownSubQuery {

    public string ReportType {
      get; set;
    } = string.Empty;

  }  // class BreakdownSubQuery



  public class BreakdownEntry {

    public string UID {
      get; set;
    } = string.Empty;


    public int Year {
      get; set;
    }

    public int Month {
      get; set;
    }

  }  // class BreakdownEntry

}  // namespace Empiria.Budgeting.Explorer.Adapters
