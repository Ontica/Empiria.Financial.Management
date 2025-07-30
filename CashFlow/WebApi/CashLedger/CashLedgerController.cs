/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                  Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Controller                      *
*  Type     : CashLedgerController                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive cash ledger transactions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.UseCases;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Query web API used to retrive cash ledger transactions.</summary>
  public class CashLedgerController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v1/cash-flow/cash-ledger")]
    public CollectionModel SearchCashLedgerTransactions([FromBody] CashLedgerQuery query) {
      base.RequireBody(query);

      using (var usecases = CashLedgerUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionDescriptor> transactions = usecases.SearchTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }

    #endregion Web Apis

  }  // class CashLedgerController

}  // namespace Empiria.CashFlow.WebApi
