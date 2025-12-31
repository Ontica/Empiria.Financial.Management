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
        CanEditDocuments = rules.CanEditDocuments()
      };
    }


    static private PaymentInstructionDto MapToDto(PaymentInstruction instruction) {
      return new PaymentInstructionDto {
        UID = instruction.UID,
        Currency = instruction.PaymentOrder.Currency.MapToNamedEntity(),
        DueTime = instruction.PaymentOrder.DueTime,
        PaymentAccount = new PaymentAccountDto(instruction.PaymentOrder.PaymentAccount),
        RequestedBy = instruction.PaymentOrder.RequestedBy.MapToNamedEntity(),
        PayTo = instruction.PaymentOrder.PayTo.MapToNamedEntity(),
        PaymentInstructionNo = instruction.PaymentInstructionNo,
        PaymentMethod = new PaymentMethodDto(instruction.PaymentOrder.PaymentMethod),
        PaymentInstructionType = instruction.PaymentOrder.PayableEntity.GetEmpiriaType().MapToNamedEntity(),
        ProgrammedDate = instruction.ProgrammedDate,
        ReferenceNumber = instruction.PaymentOrder.ReferenceNumber,
        Status = instruction.Status.MapToNamedEntityDto(),
        RequestedTime = instruction.PostingTime,
        Total = instruction.PaymentOrder.Total,
        PaymentOrderNo = instruction.PaymentOrder.PaymentOrderNo,
        Description = instruction.PaymentOrder.Description,
        LastUpdateTime = instruction.PostingTime,
      };
    }


    static private PaymentInstructionDescriptor MapToDescriptor(PaymentInstruction instruction) {
      return new PaymentInstructionDescriptor {
        UID = instruction.UID,
        PaymentInstructionTypeName = instruction.BrokerConfigData.Name,
        PayTo = instruction.PaymentOrder.PayTo.Name,
        Description = instruction.Description,
        LastUpdateTime = instruction.PostingTime,
        PaymentOrderNo = instruction.PaymentOrder.PaymentOrderNo,
        ProgrammedDate = instruction.ProgrammedDate,
        PaymentInstructionNo = instruction.PaymentInstructionNo,
        PaymentAccount = $"{instruction.PaymentOrder.PaymentAccount.Institution.Name} {instruction.PaymentOrder.PaymentAccount.AccountNo}",
        PaymentMethod = instruction.PaymentOrder.PaymentMethod.Name,
        RequestedBy = instruction.BrokerInstructionNo,
        RequestedTime = instruction.PostingTime,
        DueTime = instruction.PaymentOrder.DueTime,
        Total = instruction.PaymentOrder.Total,
        CurrencyCode = instruction.PaymentOrder.Currency.ISOCode,
        StatusName = instruction.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class PaymentInstructionMapper

}  // namespace Empiria.Payments.Adapters
