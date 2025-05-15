/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                          Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Type Extension methods                  *
*  Type     : FinancialAccountQueryExtensions            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to extend financial accounts extensions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;
using Empiria.Parties;

namespace Empiria.Financial.Accounts.Adapters {

  /// <summary>Query DTO used to extend financial accounts extensions.</summary>
  static internal class FinancialAccountQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this FinancialAccountQuery query) {
      // no-op
    }

    static internal string MapToFilterString(this FinancialAccountQuery query) {
      string statusFilter = BuildRequestStatusFilter(query.Status);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string requesterOrgUnitFilter = BuildRequesterOrgUnitFilter(query.OrganizationUnitUID);
      string accountFilter = BuildRequesterAccountFilter(query.ProjectUID);

      var filter = new Filter(statusFilter);
      filter.AppendAnd(requesterOrgUnitFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(accountFilter);

      return filter.ToString();
    }

    static internal string MapToSortString(this FinancialAccountQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "ACCT_NUMBER";
    }

    #endregion Extension methods

    #region Helpers

    private static string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("ACCT_KEYWORDS", keywords);
    }


    static private string BuildRequestStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"ACCT_STATUS <> 'X'";
      }

      return $"ACCT_STATUS = '{(char) status}'";
    }


    static private string BuildRequesterOrgUnitFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"ACCT_ORG_UNIT_ID = {requesterOrgUnit.Id}";
    }


    static private string BuildRequesterAccountFilter(string requesterAccountUID) {
      if (requesterAccountUID.Length == 0) {
        return string.Empty;
      }

      var requesterProject = FinancialProject.Parse(requesterAccountUID);

      return $"ACCT_PROJECT_ID = {requesterProject.Id}";
    }


    #endregion Helpers

  }  // class FinancialAccountQueryExtensions

}  // namespace Empiria.Financial.Accounts.Adapters
