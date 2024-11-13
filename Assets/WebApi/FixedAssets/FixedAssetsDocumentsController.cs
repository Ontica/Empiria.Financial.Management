/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                      Component : Web Api                               *
*  Assembly : Empiria.Assets.WebApi.dll                    Pattern   : Web api Controller                    *
*  Type     : FixedAssetsDocumentsController               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update fixed asset's documents.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

namespace Empiria.Assets.WebApi {

  /// <summary>Web API used to retrive and update fixed asset's documents.</summary>
  public class FixedAssetsDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/fixed-assets/{fixedAssetUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveFixedAssetDocument([FromUri] string fixedAssetUID,
                                                [FromUri] string documentUID) {

      var fixedAsset = FixedAsset.Parse(fixedAssetUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(fixedAsset, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/fixed-assets/{fixedAssetUID:guid}/documents")]
    public SingleObjectModel StoreFixedAssetDocument([FromUri] string fixedAssetUID) {

      var fixedAsset = FixedAsset.Parse(fixedAssetUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest(fields.DocumentProductUID);

      var document = DocumentServices.StoreDocument(documentFile, fixedAsset, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/fixed-assets/{fixedAssetUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateFixedAssetDocument([FromUri] string fixedAssetUID,
                                                      [FromUri] string documentUID,
                                                      [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var fixedAsset = FixedAsset.Parse(fixedAssetUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(fixedAsset, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class FixedAssetsDocumentsController

}  // namespace Empiria.Assets.WebApi
