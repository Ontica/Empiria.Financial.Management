/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Type Extension methods                  *
*  Type     : FinancialProjectQueryExtensions            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to extend financial projects extensions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;
using Empiria.Parties;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Query DTO used to extend financial projects extensions.</summary>
  static internal class FinancialProjectQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this FinancialProjectQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this FinancialProjectQuery query) {
      string statusFilter = BuildRequestStatusFilter(query.Status);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string requesterOrgUnitFilter = BuildRequesterOrgUnitFilter(query.OrganizationUnitUID);
          
      var filter = new Filter(statusFilter);
      filter.AppendAnd(requesterOrgUnitFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this FinancialProjectQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "PRJ_NO";
    }

    #endregion Extension methods

    #region Helpers

    private static string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("PRJ_KEYWORDS", keywords);
    }


    static private string BuildRequestStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"PRJ_STATUS <> 'X'";
      }

      return $"PRJ_STATUS = '{(char) status}'";
    }


    static private string BuildRequesterOrgUnitFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"PRJ_ORG_UNIT_ID = {requesterOrgUnit.Id}";
    }

    #endregion Helpers

  }  // class FinancialProjectQueryExtensions

}  // namespace Empiria.Financial.Projects.Adapters
