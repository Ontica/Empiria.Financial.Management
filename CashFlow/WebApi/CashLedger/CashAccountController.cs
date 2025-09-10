/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                  Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web Api Controller                    *
*  Type     : CashAccountController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive cash account transactions.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;

using Empiria.CashFlow.CashLedger.UseCases;
using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Web API used to retrive cash account transactions.</summary>
  public class CashAccountController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/accounts/search")]
    public async Task<CollectionModel> SearchCashAccounts([FromUri] AccountsQuery query) {

      using (var usecases = CashAccountUseCases.UseCaseInteractor()) {
        FixedList<CashAccountTotalDto> accounts = await usecases.SearchAccounts(query);

        return new CollectionModel(base.Request, accounts);
      }
    }

    #endregion Query web apis

  }  // class CashAccountController

}  // namespace Empiria.CashFlow.WebApi
