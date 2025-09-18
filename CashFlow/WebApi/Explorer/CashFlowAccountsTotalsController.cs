/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Explorer                           Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Web Api Controller              *
*  Type     : CashFlowAccountsTotalsController             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive cash flow related accounts with their totals.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;

using Empiria.CashFlow.Explorer.UseCases;

namespace Empiria.CashFlow.Explorer.WebApi {

  /// <summary>Query web API used to retrive cash flow related accounts with their totals.</summary>
  public class CashFlowAccountsTotalsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v1/cash-flow/accounts-totals/search")]
    public async Task<SingleObjectModel> SearchAccountsTotals([FromBody] AccountsTotalsQuery query) {

      using (var usecases = CashFlowAccountsTotalsUseCases.UseCaseInteractor()) {
        var accountsTotals = await usecases.AccountTotals(query);

        return new SingleObjectModel(base.Request, accountsTotals);
      }
    }

    #endregion Web apis

  }  // class CashFlowAccountsTotalsController

}  // namespace Empiria.CashFlow.Explorer.WebApi
