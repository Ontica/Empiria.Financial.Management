/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Data Service                            *
*  Type     : BudgetDataInColumns                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for the budget explorer.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Budgeting.Explorer.Data {

  /// <summary>Provides data access services for the budget explorer.</summary>
  static internal class BudgetExplorerDataService {

    static internal FixedList<BudgetDataInColumns> GetBudgetDataInMultipleColumns(Budget budget) {
      Assertion.Require(budget, nameof(budget));

      var sql = "SELECT * FROM vw_Budget_Multicolumn " +
                $"WHERE BUDGET_ID = {budget.Id}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<BudgetDataInColumns>(op);
    }


    static internal FixedList<BudgetDataInColumns> GetBudgetDataInMultipleColumnsByMonth(string filter) {
      Assertion.Require(filter, nameof(filter));

      var sql = "SELECT * FROM vw_Budget_Multicolumn_By_Month " +
                $"WHERE {filter}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<BudgetDataInColumns>(op);
    }

  }  // class BudgetTransactionDataService

}  // namespace Empiria.Budgeting.Explorer.Data
