/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Data services                           *
*  Type     : BudgetData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write services for budget-related types.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Budgeting.Budgets.Data {

  /// <summary>Provides data read and write services for budget-related types.</summary>
  static internal class BudgetData {

    static internal void Write(Budget budget, string extData) {
      var sql = "UPDATE COMMON_STORAGE " +
                $"SET OBJECT_EXT_DATA = '{extData}' " +
                $"WHERE OBJECT_ID = {budget.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

  }  // class BudgetData

}  // namespace Empiria.Budgeting.Budgets.Data
