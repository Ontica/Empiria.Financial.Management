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

      return "PAYABLE_ID";
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildDueTimeRangeFilter(DateTime fromDueTime, DateTime toDueTime) {
      return $"{DataCommonMethods.FormatSqlDbDate(fromDueTime)} <= PAYABLE_DUETIME AND " +
             $"PAYABLE_DUETIME < {DataCommonMethods.FormatSqlDbDate(toDueTime.Date.AddDays(1))}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("PAYABLE_KEYWORDS", keywords);
    }


    static private string BuildPayableTypeFilter(string payableTypeUID) {
      if (payableTypeUID.Length == 0) {
        return string.Empty;
      }

      var payableType = PayableType.Parse(payableTypeUID);

      return $"PAYABLE_TYPE_ID = {payableType.Id}";
    }


    static private string BuildBudgetTypedFilter(string budgetTypeUID) {
      if (budgetTypeUID.Length == 0) {
        return string.Empty;
      }

      var budgetType = BudgetType.Parse(budgetTypeUID);

      return $"PAYABLE_BUDGET_TYPE_ID = {budgetType.Id}";
    }


    static private string BuildRequestStatusFilter(PayableStatus status) {
      if (status == PayableStatus.All) {
        return $"PAYABLE_STATUS <> 'X'";
      }

      return $"PAYABLE_STATUS = '{(char) status}'";
    }


    static private string BuildRequesterOrgUnitFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"PAYABLE_ORG_UNIT_ID = {requesterOrgUnit.Id}";
    }

    #endregion Helpers

  }  // class PayablesQueryExtensions

}  // namespace Empiria.Payments.Payables.Adapters
