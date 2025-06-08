/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                          Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Type Extension methods                  *
*  Type     : ChartOfAccountsQueryExtensions             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for ChartOfAccountsQuery.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.Financial.Adapters {

  /// <summary>Query DTO used to extend financial accounts extensions.</summary>
  static internal class ChartOfAccountsQueryExtensions {

    #region Extension methods

    static internal FixedList<StandardAccount> ApplyRemainingFilters(this ChartOfAccountsQuery query,
                                                                     FixedList<StandardAccount> stdAccounts) {
      if (query.Level > 0) {
        return stdAccounts.FindAll(x => x.Level <= query.Level);
      }

      return stdAccounts;
    }


    static internal void EnsureIsValid(this ChartOfAccountsQuery query) {
      // no-op
    }


    static internal string MapToFilterString(this ChartOfAccountsQuery query) {
      string chartOfAccountsFilter = BuildChartOfAccountsFilter(query.ChartOfAccountsUID);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);
      string roleTypeFilter = BuildRoleTypeFilter(query.RoleType);
      string debtorCreditorTypeFilter = BuildDebtorCreditorTypeFilter(query.DebtorCreditorType);
      string fromToAccountsFilter = BuildFromToAccountsFilter(query.FromAccount, query.ToAccount);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(chartOfAccountsFilter);

      filter.AppendAnd(keywordsFilter);
      filter.AppendAnd(roleTypeFilter);
      filter.AppendAnd(debtorCreditorTypeFilter);
      filter.AppendAnd(fromToAccountsFilter);
      filter.AppendAnd(statusFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this ChartOfAccountsQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "STD_ACCT_NUMBER";
    }

    #endregion Extension methods

    #region Helpers

    static private string BuildChartOfAccountsFilter(string chartOfAccountsUID) {
      var chartOfAccounts = StandardAccountsCatalogue.Parse(chartOfAccountsUID);

      return $"STD_ACCT_CATALOGUE_ID = {chartOfAccounts.Id}";
    }


    static private string BuildDebtorCreditorTypeFilter(DebtorCreditorType debtorCreditorType) {
      if (debtorCreditorType == DebtorCreditorType.Undefined) {
        return string.Empty;
      }

      return $"STD_ACCT_DEBTOR = '{(char) debtorCreditorType}'";
    }


    static private string BuildFromToAccountsFilter(string fromAccount, string toAccount) {
      string filter = String.Empty;

      if (fromAccount.Length != 0) {
        filter = $"'{fromAccount}' <= STD_ACCT_NUMBER";
      }
      if (toAccount.Length != 0) {
        if (filter.Length != 0) {
          filter += " AND ";
        }
        filter += $"STD_ACCT_NUMBER <= '{toAccount}'";
      }

      return filter;
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("STD_ACCT_KEYWORDS", keywords);
    }


    static private string BuildRoleTypeFilter(AccountRoleType roleType) {
      if (roleType == AccountRoleType.Undefined) {
        return string.Empty;
      }

      return $"STD_ACCT_ROLE = '{(char) roleType}'";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"STD_ACCT_STATUS <> 'X'";
      }

      return $"STD_ACCT_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class ChartOfAccountsQueryExtensions

}  // namespace Empiria.Financial.Adapters
