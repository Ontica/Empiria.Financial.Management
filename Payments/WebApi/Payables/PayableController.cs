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
    [Route("v2/payments-management/payables/payable-types")]
    public CollectionModel GetPayableTypes() {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> payableTypes = usecases.GetPayableTypes();

        return new CollectionModel(Request, payableTypes);
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

    #region Payable command web apis

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
        

    [HttpPut]
    [Route("v2/payments-management/payables/{payableUID:guid}")]
    public SingleObjectModel UpdatePayable([FromUri] string payableUID,
                                           [FromBody] PayableFields fields) {
      base.RequireBody(fields);

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        PayableDto payable = usecases.UpdatePayable(payableUID, fields);

        return new SingleObjectModel(this.Request, payable);
      }
    }

    #endregion Payable command web apis

    #region Payable items command web apis

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

    #endregion Payable items command web apis

    #region PayableDataStructure

    [HttpGet]
    [Route("v2/payments-management/payables/{payableUID:guid}")]
    public SingleObjectModel GetPayableData([FromUri] string payableUID) {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {

        PayableHolderDto payableHolderDto = usecases.GetPayableData(payableUID);

        return new SingleObjectModel(this.Request, payableHolderDto);
      }
    }

    #endregion PayableDataStructure

  }  // class PayableController

}  // namespace Empiria.Payments.Payables.WebApi
