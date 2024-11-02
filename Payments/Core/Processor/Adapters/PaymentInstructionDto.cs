/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : PaymentInstructionDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that holds information about a payment instruction to be sent to a payments broker. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Interface that represents a payment instruction to integrate
  /// Empiria Payment with payments brokers.</summary>
  internal interface IPaymentInstruction {

  }  // interface IPaymentInstruction



  /// <summary>Output DTO that holds information about a payment instruction
  /// to be sent to a payments broker.</summary>
  internal class PaymentInstructionDto : IPaymentInstruction {

  }  // class PaymentInstructionDto

}  // namespace Empiria.Payments.Processor.Adapters
