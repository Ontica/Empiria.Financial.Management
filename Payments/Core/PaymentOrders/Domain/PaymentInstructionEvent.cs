/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration                             *
*  Type     : PaymentInstructionEvent                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumeration values used to control payment instruction events.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  public enum PaymentInstructionEvent {

    /// <summary>Cancels the whole payment instruction.</summary>
    Cancel,

    /// <summary>Cancels the payment request if the instruction is in waiting time.</summary>
    CancelPaymentRequest,

    /// <summary>Requests the payment associated to the instruction.</summary>
    RequestPayment,

    /// <summary>Resets the payment instruction returning it to Programmed status.</summary>
    Reset,

    /// <summary>Suspends the payment instruction.</summary>
    Suspend,

    /// <summary>Represents an empty payment instruction event.</summary>
    None,

  }  // enum PaymentInstructionEvent



  /// <summary>Extension methods for PaymentInstructionEvent.</summary>
  static public class PaymentInstructionEventExtMethods {

    static public string GetDescription(this PaymentInstructionEvent instructionEvent) {

      switch (instructionEvent) {
        case PaymentInstructionEvent.Cancel:
          return "La instrucción de pago fue cancelada.";

        case PaymentInstructionEvent.CancelPaymentRequest:
          return "El envío de la instrucción de pago fue cancelado.";

        case PaymentInstructionEvent.RequestPayment:
          return "La instrucción fue solicitada para su pago.";

        case PaymentInstructionEvent.Reset:
          return "Se activó la instrucción de pago.";

        case PaymentInstructionEvent.Suspend:
          return "Se suspendió la instrucción de pago.";

        case PaymentInstructionEvent.None:
          return "Evento de instrucción de pago no definido.";

        default:
          throw Assertion.EnsureNoReachThisCode("Unhandled PaymentInstructionEvent value.");
      }
    }

  }  // class PaymentInstructionEventExtMethods

}  // namespace Empiria.Payments
