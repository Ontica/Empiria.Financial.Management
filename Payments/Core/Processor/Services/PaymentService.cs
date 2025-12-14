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
using Empiria.Payments.Processor.Adapters;

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

      Assertion.Require(instruction.BrokerInstructionNo, nameof(instruction.BrokerInstructionNo));

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto response = await paymentsService.RequestPaymentStatus(brokerRequest);

      UpdatePaymentOrder(instruction, response);

      UpdatePaymentLog(instruction, response);
    }


    public async Task<PaymentInstructionHolderDto> SendPaymentInstruction(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "paymentInstruction must be stored.");

      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      Assertion.Require(instruction.BrokerInstructionNo.Length == 0, "BrokerInstructionNo must be empty.");

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto brokerResponse = await paymentsService.SendPaymentInstruction(brokerRequest);

      instruction.PaymentOrder.PaymentInstructions.UpdatePaymentInstruction(instruction, brokerResponse);

      instruction.PaymentOrder.Save();

      UpdatePaymentLog(instruction, brokerResponse);

      return PaymentInstructionMapper.Map(instruction);
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

      if (currentInstruction.Status == PaymentInstructionStatus.InProgress) {
        await RefreshPaymentInstruction(currentInstruction);
      }

      return PaymentOrderMapper.Map(paymentOrder);
    }

    #endregion Services

    #region Helpers

    static private BrokerRequestDto MapToBrokerRequest(PaymentInstruction instruction) {
      return new BrokerRequestDto(instruction);
    }


    static private void UpdatePaymentOrder(PaymentInstruction instruction,
                                           BrokerResponseDto brokerResponse) {
      var paymentOrder = instruction.PaymentOrder;

      if (brokerResponse.Status == PaymentInstructionStatus.Payed) {
        paymentOrder.EventHandler(instruction, PaymentOrderStatus.Payed);

      } else if (brokerResponse.Status == PaymentInstructionStatus.Failed) {
        paymentOrder.EventHandler(instruction, PaymentOrderStatus.Failed);

      }
    }


    static private void UpdatePaymentLog(PaymentInstruction instruction,
                                         BrokerResponseDto brokerResponse) {

      var logEntry = new PaymentInstructionLogEntry(instruction, brokerResponse);

      logEntry.Save();
    }

    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor
