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
  static internal class AccountDataService {


      static internal FixedList<Account> SearchAccount(string keyewords) {
      
      var sql = "SELECT * FROM FMS_ACCOUNTS " +
               $"WHERE ACCT_KEYWORDS LIKE '%{keyewords}%'" + " AND " +
               $"ACCT_STATUS <> 'X' AND ACCT_TYPE_ID = 3216";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Account>(op);
    }

    static internal FixedList<Account> SearchAccount(string filter, string sortBy) {
      
      var sql = "SELECT * FROM FMS_ACCOUNTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Account>(dataOperation);
    }

  }  // class AccountDataService

}  // namespace Empiria.Financial.Data
