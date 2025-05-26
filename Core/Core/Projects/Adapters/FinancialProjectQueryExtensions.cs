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
      string categoryFilter =  BuildCategoryFilter(query.CategoryUID);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string programFilter = BuildProgramFilter(query.ProgramUID);
      string partyFilter = BuildPartyFilter(query.PartyUID);
      string statusFilter = BuildStatusFilter(query.Status);     
      string subprogramFilter = BuildSubprogramFilter(query.SubprogramUID);
      
      var filter = new Filter(partyFilter);
      filter.AppendAnd(categoryFilter);
      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(programFilter);
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

    static private string BuildCategoryFilter(string cateogoryUID) {
      if (cateogoryUID.Length == 0) {
        return string.Empty;
      }

      var category = FinancialProjectCategory.Parse(cateogoryUID);

      return $"PRJ_CATEGORY_ID = {category.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("PRJ_KEYWORDS", keywords);
    }


    static private string BuildPartyFilter(string requesterOrgUnitUID) {
      if (requesterOrgUnitUID.Length == 0) {
        return string.Empty;
      }

      var requesterOrgUnit = OrganizationalUnit.Parse(requesterOrgUnitUID);

      return $"PRJ_ORG_UNIT_ID = {requesterOrgUnit.Id}";
    }


    static private string BuildProgramFilter(string programUID) {
      if (programUID.Length == 0) {
        return string.Empty;
      }

      return string.Empty;
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

      return $"PRJ_STD_ACCT_ID = {subprogram.Id}";
    }


    #endregion Helpers

  }  // class FinancialProjectQueryExtensions

}  // namespace Empiria.Financial.Projects.Adapters
