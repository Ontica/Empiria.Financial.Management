/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Contracts.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : ContractDocumentsController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payment contract documents.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

namespace Empiria.Contracts.WebApi {

  /// <summary>Web API used to retrive and update payment contract documents.</summary>
  public class ContractDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/contracts/{contractUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveContractDocument([FromUri] string contractUID,
                                              [FromUri] string documentUID) {

      var contract = Contract.Parse(contractUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(contract, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/contracts/{contractUID:guid}/documents")]
    public SingleObjectModel StoreContractDocument([FromUri] string contractUID) {

      var contract = Contract.Parse(contractUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest(fields.DocumentProductUID);

      var document = DocumentServices.StoreDocument(documentFile, contract, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/contracts/{contractUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateContractDocument([FromUri] string contractUID,
                                                    [FromUri] string documentUID,
                                                    [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var contract = Contract.Parse(contractUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(contract, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class ContractDocumentsController

}  // namespace Empiria.Contracts.WebApi
