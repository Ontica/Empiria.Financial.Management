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

      if (instruction.Status.IsFinal()) {
        return;
      }

      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      Assertion.Require(instruction.BrokerInstructionNo, nameof(instruction.BrokerInstructionNo));

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto brokerResponse = await paymentsService.RequestPaymentStatus(brokerRequest);

      instruction.Update(brokerResponse);
    }


    public async Task SendPaymentInstruction(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "paymentInstruction must be stored.");
      Assertion.Require(instruction.BrokerInstructionNo.Length == 0, "BrokerInstructionNo must be empty.");

      Assertion.Require(instruction.Status == PaymentInstructionStatus.WaitingRequest,
                        "PaymentInstruction status must be 'WaitingRequest'.");

      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto brokerResponse = await paymentsService.SendPaymentInstruction(brokerRequest);

      instruction.Update(brokerResponse);
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

    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor
