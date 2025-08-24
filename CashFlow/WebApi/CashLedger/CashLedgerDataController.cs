/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                  Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Query Controller                      *
*  Type     : CashLedgerDataController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive cash ledger associated data.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.CashFlow.CashLedger.Data;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Query web API used to retrive cash ledger associated data.</summary>
  public class CashLedgerDataController : WebApiController {

    private readonly CashLedgerData _cashLedgerData;

    #region Web Apis

    public CashLedgerDataController() {
      _cashLedgerData = new CashLedgerData();
    }

    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/accounting-ledgers")]
    public async Task<CollectionModel> GetAccountingLedgers() {

      FixedList<NamedEntityDto> accountingLedgers = await _cashLedgerData.GetAccountingLedgers();

      return new CollectionModel(base.Request, accountingLedgers);
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transaction-sources")]
    public async Task<CollectionModel> GetTransactionSources() {

      FixedList<NamedEntityDto> transactionTypes = await _cashLedgerData.GetTransactionSources();

      return new CollectionModel(base.Request, transactionTypes);
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transaction-types")]
    public async Task<CollectionModel> GetTransactionTypes() {

      FixedList<NamedEntityDto> transactionTypes = await _cashLedgerData.GetTransactionTypes();

      return new CollectionModel(base.Request, transactionTypes);
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/voucher-types")]
    public async Task<CollectionModel> GetVoucherTypes() {

      FixedList<NamedEntityDto> voucherTypes = await _cashLedgerData.GetVoucherTypes();

      return new CollectionModel(base.Request, voucherTypes);
    }

    #endregion Web Apis

  }  // class CashLedgerDataController

}  // namespace Empiria.CashFlow.WebApi
