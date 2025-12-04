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

namespace Empiria.Payments.Processor.Services {

  internal class PaymentService : Service {

    #region Constructors and parsers

    protected PaymentService() {
      // no-op
      PaymentValidator.Start();
    }

    static public PaymentService ServiceInteractor() {
      return CreateInstance<PaymentService>();
    }

    #endregion Constructors and parsers

    #region Services

    internal PaymentInstructionHolderDto GetPaymentInstruction(string instructionUID) {
      Assertion.Require(instructionUID, nameof(instructionUID));

      var paymentInstruction = PaymentInstruction.Parse(instructionUID);

      return PaymentInstructionMapper.Map(paymentInstruction);
    }


    internal FixedList<PaymentInstructionLogEntry> GetPaymentInstructionLogs(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      return PaymentInstructionLogEntry.GetListFor(paymentOrder);
    }


    internal FixedList<PaymentOrderDescriptor> SearchPaymentInstructions(PaymentOrdersQuery query) {
      Assertion.Require(query, nameof(query));

      var instructions = BaseObject.GetFullList<PaymentInstruction>("PYMT_INSTRUCTION_STATUS <> 'X'");

      return PaymentInstructionMapper.MapToDescriptor(instructions);
    }


    internal async Task<PaymentInstruction> SendToPay(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(!broker.IsEmptyInstance, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));

      EnsureIsNotSent(paymentOrder);

      var instruction = new PaymentInstruction(broker, paymentOrder);

      PaymentInstructionDto instructionDto = PaymentInstructionMapper.MapForBroker(instruction);

      IPaymentsBrokerService paymentsService = broker.GetService();

      PaymentInstructionResultDto paymentResult = await paymentsService.SendPaymentInstruction(instructionDto);

      UpdatePaymentInstruction(instruction, paymentResult);

      UpdatePaymentOrder(paymentOrder, paymentResult.Status);

      return instruction;
    }


    internal async Task UpdatePaymentInstructionStatus(PaymentInstruction paymentInstruction) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsNew, "paymentInstruction must be stored.");


      Assertion.Require(!paymentInstruction.Status.IsFinal(),
                        $"La instrucción de pago está en un estado final: {paymentInstruction.Status.GetName()}");


      IPaymentsBrokerService paymentsService = paymentInstruction.Broker.GetService();

      Assertion.Require(paymentInstruction.ExternalRequestUniqueNo, "ExternalRequestUniqueNo missed.");

      PaymentInstructionStatusDto newStatus = await paymentsService.GetPaymentInstructionStatus(paymentInstruction.ExternalRequestUniqueNo);

      UpdatePaymentInstruction(paymentInstruction, newStatus);

      UpdatePaymentOrder(paymentInstruction.PaymentOrder, newStatus);
    }

    #endregion Services

    #region Helpers

    static private void EnsureIsNotSent(PaymentOrder paymentOrder) {
      FixedList<PaymentInstruction> instructions = PaymentInstruction.GetListFor(paymentOrder);

      if (instructions.Contains(x => !x.Status.IsFinal())) {
        Assertion.RequireFail($"No es posible enviar la orden de pago al sistema de pagos, ya que tiene " +
                              $"una transacción pendiente.");
      }
    }


    static private void UpdatePaymentOrder(PaymentOrder paymentOrder,
                                          PaymentInstructionStatusDto newStatus) {
      if (newStatus.Status == PaymentInstructionStatus.Payed) {
        paymentOrder.SetAsPayed();
        paymentOrder.Save();
      } else if (newStatus.Status == PaymentInstructionStatus.Failed) {
        paymentOrder.Reject();
        paymentOrder.Save();
      }
    }



    static private void UpdatePaymentOrder(PaymentOrder paymentOrder,
                                           PaymentInstructionStatus status) {
      if (status == PaymentInstructionStatus.InProcess) {
        paymentOrder.SendToPay();
        paymentOrder.Save();
      } else if (status == PaymentInstructionStatus.Failed) {
        paymentOrder.SetAsPending();
        paymentOrder.Save();
      }
    }


    static private void UpdatePaymentInstruction(PaymentInstruction paymentInstruction,
                                                 PaymentInstructionResultDto paymentResultDto) {

      var logEntry = new PaymentInstructionLogEntry(paymentInstruction, paymentResultDto);

      paymentInstruction.SetExternalUniqueNo(paymentResultDto.ExternalRequestID);
      paymentInstruction.UpdateStatus(paymentResultDto.Status);

      paymentInstruction.Save();

      logEntry.Save();
    }

    private void UpdatePaymentInstruction(PaymentInstruction paymentInstruction,
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

} // namespace Empiria.Payments.Processor.Services
