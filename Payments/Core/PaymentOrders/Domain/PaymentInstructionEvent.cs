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

  internal enum PaymentInstructionEvent {

    /// <summary>Cancels the whole payment instruction.</summary>
    Cancel,

    /// <summary>Cancels the payment request if the instruction is in waiting time.</summary>
    CancelPaymentRequest,

    /// <summary>Requests the payment associated to the instruction.</summary>
    RequestPayment,

    /// <summary>Suspends the payment instruction.</summary>
    Suspend,

  }  // enum PaymentInstructionEvent

}  // namespace Empiria.Payments
