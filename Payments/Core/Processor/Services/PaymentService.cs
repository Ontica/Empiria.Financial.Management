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
    }


    public async Task<PaymentInstructionHolderDto> SendPaymentInstruction(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "paymentInstruction must be stored.");

      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      Assertion.Require(instruction.BrokerInstructionNo.Length == 0, "BrokerInstructionNo must be empty.");

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto brokerResponse = await paymentsService.SendPaymentInstruction(brokerRequest);

      UpdatePaymentOrder(instruction, brokerResponse);

      return PaymentInstructionMapper.Map(instruction);
    }


    public async Task UpdateInProgressPaymentInstructions() {
      var inProgressInstructions = PaymentInstruction.GetInProgress();

      foreach (var instruction in inProgressInstructions) {
        await RefreshPaymentInstruction(instruction);
      }
    }


    #endregion Services

    #region Helpers

    static private BrokerRequestDto MapToBrokerRequest(PaymentInstruction instruction) {
      return new BrokerRequestDto(instruction);
    }


    static private void UpdatePaymentOrder(PaymentInstruction instruction,
                                           BrokerResponseDto brokerResponse) {
      var paymentOrder = instruction.PaymentOrder;

      paymentOrder.UpdatePaymentInstruction(instruction, brokerResponse);
    }

    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor
