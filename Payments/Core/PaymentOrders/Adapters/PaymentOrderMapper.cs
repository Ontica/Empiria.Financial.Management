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
using Empiria.StateEnums;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Payments.Adapters {

  /// <summary>Provides data mapping services for payment orders.</summary>
  static public class PaymentOrderMapper {

    static internal PaymentOrderHolderDto Map(PaymentOrder paymentOrder) {
      var bills = Bill.GetListFor(paymentOrder.PayableEntity);
      var txns = BudgetTransaction.GetFor((IBudgetable) paymentOrder.PayableEntity);
      var instructions = paymentOrder.PaymentInstructions.ToFixedList();

      var documents = DocumentServices.GetAllEntityDocuments(paymentOrder);

      documents = FixedList<DocumentDto>.Merge(documents,
                                               DocumentServices.GetAllEntityDocuments((BaseObject) paymentOrder.PayableEntity));

      if (paymentOrder.PayableEntity is IBudgetable) {
        var payableEntity = (IBudgetable) paymentOrder.PayableEntity;

        foreach (var entity in payableEntity.GetPayableEntities()) {
          var entityDocuments = DocumentServices.GetAllEntityDocuments((BaseObject) entity);

          documents = FixedList<DocumentDto>.Merge(documents, entityDocuments);
        }
      }

      return new PaymentOrderHolderDto {
        PaymentOrder = MapToDto(paymentOrder),
        PayableEntity = PayableEntityMapper.Map(paymentOrder.PayableEntity),
        Items = MapItems(paymentOrder.PayableEntity.Items),
        Bills = BillMapper.MapToBillStructure(bills),
        BudgetTransactions = BudgetTransactionMapper.MapToDescriptor(txns),
        PaymentInstructions = PaymentInstructionMapper.MapToDescriptor(instructions),
        Documents = documents,
        History = HistoryServices.GetEntityHistory(paymentOrder),
        Actions = MapActions(paymentOrder)
      };
    }


    static public FixedList<PaymentOrderDescriptor> MapToDescriptor(FixedList<PaymentOrder> orders) {
      return orders.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }


    static public PaymentOrderDto MapToDto(PaymentOrder paymentOrder) {
      return new PaymentOrderDto {
        UID = paymentOrder.UID,
        PaymentType = paymentOrder.PaymentType.MapToNamedEntity(),
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.MapToNamedEntity(),
        Debtor = paymentOrder.Debtor.MapToNamedEntity(),
        RequestedDate = paymentOrder.PostingTime,
        RequestedBy = paymentOrder.RequestedBy.MapToNamedEntity(),
        RecordedBy = paymentOrder.PostedBy.MapToNamedEntity(),
        DueTime = paymentOrder.DueTime,
        Priority = paymentOrder.Priority,
        Description = paymentOrder.Description,
        Observations = paymentOrder.Observations,
        Budget = paymentOrder.PayableEntity.Budget.MapToNamedEntity(),
        BudgetType = paymentOrder.PayableEntity.Budget.MapToNamedEntity(),
        PaymentMethod = new PaymentMethodDto(paymentOrder.PaymentMethod),
        PaymentAccount = new PaymentAccountDto(paymentOrder.PaymentAccount),
        Currency = paymentOrder.Currency.MapToNamedEntity(),
        ExchangeRate = paymentOrder.ExchangeRate,
        Total = paymentOrder.Total,
        Status = paymentOrder.Status.MapToNamedEntity(),
        ReferenceNumber = paymentOrder.ReferenceNumber,
      };
    }

    #region Helpers

    static private FixedList<PaymentOrderItemDto> MapItems(FixedList<IPayableEntityItem> items) {
      return items.Select(x => MapItems(x))
                  .ToFixedList();
    }


    static private PaymentOrderItemDto MapItems(IPayableEntityItem payableItem) {
      return new PaymentOrderItemDto {
        UID = payableItem.UID,
        BudgetAccount = payableItem.BudgetAccount.MapToNamedEntity(),
        Quantity = payableItem.Quantity,
        Name = payableItem.Description,
        Unit = payableItem.Unit.Name,
        PayableEntityItemUID = payableItem.UID,
        Total = payableItem.Subtotal
      };
    }


    static private PaymentOrderDescriptor MapToDescriptor(PaymentOrder paymentOrder) {
      return new PaymentOrderDescriptor {
        UID = paymentOrder.UID,
        PaymentTypeName = paymentOrder.PaymentType.Name,
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayTo = paymentOrder.PayTo.Name,
        Debtor = paymentOrder.Debtor.Name,
        PaymentMethod = paymentOrder.PaymentMethod.Name,
        PaymentAccount = paymentOrder.PaymentAccount.AccountNo,
        CurrencyCode = paymentOrder.PayableEntity.Currency.ISOCode,
        Total = paymentOrder.Total,
        DueTime = paymentOrder.DueTime,
        PriorityName = paymentOrder.Priority.GetName(),

        PayableNo = paymentOrder.PayableEntity.EntityNo,
        PayableTypeName = paymentOrder.PayableEntity.GetEmpiriaType().DisplayName,
        PayableName = paymentOrder.PayableEntity.Name,

        BudgetName = paymentOrder.PayableEntity.Budget.Name,

        RequestedBy = paymentOrder.RequestedBy.Name,
        RequestedTime = paymentOrder.PostingTime,

        RecordedBy = paymentOrder.PostedBy.Name,

        PaymentDescription = paymentOrder.PaymentAccount.AccountNo,

        StatusName = paymentOrder.Status.GetName()
      };
    }


    static private PaymentOrderActions MapActions(PaymentOrder paymentOrder) {
      return new PaymentOrderActions {
        CanCancel = paymentOrder.Rules.CanCancel(),
        CanSuspend = paymentOrder.Rules.CanSuspend(),
        CanReset = paymentOrder.Rules.CanReset(),
        CanUpdate = paymentOrder.Rules.CanUpdate(),
        CanEditDocuments = paymentOrder.Rules.CanEditDocuments(),
        CanApproveBudget = paymentOrder.Rules.CanApproveBudget(),
        CanGeneratePaymentInstruction = paymentOrder.Rules.CanGeneratePaymentInstruction()
      };
    }

    #endregion Helpers

  }  // class PaymentOrderMapper

}  // namespace Empiria.Payments.Adapters
