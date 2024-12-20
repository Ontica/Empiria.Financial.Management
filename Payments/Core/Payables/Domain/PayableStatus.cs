/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration Type                        *
*  Type     : PayableStatus                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payable object.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Payables {

  /// <summary>Enumerates the status of a payable order.</summary>
  public enum PayableStatus {

    Pending = 'P',

    OnPayment = 'Y',

    Payed = 'C',

    Deleted = 'X',

    Rejected = 'J',

    Canceled = 'L',

    All = '@',

  }  // enum PayableStatus



  /// <summary>Extension methods for PayableStatus enumeration.</summary>
  static public class PayableStatusExtensions {

    static public string GetName(this PayableStatus status) {
      switch (status) {
        case PayableStatus.Pending:
          return "Pendiente";
        case PayableStatus.OnPayment:
          return "Enviado a pago";
        case PayableStatus.Payed:
          return "Pagado";
        case PayableStatus.Rejected:
          return "Rechazado";
        case PayableStatus.Canceled:
          return "Cancelado";
        case PayableStatus.Deleted:
          return "Eliminado";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }

    static public NamedEntityDto MapToNamedEntity(this PayableStatus status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

    }  // class PayableStatusExtensions

}  // namespace Empiria.Payments.Payables
