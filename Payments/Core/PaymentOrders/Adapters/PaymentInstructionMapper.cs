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

    static internal PaymentInstructionHolderDto Map(PaymentInstruction instruction) {

      var paymentOrder = instruction.PaymentOrder;
      var bills = Bill.GetListFor(paymentOrder.PayableEntity);

      return new PaymentInstructionHolderDto {
        PaymentOrder = PaymentOrderMapper.MapPaymentOrder(paymentOrder),
        Log = PaymentInstructionLogMapper.Map(instruction),
        Bills = BillMapper.MapToBillDto(bills),
        Documents = DocumentServices.GetEntityDocuments(paymentOrder),
        History = HistoryServices.GetEntityHistory(paymentOrder),
        Actions = new PaymentOrderActions()
      };
    }

    static internal FixedList<PaymentInstructionDescriptor> MapToDescriptor(FixedList<PaymentInstruction> instructions) {
      return instructions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }


    #region Helpers

    static private PaymentInstructionDescriptor MapToDescriptor(PaymentInstruction instruction) {
      return new PaymentInstructionDescriptor {
        UID = instruction.UID,
        PaymentOrderTypeName = instruction.BrokerConfigData.Name,
        PayTo = instruction.PaymentOrder.PayTo.Name,
        PaymentOrderNo = instruction.PaymentInstructionNo,
        PaymentAccount = $"{instruction.PaymentOrder.PaymentAccount.Institution.Name} {instruction.PaymentOrder.PaymentAccount.AccountNo}",
        PaymentMethod = instruction.PaymentOrder.PaymentMethod.Name,
        RequestedBy = instruction.BrokerInstructionNo,
        RequestedTime = instruction.PaymentOrder.RequestedTime,
        RequestedDate = instruction.PaymentOrder.RequestedTime,
        DueTime = instruction.PaymentOrder.DueTime,
        Total = instruction.PaymentOrder.Total,
        CurrencyCode = instruction.PaymentOrder.Currency.ISOCode,
        BudgetTypeName = instruction.PaymentOrder.PayableEntity.Budget.Name,
        PayableNo = instruction.PaymentOrder.PayableEntity.EntityNo,
        PayableTypeName = instruction.BrokerConfigData.Name,
        StatusName = instruction.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Adapters
