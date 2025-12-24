/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Web Api Controller                    *
*  Type     : BudgetEntryController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and edit budget transaction's entries.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

namespace Empiria.Budgeting.Transactions.WebApi {

  /// <summary>Web API used to retrieve and edit budget transaction's entries.</summary>
  public class BudgetEntryController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries")]
    public SingleObjectModel CreateBudgetEntry([FromUri] string transactionUID,
                                               [FromBody] BudgetEntryFields fields) {

      fields.TransactionUID = transactionUID;

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        BudgetEntryDto budgetEntry = usecases.CreateBudgetEntry(fields);

        return new SingleObjectModel(base.Request, budgetEntry);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/{entryUID:guid}")]
    public SingleObjectModel GetBudgetEntry([FromUri] string transactionUID,
                                            [FromUri] string entryUID) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {

        BudgetEntryDto budgetEntry = usecases.GetBudgetEntry(transactionUID, entryUID);

        return new SingleObjectModel(base.Request, budgetEntry);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/{entryUID:guid}")]
    public NoDataModel RemoveBudgetEntry([FromUri] string transactionUID,
                                         [FromUri] string entryUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveBudgetEntry(transactionUID, entryUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/{entryUID:guid}")]
    public SingleObjectModel UpdateBudgetEntry([FromUri] string transactionUID,
                                               [FromUri] string entryUID,
                                               [FromBody] BudgetEntryFields fields) {

      fields.TransactionUID = transactionUID;

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        BudgetEntryDto budgetEntry = usecases.UpdateBudgetEntry(entryUID, fields);

        return new SingleObjectModel(base.Request, budgetEntry);
      }
    }

    #endregion Web Apis

  }  // class BudgetEntryController

}  // namespace Empiria.Budgeting.Transactions.WebApi
