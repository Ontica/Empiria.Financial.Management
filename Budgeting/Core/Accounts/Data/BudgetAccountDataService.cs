/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Data Service                            *
*  Type     : BudgetAccountDataService                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for budget accounts.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;

namespace Empiria.Budgeting.Data {

  /// <summary>Provides data access services for budget accounts.</summary>
  static internal class BudgetAccountDataService {

    static internal void CleanAccount(BudgetAccount account) {
      if (account.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE FMS_BUDGET_ACCOUNTS " +
               $"SET BDG_ACCT_UID = '{Guid.NewGuid().ToString()}', " +
               $"BDG_ACCT_CODE = '{account.BaseSegment.Code}', " +
               $"BDG_ACCT_KEYWORDS = '{account.Keywords}' " +
               $"WHERE BDG_ACCT_ID = {account.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal FixedList<BudgetAccount> SearchBudgetAcccounts(string filter, string sort) {

      var sql = "SELECT * FROM FMS_BUDGET_ACCOUNTS";

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
