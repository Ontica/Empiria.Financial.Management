/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : RejectedPayment                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a rejected payment.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Orders;

namespace Empiria.Payments.Processor {

  internal class RejectedPayment : PaymentInstruction {

    public RejectedPayment(PaymentsBroker broker,
                           PaymentOrder paymentOrder,
                           RejectedPaymentData failResult) : base(broker, paymentOrder) {

      Assertion.Require(failResult, nameof(failResult));

      this.FailResult = failResult;
    }


    public RejectedPaymentData FailResult {
      get; private set;
    }

  }  // class RejectedPayment

} // namespace Empiria.Payments.Processor
