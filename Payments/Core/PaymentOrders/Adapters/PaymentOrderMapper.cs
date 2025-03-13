/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentOrderMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payment orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.History.Services;

using Empiria.Payments.Payables.Adapters;

using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Adapters;
using Empiria.Payments.Processor.Services;


namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Provides data mapping services for payment orders.</summary>
  static internal class PaymentOrderMapper {

    static internal PaymentOrderHolderDto Map(PaymentOrder paymentOrder) {
      return new PaymentOrderHolderDto {
        PaymentOrder = MapPaymentOrder(paymentOrder),
        Items = PayableItemMapper.Map(paymentOrder.Payable.GetItems()),
        Bills = ExternalServices.GetPayableBills(paymentOrder.Payable),
        Documents = DocumentServices.GetEntityDocuments(paymentOrder),
        History = HistoryServices.GetEntityHistory(paymentOrder),
        Log = GetPaymentOrderLog(paymentOrder),
        Actions = MapActions(paymentOrder)
      };
    }

    static internal FixedList<PaymentOrderDescriptor> MapToDescriptor(FixedList<PaymentOrder> orders) {
      return orders.Select(x => MapToDescriptor(x)).ToFixedList();
    }

    #region Helpers

    static private PaymentOrderDescriptor MapToDescriptor(PaymentOrder paymentOrder) {
      return new PaymentOrderDescriptor {
        UID = paymentOrder.UID,
        PaymentOrderTypeName = paymentOrder.PaymentOrderType.Name,
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.Name,
        PaymentMethod = paymentOrder.PaymentMethod.Name,
        Total = paymentOrder.Total,
        Currency = paymentOrder.Currency.Name,
        RequestedBy = paymentOrder.RequestedBy.Name,
        RequestedDate = paymentOrder.RequestedTime,
        DueTime = paymentOrder.DueTime,
        StatusName = paymentOrder.Status.GetName()
      };
    }

    private static PaymentOrderActionsDto MapActions(PaymentOrder paymentOrder) {
      var paymentOrderActions = PaymentOrderActions.SetActions(paymentOrder.Status);

      return new PaymentOrderActionsDto {
        CanDelete =  paymentOrderActions.CanDelete,
        CanEditDocuments = paymentOrderActions.CanEditDocuments,
        CanSendToPay  = paymentOrderActions.CanSendToPay,
        CanUpdate = paymentOrderActions.CanUpdate,
      };

    }


    static private PaymentAccountDto MapPaymentAccount(PaymentAccount paymentAccount) {
      return PaymentAccountMapper.Map(paymentAccount);
    }


    static private PaymentOrderDto MapPaymentOrder(PaymentOrder paymentOrder) {
      return new PaymentOrderDto {
        UID = paymentOrder.UID,
        PaymentOrderType = paymentOrder.PaymentOrderType.MapToNamedEntity(),
        OrderNo = paymentOrder.PaymentOrderNo,
        ControlNo = paymentOrder.ControlNo,
        PayTo = paymentOrder.PayTo.MapToNamedEntity(),
        RequestedBy = paymentOrder.RequestedBy.MapToNamedEntity(),
        RequestedDate = paymentOrder.RequestedTime,
        DueTime = paymentOrder.DueTime,
        Notes = paymentOrder.Description,
        PaymentMethod = paymentOrder.PaymentMethod.MapToNamedEntity(),
        PaymentAccount = MapPaymentAccount(paymentOrder.PaymentAccount),
        Currency = paymentOrder.Currency.MapToNamedEntity(),
        Total = paymentOrder.Total,
        Status = paymentOrder.Status.MapToNamedEntity(),
        ReferenceNumber = paymentOrder.ReferenceNumber,
      };
    }
    

    static private FixedList<PaymentInstructionLogDescriptorDto> GetPaymentOrderLog(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));
          
      using (var usecases = PaymentService.ServiceInteractor()) {

        FixedList<PaymentInstructionLogEntry> paymentInstructionLogs = usecases.GetPaymentInstructionLogs(paymentOrder);

        return PaymentInstructionLogMapper.Map(paymentInstructionLogs);
      }
    }

    #endregion Helpers

  }  // class PaymentOrderMapper

}  // namespace Empiria.Payments.Orders.Adapters
