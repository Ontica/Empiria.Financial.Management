/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Services interactor class               *
*  Type     : PaymentsBrokerInvoker                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides payment services using external broker providers.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;

using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Provides payment services using external broker providers.</summary>
  internal class PaymentsBrokerInvoker : Service {

    #region Constructors and parsers

    private PaymentsBrokerInvoker() {
      // Required by Empiria Framework.
    }

    static internal PaymentsBrokerInvoker ServiceInteractor() {
      return CreateInstance<PaymentsBrokerInvoker>();
    }

    #endregion Constructors and parsers

    #region Services

    internal async Task RefreshPaymentStatus(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "Payment instruction must be stored.");
      Assertion.Require(instruction.WasSent, "Payment instruction must be sent.");

      if (instruction.Status.IsFinal()) {
        return;
      }

      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto brokerResponse = await paymentsService.RequestPaymentStatus(brokerRequest);

      instruction.UpdateStatus(brokerResponse);
    }


    internal async Task SendPaymentInstruction(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "Payment instruction must be stored.");
      Assertion.Require(!instruction.WasSent, "Payment instruction already sent.");

      Assertion.Require(instruction.Rules.IsReadyToBeRequested,
                        "PaymentInstruction is not ready to be requested.");

      IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

      BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

      BrokerResponseDto brokerResponse = await paymentsService.SendPaymentInstruction(brokerRequest);

      instruction.Sent(brokerResponse);
    }

    #endregion Services

    #region Helpers

    static private BrokerRequestDto MapToBrokerRequest(PaymentInstruction instruction) {
      return new BrokerRequestDto(instruction);
    }

    #endregion Helpers

  }  // class PaymentsBrokerInvoker

} // namespace Empiria.Payments.Processor
