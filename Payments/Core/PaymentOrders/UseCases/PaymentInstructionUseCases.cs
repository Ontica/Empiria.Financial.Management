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

    #region Use cases

    public PaymentInstructionHolderDto GetPaymentInstruction(string instructionUID) {
      Assertion.Require(instructionUID, nameof(instructionUID));

      var instruction = PaymentInstruction.Parse(instructionUID);

      return PaymentInstructionMapper.Map(instruction);
    }


    public FixedList<PaymentOrderDescriptor> SearchPaymentInstructions(PaymentOrdersQuery query) {
      Assertion.Require(query, nameof(query));

      var instructions = BaseObject.GetFullList<PaymentInstruction>("PYMT_INSTRUCTION_STATUS <> 'X'");

      return PaymentInstructionMapper.MapToDescriptor(instructions);
    }

    #endregion Use cases

  }  // class PaymentInstructionUseCases

}  // namespace Empiria.Payments.UseCases
