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
using Empiria.Payments.Adapters;

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

    static internal FixedList<PaymentOrderDescriptor> MapToDescriptor(FixedList<PaymentInstruction> instructions) {
      return instructions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }

    #region Helpers

    static private PaymentOrderDescriptor MapToDescriptor(PaymentInstruction x) {
      return new PaymentOrderDescriptor {
        UID = x.UID,
        PaymentOrderTypeName = x.Broker.Name,
        PayTo = x.PaymentOrder.PayTo.Name,
        PaymentOrderNo = x.PaymentInstructionNo,
        PaymentAccount = x.PaymentOrder.PaymentAccount.AccountNo,
        PaymentMethod = x.PaymentOrder.PaymentMethod.Name,
        RequestedBy = x.PaymentOrder.RequestedBy.Name,
        RequestedDate = x.PostingTime,
        DueTime = x.PaymentOrder.DueTime,
        Total = x.PaymentOrder.Total,
      };
    }

    #endregion Helpers

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Processor.Adapters
