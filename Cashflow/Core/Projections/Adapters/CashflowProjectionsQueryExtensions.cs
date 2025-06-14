﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Extension methods                       *
*  Type     : CashFlowProjectionsQueryExtensions         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for CashFlowProjectionsQuery interface adapter.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Extension methods for CashFlowProjectionsQuery interface adapter.</summary>
  static internal class CashFlowProjectionsQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this CashFlowProjectionsQuery query) {
      // no-op
    }


    static internal FixedList<CashFlowProjection> ApplyRemainingFilters(this CashFlowProjectionsQuery query,
                                                                        FixedList<CashFlowProjection> projections) {
      if (query.ProjectTypeCategoryUID.Length != 0) {
        var projectCategory = FinancialProjectCategory.Parse(query.ProjectTypeCategoryUID);

        return projections.FindAll(x => x.BaseProject.Category.Equals(projectCategory));
      }

      return projections;
    }


    static internal string MapToFilterString(this CashFlowProjectionsQuery query) {
      string planFilter = BuildPlanFilter(query.PlanUID);
      string categoryFilter = BuildCategoryFilter(query.ProjectionCategoryTypeUID);
      string basePartyFilter = BuildBasePartyFilter(query.PartyUID);
      string baseProjectFilter = BuildBaseProjectFilter(query.ProjectUID);
      string baseAccountFilter = BuildBaseAccountFilter(query.AccountUID);
      string sourceFilter = BuildSourceFilter(query.SourceUID);
      string projectionsNoFilter = BuildProjectionsNoFilter(query.ProjectionsNo);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string entriesKeywordsFilter = BuildEntriesKeywordsFilter(query.EntriesKeywords);
      string tagsFilter = BuildTagsFilter(query.Tags);
      string stageFilter = BuildStageFilter(query.Stage);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(planFilter);

      filter.AppendAnd(categoryFilter);
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


    static internal string MapToSortString(this CashFlowProjectionsQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      } else {
        return "CFW_PJC_NO, CFW_PJC_APPLICATION_DATE, CFW_PJC_RECORDING_TIME";
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

      var category = CashFlowProjectionCategory.Parse(categoryUID);

      return $"CFW_PJC_CATEGORY_ID = {category.Id}";
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

      var plan = CashFlowPlan.Parse(planUID);

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
      if (stage == TransactionStage.ControlDesk) {
        if (ExecutionServer.CurrentPrincipal.IsInRole(CashFlowProjectionRules.CASH_FLOW_AUTHORIZER) ||
            ExecutionServer.CurrentPrincipal.IsInRole(CashFlowProjectionRules.CASH_FLOW_MANAGER)) {
          return string.Empty;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole(CashFlowProjectionRules.CASH_FLOW_PROJECTOR)) {
          return $"CFW_PJC_BASE_PARTY_ID = {ExecutionServer.CurrentContact.Organization.Id}";
        }
      }
      return SearchExpression.NoRecordsFilter;
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

  }  // class CashFlowProjectionsQueryExtensions

}  // namespace Empiria.CashFlow.Projections.Adapters
