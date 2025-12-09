/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Information builder                     *
*  Type     : AvailableBudgetBuilder                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Builds available budget information.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Explorer.Adapters;

using Empiria.Budgeting.Explorer.Data;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Builds available budget information.</summary>
  internal class AvailableBudgetBuilder {

    private readonly AvailableBudgetQuery _query;

    internal AvailableBudgetBuilder(AvailableBudgetQuery query) {
      _query = query;
    }

    internal FixedList<BudgetDataInColumns> Build() {

      string filter = BuildFilter();

      return BudgetExplorerDataService.GetBudgetDataInMultipleColumns(filter);
    }

    #region Helpers

    private string BuildFilter() {
      string budgetFilter = BuildBudgetFilter();
      string yearFilter = BuildYearFilter();
      string accountsFilter = BuildAccountsFilter();

      var filter = new Filter(budgetFilter);

      filter.AppendAnd(yearFilter);
      filter.AppendAnd(accountsFilter);

      return filter.ToString();
    }

    private string BuildAccountsFilter() {
      if (_query.Accounts.Count == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseInSet("BUDGET_ACCT_ID", _query.Accounts.Select(x => x.Id));
    }


    private string BuildBudgetFilter() {
      if (_query.Budget.IsEmptyInstance) {
        return string.Empty;
      }

      return $"BUDGET_ID = {_query.Budget.Id}";
    }


    private string BuildYearFilter() {
      if (_query.Year == 0) {
        return string.Empty;
      }

      return $"BUDGET_YEAR = {_query.Year}";
    }

    #endregion Helpers

  }  // class AvailableBudgetBuilder

}  // namespace Empiria.Budgeting.Explorer
