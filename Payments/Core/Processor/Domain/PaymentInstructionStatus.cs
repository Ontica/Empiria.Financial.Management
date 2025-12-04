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

  /// <summary>Enumerates the status of a payment instruction.</summary>
  public enum PaymentInstructionStatus {

    Pending = 'P',

    InProcess = 'A',

    Payed = 'C',

    Failed = 'F',

    Canceled = 'X',

    All = '@',

  }  // enum  PaymentInstructionStatus



  /// <summary>Extension methods for PaymentInstructionStatus enumeration.</summary>
  static public class PaymentInstructionStatusExtensions {

    static public string GetName(this PaymentInstructionStatus status) {
      switch (status) {

        case PaymentInstructionStatus.Pending:
        case PaymentInstructionStatus.InProcess:
          return "En proceso";

        case PaymentInstructionStatus.Payed:
          return "Pagada";

        case PaymentInstructionStatus.Failed:
          return "Rechazada o fallida";

        case PaymentInstructionStatus.Canceled:
          return "Cancelada";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment order status {status}.");
      }
    }


    static public bool IsFinal(this PaymentInstructionStatus status) {
      if (status == PaymentInstructionStatus.Pending || status == PaymentInstructionStatus.InProcess) {
        return false;
      }
      return true;
    }

  }  // class PaymentInstructionStatusExtensions

}  // namespace Empiria.Payments.Payables
