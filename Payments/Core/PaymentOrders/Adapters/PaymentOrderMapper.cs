/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentOrderMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payment orders.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Payables.Adapters;

namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Provides data mapping services for payment orders.</summary>
  static internal class PaymentOrderMapper {

    static internal PaymentOrderHolderDto Map(PaymentOrder paymentOrder) {
      return new PaymentOrderHolderDto {
        PaymentOrder = MapPaymentOrder(paymentOrder),
        Items = PayableItemMapper.Map(paymentOrder.Payable.GetItems()),
        Bills = ExternalServices.GetPayableBills(paymentOrder.Payable),
        Documents = ExternalServices.GetEntityDocuments(paymentOrder),
        History = ExternalServices.GetEntityHistory(paymentOrder),
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

    private static PaymentOrderActions MapActions(PaymentOrder paymentOrder) {
      return new PaymentOrderActions {
        CanDelete = true,
        CanUpdate = true,
        CanEditDocuments = true,
        CanSendToPay = true
      };
    }

    static private PaymentOrderDto MapPaymentOrder(PaymentOrder paymentOrder) {
      return new PaymentOrderDto {
        UID = paymentOrder.UID,
        OrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.Name,
        RequestedBy = paymentOrder.RequestedBy.Name,
        RequestedDate = paymentOrder.RequestedTime,
        Notes = paymentOrder.Notes,
        Total = paymentOrder.Total,
        Status = new NamedEntityDto(paymentOrder.Status.ToString(),
                                    paymentOrder.Status.GetName()),
      };
    }

    #endregion Helpers

  }  // class PaymentOrderMapper

}  // namespace Empiria.Payments.Contracts.Adapters
