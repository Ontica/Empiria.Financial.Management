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
using Empiria.StateEnums;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Financial.Adapters;

namespace Empiria.Payments.Adapters {

  static internal class PaymentInstructionMapper {

    #region Mappers

    static internal PaymentInstructionHolderDto Map(PaymentInstruction instruction) {

      var paymentOrder = instruction.PaymentOrder;
      var bills = Bill.GetListFor(paymentOrder.PayableEntity);

      return new PaymentInstructionHolderDto {
        PaymentInstruction = MapToDto(instruction),
        Log = PaymentInstructionLogMapper.Map(instruction),
        Bills = BillMapper.MapToBillStructure(bills),
        Documents = DocumentServices.GetEntityDocuments(paymentOrder),
        History = HistoryServices.GetEntityHistory(paymentOrder),
        Actions = MapActions(instruction.Rules)
      };
    }


    static internal FixedList<PaymentInstructionDescriptor> MapToDescriptor(FixedList<PaymentInstruction> instructions) {
      return instructions.Select(x => MapToDescriptor(x))
                         .ToFixedList();
    }

    #endregion Mappers

    #region Helpers

    static private PaymentInstructionActions MapActions(PaymentInstructionRules rules) {
      return new PaymentInstructionActions {
        CanUpdate = rules.CanUpdate(),
        CanCancel = rules.CanCancel(),
        CanReset = rules.CanReset(),
        CanSuspend = rules.CanSuspend(),
        CanRequestPayment = rules.CanRequestPayment(),
        CanCancelPaymentRequest = rules.CanCancelPaymentRequest(),
        CanSetAsPayed = rules.CanSetAsPayed(),
        CanEditDocuments = rules.CanEditDocuments()
      };
    }


    static private PaymentInstructionDto MapToDto(PaymentInstruction instruction) {
      PaymentOrder paymentOrder = instruction.PaymentOrder;

      return new PaymentInstructionDto {
        UID = instruction.UID,
        Currency = paymentOrder.Currency.MapToNamedEntity(),
        DueTime = paymentOrder.DueTime,
        PaymentAccount = new PaymentAccountDto(paymentOrder.PaymentAccount),
        RequestedBy = paymentOrder.RequestedBy.MapToNamedEntity(),
        RecordedBy = paymentOrder.PostedBy.MapToNamedEntity(),
        OperationNo = instruction.BrokerInstructionNo,
        PayTo = instruction.PaymentOrder.PayTo.MapToNamedEntity(),
        PaymentInstructionNo = instruction.PaymentInstructionNo,
        PaymentOrderType = instruction.PaymentOrder.PaymentType.MapToNamedEntity(),

        PaymentMethod = new PaymentMethodDto(instruction.PaymentOrder.PaymentMethod),

        Priority = paymentOrder.Priority.MapToDto(),

        PayableNo = paymentOrder.PayableEntity.EntityNo,
        PayableType = paymentOrder.PayableEntity.GetEmpiriaType().MapToNamedEntity(),
        Payable = paymentOrder.PayableEntity.MapToNamedEntity(),

        ReferenceNumber = instruction.PaymentOrder.ReferenceNumber,
        Status = instruction.Status.MapToNamedEntityDto(),
        RequestedTime = instruction.PostingTime,
        Total = instruction.PaymentOrder.Total,
        PaymentOrderNo = instruction.PaymentOrder.PaymentOrderNo,
        Description = instruction.PaymentOrder.Description,

        LastUpdateTime = instruction.LastUpdateTime,
      };
    }


    static private PaymentInstructionDescriptor MapToDescriptor(PaymentInstruction instruction) {
      PaymentOrder paymentOrder = instruction.PaymentOrder;

      return new PaymentInstructionDescriptor {
        UID = instruction.UID,
        PaymentInstructionNo = instruction.PaymentInstructionNo,
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PaymentOrderType = instruction.PaymentOrder.PaymentType.Name,
        RequestedBy = instruction.PaymentOrder.RequestedBy.Name,
        RecordedBy = instruction.PostedBy.Name,
        RequestedTime = instruction.PostingTime,
        LastUpdateTime = instruction.LastUpdateTime,
        OperationNo = instruction.BrokerInstructionNo,
        PayTo = instruction.PaymentOrder.PayTo.Name,
        Description = instruction.Description,
        PriorityName = paymentOrder.Priority.GetName(),

        PayableNo = paymentOrder.PayableEntity.EntityNo,
        PayableTypeName = paymentOrder.PayableEntity.GetEmpiriaType().DisplayName,
        PayableName = paymentOrder.PayableEntity.Name,

        PaymentAccount = $"{paymentOrder.PaymentAccount.Institution.Name} {paymentOrder.PaymentAccount.AccountNo}",
        PaymentMethod = paymentOrder.PaymentMethod.Name,


        DueTime = paymentOrder.DueTime,
        Total = paymentOrder.Total,
        CurrencyCode = instruction.PaymentOrder.Currency.ISOCode,
        StatusName = instruction.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Adapters
