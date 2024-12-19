/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentInstructionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps payment orders to payment instructions.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Orders;

namespace Empiria.Payments.Processor.Adapters {

  static internal class PaymentInstructionMapper {

    static internal IPaymentInstruction Map(PaymentOrder paymentOrder) {
      return new PaymentInstructionDto {
        Account = paymentOrder.PaymentAccount.CLABE,
        RequestedDate = System.DateTime.Now,
        DueDate = System.DateTime.Now,
        Total = paymentOrder.Total,
        Reference = paymentOrder.PaymentOrderNo,
        Description = paymentOrder.Notes
      };

    }

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Processor.Adapters
