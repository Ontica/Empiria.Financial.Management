/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialProjectDocumentsController          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update financial projects documents.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;

namespace Empiria.Financial.Projects.WebApi {

  /// <summary>Web API used to retrieve and update financial projects documents.</summary>
  public class FinancialProjectDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v1/financial-projects/{projectUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveDocument([FromUri] string projectUID,
                                      [FromUri] string documentUID) {

      var project = FinancialProject.Parse(projectUID);
      var document = DocumentServices.GetDocument(documentUID);

      DocumentServices.RemoveDocument(project, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v1/financial-projects/{projectUID:guid}/documents")]
    public SingleObjectModel StoreDocument([FromUri] string projectUID) {

      var project = FinancialProject.Parse(projectUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      FixedList<DocumentDto> currentDocuments = DocumentServices.GetEntityDocuments(project);

      if (currentDocuments.Contains(x => x.DocumentProduct.UID == fields.DocumentProductUID)) {
        Assertion.RequireFail("Esta proyección de flujo de efectivo ya tiene un documento del mismo tipo.");
      }

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, project, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v1/financial-projects/{projectUID:guid}/{documentUID:guid}")]
    public SingleObjectModel UpdateDocument([FromUri] string projectUID,
                                            [FromUri] string documentUID,
                                            [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var project = FinancialProject.Parse(projectUID);
      var document = DocumentServices.GetDocument(documentUID);

      var documentDto = DocumentServices.UpdateDocument(project, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class FinancialProjectDocumentsController

}  // namespace Empiria.Financial.Projects.WebApi
