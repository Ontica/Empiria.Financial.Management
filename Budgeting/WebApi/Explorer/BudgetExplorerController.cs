/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Query Api Controller                  *
*  Type     : BudgetExplorerController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve budget information.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.UseCases;

namespace Empiria.Budgeting.WebApi {

  /// <summary>Web API used to retrieve budget information.</summary>
  public class BudgetExplorerController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/budget-explorer/planning")]
    public SingleObjectModel RetrievePlannedBudget([FromBody] BudgetExplorerQuery query) {

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {
        BudgetExplorerResultDto result = usecases.RetrievePlannedBudget(query);

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Web Apis

  }  // class BudgetExplorerController

}  // namespace Empiria.Budgeting.WebApi
