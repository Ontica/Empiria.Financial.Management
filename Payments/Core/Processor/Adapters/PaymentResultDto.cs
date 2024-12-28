/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration Data Transfer Object        *
*  Type     : PaymentResultDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Integration DTO that holds a a payment result sent by a payments broker.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Integration DTO that holds a a payment result sent by a payments broker.</summary>
  public class PaymentResultDto {

    public string RequestID {
      get; set;
    } = string.Empty;


    public string PaymentNo { 
      get; set; 
    } = string.Empty;


    public char Status {
      get; set;
    } = 'P';


    public string StatusName {
      get; set;
    } = string.Empty;


    public string Text {
      get; set;
    } = string.Empty;


    public bool Failed {
      get; set;
    }

  }  // class PaymentResultDto

}  // namespace Empiria.Payments.Processor.Adapters
