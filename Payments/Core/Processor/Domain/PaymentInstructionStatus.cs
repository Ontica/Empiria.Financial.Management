/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration Type                        *
*  Type     : PayableStatus                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payable object.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor {

  /// <summary>Enumerates the status of a payable order.</summary>
  public enum PaymentInstructionStatus {

    Capture = 'P',

    Rejected = 'Y',

    Payed = 'C',

    Deleted = 'X',

    All = '@',

  }  // enum PayableStatus



  /// <summary>Extension methods for PayableStatus enumeration.</summary>
  static public class PaymentInstructionStatusExtensions {

    static public string GetName(this PaymentInstructionStatus status) {
      switch (status) {
        case PaymentInstructionStatus.Capture:
          return "En captura";
        case PaymentInstructionStatus.Rejected:
          return "Rechazado";
        case PaymentInstructionStatus.Payed:
          return "Pagado";
        case PaymentInstructionStatus.Deleted:
          return "Eliminado";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }

  }  // class PayableStatusExtensions

}  // namespace Empiria.Payments.Payables
