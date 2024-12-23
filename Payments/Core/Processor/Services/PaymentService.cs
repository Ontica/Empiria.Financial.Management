/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Services interactor class               *
*  Type     : PaymentService                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides payment services using external services providers.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

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

    internal PaymentInstruction SendToPay(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      // ToDo: Validate not sent and not payed

      PaymentInstructionDto instruction = PaymentInstructionMapper.Map(paymentOrder);

      IPaymentsBroker paymentsService = broker.GetService();

      PaymentResultDto paymentResult = paymentsService.SendPaymentInstruction(instruction);

      if (paymentResult.Failed) {
        return RejectedPayment(paymentOrder, paymentResult, broker);

      } else {
        return SuccessfullPayment(paymentOrder, paymentResult, broker);
      }
    }


    internal PaymentInstruction ValidateIsPaymentInstructionPayed(string paymentInstructionUD) {
      PaymentInstruction paymentInstruction = PaymentInstruction.Parse(paymentInstructionUD);

      WritePaymentLog(paymentInstruction);

      return paymentInstruction;
    }

    #endregion Services

    #region Helpers

    private static PaymentInstruction RejectedPayment(PaymentOrder paymentOrder,
                                                      PaymentResultDto paymentResult,
                                                      PaymentsBroker broker) {

      var rejectedPaymentData = new RejectedPaymentData(paymentResult);

      var rejectedPayment = new RejectedPayment(broker, paymentOrder, rejectedPaymentData);

      rejectedPayment.Save();

      return rejectedPayment;
    }


    static private PaymentInstruction SuccessfullPayment(PaymentOrder paymentOrder,
                                                         PaymentResultDto paymentResult,
                                                         PaymentsBroker broker) {

      var successfulPaymentData = new SuccessfulPaymentData(paymentResult);

      var payment = new SuccessfulPayment(broker, paymentOrder, successfulPaymentData);

      payment.Save();

      return payment;
    }


    static private void WritePaymentLog(PaymentInstruction paymentInstruction) {

      var paymentLog = new PaymentLog(paymentInstruction);

      var date = DateTime.Now;

      paymentLog.RequestTime = date;
      paymentLog.ApplicationTime = date;
      paymentLog.RecordingTime = date;
      paymentLog.Status = 'P';

      paymentLog.Save();
    }

    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor.Services
