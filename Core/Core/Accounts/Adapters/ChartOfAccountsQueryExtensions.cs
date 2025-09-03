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

using Empiria.Financial.Data;

namespace Empiria.Financial.Adapters {

  /// <summary>Query DTO used to extend financial accounts extensions.</summary>
  static internal class ChartOfAccountsQueryExtensions {

    #region Extension methods

    static internal FixedList<StandardAccount> Execute(this ChartOfAccountsQuery query) {
      string filter = GetFilterString(query);
      string sort = GetSortString(query);

      FixedList<StandardAccount> stdAccounts = StandardAccountDataService.SearchStandardAccounts(filter, sort);

      stdAccounts = ApplyRemainingFilters(query, stdAccounts);

      return stdAccounts;
    }

    #endregion Extension methods

    #region Methods

    static private FixedList<StandardAccount> ApplyRemainingFilters(ChartOfAccountsQuery query,
                                                                    FixedList<StandardAccount> stdAccounts) {
      if (query.Level > 0) {
        return stdAccounts.FindAll(x => x.Level <= query.Level);
      }

      return stdAccounts;
    }


    static private string GetFilterString(ChartOfAccountsQuery query) {
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


    static private string GetSortString(ChartOfAccountsQuery query) {
      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "STD_ACCT_NUMBER";
    }

    #endregion Methods

    #region Helpers

    static private string BuildChartOfAccountsFilter(string chartOfAccountsUID) {
      var chartOfAccounts = ChartOfAccounts.Parse(chartOfAccountsUID);

      return $"STD_ACCT_CHART_OF_ACCOUNTS_ID = {chartOfAccounts.Id}";
    }


    static private string BuildDebtorCreditorTypeFilter(DebtorCreditorType debtorCreditorType) {
      if (debtorCreditorType == DebtorCreditorType.Undefined) {
        return string.Empty;
      }

      return $"STD_ACCT_DEBTOR_CREDITOR_TYPE = '{(char) debtorCreditorType}'";
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

      return $"STD_ACCT_ROLE_TYPE = '{(char) roleType}'";
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
