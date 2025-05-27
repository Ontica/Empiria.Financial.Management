/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Account                                    Component : Data Layer                              *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Data Service                            *
*  Type     : AccountDataService                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial accounts.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.Data {

  /// <summary>Provides data access services for financial accounts.</summary>
  static internal class FinancialAccountDataService {

    static internal void CleanAccount(FinancialAccount account) {
      if (account.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE FMS_ACCOUNTS " +
                $"SET ACCT_DESCRIPTION = '{EmpiriaString.Clean(account.Description).Replace("'", "''")}', " +
                $"ACCT_KEYWORDS = '{account.Keywords}' " +
                $"WHERE ACCT_ID = {account.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

    static internal FixedList<FinancialAccount> SearchAccounts(string keywords) {

      var filter = SearchExpression.ParseOrLikeKeywords("ACCT_KEYWORDS", keywords);
      if (filter.Length != 0) {
        filter += " AND";
      }
      var sql = "SELECT * FROM FMS_ACCOUNTS " +
               $"WHERE {filter} " +
               $"ACCT_STATUS <> 'X' AND ACCT_TYPE_ID = 3245";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialAccount>(op);
    }


    static internal FixedList<FinancialAccount> SearchAccounts(string filter, string sortBy) {

      var sql = "SELECT * FROM FMS_ACCOUNTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialAccount>(dataOperation);
    }

    internal static void WriteAccount(FinancialAccount o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Account",
         o.Id, o.UID, o.FinancialAccountType.Id, o.StandardAccount.Id, o.Organization.Id,
         o.OrganizationalUnit.Id, o.Project.Id, o.LedgerId, o.AccountNo, o.Description,
         o.Identifiers, o.Tags, o.Attributes.ToString(), o.FinancialData.ToString(),
         o.ConfigData.ToString(), extensionData, o.Keywords, o.Parent.Id,
         o.StartDate, o.EndDate, o.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class AccountDataService

}  // namespace Empiria.Financial.Data
