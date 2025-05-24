/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : AccountChart                               Component : Data Layer                              *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Data Service                            *
*  Type     : StandardAccountDataService                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for standard accounts.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.Data {

  /// <summary>Provides data access services for standard accounts.</summary>
  static internal class StandardAccountDataService {

    static internal void CleanStandardAccount(StandardAccount stdAccount) {
      if (stdAccount.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE FMS_STD_ACCOUNTS " +
               $"SET STD_ACCT_UID = '{System.Guid.NewGuid().ToString()}', " +
               $"STD_ACCT_DESCRIPTION = '{EmpiriaString.Clean(stdAccount.Description).Replace("'", "''")}', " +
               $"STD_ACCT_KEYWORDS = '{stdAccount.Keywords}' " +
               $"WHERE STD_ACCT_ID = {stdAccount.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

    internal static FixedList<StandardAccount> GetStandardAccounts(StandardAccountsCatalogue catalogue) {
      var sql = "SELECT * FROM FMS_STD_ACCOUNTS " +
         $"WHERE STD_ACCT_CATALOGUE_ID = {catalogue.Id} AND " +
         $"STD_ACCT_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StandardAccount>(op);
    }

    static internal FixedList<StandardAccount> GetStandardAccounts(StandardAccountCategory category) {

      var sql = "SELECT * FROM FMS_STD_ACCOUNTS " +
               $"WHERE STD_ACCT_CATEGORY_ID = {category.Id} AND " +
               $"STD_ACCT_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StandardAccount>(op);
    }


    static internal FixedList<StandardAccount> SearchStandardAccounts(string keywords) {

      var filter = SearchExpression.ParseOrLikeKeywords("STD_ACCT_KEYWORDS", keywords);
      if (filter.Length != 0) {
        filter += " AND";
      }
      var sql = "SELECT * FROM FMS_STD_ACCOUNTS " +
               $"WHERE {filter} " +
               $"STD_ACCT_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StandardAccount>(op);
    }


    static internal FixedList<StandardAccount> SearchStandardAccounts(string filter, string sortBy) {

      var sql = "SELECT * FROM FMS_STD_ACCOUNTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StandardAccount>(dataOperation);
    }

  }  // class StandardAccountDataService

}  // namespace Empiria.Financial.Data
