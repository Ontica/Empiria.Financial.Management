/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Service provider                        *
*  Type     : BudgetAccountSearcher                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Searches budget accounts.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Data;

using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Budgeting {

  /// <summary>Searches budget accounts.</summary>
  public class BudgetAccountSearcher {

    private readonly BudgetAccountsQuery _query;

    public BudgetAccountSearcher(BudgetAccountsQuery query) {
      Assertion.Require(query, nameof(query));

      _query = query;
    }

    #region Methods

    public FixedList<BudgetAccount> Search() {

      string accountsFilter = GetAccountsFilter();
      string budgetTypeFilter = GetBudgetTypeFilter();
      string orgUnitFilter = GetOrgUnitFilter();
      string keywordsFilter = GetKeywordsFilter();
      string statusFilter = GetStatusFilter();

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(accountsFilter);
      filter.AppendAnd(statusFilter);
      filter.AppendAnd(keywordsFilter);

      return BudgetAccountData.SearchBudgetAccounts(filter.ToString(), "ACCT_NUMBER");
    }

    #endregion Methods

    #region Helpers

    private string GetAccountsFilter() {
      string accountsFilter = _query.GetTransactionType().BudgetAccountsFilter;

      accountsFilter = EmpiriaString.Clean(accountsFilter ?? string.Empty);
      accountsFilter = accountsFilter.Replace("{{ACCOUNT.CODE.FIELD}}", "ACCT_NUMBER");

      return accountsFilter;
    }


    private string GetBudgetTypeFilter() {
      return $"STD_ACCT_TYPE_ID = {_query.GetBaseBudget().BudgetType.StandardAccountType.Id}";
    }


    private string GetKeywordsFilter() {
      string keywords = EmpiriaString.Clean(_query.Keywords ?? string.Empty);

      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLikeKeywords("ACCT_KEYWORDS", keywords);
    }


    private string GetOrgUnitFilter() {
      return $"ACCT_ORG_UNIT_ID = {_query.GetBaseParty().Id}";
    }


    private string GetStatusFilter() {
      return $"ACCT_STATUS = 'A'";
    }

    #endregion Helpers

  }  // public class BudgetAccountSearcher

}  // namespace Empiria.Budgeting
