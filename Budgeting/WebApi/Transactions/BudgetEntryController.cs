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
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries")]
    public SingleObjectModel CreateBudgetEntry([FromUri] string budgetTransactionUID,
                                               [FromBody] BudgetEntryFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetEntryDto budgetEntry = usecases.CreateBudgetEntry(budgetTransactionUID, fields);

        return new SingleObjectModel(base.Request, budgetEntry);
      }
    }



    [HttpGet]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{budgetEntryUID:guid}")]
    public SingleObjectModel GetBudgetEntry([FromUri] string budgetTransactionUID,
                                            [FromUri] string budgetEntryUID) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        BudgetEntryDto budgetEntry = usecases.GetBudgetEntry(budgetTransactionUID, budgetEntryUID);

        return new SingleObjectModel(base.Request, budgetEntry);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{budgetEntryUID:guid}")]
    public NoDataModel RemoveBudgetEntry([FromUri] string budgetTransactionUID,
                                         [FromUri] string budgetEntryUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        _ = usecases.RemoveBudgetEntry(budgetTransactionUID, budgetEntryUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{budgetEntryUID:guid}")]
    public SingleObjectModel UpdateBudgetEntry([FromUri] string budgetTransactionUID,
                                               [FromUri] string budgetEntryUID,
                                               [FromBody] BudgetEntryFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetEntryDto budgetEntry = usecases.UpdateBudgetEntry(budgetTransactionUID,
                                                                budgetEntryUID,
                                                                fields);

        return new SingleObjectModel(base.Request, budgetEntry);
      }
    }

    #endregion Web Apis

  }  // class BudgetEntryController

}  // namespace Empiria.Budgeting.Transactions.WebApi
