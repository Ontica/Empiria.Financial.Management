/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Services interactor class               *
*  Type     : PaymentService                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides payment services using external services providers.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;

using Empiria.Payments.Adapters;

namespace Empiria.Payments.Processor {

  internal class PaymentService : Service {

    #region Constructors and parsers

    protected PaymentService() {
      PaymentsProcessor.Start();
    }

    static public PaymentService ServiceInteractor() {
      return CreateInstance<PaymentService>();
    }

    #endregion Constructors and parsers

    #region Services

    internal async Task RefreshPaymentInstruction(PaymentInstruction paymentInstruction) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsNew, "paymentInstruction must be stored.");


      Assertion.Require(!paymentInstruction.Status.IsFinal(),
                        $"La instrucción de pago está en un estado final: {paymentInstruction.Status.GetName()}");


      IPaymentsBrokerService paymentsService = paymentInstruction.Broker.GetService();

      Assertion.Require(paymentInstruction.ExternalRequestUniqueNo, "ExternalRequestUniqueNo missed.");

      PaymentInstructionStatusDto newStatus = await paymentsService.GetPaymentInstructionStatus(paymentInstruction.ExternalRequestUniqueNo);

      UpdatePaymentLog(paymentInstruction, newStatus);

      UpdatePaymentOrder(paymentInstruction.PaymentOrder, newStatus);
    }


    public async Task<PaymentOrderHolderDto> SendPaymentOrderToPay(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);

      PaymentsBroker broker = PaymentsBroker.GetPaymentsBroker(paymentOrder);

      _ = await SendToPay(broker, paymentOrder);

      return PaymentOrderMapper.Map(paymentOrder);
    }


    public async Task<int> ValidatePayment() {
      var paymentInstructions = PaymentInstruction.GetInProccessPaymentInstructions();
      int count = 0;

      foreach (var paymentInstruction in paymentInstructions) {
        await RefreshPaymentInstruction(paymentInstruction);
        count++;
      }

      return count;
    }


    public async Task<PaymentOrderHolderDto> ValidatePaymentOrderIsPayed(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);

      PaymentsBroker broker = PaymentsBroker.GetPaymentsBroker(paymentOrder);

      var paymentInstruction = PaymentInstruction.GetListFor(paymentOrder)
                                                 .Find(x => x.Status == PaymentInstructionStatus.InProcess);

      await RefreshPaymentInstruction(paymentInstruction);

      return PaymentOrderMapper.Map(paymentOrder);
    }


    internal async Task<PaymentInstruction> SendToPay(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(!broker.IsEmptyInstance, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));

      await Task.CompletedTask;

      Assertion.Require(paymentOrder.CanCreatePaymentInstruction(),
                        $"No es posible crear la instrucción de pago debido a que " +
                        $"la solicitud de pago está en estado {paymentOrder.Status.GetName()}.");

      IPaymentsBrokerService brokerService = broker.GetService();

      PaymentInstruction instruction = paymentOrder.CreatePaymentInstruction(broker);

      paymentOrder.Save();

      PaymentInstructionDto instructionDto = PaymentInstructionMapper.MapForBroker(instruction);

      PaymentInstructionResultDto paymentResult = await brokerService.SendPaymentInstruction(instructionDto);

      paymentOrder.UpdatePaymentInstruction(instruction,
                                            paymentResult.ExternalRequestID,
                                            paymentResult.Status);

      paymentOrder.Save();

      UpdatePaymentLog(instruction, paymentResult);

      return instruction;
    }


    #endregion Services

    #region Helpers

    static private void UpdatePaymentOrder(PaymentOrder paymentOrder,
                                          PaymentInstructionStatusDto newStatus) {
      if (newStatus.Status == PaymentInstructionStatus.Payed) {
        paymentOrder.SetAsPayed();
        paymentOrder.Save();
      } else if (newStatus.Status == PaymentInstructionStatus.Failed) {
        paymentOrder.Cancel();
        paymentOrder.Save();
      }
    }


    static private void UpdatePaymentLog(PaymentInstruction paymentInstruction,
                                         PaymentInstructionResultDto paymentResultDto) {

      var logEntry = new PaymentInstructionLogEntry(paymentInstruction, paymentResultDto);

      logEntry.Save();
    }

    private void UpdatePaymentLog(PaymentInstruction paymentInstruction,
                                  PaymentInstructionStatusDto newStatus) {
      var logEntry = new PaymentInstructionLogEntry(paymentInstruction, newStatus);

      if (paymentInstruction.Status != newStatus.Status) {
        paymentInstruction.UpdateStatus(newStatus.Status);
        paymentInstruction.Save();
      }

      logEntry.Save();
    }

    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor
