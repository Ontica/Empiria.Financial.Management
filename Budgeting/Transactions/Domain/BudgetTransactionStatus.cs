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



  /// <summary>Extension methods for BudgetTransactionStatus type.</summary>
  static public class BudgetTransactionStatusExtensions {

    static public string GetName(this BudgetTransactionStatus status) {

      switch (status) {
        case BudgetTransactionStatus.Pending:
          return "Pendiente";
        case BudgetTransactionStatus.OnAuthorization:
          return "En autorización";
        case BudgetTransactionStatus.Completed:
          return "Completada";
        case BudgetTransactionStatus.Deleted:
          return "Eliminada";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized budget transaction status {status}.");
      }
    }


    static public NamedEntityDto MapToNamedEntity(this BudgetTransactionStatus status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

  }  // class BudgetTransactionStatusExtensions

}  // namespace Empiria.Budgeting.Transactions
