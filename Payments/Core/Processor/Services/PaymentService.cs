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

    internal async Task RefreshPaymentInstruction(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "paymentInstruction must be stored.");


      Assertion.Require(!instruction.Status.IsFinal(),
                        $"La instrucción de pago está en un estado final: {instruction.Status.GetName()}");


      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      Assertion.Require(instruction.ExternalRequestUniqueNo, "ExternalRequestUniqueNo missed.");

      PaymentInstructionStatusDto newStatus = await paymentsService.GetPaymentInstructionStatus(instruction.ExternalRequestUniqueNo);

      UpdatePaymentOrder(instruction, newStatus);

      UpdatePaymentLog(instruction, newStatus);
    }


    public async Task<PaymentOrderHolderDto> SendPaymentOrderToPay(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);

      PaymentsBrokerConfigData broker = PaymentsBrokerConfigData.GetPaymentsBroker(paymentOrder);

      _ = await SendToPay(broker, paymentOrder);

      return PaymentOrderMapper.Map(paymentOrder);
    }


    public async Task<int> ValidatePayment() {
      var paymentInstructions = PaymentInstruction.GetInProgress();
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

      PaymentsBrokerConfigData broker = PaymentsBrokerConfigData.GetPaymentsBroker(paymentOrder);

      var currentInstruction = paymentOrder.PaymentInstructions.Current;

      if (currentInstruction.Status == PaymentInstructionStatus.InProcess) {
        await RefreshPaymentInstruction(currentInstruction);
      }

      return PaymentOrderMapper.Map(paymentOrder);
    }


    internal async Task<PaymentInstruction> SendToPay(PaymentsBrokerConfigData broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(!broker.IsEmptyInstance, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));

      await Task.CompletedTask;

      Assertion.Require(paymentOrder.PaymentInstructions.CanCreateNewInstruction(),
                        $"No es posible crear la instrucción de pago debido a que " +
                        $"la solicitud de pago está en estado {paymentOrder.Status.GetName()}.");

      IPaymentsBrokerService brokerService = broker.GetService();

      PaymentInstruction instruction = paymentOrder.PaymentInstructions.CreatePaymentInstruction(broker);

      paymentOrder.Save();

      PaymentInstructionDto instructionDto = PaymentInstructionMapper.MapForBroker(instruction);

      PaymentInstructionResultDto paymentResult = await brokerService.SendPaymentInstruction(instructionDto);

      paymentOrder.PaymentInstructions.UpdatePaymentInstruction(instruction,
                                            paymentResult.ExternalRequestID,
                                            paymentResult.Status);

      paymentOrder.Save();

      UpdatePaymentLog(instruction, paymentResult);

      return instruction;
    }


    #endregion Services

    #region Helpers

    static private void UpdatePaymentOrder(PaymentInstruction instruction,
                                           PaymentInstructionStatusDto newStatus) {
      var paymentOrder = instruction.PaymentOrder;

      if (newStatus.Status == PaymentInstructionStatus.Payed) {
        paymentOrder.EventHandler(instruction, PaymentOrderStatus.Payed);

      } else if (newStatus.Status == PaymentInstructionStatus.Failed) {
        paymentOrder.EventHandler(instruction, PaymentOrderStatus.Failed);

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
