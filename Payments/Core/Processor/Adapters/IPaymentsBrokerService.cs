/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adpaters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : IPaymentsBrokerService                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface to integrate payments broker service providers with Empiria Payments.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Interface to integrate payments broker service providers with Empiria Payments.</summary>
  public interface IPaymentsBrokerService {

    PaymentInstructionResultDto CancelPaymentInstruction(PaymentInstructionDto instruction);

    PaymentInstructionStatusDto GetPaymentInstructionStatus(string instructionUID);

    PaymentInstructionResultDto SendPaymentInstruction(PaymentInstructionDto instruction);

  }  // interface IPaymentsBrokerService

}  // namespace Empiria.Payments.Processor.Adapters
