/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Type Extensions                         *
*  Type     : PayeesQueryExtensions                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for PayeesQuery type.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Payments.Adapters {

  /// <summary>Extension methods for PayeesQuery type.</summary>
  static internal class PayeesQueryExtensions {

    #region Extension Methods

    static internal void EnsureIsValid(this PayeesQuery query) {
      // no - op
    }

    static internal string MapToFilterString(this PayeesQuery query) {

      string roleFilter = BuildRoleFilter();

      string statusFilter = BuildStatusFilter(query.Status);

      string keywordFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(roleFilter);

      filter.AppendAnd(statusFilter);

      filter.AppendAnd(keywordFilter);

      return filter.ToString();
    }


    internal static string MapToSortString(this PayeesQuery query) {

      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "PARTY_NAME";
    }

    #endregion Extension Methods

    #region Helpers

    static private string BuildKeywordsFilter(string keywords) {
      if (keywords == string.Empty) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("PARTY_KEYWORDS", keywords);
    }


    static private string BuildRoleFilter() {
      return "PARTY_ROLES LIKE '%supplier%'";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return "PARTY_STATUS <> 'X' ";
      }

      return $"PARTY_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class PayeesQueryExtensions

} // namespace Empiria.Payments.Adapters
