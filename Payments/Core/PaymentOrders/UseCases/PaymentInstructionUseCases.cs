/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PaymentInstructionUseCases                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payment instructions.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Payments.Adapters;
using Empiria.Payments.Data;

namespace Empiria.Payments.UseCases {

  /// <summary>Use cases for payment instructions.</summary>
  public class PaymentInstructionUseCases : UseCase {

    #region Constructors and parsers

    protected PaymentInstructionUseCases() {
      // no-op
    }

    static public PaymentInstructionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<PaymentInstructionUseCases>();
    }

    #endregion Constructors and parsers

    #region Query use cases

    public PaymentInstructionHolderDto GetPaymentInstruction(string instructionUID) {
      Assertion.Require(instructionUID, nameof(instructionUID));

      var instruction = PaymentInstruction.Parse(instructionUID);

      return PaymentInstructionMapper.Map(instruction);
    }


    public FixedList<PaymentInstructionDescriptor> SearchPaymentInstructions(PaymentOrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      query.SearchPaymentInstructions = true;

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<PaymentInstruction> instructions = PaymentInstructionData.SearchPaymentInstructions(filter, sort);

      return PaymentInstructionMapper.MapToDescriptor(instructions);
    }

    #endregion Query use cases

    #region Command use cases

    public PaymentInstructionHolderDto Cancel(string instructionUID) {

      return ProcessEvent(instructionUID, PaymentInstructionEvent.Cancel);
    }


    public PaymentInstructionHolderDto CancelPaymentRequest(string instructionUID) {

      return ProcessEvent(instructionUID, PaymentInstructionEvent.CancelPaymentRequest);
    }


    public PaymentInstructionHolderDto RequestPayment(string instructionUID) {

      return ProcessEvent(instructionUID, PaymentInstructionEvent.RequestPayment);
    }


    public PaymentInstructionHolderDto Reset(string instructionUID) {

      return ProcessEvent(instructionUID, PaymentInstructionEvent.Reset);
    }


    public PaymentInstructionHolderDto SetAsPayed(string instructionUID, string message) {

      return ProcessEvent(instructionUID, PaymentInstructionEvent.SetAsPayed, message);
    }


    public PaymentInstructionHolderDto Suspend(string instructionUID) {

      return ProcessEvent(instructionUID, PaymentInstructionEvent.Suspend);
    }

    #endregion Command use cases

    #region Helpers

    private PaymentInstructionHolderDto ProcessEvent(string instructionUID,
                                                     PaymentInstructionEvent instructionEvent) {
      return ProcessEvent(instructionUID, instructionEvent, string.Empty);
    }


    private PaymentInstructionHolderDto ProcessEvent(string instructionUID,
                                                     PaymentInstructionEvent instructionEvent,
                                                     string userMessage) {
      Assertion.Require(instructionUID, nameof(instructionUID));

      userMessage = userMessage ?? string.Empty;

      var instruction = PaymentInstruction.Parse(instructionUID);

      Assertion.Require(!instruction.IsEmptyInstance,
                        "Instruction can't be the empty instance");

      Assertion.Require(!instruction.Status.IsFinal(),
                        $"La instrucción de pago está en el estado final {instruction.Status.GetName()}.");

      instruction.EventHandler(instructionEvent, userMessage);

      instruction.Save();

      return PaymentInstructionMapper.Map(instruction);
    }

    #endregion Helpers

  }  // class PaymentInstructionUseCases

}  // namespace Empiria.Payments.UseCases
