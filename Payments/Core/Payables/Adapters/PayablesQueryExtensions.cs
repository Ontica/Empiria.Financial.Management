/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Type Extension methods                  *
*  Type     : PayablesQueryExtensions                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for PayablesQuery interface adapter.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.Parties;

using Empiria.Budgeting;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Extension methods for PayablesQuery interface adapter.</summary>
  static internal class PayablesQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this PayablesQuery query) {
      // no-op
    }

    static internal string MapToFilterString(this PayablesQuery query) {
      string statusFilter = BuildRequestStatusFilter(query.Status);
      string payableTypeFilter = BuildPayableTypeFilter(query.PayableTypeUID);
      string requesterOrgUnitFilter = BuildRequesterOrgUnitFilter(query.RequesterOrgUnitUID);
      string budgetTypeFilter = BuildBudgetTypedFilter(query.BudgetTypeUID);
      string dateRangeFilter = BuildDueTimeRangeFilter(query.FromDate, query.ToDate);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(statusFilter);
      filter.AppendAnd(payableTypeFilter);
      filter.AppendAnd(requesterOrgUnitFilter);
      filter.AppendAnd(budgetTypeFilter);
      filter.AppendAnd(dateRangeFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this PayablesQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "PYM_PYB_ID";
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildDueTimeRangeFilter(DateTime fromDueTime, DateTime toDueTime) {
      return $"{DataCommonMethods.FormatSqlDbDate(fromDueTime)} <= PYM_PYB_DUETIME AND " +
             $"PYM_PYB_DUETIME < {DataCommonMethods.FormatSqlDbDate(toDueTime.Date.AddDays(1))}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("PYM_PYB_KEYWORDS", keywords);
    }


    static private string BuildPayableTypeFilter(string payableTypeUID) {
      if (payableTypeUID.Length == 0) {
        return string.Empty;
      }

      var payableType = PayableType.Parse(payableTypeUID);

      return $"PYM_PYB_TYPE_ID = {payableType.Id}";
    }


    static private string BuildBudgetTypedFilter(string budgetTypeUID) {
      if (budgetTypeUID.Length == 0) {
        return string.Empty;
      }

      var budgetType = BudgetType.Parse(budgetTypeUID);

      return $"PYM_PYB_BUDGET_TYPE_ID = {budgetType.Id}";
    }


    static private string BuildRequestStatusFilter(PayableStatus status) {
      if (status == PayableStatus.All) {
        return $"PYM_PYB_STATUS <> 'X'";
      }

      return $"PYM_PYB_STATUS = '{(char) status}'";
    }


    static private string BuildRequesterOrgUnitFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"PYM_PYB_ORG_UNIT_ID = {requesterOrgUnit.Id}";
    }

    #endregion Helpers

  }  // class PayablesQueryExtensions

}  // namespace Empiria.Payments.Payables.Adapters
