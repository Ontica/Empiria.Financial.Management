/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration Data Transfer Object        *
*  Type     : BrokerResponseDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Integration DTO that holds a response sent by a payments broker.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Integration DTO that holds a response sent by a payments broker.</summary>
  public class BrokerResponseDto {

    public string BrokerInstructionNo {
      get; set;
    } = string.Empty;


    public string InstructionNo {
      get; set;
    } = string.Empty;


    public string BrokerMessage {
      get; set;
    } = string.Empty;


    public string BrokerStatusText {
      get; set;
    } = string.Empty;


    public PaymentInstructionStatus Status {
      get; set;
    } = PaymentInstructionStatus.Requested;


  }  // class BrokerResponseDto

}  // namespace Empiria.Payments.Processor.Adapters
