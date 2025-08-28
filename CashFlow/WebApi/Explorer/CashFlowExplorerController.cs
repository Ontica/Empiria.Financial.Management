/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Explorer                           Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Web Api Controller              *
*  Type     : CashFlowExplorerController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive cash flow explorer information.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.CashFlow.Explorer.Adapters;
using Empiria.CashFlow.Explorer.UseCases;

namespace Empiria.CashFlow.Explorer.WebApi {

  /// <summary>Web API used to retrive and update cash ledger transactions.</summary>
  public class CashFlowExplorerController : WebApiController {

    #region Web apis


    [HttpPost]
    [Route("v1/cash-flow/explorer")]
    public async Task<SingleObjectModel> ExploreCashFlow([FromBody] CashFlowExplorerQuery query) {

      using (var usecases = CashFlowExplorerUseCases.UseCaseInteractor()) {
        CashFlowExplorerResultDto result = await usecases.ExploreCashFlow(query);

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Web apis

  }  // class CashFlowExplorerController

}  // namespace Empiria.CashFlow.Explorer.WebApi
