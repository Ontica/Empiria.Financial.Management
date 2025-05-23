﻿/* Empiria Financial *****************************************************************************************
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

      fields.TransactionUID = budgetTransactionUID;

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.CreateBudgetEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{entryByYearUID}/get-annually")]
    public SingleObjectModel GetBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                    [FromUri] string entryByYearUID) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.GetBudgetEntryByYear(budgetTransactionUID, entryByYearUID);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{entryByYearUID}/remove-annually")]
    public NoDataModel RemoveBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                 [FromUri] string entryByYearUID) {

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        usecases.RemoveBudgetEntryByYear(budgetTransactionUID, entryByYearUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/entries/{entryByYearUID}/update-annually")]
    public SingleObjectModel UpdateBudgetEntriesByYear([FromUri] string budgetTransactionUID,
                                                       [FromUri] string entryByYearUID,
                                                       [FromBody] BudgetEntryByYearFields fields) {

      fields.UID = entryByYearUID;
      fields.TransactionUID = budgetTransactionUID;

      using (var usecases = BudgetEntryByYearEditionUseCases.UseCaseInteractor()) {
        BudgetEntryByYearDto entryByYear = usecases.UpdateBudgetEntryByYear(fields);

        return new SingleObjectModel(base.Request, entryByYear);
      }
    }

    #endregion Web Apis

  }  // class BudgetEntryByYearController

}  // namespace Empiria.Budgeting.Transactions.WebApi
