/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Type Extension methods                  *
*  Type     : PaymentOrdersQueryExtensions               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search payment orders.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Extension methods for RequestsQuery interface adapter.</summary>
  static internal class PaymentOrdersQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this PaymentOrdersQuery query) {
      // no-op
    }

    static internal string MapToFilterString(this PaymentOrdersQuery query) {
      string statusFilter = BuildRequestStatusFilter(query.Status);
      string requesterOrgUnitFilter = BuildRequesterOrgUnitFilter(query.RequesterOrgUnitUID);
      string paymentOrderTypeFilter = BuildPaymentOrderTypeFilter(query.PaymentOrderTypeUID);
      string paymentMethodFilter = BuildPaymentMethodFilter(query.PaymentMethodUID);
      string dateRangeFilter = BuildDueTimeRangeFilter(query.FromDate, query.ToDate);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(statusFilter);
      filter.AppendAnd(requesterOrgUnitFilter);
      filter.AppendAnd(paymentOrderTypeFilter);
      filter.AppendAnd(paymentMethodFilter);
      filter.AppendAnd(dateRangeFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this PaymentOrdersQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "PYMT_ORD_NO";
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildDueTimeRangeFilter(DateTime fromDueTime, DateTime toDueTime) {
      return $"{DataCommonMethods.FormatSqlDbDate(fromDueTime)} <= PYMT_ORD_DUETIME AND " +
             $"PYMT_ORD_DUETIME < {DataCommonMethods.FormatSqlDbDate(toDueTime.Date.AddDays(1))}";
    }


    private static string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("PYMT_ORD_KEYWORDS", keywords);
    }


    static private string BuildPaymentMethodFilter(string paymentMethodUID) {
      if (paymentMethodUID.Length == 0) {
        return string.Empty;
      }

      var paymentMethod = PaymentMethod.Parse(paymentMethodUID);

      return $"PYMT_ORD_PAYMENT_METHOD_ID = {paymentMethod.Id}";
    }


    static private string BuildPaymentOrderTypeFilter(string paymentOrderTypeUID) {
      if (paymentOrderTypeUID.Length == 0) {
        return string.Empty;
      }

      return string.Empty;
    }


    static private string BuildRequestStatusFilter(PaymentOrderStatus status) {
      if (status == PaymentOrderStatus.All) {
        return $"PYMT_ORD_STATUS <> 'X'";
      }

      return $"PYMT_ORD_STATUS = '{(char) status}'";
    }


    static private string BuildRequesterOrgUnitFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"PYMT_ORD_REQUESTED_BY_ID = {requesterOrgUnit.Id}";
    }

    #endregion Helpers

  }  // class PaymentOrdersQueryExtensions

}  // namespace Empiria.Payments.Orders.Adapters
