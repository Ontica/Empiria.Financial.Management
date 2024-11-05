/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : BudgetExplorerController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve dynamic budget information.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Explorer.Adapters;
using Empiria.Budgeting.Explorer.UseCases;

namespace Empiria.Budgeting.Explorer.WebApi {

  /// <summary>Web API used to retrieve dynamic budget information.</summary>
  public class BudgetExplorerController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/budget-explorer/planning")]
    public SingleObjectModel ExplorePlannedBudget([FromBody] BudgetExplorerQuery query) {

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {
        BudgetExplorerResultDto result = usecases.ExploreBudget(query);

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Web Apis

  }  // class BudgetExplorerController

}  // namespace Empiria.Budgeting.Explorer.WebApi
