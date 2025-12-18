/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Enumeration type                        *
*  Type     : BudgetOperationType                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the operation type for a BudgetTransactionType.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Enumerates the operation type for a BudgetTransactionType.</summary>
  public enum BudgetOperationType {

    Plan,

    Authorize,

    Expand,

    Modify,

    Request,

    Commit,

    ApprovePayment,

    Exercise,

    None

  }  // enum BudgetOperationType

}  // namespace Empiria.Budgeting.Transactions
