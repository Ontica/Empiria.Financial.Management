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
using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.History;

using Empiria.Billing.Adapters;
using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Payments.Adapters {

  /// <summary>Provides data mapping services for payment orders.</summary>
  static public class PaymentOrderMapper {

    static internal PaymentOrderHolderDto Map(PaymentOrder paymentOrder) {
      var bills = Billing.Bill.GetListFor(paymentOrder.PayableEntity);
      var txns = BudgetTransaction.GetFor((IBudgetable) paymentOrder.PayableEntity);
      var instructions = paymentOrder.PaymentInstructions.ToFixedList();

      return new PaymentOrderHolderDto {
        PaymentOrder = MapPaymentOrder(paymentOrder),
        PayableEntity = PayableEntityMapper.Map(paymentOrder.PayableEntity),
        Items = MapItems(paymentOrder.PayableEntity.Items),
        Bills = BillMapper.MapToBillDto(bills),
        BudgetTransactions = BudgetTransactionMapper.MapToDescriptor(txns),
        PaymentInstructions = PaymentInstructionMapper.MapToDescriptor(instructions),
        Documents = DocumentServices.GetAllEntityDocuments((BaseObject) paymentOrder.PayableEntity),
        History = HistoryServices.GetEntityHistory(paymentOrder),
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
        BudgetAccount = payableItem.BudgetAccount.MapToNamedEntity(),
        Quantity = payableItem.Quantity,
        Name = payableItem.Description,
        Unit = payableItem.Unit.Name,
        PayableEntityItemUID = payableItem.UID,
        Total = payableItem.Subtotal,
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
        Total = paymentOrder.Total,
        DueTime = paymentOrder.DueTime,

        PayableNo = paymentOrder.PayableEntity.EntityNo,
        PayableTypeName = paymentOrder.PayableEntity.GetEmpiriaType().DisplayName,
        ContractNo = "No aplica",
        BudgetTypeName = paymentOrder.PayableEntity.Budget.Name,

        RequestedBy = paymentOrder.RequestedBy.Name,
        RequestedTime = paymentOrder.RequestedTime,
        RequestedDate = paymentOrder.RequestedTime,

        StatusName = paymentOrder.Status.GetName()
      };
    }


    static private PaymentOrderActionsDto MapActions(PaymentOrder paymentOrder) {
      var paymentOrderActions = PaymentOrderActions.SetActions(paymentOrder.Status);

      return new PaymentOrderActionsDto {
        CanDelete = paymentOrderActions.CanDelete,
        CanEditDocuments = paymentOrderActions.CanEditDocuments,
        CanGeneratePaymentInstruction = true,
        CanUpdate = paymentOrderActions.CanUpdate,
        CanApprovePayment = true,
        CanExerciseBudget = true
      };

    }

    static internal PaymentOrderDto MapPaymentOrder(PaymentOrder paymentOrder) {
      return new PaymentOrderDto {
        UID = paymentOrder.UID,
        PaymentOrderType = paymentOrder.GetEmpiriaType().MapToNamedEntity(),
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.MapToNamedEntity(),
        RequestedBy = paymentOrder.RequestedBy.MapToNamedEntity(),
        RequestedDate = paymentOrder.RequestedTime,
        DueTime = paymentOrder.DueTime,
        Description = paymentOrder.Description,
        Budget = paymentOrder.PayableEntity.Budget.MapToNamedEntity(),
        BudgetType = paymentOrder.PayableEntity.Budget.MapToNamedEntity(),
        PaymentMethod = new PaymentMethodDto(paymentOrder.PaymentMethod),
        PaymentAccount = new PaymentAccountDto(paymentOrder.PaymentAccount),
        Currency = paymentOrder.Currency.MapToNamedEntity(),
        Total = paymentOrder.Total,
        Status = paymentOrder.Status.MapToNamedEntity(),
        ReferenceNumber = paymentOrder.ReferenceNumber,
      };
    }

    #endregion Helpers

  }  // class PaymentOrderMapper

}  // namespace Empiria.Payments.Adapters
