/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Query DTO                               *
*  Type     : BudgetExplorerCommand                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Command information used by the BudgetExplorer.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

using Empiria.Budgeting.Explorer.Adapters;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Command information used by the BudgetExplorer.</summary>
  internal class BudgetExplorerCommand {

    public BudgetReportType ReportType {
      get; set;
    } = BudgetReportType.ByColumn;


    internal Budget Budget {
      get; set;
    }

    internal FixedList<OrganizationalUnit> OrganizationalUnits {
      get; set;
    }

    internal BudgetExplorerGroupBy GroupBy {
      get; set;
    }

    internal string[] BudgetAccounts {
      get; set;
    }

  }  // class BudgetExplorerCommand


}  // namespace Empiria.Budgeting.Explorer
