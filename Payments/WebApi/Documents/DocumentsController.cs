/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : DocumentsController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update documents for payments entities.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

using Empiria.Payments.Orders;

using Empiria.Payments.Payables;
using Empiria.Payments.Payables.UseCases;

namespace Empiria.Payments.WebApi {

  /// <summary>Web API used to retrive and update documents for payments entities.</summary>
  public class DocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/payments-management/payables/{payableUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemovePayableDocument([FromUri] string payableUID,
                                             [FromUri] string documentUID) {

      var payable = Payable.Parse(payableUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(payable, document);

      return new NoDataModel(this.Request);
    }


    [HttpDelete]
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemovePaymentOrderDocument([FromUri] string paymentOrderUID,
                                                  [FromUri] string documentUID) {

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(paymentOrder, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/payments-management/payables/{payableUID:guid}/documents")]
    public SingleObjectModel StorePayableDocument([FromUri] string payableUID) {

      var payable = Payable.Parse(payableUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest(fields.DocumentProductUID);

      var document = DocumentServices.StoreDocument(documentFile, payable, fields);

      if (document.ApplicationContentType.Length == 0) {
        return new SingleObjectModel(base.Request, document);
      }

      try {
        using (var usecases = PayableDocumentUseCases.UseCaseInteractor()) {

          document = usecases.ProcessPayableDocument(payableUID, document);

          return new SingleObjectModel(base.Request, document);
        }
      } catch {
        DocumentServices.RemoveDocument(payable, Document.Parse(document.UID));
        throw;
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}/documents")]
    public SingleObjectModel StorePaymentOrderDocument([FromUri] string paymentOrderUID) {

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest(fields.DocumentProductUID);

      var document = DocumentServices.StoreDocument(documentFile, paymentOrder, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/payments-management/payables/{payableUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdatePayableDocument([FromUri] string payableUID,
                                                   [FromUri] string documentUID,
                                                   [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var payable = Payable.Parse(payableUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(payable, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }


    [HttpPut, HttpPatch]
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdatePaymentOrderDocument([FromUri] string paymentOrderUID,
                                                        [FromUri] string documentUID,
                                                        [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(paymentOrder, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class DocumentsController

}  // namespace Empiria.Payments.WebApi
