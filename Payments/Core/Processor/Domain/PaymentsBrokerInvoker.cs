/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Services interactor class               *
*  Type     : PaymentsBrokerInvoker                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides payment services using external broker providers.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
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

      try {
        IPaymentsBrokerService paymentsService = instruction.BrokerConfigData.GetService();

        BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

        BrokerResponseDto brokerResponse = await paymentsService.RequestPaymentStatus(brokerRequest);

        if (brokerResponse.Status == PaymentInstructionStatus.Payed &&
            instruction.Rules.IsReadyToBeMarkedAsPayed) {

          brokerResponse.Status = PaymentInstructionStatus.Payed;

        } else if (brokerResponse.Status == PaymentInstructionStatus.Payed &&
                   instruction.Status == PaymentInstructionStatus.Requested) {

          brokerResponse.Status = PaymentInstructionStatus.PaymentConfirmation;
        }

        instruction.UpdateStatus(brokerResponse);

      } catch (Exception e) {

        BrokerResponseDto exceptionResponse = MapToBrokerResponse(instruction, e);

        instruction.UpdateStatus(exceptionResponse);

        throw;
      }
    }


    internal async Task SendPaymentInstruction(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "Payment instruction must be stored.");
      Assertion.Require(!instruction.WasSent, "Payment instruction already sent.");
      Assertion.Require(instruction.Rules.IsReadyToBeRequested,
                        "PaymentInstruction is not ready to be requested.");

      try {

        var brokerData = instruction.BrokerConfigData;

        IPaymentsBrokerService paymentsService = brokerData.GetService();

        BrokerRequestDto brokerRequest = MapToBrokerRequest(instruction);

        BrokerResponseDto brokerResponse = await paymentsService.SendPaymentInstruction(brokerRequest);

        Assertion.Require(brokerResponse,
                          $"La resupuesta de {brokerData.Name} regresó vacía.");

        if (brokerResponse.Status == PaymentInstructionStatus.Failed) {
          throw new ApplicationException(
            $"Ocurrió un problema en {brokerData.Name} al recibir la instrucción de pago: " +
            $"{brokerResponse.BrokerMessage}");
        }

        Assertion.Require(brokerResponse.BrokerInstructionNo,
                          $"Ocurrió un problema: {brokerData.Name} regresó un Id de solicitud vacío.");

        instruction.Sent(brokerResponse);

      } catch (Exception e) {

        BrokerResponseDto exceptionResponse = MapToBrokerResponse(instruction, e);

        instruction.UpdateStatus(exceptionResponse);

        throw;
      }
    }

    #endregion Services

    #region Helpers

    static private BrokerRequestDto MapToBrokerRequest(PaymentInstruction instruction) {
      return new BrokerRequestDto(instruction);
    }


    static private BrokerResponseDto MapToBrokerResponse(PaymentInstruction instruction, Exception exception) {
      return new BrokerResponseDto {
        BrokerInstructionNo = instruction.BrokerInstructionNo,
        PaymentInstructionNo = instruction.PaymentInstructionNo,
        BrokerMessage = exception.Message,
        BrokerStatusText = exception.Message,
        Status = PaymentInstructionStatus.Exception
      };
    }

    #endregion Helpers

  }  // class PaymentsBrokerInvoker

} // namespace Empiria.Payments.Processor
