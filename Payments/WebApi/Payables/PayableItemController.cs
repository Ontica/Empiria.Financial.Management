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


namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive and update payable items.</summary>
  public class PayableItemController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/payments-management/payables/{payableUID:guid}/items/{payableItemUID:guid}")]
    public NoDataModel RemovePayableItem([FromUri] string payableUID,
                                         [FromUri] string payableItemUID) {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        usecases.RemovePayableItem(payableUID, payableItemUID);

        return new NoDataModel(this.Request);
      }
    }

    #endregion Command web apis

  }  // class PayableItemController

}  // namespace Empiria.Payments.Payables.WebApi
