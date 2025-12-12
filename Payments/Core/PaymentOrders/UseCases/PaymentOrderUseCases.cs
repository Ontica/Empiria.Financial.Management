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

using Empiria.Financial;
using Empiria.Parties;
using Empiria.Services;

using Empiria.Payments.Adapters;
using Empiria.Payments.Data;

namespace Empiria.Payments.UseCases {

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


      var payableEntity = (IPayableEntity) BaseObject.Parse(fields.PayableEntityTypeUID, fields.PayableEntityUID);

      var order = new PaymentOrder(payableEntity);

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

      var suspendedBy = Party.Parse(suspendedByUID);

      order.Suspend(suspendedBy, suspendedUntil);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto UpdatePaymentOrder(string uid, PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      fields.PaymentOrderTypeUID = "fe85b014-9929-4339-b56f-5e650d3bd42c";

      var order = PaymentOrder.Parse(uid);

      order.Update(fields);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }

    #endregion Use cases

  }  // class PaymentOrderUseCases

}  // namespace Empiria.Payments.UseCases
