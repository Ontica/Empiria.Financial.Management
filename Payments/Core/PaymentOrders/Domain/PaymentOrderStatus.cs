/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration Type                        *
*  Type     : PaymentOrderStatus                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payment order.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Enumerates the status of a payment order.</summary>
  public enum PaymentOrderStatus {

    Pending = 'P',

    Received = 'R',

    Suspended = 'S',

    Returned = 'T',

    Rejected = 'J',

    Committed = 'M',

    Programmed = 'G',

    Payed = 'C',

    Deleted = 'X',

    All = '@',

  }  // enum PaymentOrderStatus



  /// <summary>Extension methods for PaymentOrderStatus enumeration.</summary>
  static public class PaymentOrderStatusExtensions {

    static public string GetName(this PaymentOrderStatus status) {
      switch (status) {
        case PaymentOrderStatus.Pending:
          return "Pendiente";
        case PaymentOrderStatus.Received:
          return "Recibida";
        case PaymentOrderStatus.Suspended:
          return "Suspendida";
        case PaymentOrderStatus.Returned:
          return "Regresada";
        case PaymentOrderStatus.Rejected:
          return "Rechazada";
        case PaymentOrderStatus.Committed:
          return "Comprometida";
        case PaymentOrderStatus.Programmed:
          return "Programada";
        case PaymentOrderStatus.Payed:
          return "Pagada";
        case PaymentOrderStatus.Deleted:
          return "Eliminada";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }


    static public NamedEntityDto MapToNamedEntity(this PaymentOrderStatus status) {
      return new NamedEntityDto(status.ToString(), GetName(status));
    }

  }  // class PaymentOrderStatusExtensions

}  // namespace Empiria.Payments
