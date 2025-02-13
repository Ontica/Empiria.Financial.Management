/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PaymentOrderUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payment orders management.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;

using Empiria.Payments.Payables;

using Empiria.Payments.Orders.Adapters;
using Empiria.Payments.Orders.Data;

using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Services;


namespace Empiria.Payments.Orders.UseCases {

  /// <summary>Use cases for payment orders management.</summary>
  public class PaymentOrderUseCases : UseCase {

    #region Constructors and parsers

    protected PaymentOrderUseCases() {
      // no-op
    }

    static public PaymentOrderUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<PaymentOrderUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public PaymentOrderHolderDto CreatePaymentOrder(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payable = Payable.Parse(fields.PayableUID);

      if (PaymentOrder.TryGetFor(payable) != null) {
        Assertion.EnsureNoReachThisCode("La obligación de pago ya cuenta con una orden de pago");
      }

      var order = new PaymentOrder(fields);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public void DeletePaymentOrder(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      order.Delete();

      order.Save();
    }


    public PaymentOrderHolderDto GetPaymentOrder(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      return PaymentOrderMapper.Map(order);
    }


    public FixedList<NamedEntityDto> GetPaymentOrderTypes() {
      var paymentOrderTypes = PaymentOrderType.GetList();

      return paymentOrderTypes.MapToNamedEntityList();
    }
         


    public PaymentOrderHolderDto SendPaymentOrderToPay(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);   

      PaymentsBroker broker = PaymentsBroker.GetPaymentsBroker(paymentOrder);

      using (var usecases = PaymentService.ServiceInteractor()) {

        _ = usecases.SendToPay(broker, paymentOrder);

        return PaymentOrderMapper.Map(paymentOrder);
      }
    }
        

    public FixedList<PaymentOrderDescriptor> SearchPaymentOrders(PaymentOrdersQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<PaymentOrder> paymentOrders = PaymentOrderData.GetPaymentOrders(filter, sort);

      return PaymentOrderMapper.MapToDescriptor(paymentOrders);
    }


    public PaymentOrderHolderDto SuspendPaymentOrder(string paymentOrderUID,
                                                     string suspendedByUID,
                                                     DateTime suspendedUntil) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));
      Assertion.Require(suspendedByUID, nameof(suspendedByUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      var suspendedBy = Contacts.Contact.Parse(suspendedByUID);

      order.Suspend(suspendedBy, suspendedUntil);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto UpdatePaymentOrder(string uid, PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = PaymentOrder.Parse(uid);

      order.Update(fields);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto ValidatePaymentOrderIsPayed(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);

      PaymentsBroker broker = PaymentsBroker.GetPaymentsBroker(paymentOrder);

      var paymentInstructions = PaymentInstruction.GetListFor(paymentOrder)
                               .FindAll(instruction => instruction.Status == PaymentInstructionStatus.InProcess);
   
      using (var usecases = PaymentService.ServiceInteractor()) {
        foreach (var instruction in paymentInstructions) {
          usecases.UpdatePaymentInstructionStatus(instruction);
        }
       
        return PaymentOrderMapper.Map(paymentOrder);
      }

    }

    #endregion Use cases

  }  // class PaymentOrderUseCases

}  // namespace Empiria.Payments.Orders.UseCases
