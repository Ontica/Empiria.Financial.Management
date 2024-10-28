/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Data Service                            *
*  Type     : BudgetTransactionDataService               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for budget transactions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Budgeting.Transactions.Data {

  /// <summary>Provides data access services for budget transactions.</summary>
  static internal class BudgetTransactionDataService {

    static internal FixedList<BudgetTransaction> SearchTransactions(string filter, string sort) {
      Assertion.Require(filter, nameof(filter));
      Assertion.Require(sort, nameof(sort));

      var sql = "SELECT * FROM FMS_BUDGET_TRANSACTIONS " +
               $"WHERE {filter} " +
               $"ORDER BY {sort}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetTransaction>(op);
    }

  }  // class BudgetTransactionDataService

}  // namespace Empiria.Budgeting.Transactions.Data
