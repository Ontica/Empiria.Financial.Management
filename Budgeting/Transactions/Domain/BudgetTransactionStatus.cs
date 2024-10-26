/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Enumeration                             *
*  Type     : BudgetTransactionStatus                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the distinct status control values for budget transactions.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Enumerates the distinct status control values for budget transactions.</summary>
  public enum BudgetTransactionStatus {

    Pending = 'P',

    OnAuthorization = 'A',

    Completed = 'C',

    Deleted = 'X',

    All = '*'

  }  // enum BudgetTransactionStatus

}  // namespace Empiria.Budgeting.Transactions
