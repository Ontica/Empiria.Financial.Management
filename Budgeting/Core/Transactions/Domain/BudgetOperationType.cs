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

    Reduce,

    Modify,

    Request,

    Commit,

    ApprovePayment,

    Exercise,

    None

  }  // enum BudgetOperationType



  /// <summary>Extension methods for BudgetOperationType enumeration.</summary>
  public static class BudgetOperationTypeExtensionMethods {

    static public BalanceColumn DepositColumn(this BudgetOperationType operationType) {

      switch (operationType) {

        case BudgetOperationType.Plan:
          return BalanceColumn.Planned;

        case BudgetOperationType.Authorize:
          return BalanceColumn.Authorized;

        case BudgetOperationType.Expand:
          return BalanceColumn.Expanded;

        case BudgetOperationType.Reduce:
          return BalanceColumn.Reduced;

        case BudgetOperationType.Request:
          return BalanceColumn.Requested;

        case BudgetOperationType.Commit:
          return BalanceColumn.Commited;

        case BudgetOperationType.ApprovePayment:
          return BalanceColumn.ToPay;

        case BudgetOperationType.Exercise:
          return BalanceColumn.Exercised;

        case BudgetOperationType.None:
          return BalanceColumn.Empty;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled budget operation type '{operationType}'.");
      }
    }


    static public BalanceColumn DefaultWithdrawalColumn(this BudgetOperationType operationType) {

      switch (operationType) {

        case BudgetOperationType.Plan:
          return BalanceColumn.Empty;

        case BudgetOperationType.Authorize:
          return BalanceColumn.Empty;

        case BudgetOperationType.Expand:
          return BalanceColumn.Expanded;

        case BudgetOperationType.Reduce:
          return BalanceColumn.Reduced;

        case BudgetOperationType.Request:
          return BalanceColumn.Available;

        case BudgetOperationType.Commit:
          return BalanceColumn.Requested;

        case BudgetOperationType.ApprovePayment:
          return BalanceColumn.Commited;

        case BudgetOperationType.Exercise:
          return BalanceColumn.ToPay;

        case BudgetOperationType.None:
          return BalanceColumn.Empty;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled budget operation type '{operationType}'.");
      }
    }

  }  // class BudgetOperationTypeExtensionMethods

}  // namespace Empiria.Budgeting.Transactions
