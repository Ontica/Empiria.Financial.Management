/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentInstructionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps payment orders to payment instructions.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Adapters {

  static internal class PaymentInstructionMapper {

    static internal PaymentInstructionDto Map(PaymentInstruction paymentInstruction) {
      return new PaymentInstructionDto {
        RequestedTime = DateTime.Now,
        ReferenceNo = paymentInstruction.PaymentInstructionNo,
        RequestUniqueNo = paymentInstruction.PaymentInstructionNo,
        PaymentOrder = paymentInstruction.PaymentOrder,
      };
    }

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Processor.Adapters
