/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Type Extension methods                  *
*  Type     : FinancialConceptsQueryExtensions           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for FinancialConceptsQuery type.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.StateEnums;

using Empiria.Financial.Concepts.Data;

namespace Empiria.Financial.Concepts.Adapters {

  /// <summary>Extension methods for FinancialConceptsQuery type.</summary>
  static internal class FinancialConceptsQueryExtensions {

    #region Extension methods

    static internal FixedList<FinancialConcept> Execute(this FinancialConceptsQuery query) {
      string filter = GetFilterString(query);

      return FinancialConceptsData.SearchFinancialConcepts(filter);
    }

    #endregion Extension methods

    #region Methods

    static private string GetFilterString(FinancialConceptsQuery query) {
      string groupFilter = BuildGroupFilter(query.GroupUID);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string statusFilter = BuildStatusFilter(query.Status);
      string dateFilter = BuildDateFilter(query.Date);

      var filter = new Filter(groupFilter);

      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(statusFilter);
      filter.AppendAnd(dateFilter);

      return filter.ToString();
    }

    #endregion Methods

    #region Helpers

    static private string BuildDateFilter(DateTime date) {
      if (ExecutionServer.IsMinOrMaxDate(date)) {
        return string.Empty;
      }

      return $"(CPT_START_DATE <= {DataCommonMethods.FormatSqlDbDate(date)} AND " +
             $"{DataCommonMethods.FormatSqlDbDate(date)} <= CPT_END_DATE)";
    }


    static private string BuildGroupFilter(string groupUID) {
      var group = FinancialConceptGroup.Parse(groupUID);

      return $"CPT_GROUP_ID = {group.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLikeKeywords("CPT_KEYWORDS", keywords);
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"CPT_STATUS <> 'X'";
      }

      return $"CPT_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class FinancialConceptsQueryExtensions

}  // namespace Empiria.Financial.Concepts.Adapters
