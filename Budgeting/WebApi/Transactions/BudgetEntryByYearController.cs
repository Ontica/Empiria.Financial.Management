/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Web Api Controller                    *
*  Type     : BudgetEntryByYearController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and edit budget transaction's entries for a whole year.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

namespace Empiria.Budgeting.Transactions.WebApi {

  /// <summary>Web API used to retrieve and edit budget transaction's entries for a whole year.</summary>
  public class BudgetEntryByYearController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/create-annually")]
    public SingleObjectModel CreateBudgetEntriesByYear([FromUri] string transactionUID,
                                                       [FromBody] BudgetEntryByYearFields fields) {

      fields.TransactionUID = transactionUID;

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.CreateBudgetEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/{entryByYearUID}/get-annually")]
    public SingleObjectModel GetBudgetEntriesByYear([FromUri] string transactionUID,
                                                    [FromUri] string entryByYearUID) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.GetBudgetEntryByYear(transactionUID, entryByYearUID);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/{entryByYearUID}/remove-annually")]
    public NoDataModel RemoveBudgetEntriesByYear([FromUri] string transactionUID,
                                                 [FromUri] string entryByYearUID) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        usecases.RemoveBudgetEntryByYear(transactionUID, entryByYearUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/entries/{entryByYearUID}/update-annually")]
    public SingleObjectModel UpdateBudgetEntriesByYear([FromUri] string transactionUID,
                                                       [FromUri] string entryByYearUID,
                                                       [FromBody] BudgetEntryByYearFields fields) {

      fields.UID = entryByYearUID;
      fields.TransactionUID = transactionUID;

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.UpdateBudgetEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }

    #endregion Web Apis

  }  // class BudgetEntryByYearController

}  // namespace Empiria.Budgeting.Transactions.WebApi
