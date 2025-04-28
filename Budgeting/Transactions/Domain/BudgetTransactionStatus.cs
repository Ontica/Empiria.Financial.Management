/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Enumeration                             *
*  Type     : BudgetTransactionStatus                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes the status of a budget transaction.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Describes the status of a budget transaction.</summary>
  public enum BudgetTransactionStatus {

    Pending = 'P',

    OnAuthorization = 'T',

    Authorized = 'A',

    Closed = 'C',

    Rejected = 'J',

    Canceled = 'L',

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

        case BudgetTransactionStatus.Authorized:
          return "Autorizada";

        case BudgetTransactionStatus.Closed:
          return "Cerrada";

        case BudgetTransactionStatus.Rejected:
          return "Rechazada";

        case BudgetTransactionStatus.Canceled:
          return "Cancelada";

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
