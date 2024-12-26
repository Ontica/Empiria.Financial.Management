/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Query payload                           *
*  Type     : BillsQueryExtensions                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Data;
using Empiria.Parties;

namespace Empiria.Billing.Adapters {
  
  /// <summary></summary>
  static internal class BillsQueryExtensions {


    #region Public methods


    static internal string MapToFilterString(this BillsQuery query) {

      string dateFilter = GetDateRangeFilter(query);
      string categoryFilter = GetCategoryFilter(query.BillCategoryUID);
      string managedByFilter = GetManagedByFilter(query.ManagedByUID);
      string tagsFilter = GetTagsFilter(query.Tags);
      string keywordsFilter = GetKeywordsFilter(query.Keywords);
      //string conceptsKeywordsFilter = GetConceptsKeywordsFilter(query.ConceptsKeywords);
      string statusFilter = GetStatusFilter(query.Status);
      //string billDateType = GetBillDateTypeFilter(query.BillDateType);

      var filter = new Filter();

      filter.AppendAnd(dateFilter);
      filter.AppendAnd(categoryFilter);
      filter.AppendAnd(managedByFilter);
      filter.AppendAnd(tagsFilter);
      filter.AppendAnd(keywordsFilter);
      //filter.AppendAnd(conceptsKeywordsFilter);
      filter.AppendAnd(statusFilter);
      //filter.AppendAnd(billDateType);

      return filter.ToString().Length > 0 ? $"{filter}" : "";
    }


    static internal string MapToSortString(this BillsQuery query) {

      return string.Empty;
    }

    #endregion Public methods


    #region Helpers

    static private string GetBillDateTypeFilter(BillQueryDateType billDateType) {

      if (billDateType == BillQueryDateType.None) {
        return string.Empty;
      }

      return $"BILL_DATE_TYPE = {billDateType}";
    }


    static private string GetCategoryFilter(string billCategoryUID) {

      if (billCategoryUID == string.Empty) {
        return string.Empty;
      }

      return $"BILL_CATEGORY_ID = {BillCategory.Parse(billCategoryUID).Id}";
    }


    static private string GetConceptsKeywordsFilter(string conceptsKeywords) {

      if (conceptsKeywords == string.Empty) {
        return string.Empty;
      }

      return $"{SearchExpression.ParseAndLikeKeywords("ConceptsKeywords", conceptsKeywords)} ";
    }


    private static string GetDateRangeFilter(BillsQuery query) {

      if (query.FromDate == ExecutionServer.DateMinValue &&
          query.ToDate == ExecutionServer.DateMinValue) {

        return string.Empty;
      }

      return $"BILL_ISSUE_DATE >= {DataCommonMethods.FormatSqlDbDate(query.FromDate)} " +
             $"AND BILL_ISSUE_DATE <= {DataCommonMethods.FormatSqlDbDate(query.ToDate)}";
    }


    static string GetKeywordsFilter(string keywords) {

      if (keywords == string.Empty) {
        return string.Empty;
      }

      return $"{SearchExpression.ParseAndLikeKeywords("BILL_KEYWORDS", keywords)} ";
    }


    static private string GetManagedByFilter(string managedByUID) {

      if (managedByUID == string.Empty) {
        return string.Empty;
      }

      return $"BILL_MANAGED_BY_ID = {Party.Parse(managedByUID).Id}";
    }


    static private string GetStatusFilter(BillStatus status) {

      if (status == BillStatus.All) {
        return string.Empty;
      }

      return $"BILL_STATUS = '{(char) status}'";
    }


    static private string GetTagsFilter(string[] tags) {

      //var _tags = string.Join(",", tags);
      //return $"BILL_TAGS IN ('{_tags}')";
      return string.Empty;
    }

    #endregion Helpers


  } // class BillsQueryExtensions

} // namespace Empiria.Billing.Adapters
