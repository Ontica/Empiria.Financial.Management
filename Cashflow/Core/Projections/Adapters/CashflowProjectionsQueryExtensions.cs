/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Extension methods                       *
*  Type     : CashflowProjectionsQueryExtensions         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for CashflowProjectionsQuery interface adapter.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.Cashflow.Projections.Adapters {

  /// <summary>Extension methods for CashflowProjectionsQuery interface adapter.</summary>
  static internal class CashflowProjectionsQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this CashflowProjectionsQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this CashflowProjectionsQuery query) {
      string planFilter = BuildPlanFilter(query.PlanUID);
      string categoryFilter = BuildCategoryFilter(query.CategoryUID);
      string classificationFilter = BuildClassificationFilter(query.ClassificationUID);
      string basePartyFilter = BuildBasePartyFilter(query.BasePartyUID);
      string baseProjectFilter = BuildBaseProjectFilter(query.BaseProjectUID);
      string baseAccountFilter = BuildBaseAccountFilter(query.BaseAccountUID);
      string sourceFilter = BuildSourceFilter(query.SourceUID);
      string projectionsNoFilter = BuildProjectionsNoFilter(query.ProjectionsNo);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string entriesKeywordsFilter = BuildEntriesKeywordsFilter(query.EntriesKeywords);
      string tagsFilter = BuildTagsFilter(query.Tags);
      string stageFilter = BuildStageFilter(query.Stage);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(planFilter);

      filter.AppendAnd(categoryFilter);
      filter.AppendAnd(classificationFilter);
      filter.AppendAnd(basePartyFilter);
      filter.AppendAnd(baseProjectFilter);
      filter.AppendAnd(baseAccountFilter);
      filter.AppendAnd(sourceFilter);
      filter.AppendAnd(projectionsNoFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(entriesKeywordsFilter);
      filter.AppendAnd(tagsFilter);
      filter.AppendAnd(stageFilter);
      filter.AppendAnd(statusFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this CashflowProjectionsQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      } else {
        return "CFW_PJC_NO DESC, CFW_PJC_APPLICATION_DATE, CFW_PJC_RECORDING_TIME";
      }
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildBaseAccountFilter(string baseAccountUID) {
      if (baseAccountUID.Length == 0) {
        return string.Empty;
      }

      var account = FinancialAccount.Parse(baseAccountUID);

      return $"CFW_PJC_BASE_ACCOUNT_ID = {account.Id}";
    }


    static private string BuildBasePartyFilter(string basePartyUID) {
      if (basePartyUID.Length == 0) {
        return string.Empty;
      }

      var baseParty = Party.Parse(basePartyUID);

      return $"CFW_PJC_BASE_PARTY_ID = {baseParty.Id}";
    }


    static private string BuildBaseProjectFilter(string baseProjectUID) {
      if (baseProjectUID.Length == 0) {
        return string.Empty;
      }

      var project = FinancialProject.Parse(baseProjectUID);

      return $"CFW_PJC_BASE_PROJECT_ID = {project.Id}";
    }


    static private string BuildEntriesKeywordsFilter(string entriesKeywords) {
      return string.Empty;
    }


    static private string BuildCategoryFilter(string categoryUID) {
      if (categoryUID.Length == 0) {
        return string.Empty;
      }

      var category = CashflowProjectionCategory.Parse(categoryUID);

      return $"CFW_PJC_CATEGORY_ID = {category.Id}";
    }


    static private string BuildClassificationFilter(string classificationUID) {
      return string.Empty;
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("CFW_PJC_KEYWORDS", keywords);
    }


    static private string BuildPlanFilter(string planUID) {
      if (planUID.Length == 0) {
        return string.Empty;
      }

      var plan = CashflowPlan.Parse(planUID);

      return $"CFW_PJC_PLAN_ID = {plan.Id}";
    }


    static private string BuildProjectionsNoFilter(string[] projectionsNo) {
      return string.Empty;
    }


    static private string BuildSourceFilter(string sourceUID) {
      if (sourceUID.Length == 0) {
        return string.Empty;
      }

      var source = OperationSource.Parse(sourceUID);

      return $"CFW_PJC_SOURCE_ID = {source.Id}";
    }


    static private string BuildStageFilter(TransactionStage stage) {
      int userId = ExecutionServer.CurrentUserId;

      if (stage == TransactionStage.MyInbox) {
        return $"(CFW_PJC_POSTED_BY_ID = {userId} OR " +
               $"CFW_PJC_RECORDED_BY_ID = {userId} OR " +
               $"CFW_PJC_REQUESTED_BY_ID = {userId} OR " +
               $"CFW_PJC_AUTHORIZED_BY_ID = {userId} OR " +
               $"CFW_PJC_APPLIED_BY_ID = {userId})";
      }

      return string.Empty;
    }


    static private string BuildStatusFilter(TransactionStatus status) {
      if (status == TransactionStatus.All) {
        return "CFW_PJC_STATUS <> 'X'";
      }

      return $"(CFW_PJC_STATUS = '{(char) status}' AND CFW_PJC_ID <> -1)";
    }


    static private string BuildTagsFilter(string[] tags) {
      if (tags.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseOrLikeKeywords("CFW_PJC_TAGS", string.Join(" ", tags));
    }

    #endregion Helpers

  }  // class CashflowProjectionsQueryExtensions

}  // namespace Empiria.Cashflow.Projections.Adapters
