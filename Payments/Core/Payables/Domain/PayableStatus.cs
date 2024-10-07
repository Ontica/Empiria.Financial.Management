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

    Capture = 'P',

    OnPayment = 'Y',

    Payed = 'C',

    Deleted = 'X',

    All = '@',

  }  // enum PayableStatus



  /// <summary>Extension methods for PayableStatus enumeration.</summary>
  static public class PayableStatusExtensions {

    static public string GetName(this PayableStatus status) {
      switch (status) {
        case PayableStatus.Capture:
          return "En captura";
        case PayableStatus.OnPayment:
          return "Enviado a pago";
        case PayableStatus.Payed:
          return "Pagado";
        case PayableStatus.Deleted:
          return "Eliminado";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }

  }  // class PayableStatusExtensions

}  // namespace Empiria.Payments.Payables
