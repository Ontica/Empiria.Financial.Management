/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                          Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Type Extension methods                  *
*  Type     : FinancialAccountQueryExtensions            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to extend financial accounts extensions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Projects;

using Empiria.Financial.Data;

namespace Empiria.Financial.Adapters {

  /// <summary>Query DTO used to extend financial accounts extensions.</summary>
  static internal class FinancialAccountQueryExtensions {

    #region Extension methods

    static internal FixedList<FinancialAccount> Execute(this FinancialAccountQuery query) {
      string filter = GetFilterString(query);

      FixedList<FinancialAccount> accounts = FinancialAccountDataService.SearchAccounts(filter);

      accounts = ApplyRemainingFilters(query, accounts);

      return accounts;
    }

    #endregion Extension methods

    #region Methods

    static private FixedList<FinancialAccount> ApplyRemainingFilters(FinancialAccountQuery query,
                                                                     FixedList<FinancialAccount> accounts) {
      if (query.ProjectCategoryUID.Length != 0) {
        var category = FinancialProjectCategory.Parse(query.ProjectCategoryUID);

        return accounts.FindAll(x => x.Project.Category.Equals(category));
      }

      return accounts;
    }

    static private string GetFilterString(FinancialAccountQuery query) {

      string orgUnitFilter = BuildOrgUnitFilter(query.OrganizationUnitUID);
      string accountTypeFilter = BuildAccountTypeFilter(query.AccountTypeUID);
      string standardAccountFilter = BuildStandardAccountFilter(query.StandardAccountUID);
      string currencyFilter = BuildCurrencyFilter(query.CurrencyUID);
      string projectFilter = BuildProjectFilter(query.ProjectUID);
      string subledgerAccountFilter = BuildSubledgerAccountFilter(query.SubledgerAcccountNo);
      string statusFilter = BuildStatusFilter(query.Status);

      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(orgUnitFilter);

      filter.AppendAnd(accountTypeFilter);
      filter.AppendAnd(standardAccountFilter);
      filter.AppendAnd(currencyFilter);

      filter.AppendAnd(projectFilter);
      filter.AppendAnd(subledgerAccountFilter);
      filter.AppendAnd(statusFilter);

      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }

    #endregion Methods

    #region Helpers

    static private string BuildAccountTypeFilter(string accountTypeUID) {
      accountTypeUID = EmpiriaString.Clean(accountTypeUID);

      if (accountTypeUID.Length == 0) {
        return string.Empty;
      }

      var accountType = FinancialAccountType.Parse(accountTypeUID);

      return $"ACCT_TYPE_ID = {accountType.Id}";
    }


    static private string BuildCurrencyFilter(string currencyUID) {
      currencyUID = EmpiriaString.Clean(currencyUID);

      if (currencyUID.Length == 0) {
        return string.Empty;
      }

      var currency = Currency.Parse(currencyUID);

      return $"ACCT_CURRENCY_ID = {currency.Id}";
    }


    static private string BuildKeywordsFilter(string keywords) {
      keywords = EmpiriaString.Clean(keywords);

      if (keywords.Length == 0) {
        return string.Empty;
      }
      return SearchExpression.ParseAndLikeKeywords("ACCT_KEYWORDS", keywords);
    }


    static private string BuildOrgUnitFilter(string orgUnitUID) {
      orgUnitUID = EmpiriaString.Clean(orgUnitUID);

      if (orgUnitUID.Length == 0) {
        return string.Empty;
      }

      var orgUnit = OrganizationalUnit.Parse(orgUnitUID);

      return $"ACCT_ORG_UNIT_ID = {orgUnit.Id}";
    }


    static private string BuildProjectFilter(string projectUID) {
      projectUID = EmpiriaString.Clean(projectUID);

      if (projectUID.Length == 0) {
        return string.Empty;
      }

      var project = FinancialProject.Parse(projectUID);

      return $"ACCT_PROJECT_ID = {project.Id}";
    }


    static private string BuildStandardAccountFilter(string standardAccountUID) {
      standardAccountUID = EmpiriaString.Clean(standardAccountUID);

      if (standardAccountUID.Length == 0) {
        return string.Empty;
      }

      var stdAccount = StandardAccount.Parse(standardAccountUID);

      return $"ACCT_STD_ACCT_ID = {stdAccount.Id}";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return $"ACCT_STATUS <> 'X'";
      }

      return $"ACCT_STATUS = '{(char) status}'";
    }


    static private string BuildSubledgerAccountFilter(string subledgerAccountNo) {
      subledgerAccountNo = EmpiriaString.Clean(subledgerAccountNo);

      if (subledgerAccountNo.Length == 0) {
        return string.Empty;
      }

      return $"ACCT_SUBLEDGER_ACCT_NO LIKE '%{subledgerAccountNo}%'";
    }

    #endregion Helpers

  }  // class FinancialAccountQueryExtensions

}  // namespace Empiria.Financial.Adapters
