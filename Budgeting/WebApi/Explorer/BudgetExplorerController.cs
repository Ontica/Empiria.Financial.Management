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

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Budgeting.Reporting;

using Empiria.Budgeting.Explorer.Adapters;
using Empiria.Budgeting.Explorer.UseCases;

namespace Empiria.Budgeting.Explorer.WebApi {

  /// <summary>Web API used to retrieve dynamic budget information.</summary>
  public class BudgetExplorerController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/budget-explorer/breakdown")]
    public SingleObjectModel Breakdown([FromBody] ExplorerBreakdownQuery query) {

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {
        BudgetExplorerResultDto result = usecases.BreakdownBudget(query.Query, query.Entry.UID);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/budget-explorer/export")]
    public SingleObjectModel ExportBudget([FromBody] BudgetExplorerQuery query) {

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {
        BudgetExplorerResultDto budgetExplorerResult = usecases.ExploreBudget(query);


        using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

          FileDto fileDto = reportingService.ExportBudgetExplorerResultToExcel(budgetExplorerResult);

          SetOperation($"Se exportó el explorador de presupuestos con {budgetExplorerResult.Entries.Count} entradas a Excel.");

          return new SingleObjectModel(base.Request, fileDto);
        }
      }
    }


    [HttpPost]
    [Route("v2/budgeting/budget-explorer/search")]
    public SingleObjectModel SearchBudget([FromBody] BudgetExplorerQuery query) {

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {
        BudgetExplorerResultDto result = usecases.ExploreBudget(query);

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Web Apis

  }  // class BudgetExplorerController


  public class ExplorerBreakdownQuery {

    public BudgetExplorerQuery Query {
      get; set;
    }

    public BreakdownEntry Entry {
      get; set;
    }
  }


  public class BreakdownEntry {

    public string UID {
      get; set;
    }

  }


}  // namespace Empiria.Budgeting.Explorer.WebApi
