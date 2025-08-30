/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Type Extension methods                  *
*  Type     : FinancialProjectQueryExtensions            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to extend financial projects extensions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Query DTO used to extend financial projects extensions.</summary>
  static internal class FinancialProjectQueryExtensions {

    #region Extension methods

    static internal FixedList<FinancialProject> ApplyRemainingFilters(this FinancialProjectQuery query,
                                                                      FixedList<FinancialProject> projects) {
      if (query.ProgramUID.Length != 0) {
        return projects.FindAll(x => x.Program.UID == query.ProgramUID);
      }

      return projects;
    }

    static internal void EnsureIsValid(this FinancialProjectQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this FinancialProjectQuery query) {
      string baseOrgUnitFilter = BuildBaseOrgUnitFilter(query.BaseOrgUnitUID);
      string projectTypeCategoryFilter = BuildProjectTypeCategoryFilter(query.ProjectTypeCategoryUID);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string statusFilter = BuildStatusFilter(query.Status);
      string subprogramFilter = BuildSubprogramFilter(query.SubprogramUID);

      var filter = new Filter(baseOrgUnitFilter);
      filter.AppendAnd(projectTypeCategoryFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(statusFilter);
      filter.AppendAnd(subprogramFilter);

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

    static private string BuildBaseOrgUnitFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"PRJ_BASE_ORG_UNIT_ID = {requesterOrgUnit.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("PRJ_KEYWORDS", keywords);
    }


    static private string BuildProjectTypeCategoryFilter(string cateogoryUID) {
      if (cateogoryUID.Length == 0) {
        return string.Empty;
      }

      var category = FinancialProjectCategory.Parse(cateogoryUID);

      return $"PRJ_CATEGORY_ID = {category.Id}";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"PRJ_STATUS <> 'X'";
      }

      return $"PRJ_STATUS = '{(char) status}'";
    }


    static private string BuildSubprogramFilter(string subprogramUID) {
      if (subprogramUID.Length == 0) {
        return string.Empty;
      }

      var subprogram = StandardAccount.Parse(subprogramUID);

      return $"PRJ_SUBPROGRAM_ID = {subprogram.Id}";
    }

    #endregion Helpers

  }  // class FinancialProjectQueryExtensions

}  // namespace Empiria.Financial.Projects.Adapters
