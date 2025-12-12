/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PaymentServiceController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to interact with the payment service broker.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;
using System.Threading.Tasks;

using Empiria.WebApi;

using Empiria.Payments.Adapters;

using Empiria.Payments.Processor;

namespace Empiria.Payments.WebApi {

  /// <summary>Web API used to interact with the payment service broker.</summary>
  public class PaymentServiceController : WebApiController {

    #region Command web apis

    [HttpPost]
    [Route("v8/payments-management/payables/{paymentOrderUID:guid}/send-to-pay")] // ToDo: Remove this deprecated route in future versions.
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}/pay")]
    public async Task<SingleObjectModel> SendPaymentOrderToPay([FromUri] string paymentOrderUID) {

      using (var services = PaymentService.ServiceInteractor()) {

        PaymentOrderHolderDto paymentOrder = await services.SendPaymentOrderToPay(paymentOrderUID);

        return new SingleObjectModel(this.Request, paymentOrder);
      }
    }

    #endregion Command web apis

  }  // class PaymentServiceController

}  // namespace Empiria.Payments.WebApi
