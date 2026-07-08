/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Data Service                            *
*  Type     : BudgetAccountDataService                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for budget accounts.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.Budgeting.Data {

  /// <summary>Provides data access services for budget accounts.</summary>
  static internal class BudgetAccountData {

    static internal FixedList<StandardAccount> SearchAvailableBudgetAccounts(OrganizationalUnit orgUnit,
                                                                             string filter, string sort) {

      var sql = "SELECT FMS_STD_ACCOUNTS.* " +
                "FROM FMS_STD_ACCOUNTS " +
                "WHERE {{FILTER}} STD_ACCT_ROLE_TYPE = 'P' AND STD_ACCT_STATUS = 'A' AND " +
                $"STD_ACCT_ID NOT IN (SELECT ACCT_STD_ACCT_ID " +
                                    $"FROM FMS_ACCOUNTS " +
                                    $"WHERE ACCT_ORG_UNIT_ID = {orgUnit.Id})";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql = sql.Replace("{{FILTER}}", $"{filter} AND");
      } else {
        sql = sql.Replace("{{FILTER}}", string.Empty);
      }

      if (!string.IsNullOrWhiteSpace(sort)) {
        sql += $" ORDER BY {sort}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StandardAccount>(op);
    }


    static internal FixedList<BudgetAccount> SearchBudgetAccounts(string filter, string sort) {

      var sql = "SELECT FMS_ACCOUNTS.* " +
                "FROM FMS_ACCOUNTS INNER JOIN FMS_STD_ACCOUNTS " +
                "ON FMS_ACCOUNTS.ACCT_STD_ACCT_ID = FMS_STD_ACCOUNTS.STD_ACCT_ID " +
                "INNER JOIN PARTIES " +
                "ON FMS_ACCOUNTS.ACCT_ORG_UNIT_ID = PARTIES.PARTY_ID";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sort)) {
        sql += $" ORDER BY {sort}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetAccount>(op);
    }

  }  // class BudgetAccountDataService

}  // namespace Empiria.Budgeting.Data
