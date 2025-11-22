/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : ChartOfAccountsController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update chart of accounts.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Data;

using Empiria.Financial.Projects;
using Empiria.Financial.Projects.Data;

using Empiria.Financial.Adapters;
using Empiria.Financial.UseCases;

namespace Empiria.Financial.WebApi {

  /// <summary>Web API used to retrive and update chart of accounts.</summary>
  public class ChartOfAccountsController : WebApiController {

    [HttpPost]
    [Route("v3/charts-of-accounts/clean")]
    public NoDataModel Clean() {

      var stdAccounts = BaseObject.GetFullList<StandardAccount>();

      foreach (var stdAccount in stdAccounts) {
        StandardAccountDataService.CleanStandardAccount(stdAccount);
      }


      var projects = BaseObject.GetFullList<FinancialProject>();

      foreach (var project in projects) {
        FinancialProjectDataService.CleanProject(project);
      }


      var accounts = BaseObject.GetFullList<FinancialAccount>();

      foreach (var account in accounts) {
        FinancialAccountDataService.CleanAccount(account);
      }

      return new NoDataModel(base.Request);
    }


    #region Query web apis

    [HttpGet]
    [Route("v3/charts-of-accounts/{chartOfAccountsUID:guid}")]
    public SingleObjectModel GetStandardAccountByCategory([FromUri] string chartOfAccountsUID) {

      using (var usecases = ChartOfAccountsUseCases.UseCaseInteractor()) {

        ChartOfAccountsDto chartOfAccounts = usecases.GetChartOfAccounts(chartOfAccountsUID);

        return new SingleObjectModel(base.Request, chartOfAccounts);
      }
    }


    [HttpGet]
    [Route("v3/charts-of-accounts")]
    public CollectionModel GetChartsOfAccounts() {

      using (var usecases = ChartOfAccountsUseCases.UseCaseInteractor()) {

        FixedList<ChartOfAccountsDefinitionDto> chartsOfAccounts = usecases.GetChartsOfAccountsList();

        return new CollectionModel(base.Request, chartsOfAccounts);
      }
    }


    [HttpGet]
    [Route("v3/charts-of-accounts/{chartOfAccountsUID:guid}/standard-accounts/{stdAccountUID:guid}")]
    public SingleObjectModel GetStandardAccount([FromUri] string chartOfAccountsUID,
                                                [FromUri] string stdAccountUID) {

      using (var usecases = ChartOfAccountsUseCases.UseCaseInteractor()) {

        StandardAccountHolder stdAccount = usecases.GetStandardAccount(chartOfAccountsUID, stdAccountUID);

        return new SingleObjectModel(base.Request, stdAccount);
      }
    }


    [HttpPost]
    [Route("v3/charts-of-accounts/{chartOfAccountsUID:guid}")]
    public SingleObjectModel SearchChartOfAccounts([FromUri] string chartOfAccountsUID,
                                                   [FromBody] ChartOfAccountsQuery query) {

      query.ChartOfAccountsUID = chartOfAccountsUID;

      using (var usecases = ChartOfAccountsUseCases.UseCaseInteractor()) {

        ChartOfAccountsDto chartOfAccounts = usecases.SearchChartOfAccounts(query);

        return new SingleObjectModel(base.Request, chartOfAccounts);
      }
    }

    #endregion Query web apis

  }  // class StandardAccountController

}  // namespace Empiria.Financial.Projects.WebApi
