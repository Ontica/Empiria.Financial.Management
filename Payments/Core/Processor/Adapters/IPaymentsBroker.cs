/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adpaters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : IPaymentsBroker                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface to integrate payment brokers with Empiria Payments.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Interface to integrate payment brokers with Empiria Payments.</summary>
  public interface IPaymentsBroker {

    PaymentResultDto CancelPaymentInstruction(PaymentInstructionDto instruction);

    PaymentResultDto GetPaymentInstructionStatus(string instructionUID);

    PaymentResultDto SendPaymentInstruction(PaymentInstructionDto instruction);

  }  // interface IPaymentsBroker

}  // namespace Empiria.Payments.Processor.Adapters
