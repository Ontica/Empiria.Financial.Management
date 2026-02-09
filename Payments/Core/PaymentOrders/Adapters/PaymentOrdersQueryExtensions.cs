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
using Empiria.Parties;

namespace Empiria.Payments.Adapters {

  /// <summary>Extension methods for RequestsQuery interface adapter.</summary>
  static internal class PaymentOrdersQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this PaymentOrdersQuery query) {
      // no-op
    }

    static internal string MapToFilterString(this PaymentOrdersQuery query) {
      string statusFilter = BuildStatusFilter(query.SearchPaymentInstructions, query.Status);
      string requesterOrgUnitFilter = BuildRequesterOrgUnitFilter(query.RequesterOrgUnitUID);
      string paymentTypeFilter = BuildPaymentTypeFilter(query.PaymentTypeUID);
      string paymentMethodFilter = BuildPaymentMethodFilter(query.PaymentMethodUID);
      string dateRangeFilter = BuildDateRangeFilter(query.SearchPaymentInstructions,
                                                    query.FromDate, query.ToDate);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(statusFilter);
      filter.AppendAnd(requesterOrgUnitFilter);
      filter.AppendAnd(paymentTypeFilter);
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

    static private string BuildDateRangeFilter(bool searchPaymentInstructions,
                                               DateTime fromDueTime, DateTime toDueTime) {

      if (searchPaymentInstructions) {
        return $"{DataCommonMethods.FormatSqlDbDate(fromDueTime)} <= PYMT_ORD_POSTING_TIME AND " +
               $"PYMT_INSTRUCTION_POSTING_TIME < {DataCommonMethods.FormatSqlDbDate(toDueTime.Date.AddDays(1))}";

      }

      return $"{DataCommonMethods.FormatSqlDbDate(fromDueTime)} <= PYMT_ORD_POSTING_TIME AND " +
             $"PYMT_ORD_POSTING_TIME < {DataCommonMethods.FormatSqlDbDate(toDueTime.Date.AddDays(1))}";
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


    static private string BuildPaymentTypeFilter(string paymentTypeUID) {
      if (paymentTypeUID.Length == 0) {
        return string.Empty;
      }

      var paymentType = PaymentType.Parse(paymentTypeUID);

      return $"PYMT_ORD_PAYMENT_TYPE_ID = {paymentType.Id}";
    }


    static private string BuildStatusFilter(bool searchPaymentInstructions,
                                            PaymentOrderStatus status) {
      if (searchPaymentInstructions) {
        return BuildPaymentInstructionStatusFilter(status);
      }

      if (status == PaymentOrderStatus.All) {
        return $"PYMT_ORD_STATUS <> 'X'";
      }

      return $"PYMT_ORD_STATUS = '{(char) status}'";
    }

    static private string BuildPaymentInstructionStatusFilter(PaymentOrderStatus status) {
      if (status == PaymentOrderStatus.All) {
        return $"PYMT_INSTRUCTION_STATUS <> 'X'";
      }

      switch (status) {
        case PaymentOrderStatus.Programmed:
          return "PYMT_INSTRUCTION_STATUS IN ('G', 'S')";
        case PaymentOrderStatus.InProgress:
          return "PYMT_INSTRUCTION_STATUS IN ('W', 'R', 'I', 'T')";
        case PaymentOrderStatus.Failed:
          return "PYMT_INSTRUCTION_STATUS IN ('F', 'E')";
        default:
          return $"PYMT_INSTRUCTION_STATUS = '{(char) status}'";
      }
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

}  // namespace Empiria.Payments.Adapters
