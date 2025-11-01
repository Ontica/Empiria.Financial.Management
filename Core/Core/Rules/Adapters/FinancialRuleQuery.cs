/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input Query DTO                       *
*  Type     : FinancialRuleQuery                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input query used to search financial rules.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;

using Empiria.Financial.Rules.Data;

namespace Empiria.Financial.Rules.Adapters {

  /// <summary>Input query used to search financial rules.</summary>
  public class FinancialRuleQuery {

    public string CategoryUID {
      get; set;
    } = string.Empty;


    public DateTime Date {
      get; set;
    } = ExecutionServer.DateMinValue;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class FinancialRuleQuery



  /// <summary>FinancialRuleQuery extension methods.</summary>
  static internal class FinancialRuleQueryExtensions {

    static internal FixedList<FinancialRule> Execute(this FinancialRuleQuery query) {
      string filter = GetFilterString(query);

      return FinancialRulesData.SearchFinancialRules(filter);
    }


    static private string GetFilterString(FinancialRuleQuery query) {
      string categoryFilter = BuildCategoryFilter(query.CategoryUID);
      string dateFilter = BuildDateFilter(query.Date);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string statusFilter = BuildStatusFilter();

      var filter = new Filter(categoryFilter);

      filter.AppendAnd(dateFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(statusFilter);

      return filter.ToString();
    }

    #region Helpers

    static private string BuildCategoryFilter(string categoryUID) {
      var category = FinancialRuleCategory.Parse(categoryUID);

      return $"RULE_CATEGORY_ID = {category.Id}";
    }


    static private string BuildDateFilter(DateTime date) {
      if (ExecutionServer.IsMinOrMaxDate(date)) {
        return string.Empty;
      }

      return $"(RULE_START_DATE <= {DataCommonMethods.FormatSqlDbDate(date)} AND " +
             $"{DataCommonMethods.FormatSqlDbDate(date)} <= RULE_END_DATE)";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLikeKeywords("RULE_KEYWORDS", keywords);
    }


    static private string BuildStatusFilter() {
      return $"RULE_STATUS <> 'X'";
    }

    #endregion Helpers

  }  // class FinancialRuleQueryExtensions

}  // namespace Empiria.Financial.Rules.Adapters
