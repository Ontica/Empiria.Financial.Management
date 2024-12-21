/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                      Component : Web Api                               *
*  Assembly : Empiria.Billing.WebApi.dll                   Pattern   : Web api Controller                    *
*  Type     : BillDocumentsController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update bills documents.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

namespace Empiria.Billing.WebApi {

  /// <summary> Web API used to retrive and update bills documents.</summary>
  public class BillDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/billing-management/bills/{billUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveBillDocument([FromUri] string billUID,
                                          [FromUri] string documentUID) {

      var bill = Bill.Parse(billUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(bill, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/billing-management/bills/{billUID:guid}/documents")]
    public SingleObjectModel StoreBillDocument([FromUri] string billUID) {

      var bill = Bill.Parse(billUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, bill, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/billing-management/bills/{billUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateBillDocument([FromUri] string billUID,
                                                [FromUri] string documentUID,
                                                [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var bill = Bill.Parse(billUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(bill, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class BillDocumentsController

}  // namespace Empiria.Payments.Orders.WebApi
