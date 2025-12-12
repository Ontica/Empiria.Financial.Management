/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PaymentOrderController                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update payment orders and their catalogues.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

namespace Empiria.Payments.WebApi {

  /// <summary>Web API used to retrieve and update payment orders and their catalogues.</summary>
  public class PaymentOrderController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/payments-management/payables/{paymentOrderUID:guid}")]  // ToDo: Remove this deprecated route in future versions.
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}")]
    public SingleObjectModel GetPaymentOrder([FromUri] string paymentOrderUID) {

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {
        PaymentOrderHolderDto paymentOrder = usecases.GetPaymentOrder(paymentOrderUID);

        return new SingleObjectModel(base.Request, paymentOrder);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payables/search")]   // ToDo: Remove this endpoint in future versions.
    // [Route("v2/payments-management/payment-orders/search")] // Activate this route on next version
    public CollectionModel SearchPaymentOrders([FromBody] PaymentOrdersQuery query) {

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {
        FixedList<PaymentOrderDescriptor> paymentOrders = usecases.SearchPaymentOrders(query);

        return new CollectionModel(base.Request, paymentOrders);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/payments-management/payment-orders")]
    public SingleObjectModel CreatePaymentOrder([FromBody] PaymentOrderFields fields) {

      base.RequireBody(fields);

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {
        PaymentOrderHolderDto paymentOrder = usecases.CreatePaymentOrder(fields);

        return new SingleObjectModel(base.Request, paymentOrder);
      }
    }


    [HttpDelete]
    [Route("v2/payments-management/payables/{paymentOrderUID:guid}")]   // ToDo: Remove this deprecated route in future versions.
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}")]
    public NoDataModel DeletePaymentOrder([FromUri] string paymentOrderUID) {

      base.RequireResource(paymentOrderUID, nameof(paymentOrderUID));

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {

        usecases.DeletePaymentOrder(paymentOrderUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}/suspend")]
    public SingleObjectModel SuspendPaymentOrder([FromUri] string paymentOrderUID,
                                                 [FromUri] string suspendedByUID,
                                                 [FromUri] DateTime suspendedUntil) {

      base.RequireResource(suspendedByUID, nameof(suspendedByUID));

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {

        PaymentOrderHolderDto paymentOrder = usecases.SuspendPaymentOrder(paymentOrderUID,
                                                                          suspendedByUID,
                                                                          suspendedUntil);

        return new SingleObjectModel(this.Request, paymentOrder);
      }
    }


    [HttpPut]
    [Route("v2/payments-management/payables/{paymentOrderUID:guid}")]   // ToDo: Remove this deprecated route in future versions.
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}")]
    public SingleObjectModel UpdatePaymentOrder([FromUri] string paymentOrderUID,
                                                [FromBody] PaymentOrderFields fields) {

      base.RequireBody(fields);

      using (var usecases = PaymentOrderUseCases.UseCaseInteractor()) {

        PaymentOrderHolderDto paymentOrder = usecases.UpdatePaymentOrder(paymentOrderUID,
                                                                         fields);

        return new SingleObjectModel(this.Request, paymentOrder);
      }
    }

    #endregion Command web apis

  }  // class PaymentOrderController

}  // namespace Empiria.Payments.WebApi
