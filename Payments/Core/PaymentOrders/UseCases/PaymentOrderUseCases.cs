/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PaymentOrderUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payment orders management.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

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

    public PaymentOrderHolderDto CreatePaymentInstruction(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      Assertion.Require(order.Rules.CanGeneratePaymentInstruction(),
                       "No se puede crear la instrucción de pago");

      order.CreatePaymentInstruction();

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto CreatePaymentOrder(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payableEntity = (IPayableEntity) BaseObject.Parse(fields.PayableEntityTypeUID, fields.PayableEntityUID);

      var currentPaymentOrders = PaymentOrder.GetListFor(payableEntity);

      Assertion.Require(currentPaymentOrders.All(x => x.Status == PaymentOrderStatus.Canceled ||
                                                      x.Status == PaymentOrderStatus.Payed),
                        "Existe una solicitud de pago en proceso o el pago correspondiente ya se efectuó.");

      var paymentType = PaymentType.Parse(fields.PaymentTypeUID);
      var payTo = Party.Parse(fields.PayToUID);

      var order = new PaymentOrder(paymentType, payTo, payableEntity, fields.Total);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto CancelPaymentOrder(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      order.Cancel();

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto GetPaymentOrder(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      return PaymentOrderMapper.Map(order);
    }


    internal PaymentOrderHolderDto ResetPaymentOrder(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      order.Reset();

      order.Save();

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


    public PaymentOrderHolderDto SuspendPaymentOrder(string paymentOrderUID) {
      Assertion.Require(paymentOrderUID, nameof(paymentOrderUID));

      var order = PaymentOrder.Parse(paymentOrderUID);

      order.Suspend();

      order.Save();

      return PaymentOrderMapper.Map(order);
    }


    public PaymentOrderHolderDto UpdatePaymentOrder(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var order = PaymentOrder.Parse(fields.UID);

      order.Update(fields);

      order.Save();

      return PaymentOrderMapper.Map(order);
    }

    #endregion Use cases

  }  // class PaymentOrderUseCases

}  // namespace Empiria.Payments.UseCases
