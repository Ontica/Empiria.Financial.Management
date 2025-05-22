/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                          Component : Web Api                               *
*  Assembly : Empiria.Cashflow.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : CashflowProjectionsDocumentsController       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update cash flow projections documents.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;

namespace Empiria.Cashflow.Projections.WebApi {

  /// <summary>Web API used to retrive and update cash flow projections documents.</summary>
  public class CashflowProjectionsDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/cashflow/projections/{projectionUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveDocument([FromUri] string projectionUID,
                                      [FromUri] string documentUID) {

      var projection = CashflowProjection.Parse(projectionUID);
      var document = DocumentServices.GetDocument(documentUID);

      DocumentServices.RemoveDocument(projection, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/cashflow/projections/{projectionUID:guid}/documents")]
    public SingleObjectModel StoreDocument([FromUri] string projectionUID) {

      var projection = CashflowProjection.Parse(projectionUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      FixedList<DocumentDto> currentDocuments = DocumentServices.GetEntityDocuments(projection);

      if (currentDocuments.Contains(x => x.DocumentProduct.UID == fields.DocumentProductUID)) {
        Assertion.RequireFail("Esta proyección de flujo de efectivo ya tiene un documento del mismo tipo.");
      }

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, projection, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/cashflow/projections/{projectionUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateDocument([FromUri] string projectionUID,
                                            [FromUri] string documentUID,
                                            [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var projection = CashflowProjection.Parse(projectionUID);
      var document = DocumentServices.GetDocument(documentUID);

      var documentDto = DocumentServices.UpdateDocument(projection, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class CashflowProjectionsDocumentsController

}  // namespace Empiria.Budgeting.WebApi
