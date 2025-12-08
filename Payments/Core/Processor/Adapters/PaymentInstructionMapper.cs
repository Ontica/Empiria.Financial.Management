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

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Payments.Adapters;


namespace Empiria.Payments.Processor.Adapters {

  static internal class PaymentInstructionMapper {

    static internal PaymentInstructionHolderDto Map(PaymentInstruction paymentInstruction) {

      var paymentOrder = paymentInstruction.PaymentOrder;
      var bills = Bill.GetListFor(paymentOrder.PayableEntity);

      return new PaymentInstructionHolderDto {
        PaymentOrder = PaymentOrderMapper.MapPaymentOrder(paymentOrder),
        Log = PaymentInstructionLogMapper.Map(paymentInstruction),
        Bills = BillMapper.MapToBillDto(bills),
        Documents = DocumentServices.GetEntityDocuments(paymentOrder),
        History = HistoryServices.GetEntityHistory(paymentOrder),
        Actions = new PaymentOrderActionsDto()
      };
    }


    static internal FixedList<PaymentOrderDescriptor> MapToDescriptor(FixedList<PaymentInstruction> instructions) {
      return instructions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }


    static internal PaymentInstructionDto MapForBroker(PaymentInstruction paymentInstruction) {
      return new PaymentInstructionDto {
        RequestedTime = DateTime.Now,
        ReferenceNo = paymentInstruction.PaymentInstructionNo,
        RequestUniqueNo = paymentInstruction.PaymentInstructionNo,
        PaymentOrder = paymentInstruction.PaymentOrder,
      };
    }

    #region Helpers

    static private PaymentOrderDescriptor MapToDescriptor(PaymentInstruction x) {
      return new PaymentOrderDescriptor {
        UID = x.UID,
        PaymentOrderTypeName = x.Broker.Name,
        PayTo = x.PaymentOrder.PayTo.Name,
        PaymentOrderNo = x.PaymentInstructionNo,
        PaymentAccount = $"{x.PaymentOrder.PaymentAccount.Institution.Name} {x.PaymentOrder.PaymentAccount.AccountNo}",
        PaymentMethod = x.PaymentOrder.PaymentMethod.Name,
        RequestedBy = x.ExternalRequestUniqueNo,
        RequestedTime = x.PaymentOrder.RequestedTime,
        RequestedDate = x.PaymentOrder.RequestedTime,
        DueTime = x.PaymentOrder.DueTime,
        Total = x.PaymentOrder.Total,
        CurrencyCode = x.PaymentOrder.Currency.ISOCode,
        BudgetTypeName = x.PaymentOrder.PayableEntity.Budget.Name,
        PayableNo = x.PaymentOrder.PayableEntity.EntityNo,
        PayableTypeName = x.Broker.Name,
        StatusName = x.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Processor.Adapters
