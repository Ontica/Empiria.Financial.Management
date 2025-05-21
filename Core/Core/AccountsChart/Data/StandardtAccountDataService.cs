/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : AccountChart                               Component : Data Layer                              *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Data Service                            *
*  Type     : StandardtAccountDataService                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for standard accounts.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

using Empiria.Financial.Accounts;

namespace Empiria.Financial.Data {

  /// <summary>Provides data access services for standard accounts.</summary>
  static internal class StandardtAccountDataService {

    static internal FixedList<StandardAccount> GetStdAcctByCategory(int categoryId) {

      var sql = "SELECT * FROM FMS_STD_ACCOUNTS " +
               $"WHERE STD_ACCT_CATEGORY_ID = {categoryId} " + " AND " +
               $"STD_ACCT_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StandardAccount>(op);
    }


    static internal FixedList<StandardAccount> SearchStandardtAccounts(string keywords) {

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


    static internal FixedList<StandardAccount> SearchStandardtAccounts(string filter, string sortBy) {

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

  }  // class StandardtAccountDataService

}  // namespace Empiria.Financial.Data
