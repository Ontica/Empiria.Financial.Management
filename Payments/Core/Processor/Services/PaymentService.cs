/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Services interactor class               *
*  Type     : PaymentService                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides payment services using external services providers.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Adapters;


namespace Empiria.Payments.Processor.Services {

  internal class PaymentService : Service {

    #region Constructors and parsers

    protected PaymentService() {
      // no-op
    }

    static public PaymentService ServiceInteractor() {
      return CreateInstance<PaymentService>();
    }

    #endregion Constructors and parsers

    #region Services

    internal PaymentInstruction Pay(PaymentOrder paymentOrder) {
      IPaymentInstruction paymentInstruction = PaymentInstructionMapper.Map(paymentOrder);

      IPaymentsBroker brokerProvider = PaymentsBroker.SelectPaymentBroker(paymentInstruction);

      IPaymentResult paymentResult = brokerProvider.Pay(paymentInstruction);

      var broker = PaymentsBroker.Parse(brokerProvider.UID);

      if (paymentResult.Failed) {
        return RejectedPayment(paymentOrder, paymentResult, broker);

      } else {
        return SuccessfullPayment(paymentOrder, paymentResult, broker);
      }
    }

    #endregion Services

    #region Helpers

    private static PaymentInstruction RejectedPayment(PaymentOrder paymentOrder,
                                                 IPaymentResult paymentResult,
                                                 PaymentsBroker broker) {

      var rejectedPaymentData = new RejectedPaymentData(paymentResult);

      var rejectedPayment = new RejectedPayment(broker, paymentOrder, rejectedPaymentData);

      rejectedPayment.Save();

      return rejectedPayment;
    }


    static private PaymentInstruction SuccessfullPayment(PaymentOrder paymentOrder,
                                                    IPaymentResult paymentResult,
                                                    PaymentsBroker broker) {

      var successfulPaymentData = new SuccessfulPaymentData(paymentResult);

      var payment = new SuccessfulPayment(broker, paymentOrder, successfulPaymentData);

      payment.Save();

      return payment;
    }

    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor.Services
