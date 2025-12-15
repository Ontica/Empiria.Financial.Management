/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration Type                        *
*  Type     : PaymentOrderStatus                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payment instruction.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Enumerates the status of a payment instruction.</summary>
  public enum PaymentInstructionStatus {

    Programmed = 'G',

    Canceled = 'X',

    Suspended = 'S',

    WaitingRequest = 'W',

    Requested = 'R',    // 'O', 'K'

    InProgress = 'I',   // 'O' a (no cambia el status), 'L' => 'T' , 'K'

    PaymentConfirmation = 'T', // 'L' && WaitingTime reaches 0 => Y, 'K'

    Payed = 'Y',

    Failed = 'F',

    All = '@',

  }  // enum  PaymentInstructionStatus



  /// <summary>Extension methods for PaymentInstructionStatus.</summary>
  static public class PaymentInstructionStatusExtensions {

    static internal void EnsureCanUpdateTo(this PaymentInstructionStatus currentStatus,
                                           PaymentInstructionStatus newStatus) {

      if (currentStatus.IsFinal()) {
        Assertion.RequireFail("El estado de la instrucción de pago no se puede modificar " +
                               $"debido a que está en el estado final: {currentStatus.GetName()}.");
      }


      switch (currentStatus) {
        case PaymentInstructionStatus.Programmed:

          if (newStatus == PaymentInstructionStatus.WaitingRequest ||
              newStatus == PaymentInstructionStatus.Canceled ||
              newStatus == PaymentInstructionStatus.Suspended) {

            return;
          }

          break;

        case PaymentInstructionStatus.Suspended:

          if (newStatus == PaymentInstructionStatus.Programmed ||
              newStatus == PaymentInstructionStatus.Canceled) {

            return;
          }

          break;

        case PaymentInstructionStatus.WaitingRequest:

          if (newStatus == PaymentInstructionStatus.Programmed ||
              newStatus == PaymentInstructionStatus.Requested) {

            return;
          }

          break;

        case PaymentInstructionStatus.Requested:

          if (newStatus == PaymentInstructionStatus.InProgress ||
              newStatus == PaymentInstructionStatus.Failed) {

            return;
          }

          break;

        case PaymentInstructionStatus.InProgress:

          if (newStatus == PaymentInstructionStatus.InProgress ||
              newStatus == PaymentInstructionStatus.PaymentConfirmation ||
              newStatus == PaymentInstructionStatus.Failed) {

            return;
          }

          break;

        case PaymentInstructionStatus.PaymentConfirmation:

          if (newStatus == PaymentInstructionStatus.Payed ||
              newStatus == PaymentInstructionStatus.Failed) {

            return;
          }

          break;

        default:

          throw Assertion.EnsureNoReachThisCode($"Unhandled payment instruction status change from " +
                                                $"{currentStatus.GetName()} to {newStatus.GetName()}.");
      }


      Assertion.RequireFail($"No es posible cambiar el estado de la instrucción de pago " +
                            $"del estado {currentStatus.GetName()} a {newStatus.GetName()}.");
    }


    static public bool IsFinal(this PaymentInstructionStatus status) {
      if (status == PaymentInstructionStatus.Canceled ||
          status == PaymentInstructionStatus.Payed ||
          status == PaymentInstructionStatus.Failed) {
        return true;
      }
      return false;
    }


    static public string GetName(this PaymentInstructionStatus status) {
      switch (status) {

        case PaymentInstructionStatus.Programmed:
          return "Programada";

        case PaymentInstructionStatus.Canceled:
          return "Cancelada";

        case PaymentInstructionStatus.Suspended:
          return "Suspendida";

        case PaymentInstructionStatus.WaitingRequest:
          return "Antes de enviar";

        case PaymentInstructionStatus.Requested:
          return "Enviada";

        case PaymentInstructionStatus.InProgress:
          return "En progreso";

        case PaymentInstructionStatus.PaymentConfirmation:
          return "Confirmando el pago";

        case PaymentInstructionStatus.Payed:
          return "Pagada";

        case PaymentInstructionStatus.Failed:
          return "Pago rechazado";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }


    static internal NamedEntityDto MapToNamedEntityDto(this PaymentInstructionStatus status) {
      return new NamedEntityDto(status.ToString(), GetName(status));
    }

  }  // class PaymentInstructionStatusExtensions

}  // namespace Empiria.Payments
