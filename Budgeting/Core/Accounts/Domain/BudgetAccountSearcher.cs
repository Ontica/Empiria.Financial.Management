﻿/* Empiria Financial *****************************************************************************************
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

    public BudgetAccountSearcher(BudgetType budgetType) {

      _budgetType = budgetType;
    }

    public FixedList<BudgetAccount> Search(OrganizationalUnit orgUnit,
                                           FixedList<BudgetAccountSegment> baseSegments) {

      string baseSegmentsFilter = GetSegmentsFilter("BDG_ACCT_BASE_SEGMENT_ID", baseSegments);
      string orgUnitFilter = GetOrgUnitFilter(orgUnit);
      string budgetTypeFilter = GetBudgetTypeFilter();

      var filter = new Filter(baseSegmentsFilter);

      filter.AppendAnd(orgUnitFilter);
      filter.AppendAnd(budgetTypeFilter);

      return BudgetAccountDataService.SearchBudgetAcccounts(filter.ToString(), "BDG_ACCT_CODE");
    }

    #region Helpers

    private string GetBudgetTypeFilter() {
      return $"BDG_ACCT_BUDGET_TYPE_ID = {_budgetType.Id}";
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
