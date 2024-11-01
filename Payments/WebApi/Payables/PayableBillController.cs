/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PayableBillController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable bills.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;

using Empiria.Payments.Payables.UseCases;
using Empiria.Payments.Payables.Adapters;

namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive and update payable bills.</summary>
  public class PayableBillController : WebApiController {

    #region Command web apis

    [HttpPost]
    [Route("v2/payments-management/payables/{payableUID:guid}/bills")]
    public SingleObjectModel AppendPayableBill([FromUri] string payableUID) {

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile billFile = base.GetInputFileFromHttpRequest("FacturaElectronica");

      using (var usecases = PayableBillUseCases.UseCaseInteractor()) {

        PayableHolderDto payable = usecases.AppendPayableBill(payableUID, fields, billFile);

        return new SingleObjectModel(base.Request, payable);
      }
    }


    [HttpDelete]
    [Route("v2/payments-management/payables/{payableUID:guid}/bills/{billUID:guid}")]
    public NoDataModel DeletePayableBill([FromUri] string payableUID,
                                         [FromUri] string billUID) {

      using (var usecases = PayableBillUseCases.UseCaseInteractor()) {

        usecases.DeletePayableBill(payableUID, billUID);

        return new NoDataModel(this.Request);
      }
    }

    #endregion Command web apis

  }  // class PayableBillController

}  // namespace Empiria.Payments.Payables.WebApi
