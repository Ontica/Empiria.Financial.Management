/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PayableController                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable objects and their catalogues.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Payables.UseCases;
using Empiria.Payments.Payables.Adapters;


namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive and update payable objects and their catalogues.</summary>
  public class PayableController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/payments-management/payables/{payableUID:guid}")]
    public SingleObjectModel GetPayable([FromUri] string payableUID) {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        PayableHolderDto payableHolderDto = usecases.GetPayableData(payableUID);

        return new SingleObjectModel(this.Request, payableHolderDto);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payables/search")]
    public CollectionModel SearchPayables([FromBody] PayablesQuery query) {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {
        FixedList<PayableDescriptor> payables  = usecases.SearchPayables(query);

        return new CollectionModel(base.Request, payables);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/payments-management/payables")]
    public SingleObjectModel CreatePayable([FromBody] PayableFields fields) {

      base.RequireBody(fields);

      using (var usecases = PayableUseCases.UseCaseInteractor()) {
        PayableHolderDto payable = usecases.CreatePayable(fields);

        return new SingleObjectModel(base.Request, payable);
      }
    }


    [HttpDelete]
    [Route("v2/payments-management/payables/{payableUID:guid}")]
    public NoDataModel DeletePayable([FromUri] string payableUID) {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        usecases.DeletePayable(payableUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payables/{payableUID:guid}/payment-instruction")]
    public SingleObjectModel SetPaymentInstruction([FromUri] string payableUID) {
            
      using (var usecases = PayableUseCases.UseCaseInteractor()) {
        PayableHolderDto payable = usecases.SetPaymentInstruction(payableUID);

        return new SingleObjectModel(base.Request, payable);
      }
    }


    [HttpPut]
    [Route("v2/payments-management/payables/{payableUID:guid}")]
    public SingleObjectModel UpdatePayable([FromUri] string payableUID,
                                           [FromBody] PayableFields fields) {
      base.RequireBody(fields);

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        PayableHolderDto payable = usecases.UpdatePayable(payableUID, fields);

        return new SingleObjectModel(this.Request, payable);
      }
    }

    [HttpPut]
    [Route("v2/payments-management/payables/{payableUID:guid}/payment-data")]
    public SingleObjectModel UpdatePayablePayment([FromUri] string payableUID,
                                          [FromBody] PayablePaymentFields fields) {
      base.RequireBody(fields);

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        PayableHolderDto payable = usecases.UpdatePayablePayment(payableUID, fields);

        return new SingleObjectModel(this.Request, payable);
      }
    }

    #endregion Command web apis

  }  // class PayableController

}  // namespace Empiria.Payments.Payables.WebApi
