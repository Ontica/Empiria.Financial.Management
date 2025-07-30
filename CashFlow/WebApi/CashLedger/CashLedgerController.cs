/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                  Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Controller                      *
*  Type     : CashLedgerController                         License   : Please read LICENSE.txt file          *
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
  public class CashLedgerController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/accounting-ledgers")]
    public async Task<CollectionModel> GetAccountingLedgers() {

      using (var usecases = CashLedgerUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> accountingLedgers = await usecases.GetAccountingLedgers();

        return new CollectionModel(base.Request, accountingLedgers);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transaction-sources")]
    public async Task<CollectionModel> GetTransactionSources() {

      using (var usecases = CashLedgerUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> transactionTypes = await usecases.GetTransactionSources();

        return new CollectionModel(base.Request, transactionTypes);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transaction-types")]
    public async Task<CollectionModel> GetTransactionTypes() {

      using (var usecases = CashLedgerUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> transactionTypes = await usecases.GetTransactionTypes();

        return new CollectionModel(base.Request, transactionTypes);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/voucher-types")]
    public async Task<CollectionModel> GetVoucherTypes() {

      using (var usecases = CashLedgerUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> voucherTypes = await usecases.GetVoucherTypes();

        return new CollectionModel(base.Request, voucherTypes);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/search")]
    public async Task<CollectionModel> SearchCashLedgerTransactions([FromBody] CashLedgerQuery query) {

      using (var usecases = CashLedgerUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionDescriptor> transactions = await usecases.SearchTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }

    #endregion Web Apis

  }  // class CashLedgerController

}  // namespace Empiria.CashFlow.WebApi
