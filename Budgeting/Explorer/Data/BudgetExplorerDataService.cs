/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Data Service                            *
*  Type     : BudgetExplorerDataService                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for the budget explorer.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Parties;

using Empiria.Budgeting.Transactions;

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


    static internal FixedList<BudgetDataInColumns> GetBudgetDataInMultipleColumnsByMonth(Budget budget) {
      Assertion.Require(budget, nameof(budget));

      var sql = "SELECT * FROM vw_Budget_Multicolumn_By_Month " +
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


    static internal FixedList<BudgetEntry> GetBudgetEntries(OrganizationalUnit orgUnit,
                                                            BudgetAccount account,
                                                            int year, int month) {
      var filter = new Filter();

      if (!orgUnit.IsEmptyInstance) {
        filter.AppendAnd($"ACCT_ORG_UNIT_ID = {orgUnit.Id}");
      }
      if (!account.IsEmptyInstance) {
        filter.AppendAnd($"BDG_ENTRY_BUDGET_ACCT_ID = {account.Id}");
      }
      if (year > 0) {
        filter.AppendAnd($"BDG_ENTRY_YEAR = {year}");
      }
      if (month > 0) {
        filter.AppendAnd($"BDG_ENTRY_MONTH = {month}");
      }

      filter.AppendAnd($"BDG_ENTRY_STATUS = 'C'");

      var sql = "SELECT BDG_ENTRY_ID FROM VW_BUDGET_TRANSACTIONS_GRAL " +
                $"WHERE {filter.ToString()} " +
                $"ORDER BY BDG_ENTRY_YEAR, BDG_ENTRY_MONTH, BDG_TXN_APPLICATION_DATE";

      var op = DataOperation.Parse(sql);

      var entriesIds = DataReader.GetFieldValues<int>(op).ToFixedList();

      return entriesIds.Select(x => BudgetEntry.Parse(x)).ToFixedList();
    }


    static internal FixedList<BudgetTransaction> GetBudgetTransactions(OrganizationalUnit orgUnit,
                                                                       BudgetAccount account,
                                                                       int year, int month) {
      var filter = new Filter();

      if (!orgUnit.IsEmptyInstance) {
        filter.AppendAnd($"ACCT_ORG_UNIT_ID = {orgUnit.Id}");
      }
      if (!account.IsEmptyInstance) {
        filter.AppendAnd($"BDG_ENTRY_BUDGET_ACCT_ID = {account.Id}");
      }
      if (year > 0) {
        filter.AppendAnd($"BDG_ENTRY_YEAR = {year}");
      }
      if (month > 0) {
        filter.AppendAnd($"BDG_ENTRY_MONTH = {month}");
      }

      filter.AppendAnd($"BDG_ENTRY_STATUS = 'C'");

      var sql = "SELECT BDG_ENTRY_TXN_ID FROM VW_BUDGET_TRANSACTIONS_GRAL " +
                $"WHERE {filter.ToString()} " +
                $"ORDER BY BDG_ENTRY_YEAR, BDG_ENTRY_MONTH, BDG_TXN_APPLICATION_DATE";

      var op = DataOperation.Parse(sql);

      var txnsIds = DataReader.GetFieldValues<int>(op).ToFixedList();

      return txnsIds.SelectDistinct(x => BudgetTransaction.Parse(x)).ToFixedList();
    }


    static internal FixedList<BudgetDataInColumns> GetMonthBalances(Budget budget) {
      Assertion.Require(budget, nameof(budget));

      var sql = "SELECT * FROM vw_Budget_Multicolumn_By_Year " +
                $"WHERE BUDGET_ID = {budget.Id} AND " +
                $"BUDGET_YEAR = {budget.Year} " +
                "ORDER BY BUDGET_MONTH";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<BudgetDataInColumns>(op);
    }

  }  // class BudgetTransactionDataService

}  // namespace Empiria.Budgeting.Explorer.Data
