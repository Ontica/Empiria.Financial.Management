/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentOrderMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payment orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Financial;
using Empiria.Financial.Adapters;

using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Adapters;
using Empiria.Payments.Processor.Services;

namespace Empiria.Payments.Adapters {

  /// <summary>Provides data mapping services for payment orders.</summary>
  static public class PaymentOrderMapper {

    static internal PaymentOrderHolderDto Map(PaymentOrder paymentOrder) {
      return new PaymentOrderHolderDto {
        PaymentOrder = MapPaymentOrder(paymentOrder),
        Items = MapItems(paymentOrder.PayableEntity.Items),
        // Bills = ExternalServices.GetPayableBills(paymentOrder.Payable),
        Documents = DocumentServices.GetAllEntityDocuments(paymentOrder),
        History = HistoryServices.GetEntityHistory(paymentOrder),
        Log = GetPaymentOrderLog(paymentOrder),
        Actions = MapActions(paymentOrder)
      };
    }

    static public FixedList<PaymentOrderDescriptor> MapToDescriptor(FixedList<PaymentOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }

    #region Helpers

    static internal FixedList<PaymentOrderItemDto> MapItems(FixedList<IPayableEntityItem> items) {
      return items.Select(x => MapItems(x))
                  .ToFixedList();
    }


    static internal PaymentOrderItemDto MapItems(IPayableEntityItem payableItem) {
      return new PaymentOrderItemDto {
        UID = payableItem.UID,
        Subtotal = payableItem.Subtotal,
        Currency = payableItem.Currency.MapToNamedEntity()
      };
    }


    static private PaymentOrderDescriptor MapToDescriptor(PaymentOrder paymentOrder) {
      return new PaymentOrderDescriptor {
        UID = paymentOrder.UID,
        PaymentOrderTypeName = paymentOrder.GetEmpiriaType().DisplayName,
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.Name,
        PaymentMethod = paymentOrder.PaymentMethod.Name,
        PaymentAccount = paymentOrder.PaymentAccount.AccountNo,
        CurrencyCode = paymentOrder.PayableEntity.Currency.Name,
        Total = paymentOrder.PayableEntity.Total,
        DueTime = paymentOrder.DueTime,

        PayableNo = paymentOrder.PayableEntity.EntityNo,
        PayableTypeName = paymentOrder.PayableEntity.GetEmpiriaType().DisplayName,
        ContractNo = "No aplica",
        BudgetTypeName = paymentOrder.PayableEntity.Budget.Name,

        RequestedBy = paymentOrder.RequestedBy.Name,
        RequestedDate = paymentOrder.RequestedTime,

        StatusName = paymentOrder.Status.GetName()
      };
    }

    private static PaymentOrderActionsDto MapActions(PaymentOrder paymentOrder) {
      var paymentOrderActions = PaymentOrderActions.SetActions(paymentOrder.Status);

      return new PaymentOrderActionsDto {
        CanDelete = paymentOrderActions.CanDelete,
        CanEditDocuments = paymentOrderActions.CanEditDocuments,
        CanSendToPay = paymentOrderActions.CanSendToPay,
        CanUpdate = paymentOrderActions.CanUpdate,
      };

    }

    static private PaymentAccountDto MapPaymentAccount(PaymentAccount paymentAccount) {
      return PaymentAccountMapper.Map(paymentAccount);
    }

    static private PaymentMethodDto MapPaymentMethod(PaymentMethod paymentMethod) {
      return PaymentMethodMapper.Map(paymentMethod);
    }

    static private PaymentOrderDto MapPaymentOrder(PaymentOrder paymentOrder) {
      return new PaymentOrderDto {
        UID = paymentOrder.UID,
        OrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.MapToNamedEntity(),
        RequestedBy = paymentOrder.RequestedBy.MapToNamedEntity(),
        RequestedDate = paymentOrder.RequestedTime,
        DueTime = paymentOrder.DueTime,
        Notes = paymentOrder.Description,
        PaymentMethod = MapPaymentMethod(paymentOrder.PaymentMethod),
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

}  // namespace Empiria.Payments.Adapters
