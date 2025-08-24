/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Data Layer                              *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Web api client data services            *
*  Type     : CashLedgerData                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides cash ledger transactions data services using a web proxy.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.WebApi.Client;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  public class CashLedgerData {

    private readonly WebApiClient _webApiClient;

    public CashLedgerData() {
      _webApiClient = WebApiClient.GetInstance("SICOFIN");
    }


    public async Task<FixedList<NamedEntityDto>> GetAccountingLedgers() {

      string path = "v2/financial-accounting/ledgers/ifrs";

      return await _webApiClient.GetAsync<FixedList<NamedEntityDto>>(path);
    }


    internal async Task<FixedList<CashTransactionDescriptor>> GetTransactions(CashLedgerQuery query) {

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      return await _webApiClient.PostAsync<FixedList<CashTransactionDescriptor>>(query, path);
    }


    public async Task<FixedList<NamedEntityDto>> GetTransactionSources() {

      string path = "v2/financial-accounting/vouchers/functional-areas";

      return await _webApiClient.GetAsync<FixedList<NamedEntityDto>>(path);
    }


    public async Task<FixedList<NamedEntityDto>> GetTransactionTypes() {

      string path = "v2/financial-accounting/vouchers/transaction-types";

      return await _webApiClient.GetAsync<FixedList<NamedEntityDto>>(path);
    }


    public async Task<FixedList<NamedEntityDto>> GetVoucherTypes() {

      string path = "v2/financial-accounting/vouchers/voucher-types";

      return await _webApiClient.GetAsync<FixedList<NamedEntityDto>>(path);
    }

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
