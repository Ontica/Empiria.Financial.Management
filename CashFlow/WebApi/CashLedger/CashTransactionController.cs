/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                  Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Controller                      *
*  Type     : CashTransactionController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive cash ledger transactions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.UseCases;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Query web API used to retrive cash ledger transactions.</summary>
  public class CashTransactionController : WebApiController {

    #region Web Apis


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/search")]
    public async Task<CollectionModel> SearchCashLedgerTransactions([FromBody] CashLedgerQuery query) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionDescriptor> transactions = await usecases.SearchTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }

    #endregion Web Apis

  }  // class CashTransactionController

}  // namespace Empiria.CashFlow.WebApi
