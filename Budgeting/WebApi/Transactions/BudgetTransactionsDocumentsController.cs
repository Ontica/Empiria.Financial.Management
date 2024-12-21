/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Contracts.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : BudgetTransactionsDocumentsController        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update budget transaction documents.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Documents;
using Empiria.Documents.Services;

using Empiria.Budgeting.Transactions;


namespace Empiria.Budgeting.WebApi {

  /// <summary>Web API used to retrive and update budget transaction documents.</summary>
  public class BudgetTransactionsDocumentsController : WebApiController {

    #region Command web apis

    [HttpDelete]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/documents/{documentUID:guid}")]
    public NoDataModel RemoveTransactionDocument([FromUri] string budgetTransactionUID,
                                                 [FromUri] string documentUID) {

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);
      var document = Document.Parse(documentUID);

      DocumentServices.RemoveDocument(transaction, document);

      return new NoDataModel(this.Request);
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/documents")]
    public SingleObjectModel StoreTransactionDocument([FromUri] string budgetTransactionUID) {

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      DocumentFields fields = GetFormDataFromHttpRequest<DocumentFields>("document");

      InputFile documentFile = base.GetInputFileFromHttpRequest();

      var document = DocumentServices.StoreDocument(documentFile, transaction, fields);

      return new SingleObjectModel(base.Request, document);
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/documents/{documentUID:guid}")]
    public SingleObjectModel UpdateTransactionDocument([FromUri] string budgetTransactionUID,
                                                       [FromUri] string documentUID,
                                                       [FromBody] DocumentFields fields) {
      base.RequireBody(fields);

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);
      var document = Document.Parse(documentUID);

      var documentDto = DocumentServices.UpdateDocument(transaction, document, fields);

      return new SingleObjectModel(base.Request, documentDto);
    }

    #endregion Command web apis

  }  // class BudgetTransactionsDocumentsController

}  // namespace Empiria.Budgeting.WebApi
