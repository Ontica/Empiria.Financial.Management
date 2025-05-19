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

using Empiria.Financial.Projects;

namespace Empiria.Financial.Accounts.Adapters {

  /// <summary>Query DTO used to extend financial accounts extensions.</summary>
  static internal class FinancialAccountQueryExtensions {

    #region Extension methods

    static internal void EnsureIsValid(this FinancialAccountQuery query) {
      // no-op
    }

    static internal string MapToFilterString(this FinancialAccountQuery query) {
      string statusFilter = BuildStatusFilter(query.Status);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string requesterOrgUnitFilter = BuildOrgUnitFilter(query.OrganizationUnitUID);
      string projectFilter = BuildProjectFilter(query.ProjectUID);

      var filter = new Filter(statusFilter);
      filter.AppendAnd(requesterOrgUnitFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(projectFilter);

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

    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("ACCT_KEYWORDS", keywords);
    }


    static private string BuildOrgUnitFilter(string orgUnitUID) {
      if (orgUnitUID.Length == 0) {
        return string.Empty;
      }

      var orgUnit = OrganizationalUnit.Parse(orgUnitUID);

      return $"ACCT_ORG_UNIT_ID = {orgUnit.Id}";
    }


    static private string BuildProjectFilter(string projectUID) {
      if (projectUID.Length == 0) {
        return string.Empty;
      }

      var project = FinancialProject.Parse(projectUID);

      return $"ACCT_PROJECT_ID = {project.Id}";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"ACCT_STATUS <> 'X'";
      }

      return $"ACCT_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class FinancialAccountQueryExtensions

}  // namespace Empiria.Financial.Accounts.Adapters
