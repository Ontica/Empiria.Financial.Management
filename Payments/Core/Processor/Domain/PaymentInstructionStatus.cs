/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration Type                        *
*  Type     : PaymentOrderStatus                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payment instruction.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor {

  /// <summary>EEnumerates the status of a payment instruction.</summary>
  public enum PaymentInstructionStatus {

    Capture = 'P',

    Rejected = 'Y',

    InPaymentLine = 'L',

    AuthorizationRequired = 'R',

    Payed = 'C',

    Failed = 'F', 

    Deleted = 'X',

    All = '@',

  }  // enum  PaymentInstructionStatus



  /// <summary>Extension methods for  PaymentInstructionStatus enumeration.</summary>
  static public class PaymentInstructionStatusExtensions {

    static public string GetName(this PaymentInstructionStatus status) {
      switch (status) {
        case PaymentInstructionStatus.Capture:
          return "En captura";
        case PaymentInstructionStatus.Rejected:
          return "Rechazado";
        case PaymentInstructionStatus.InPaymentLine:
          return "En linea de pago";
        case PaymentInstructionStatus.AuthorizationRequired:
          return "Requiere autorizacion";
        case PaymentInstructionStatus.Payed:
          return "Pagado";
        case PaymentInstructionStatus.Failed:
          return "Fallida";
        case PaymentInstructionStatus.Deleted:
          return "Eliminado";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }

  }  // class PaymentInstructionStatusExtensions

}  // namespace Empiria.Payments.Payables
