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

using Empiria.Financial.Adapters;
using Empiria.Financial.UseCases;

namespace Empiria.Financial.WebApi {

  /// <summary>Web API used to retrive and update chart of accounts.</summary>
  public class ChartOfAccountsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/charts-of-accounts/{chartOfAccountsUID:guid}")]
    public SingleObjectModel GetStandardAccountByCategory([FromUri] string chartOfAccountsUID) {

      using (var usecases = ChartOfAccountsUseCases.UseCaseInteractor()) {

        ChartOfAccountsDto chartOfAccounts = usecases.GetChartOfAcounts(chartOfAccountsUID);

        return new SingleObjectModel(base.Request, chartOfAccounts);
      }
    }


    [HttpGet]
    [Route("v3/charts-of-accounts")]
    public CollectionModel GetChartsOfAccounts() {

      using (var usecases = ChartOfAccountsUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> chartsOfAccounts = usecases.GetChartsOfAcountsList();

        return new CollectionModel(base.Request, chartsOfAccounts);
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
