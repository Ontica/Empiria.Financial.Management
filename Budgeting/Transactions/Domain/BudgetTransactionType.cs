/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Infomation Holder                       *
*  Type     : BudgetTransactionType                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a budget transaction type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Represents a budget transaction type.</summary>
  internal class BudgetTransactionType : GeneralObject {


    static internal BudgetTransactionType Empty => BaseObject.ParseEmpty<BudgetTransactionType>();

  }  // class BudgetTransactionType

}  // namespace Empiria.Budgeting.Transactions
