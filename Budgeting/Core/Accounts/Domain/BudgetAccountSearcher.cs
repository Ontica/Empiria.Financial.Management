/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Service provider                        *
*  Type     : BudgetAccountSearcher                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Searches budget accounts.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

using Empiria.Budgeting.Data;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions;
using Empiria.Parties;

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

      string budgetTypeFilter = GetBudgetTypeFilter();
      string orgUnitFilter = GetOrgUnitFilter();
      string accountsFilter = GetAccountsFilter("ACCT_NUMBER");
      string statusFilter = GetStatusFilter();
      string keywordsFilter = GetKeywordsFilter("ACCT_KEYWORDS");


      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(accountsFilter);
      filter.AppendAnd(statusFilter);
      filter.AppendAnd(keywordsFilter);

      return BudgetAccountData.SearchBudgetAccounts(filter.ToString(), "ACCT_NUMBER");
    }


    internal FixedList<StandardAccount> SearchAvailable(OrganizationalUnit orgUnit) {

      string budgetTypeFilter = GetBudgetTypeFilter();
      string accountsFilter = GetAccountsFilter("STD_ACCT_NUMBER");
      string keywordsFilter = GetKeywordsFilter("STD_ACCT_KEYWORDS");

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(accountsFilter);
      filter.AppendAnd(keywordsFilter);

      return BudgetAccountData.SearchAvailableBudgetAccounts(orgUnit, filter.ToString(), "STD_ACCT_NUMBER");
    }

    #endregion Methods

    #region Helpers

    private string GetAccountsFilter(string accountCodeField) {
      string accountsFilter = _query.GetTransactionType().BudgetAccountsFilter;

      accountsFilter = EmpiriaString.Clean(accountsFilter ?? string.Empty);
      accountsFilter = accountsFilter.Replace("{{ACCOUNT.CODE.FIELD}}", accountCodeField);

      return accountsFilter;
    }


    private string GetBudgetTypeFilter() {
      return $"STD_ACCT_TYPE_ID = {_query.GetBaseBudget().BudgetType.StandardAccountType.Id}";
    }


    private string GetKeywordsFilter(string keywordsFieldName) {
      string keywords = EmpiriaString.Clean(_query.Keywords ?? string.Empty);

      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLikeKeywords(keywordsFieldName, keywords);
    }


    private string GetOrgUnitFilter() {
      return $"ACCT_ORG_UNIT_ID = {_query.GetBaseParty().Id}";
    }


    private string GetStatusFilter() {
      if (_query.GetTransactionType().OperationType == BudgetOperationType.Plan) {
        return $"ACCT_STATUS IN ('A', 'P')";
      }

      return $"ACCT_STATUS = 'A'";
    }

    #endregion Helpers

  }  // public class BudgetAccountSearcher

}  // namespace Empiria.Budgeting
