/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : SuccessfulPayment                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a succesful payment.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Orders;

namespace Empiria.Payments.Processor {

  /// <summary>Represents an actual payment.</summary>
  internal class SuccessfulPayment : PaymentInstruction {

    public SuccessfulPayment(PaymentsBroker broker, PaymentOrder paymentOrder,
                             SuccessfulPaymentData successfulResult) : base(broker, paymentOrder) {

      Assertion.Require(successfulResult, nameof(successfulResult));

      this.SuccessfulResult = successfulResult;
    }


    public SuccessfulPaymentData SuccessfulResult {
      get; private set;
    }

  }  // class Payment

} // namespace Empiria.Payments.Processor
