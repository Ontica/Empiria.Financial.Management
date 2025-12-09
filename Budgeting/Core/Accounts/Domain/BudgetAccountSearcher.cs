/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Service provider                        *
*  Type     : BudgetAccountSearcher                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Searches budget accounts.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

using Empiria.Budgeting.Data;

namespace Empiria.Budgeting {

  /// <summary>Searches budget accounts.</summary>
  public class BudgetAccountSearcher {

    private readonly BudgetType _budgetType;
    private readonly string _keywords;

    public BudgetAccountSearcher(BudgetType budgetType, string keywords = "") {
      Assertion.Require(budgetType, nameof(budgetType));

      _budgetType = budgetType;
      _keywords = EmpiriaString.Clean(keywords ?? string.Empty);
    }

    #region Methods

    public FixedList<BudgetAccount> Search(OrganizationalUnit orgUnit, string filterString) {
      Assertion.Require(orgUnit, nameof(orgUnit));

      filterString = EmpiriaString.Clean(filterString ?? string.Empty);
      filterString = filterString.Replace("{{ACCOUNT.CODE.FIELD}}", "ACCT_NUMBER");

      string budgetTypeFilter = GetBudgetTypeFilter();
      string orgUnitFilter = GetOrgUnitFilter(orgUnit);
      string keywordsFilter = GetKeywordsFilter();
      string statusFilter = GetStatusFilter();

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(filterString);
      filter.AppendAnd(statusFilter);

      return BudgetAccountData.SearchBudgetAccounts(filter.ToString(), "ACCT_NUMBER");
    }

    #endregion Methods

    #region Helpers

    private string GetBudgetTypeFilter() {
      return $"STD_ACCT_TYPE_ID = {_budgetType.StandardAccountType.Id}";
    }


    private string GetKeywordsFilter() {
      if (_keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("ACCT_KEYWORDS", _keywords);
    }


    private string GetOrgUnitFilter(Party party) {
      return $"ACCT_ORG_UNIT_ID = {party.Id}";
    }


    private string GetStatusFilter() {
      return $"ACCT_STATUS = 'A'";
    }

    #endregion Helpers

  }  // public class BudgetAccountSearcher

}  // namespace Empiria.Budgeting
