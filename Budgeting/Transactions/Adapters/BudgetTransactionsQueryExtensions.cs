﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Extension methods                       *
*  Type     : BudgetTransactionsQueryExtensions          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for BudgetTransactionsQuery interface adapter.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Extension methods for BudgetTransactionsQuery interface adapter.</summary>
  static internal class BudgetTransactionsQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this BudgetTransactionsQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this BudgetTransactionsQuery query) {
      string budgetTypeFilter = BuildBudgetTypeFilter(query.BudgetTypeUID);
      string baseBudgetFilter = BuildBaseBudgetFilter(query.BaseBudgetUID);
      string transactionTypeFilter = BuildTransactionTypeFilter(query.TransactionTypeUID);
      string basePartyFilter = BuildBasePartyFilter(query.BasePartyUID);
      string operationSourceFilter = BuildOperationSourceFilter(query.OperationSourceUID);
      string transactionStageFilter = BuildTransactionStageFilter(query.Stage);

      string tagsFilter = BuildTagsFilter(query.Tags);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(budgetTypeFilter);

      filter.AppendAnd(baseBudgetFilter);
      filter.AppendAnd(transactionTypeFilter);
      filter.AppendAnd(basePartyFilter);
      filter.AppendAnd(operationSourceFilter);
      filter.AppendAnd(transactionStageFilter);

      filter.AppendAnd(tagsFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(statusFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this BudgetTransactionsQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      } else {
        return "BDG_TXN_NUMBER, BDG_TXN_APPLICATION_DATE, BDG_TXN_REQUESTED_TIME";
      }
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildBasePartyFilter(string basePartyUID) {
      if (basePartyUID.Length == 0 && (ExecutionServer.CurrentPrincipal.IsInRole("budget-manager") ||
                                      ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer"))) {
        return string.Empty;
      }

      if (basePartyUID.Length == 0 && ExecutionServer.CurrentPrincipal.IsInRole("acquisition-manager")) {
        return $"BDG_TXN_BASE_PARTY_ID = {ExecutionServer.CurrentContact.Organization.Id}";
      }

      var baseParty = Party.Parse(basePartyUID);

      return $"BDG_TXN_BASE_PARTY_ID = {baseParty.Id}";
    }


    static private string BuildBaseBudgetFilter(string baseBudgetUID) {
      if (baseBudgetUID.Length == 0) {
        return string.Empty;
      }

      var baseBudget = Budget.Parse(baseBudgetUID);

      return $"BDG_TXN_BASE_BUDGET_ID = {baseBudget.Id}";
    }


    static private string BuildBudgetTypeFilter(string budgetTypeUID) {
      if (budgetTypeUID.Length == 0) {
        return string.Empty;
      }

      var budgetType = BudgetType.Parse(budgetTypeUID);

      FixedList<Budget> budgets = Budget.GetList(budgetType);

      return SearchExpression.ParseInSet("BDG_TXN_BASE_BUDGET_ID", budgets.Select(x => x.Id));
    }


    static private string BuildOperationSourceFilter(string operationSourceUID) {
      if (operationSourceUID.Length == 0) {
        return string.Empty;
      }

      var operationSource = OperationSource.Parse(operationSourceUID);

      return $"BDG_TXN_SOURCE_ID = {operationSource.Id}";
    }


    static private string BuildTransactionStageFilter(TransactionStage stage) {
      int userId = ExecutionServer.CurrentUserId;

      if (stage == TransactionStage.MyInbox) {
        return $"(BDG_TXN_POSTED_BY_ID = {userId} OR " +
               $"BDG_TXN_RECORDED_BY_ID = {userId} OR " +
               $"BDG_TXN_REQUESTED_BY_ID = {userId} OR " +
               $"BDG_TXN_AUTHORIZED_BY_ID = {userId} OR " +
               $"BDG_TXN_APPLIED_BY_ID = {userId})";
      }
      if (stage == TransactionStage.ControlDesk) {
        if (ExecutionServer.CurrentPrincipal.IsInRole("budget-manager") ||
            ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer")) {
          return string.Empty;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("acquisition-manager")) {
          return $"BDG_TXN_BASE_PARTY_ID = {ExecutionServer.CurrentContact.Organization.Id}";
        }
      }
      return SearchExpression.NoRecordsFilter;
    }


    static private string BuildTransactionTypeFilter(string transactionTypeUID) {
      if (transactionTypeUID.Length == 0) {
        return string.Empty;
      }

      var transactionType = BudgetTransactionType.Parse(transactionTypeUID);

      return $"BDG_TXN_TYPE_ID = {transactionType.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("BDG_TXN_KEYWORDS", keywords);
    }


    static private string BuildStatusFilter(TransactionStatus status) {
      if (status == TransactionStatus.All) {
        return "BDG_TXN_STATUS <> 'X' ";
      }

      return $"(BDG_TXN_STATUS = '{(char) status}' AND BDG_TXN_ID <> -1)";
    }


    static private string BuildTagsFilter(string[] tags) {
      if (tags.Length == 0) {
        return string.Empty;
      }

      return string.Empty;

      //var filter = SearchExpression.ParseOrLikeKeywords("PRODUCT_TAGS", string.Join(" ", tags));

      //return $"({filter})";
    }

    #endregion Helpers

  }  // class BudgetTransactionsQueryExtensions

}  // namespace Empiria.Budgeting.Transactions.Adapters
