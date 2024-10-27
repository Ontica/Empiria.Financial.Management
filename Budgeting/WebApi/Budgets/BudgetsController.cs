/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : BudgetsController                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and create budgets.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.UseCases;

namespace Empiria.Budgeting.WebApi {

  /// <summary>Query web API used to retrive and create budgets.</summary>
  public class BudgetsController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/budgeting/budgets")]
    public CollectionModel GetBudgetsList() {

      using (var usecases = BudgetUseCases.UseCaseInteractor()) {
        FixedList<BudgetDto> list = usecases.BudgetsList();

        return new CollectionModel(base.Request, list);
      }
    }

    #endregion Web Apis

  }  // class BudgetsController

}  // namespace Empiria.Budgeting.WebApi
