﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Service provider                        *
*  Type     : BudgetAccountSearcher                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Searches budget accounts.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

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

    public bool HasSegment(OrganizationalUnit orgUnit, BudgetAccountSegment segment) {
      string budgetTypeFilter = GetBudgetTypeFilter();
      string orgUnitFilter = GetOrgUnitFilter(orgUnit);
      string baseSegmentFilter = $"BDG_ACCT_BASE_SEGMENT_ID = {segment.Id}";

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(baseSegmentFilter);

      var accounts = BudgetAccountDataService.SearchBudgetAcccounts(filter.ToString(), "BDG_ACCT_CODE");

      return accounts.Count != 0;
    }


    public FixedList<BudgetAccount> Search(OrganizationalUnit orgUnit,
                                           FixedList<BudgetAccountSegment> baseSegments) {

      string budgetTypeFilter = GetBudgetTypeFilter();
      string orgUnitFilter = GetOrgUnitFilter(orgUnit);
      string baseSegmentsFilter = GetSegmentsFilter("BDG_ACCT_BASE_SEGMENT_ID", baseSegments);
      string keywordsFilter = GetKeywordsFilter();

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(baseSegmentsFilter);
      filter.AppendAnd(keywordsFilter);

      return BudgetAccountDataService.SearchBudgetAcccounts(filter.ToString(), "BDG_ACCT_CODE");
    }


    public FixedList<BudgetAccount> Search(OrganizationalUnit orgUnit, string filterString) {
      Assertion.Require(orgUnit, nameof(orgUnit));

      filterString = EmpiriaString.Clean(filterString ?? string.Empty);
      filterString = filterString.Replace("{{ACCOUNT.CODE.FIELD}}", "BDG_ACCT_CODE");

      string budgetTypeFilter = GetBudgetTypeFilter();
      string orgUnitFilter = GetOrgUnitFilter(orgUnit);
      string keywordsFilter = GetKeywordsFilter();

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(filterString);

      return BudgetAccountDataService.SearchBudgetAcccounts(filter.ToString(), "BDG_ACCT_CODE");
    }


    public FixedList<BudgetAccountSegment> SearchUnassignedBaseSegments(OrganizationalUnit orgUnit,
                                                                        string filterString) {
      Assertion.Require(orgUnit, nameof(orgUnit));

      filterString = EmpiriaString.Clean(filterString ?? string.Empty);

      FixedList<BudgetAccount> assignedAccounts = Search(orgUnit, filterString);

      FixedList<BudgetAccountSegment> allSegments = _budgetType.ProductProcurementSegmentType.SearchInstances(filterString, _keywords);

      return allSegments.Remove(assignedAccounts.Select(x => x.BaseSegment))
                        .Distinct()
                        .ToFixedList()
                        .Sort((x, y) => x.Code.CompareTo(y.Code));
    }

    #endregion Methods

    #region Helpers

    private string GetBudgetTypeFilter() {
      return $"BDG_ACCT_BUDGET_TYPE_ID = {_budgetType.Id}";
    }


    private string GetKeywordsFilter() {
      if (_keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("BDG_ACCT_KEYWORDS", _keywords);
    }


    private string GetOrgUnitFilter(Party party) {
      return $"BDG_ACCT_ORG_UNIT_ID = {party.Id}";
    }


    private string GetSegmentsFilter(string segmentColumn,
                                     FixedList<BudgetAccountSegment> segments) {
      return SearchExpression.ParseInSet(segmentColumn, segments.Select(x => x.Id));
    }

    #endregion Helpers

  }  // public class BudgetAccountSearcher

}  // namespace Empiria.Budgeting
