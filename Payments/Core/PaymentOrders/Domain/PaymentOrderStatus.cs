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

    Canceled = 'X',

    Suspended = 'S',

    Programmed = 'G',

    InProgress = 'I',

    Payed = 'Y',

    Failed = 'F',

    All = '@'

  }  // enum PaymentOrderStatus



  /// <summary>Extension methods for PaymentOrderStatus enumeration.</summary>
  static public class PaymentOrderStatusExtensions {

    static public string GetName(this PaymentOrderStatus status) {
      switch (status) {
        case PaymentOrderStatus.Pending:
          return "Pendiente";

        case PaymentOrderStatus.Canceled:
          return "Cancelada";

        case PaymentOrderStatus.Suspended:
          return "Suspendida";

        case PaymentOrderStatus.Programmed:
          return "Programada";

        case PaymentOrderStatus.InProgress:
          return "En progreso";

        case PaymentOrderStatus.Payed:
          return "Pagada";

        case PaymentOrderStatus.Failed:
          return "Pago rechazado";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }


    static public bool IsFinal(this PaymentOrderStatus status) {
      if (status == PaymentOrderStatus.Canceled ||
          status == PaymentOrderStatus.Payed ||
          status == PaymentOrderStatus.Failed) {
        return true;
      }
      return false;
    }


    static public NamedEntityDto MapToNamedEntity(this PaymentOrderStatus status) {
      return new NamedEntityDto(status.ToString(), GetName(status));
    }

  }  // class PaymentOrderStatusExtensions

}  // namespace Empiria.Payments
