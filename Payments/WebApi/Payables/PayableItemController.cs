/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PayableItemController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable items.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Payables.UseCases;
using Empiria.Payments.Payables.Adapters;


namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive and update payable items.</summary>
  public class PayableItemController : WebApiController {

    #region Command web apis

    [HttpPost]
    [Route("v2/payments-management/payables/{payableUID:guid}/items")]
    public SingleObjectModel AddPayableItem([FromUri] string payableUID,
                                            [FromBody] PayableItemFields fields) {
      base.RequireBody(fields);

      using (var usecases = PayableUseCases.UseCaseInteractor()) {
        PayableItemDto payableItem = usecases.AddPayableItem(payableUID, fields);

        return new SingleObjectModel(base.Request, payableItem);
      }
    }


    [HttpDelete]
    [Route("v2/payments-management/payables/{payableUID:guid}/items/{payableItemUID:guid}")]
    public NoDataModel RemovePayableItem([FromUri] string payableUID,
                                         [FromUri] string payableItemUID) {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        usecases.RemovePayableItem(payableUID, payableItemUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v2/payments-management/payables/{payableUID:guid}/items/{payableItemUID:guid}")]
    public SingleObjectModel UpdatePayableItem([FromUri] string payableUID,
                                               [FromUri] string payableItemUID,
                                               [FromBody] PayableItemFields fields) {

      base.RequireBody(fields);

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        PayableItemDto payableItem = usecases.UpdatePayableItem(payableUID, payableItemUID, fields);

        return new SingleObjectModel(this.Request, payableItem);
      }
    }

    #endregion Command web apis

  }  // class PayableItemController

}  // namespace Empiria.Payments.Payables.WebApi
