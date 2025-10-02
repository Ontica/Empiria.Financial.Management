/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Explorer                           Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Web Api Controller              *
*  Type     : SearchServicesController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to provide search services for cash flow related data and entities.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.DynamicData;
using Empiria.WebApi;

using Empiria.Financial.Adapters;

using Empiria.CashFlow.Explorer.UseCases;

namespace Empiria.CashFlow.Explorer.WebApi {

  /// <summary>Query web API used to provide search services for cash flow related data and entities.</summary>
  public class SearchServicesController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v1/cash-flow/accounts-totals/search")]
    public async Task<SingleObjectModel> SearchAccountsTotals([FromBody] RecordsSearchQuery query) {

      using (var usecases = CashFlowAccountsTotalsUseCases.UseCaseInteractor()) {
        var accountsTotals = await usecases.AccountTotals(query);

        return new SingleObjectModel(base.Request, accountsTotals);
      }
    }


    [HttpPost]
    [Route("v1/financial-management/search-services/CreditEntries")]
    public SingleObjectModel SearchExternalCreditEntries([FromBody] RecordsSearchQuery query) {

      query.QueryType = RecordSearchQueryType.CreditEntries;

      using (var services = CashFlowSearchServices.ServiceInteractor()) {
        DynamicDto<ICreditEntryData> entries = services.SearchExternalCreditEntries(query);

        return new SingleObjectModel(base.Request, entries);
      }
    }

    #endregion Web apis

  }  // class SearchServicesController

}  // namespace Empiria.CashFlow.Explorer.WebApi
