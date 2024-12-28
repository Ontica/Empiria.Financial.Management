/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration Data Transfer Object        *
*  Type     : PaymentInstructionResultDto                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Integration DTO that holds a a payment instruction result sent by a payments broker.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Integration DTO that holds a a payment result sent by a payments broker.</summary>
  public class PaymentInstructionResultDto {

    public string ExternalRequestID {
      get; set;
    } = string.Empty;


    public string PaymentNo {
      get; set;
    } = string.Empty;


    public string ExternalStatusName {
      get; set;
    } = string.Empty;


    public string ExternalResultText {
      get; set;
    } = string.Empty;


    public PaymentInstructionStatus Status {
      get; set;
    } = PaymentInstructionStatus.Pending;


  }  // class PaymentInstructionResultDto



  public class PaymentInstructionStatusDto {

    public string ExternalRequestID {
      get; set;
    } = string.Empty;


    public string ExternalStatusName {
      get; set;
    } = string.Empty;


    public PaymentInstructionStatus Status {
      get; set;
    } = PaymentInstructionStatus.Pending;

  }  // class PaymentInstructionStatusDto

}  // namespace Empiria.Payments.Processor.Adapters
