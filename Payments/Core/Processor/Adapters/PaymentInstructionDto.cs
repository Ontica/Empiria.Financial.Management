/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : PaymentInstructionDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that holds information about a payment instruction to be sent to a payments broker. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Output DTO that holds information about a payment instruction
  /// to be sent to a payments broker.</summary>
  public class PaymentInstructionDto {

    public PaymentOrder PaymentOrder {
      get; internal set;
    }

    public string ReferenceNo {
      get; internal set;
    }

    public string RequestUniqueNo {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

  }  // class PaymentInstructionDto

}  // namespace Empiria.Payments.Processor.Adapters
