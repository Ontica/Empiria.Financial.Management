/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentInstructionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps payment orders to payment instructions.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing;
using Empiria.Billing.Adapters;

namespace Empiria.Payments.Adapters {

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

    static internal FixedList<PaymentInstructionDescriptor> MapToDescriptor(FixedList<PaymentInstruction> instructions) {
      return instructions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }


    #region Helpers

    static private PaymentInstructionDescriptor MapToDescriptor(PaymentInstruction x) {
      return new PaymentInstructionDescriptor {
        UID = x.UID,
        PaymentOrderTypeName = x.BrokerConfigData.Name,
        PayTo = x.PaymentOrder.PayTo.Name,
        PaymentOrderNo = x.PaymentInstructionNo,
        PaymentAccount = $"{x.PaymentOrder.PaymentAccount.Institution.Name} {x.PaymentOrder.PaymentAccount.AccountNo}",
        PaymentMethod = x.PaymentOrder.PaymentMethod.Name,
        RequestedBy = x.BrokerInstructionNo,
        RequestedTime = x.PaymentOrder.RequestedTime,
        RequestedDate = x.PaymentOrder.RequestedTime,
        DueTime = x.PaymentOrder.DueTime,
        Total = x.PaymentOrder.Total,
        CurrencyCode = x.PaymentOrder.Currency.ISOCode,
        BudgetTypeName = x.PaymentOrder.PayableEntity.Budget.Name,
        PayableNo = x.PaymentOrder.PayableEntity.EntityNo,
        PayableTypeName = x.BrokerConfigData.Name,
        StatusName = x.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Adapters
