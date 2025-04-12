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
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/create-annually")]
    public SingleObjectModel CreateBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                       [FromBody] BudgetEntryByYearFields fields) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.CreateBudgetEntryByYear(budgetTransactionUID, fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{entryByYearUID:guid}/get-annually")]
    public SingleObjectModel GetBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                    [FromUri] string entryByYearUID) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.GetBudgetEntryByYear(budgetTransactionUID, entryByYearUID);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{entryByYearUID:guid}/remove-annually")]
    public NoDataModel RemoveBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                 [FromUri] string entryByYearUID) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveBudgetEntryByYear(budgetTransactionUID, entryByYearUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{entryByYearUID:guid}/update-annually")]
    public SingleObjectModel UpdateBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                       [FromUri] string entryByYearUID,
                                                       [FromBody] BudgetEntryByYearFields fields) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.UpdateBudgetEntryByYear(budgetTransactionUID,
                                                                            entryByYearUID,
                                                                            fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }

    #endregion Web Apis

  }  // class BudgetEntryByYearController

}  // namespace Empiria.Budgeting.Transactions.WebApi
