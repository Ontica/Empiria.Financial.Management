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


    internal FixedList<PaymentLog> GetPaymentLogs(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      var paymentInstruction = PaymentInstruction.TryGetFor(paymentOrder);

      return PaymentLog.GetPaymentLog(paymentInstruction.Id);
    }


    internal PaymentInstruction SendToPay(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      EnsureIsNotPayed(paymentOrder);

      PaymentInstructionDto instruction = PaymentInstructionMapper.Map(paymentOrder);

      IPaymentsBroker paymentsService = broker.GetService();

      PaymentResultDto paymentResult = paymentsService.SendPaymentInstruction(instruction);

      if (paymentResult.Failed) {
        var rejectedPayment = RejectedPayment(paymentOrder, paymentResult, broker);
        WritePaymentLog(rejectedPayment, paymentResult);

        return rejectedPayment;
      } else {
        var successfullPayment = SuccessfullPayment(paymentOrder, paymentResult, broker);
        WritePaymentLog(successfullPayment, paymentResult);

        return successfullPayment;
      }
    }
       

    internal void ValidateIsPaymentInstructionIsPayed(PaymentInstruction paymentInstruction) {

      Assertion.Require(paymentInstruction.Status == PaymentInstructionStatus.Payed,
                        $"La orden de pago ya ha sido pagada ");

      var paymentLog = PaymentLog.TryGetFor(paymentInstruction);

      //send to Simefin Ws
      PaymentResultDto paymentResult = new PaymentResultDto();

      Assertion.Require(paymentResult.Status == 'K',
                 $"La orden de pago fue cancelada o rechazada.");

      switch (paymentResult.Status) {
        case 'O':
          WritePaymentLog(paymentInstruction, paymentResult);
          break;
        case 'P':
          paymentInstruction.SetAuthorizationRequired();
          paymentInstruction.Save();
          WritePaymentLog(paymentInstruction, paymentResult);
          break;
        case 'K': {
          paymentInstruction.SetFailed();
          paymentInstruction.Save();
          WritePaymentLog(paymentInstruction, paymentResult);
          break;
        }
        case 'L': {
          paymentInstruction.SetPayed();
          paymentInstruction.Save();
          WritePaymentLog(paymentInstruction, paymentResult);
          break;
        }
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment result status .");
      }

    }

    #endregion Services

    #region Helpers


    static private void EnsureIsNotPayed(PaymentOrder paymentOrder) {
      var paymentInstruction = PaymentInstruction.TryGetFor(paymentOrder);

      if (paymentInstruction != null) {
        Assertion.Require(paymentInstruction.Status != PaymentInstructionStatus.Rejected,
                $"No es posible enviar la orden de pago, ya que existe una orden pago " +
                $"con los mismos datos en status: {paymentInstruction.Status.GetName()}.");
      }
    }


    static private PaymentInstruction RejectedPayment(PaymentOrder paymentOrder,
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
     

    static private void WritePaymentLog(PaymentInstruction PaymentInstruction, PaymentResultDto paymentResultDto) {
      PaymentLog paymentLog = new PaymentLog(PaymentInstruction);
      paymentLog.Update(paymentResultDto);
      paymentLog.Save();
    }

 
    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor.Services
