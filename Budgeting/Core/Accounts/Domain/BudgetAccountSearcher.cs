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

    public BudgetAccountSearcher(BudgetType budgetType) {

      _budgetType = budgetType;
    }

    public FixedList<BudgetAccount> Search(Party party,
                                           FixedList<BudgetAccountSegment> primarySegments) {

      string primarySegmentsFilter = GetSegmentsFilter("BDG_ACCT_SEGMENT_02_ID", primarySegments);
      string partyFilter = GetPartyFilter(party);
      string budgetTypeFilter = GetBudgetTypeFilter();

      var filter = new Filter(primarySegmentsFilter);

      filter.AppendAnd(partyFilter);
      filter.AppendAnd(budgetTypeFilter);

      return BudgetAccountDataService.SearchBudgetAcccounts(filter.ToString(), "BDG_ACCT_CODE");
    }


    #region Helpers

    private string GetBudgetTypeFilter() {
      return $"BDG_ACCT_BUDGET_TYPE_ID = {_budgetType.Id}";
    }


    private string GetPartyFilter(Party party) {
      return $"BDG_ACCT_SEGMENT_01_ID = {party.Id}";
    }


    private string GetSegmentsFilter(string segmentColumn,
                                     FixedList<BudgetAccountSegment> segments) {
      return SearchExpression.ParseInSet(segmentColumn, segments.Select(x => x.Id));
    }

    #endregion Helpers

  }  // public class BudgetAccountSearcher

}  // namespace Empiria.Budgeting
