/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Data Layer                              *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Data Services web api proxy             *
*  Type     : CashLedgerData                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides cash ledger transactions data services using a web proxy.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Json;
using Empiria.Storage;
using Empiria.WebApi.Client;

using Empiria.Financial.Integration.CashLedger;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  static internal class CashTransactionData {

    static internal async Task<CashTransactionHolderDto> GetTransaction(long id) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/{id}";

      JsonObject json = await webApiClient.GetAsync<JsonObject>(path);

      return json.Get<CashTransactionHolderDto>("data");
    }


    static internal async Task<FileDto> GetTransactionAsPdfFile(long id) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/vouchers/{id}/print";

      JsonObject json = await webApiClient.GetAsync<JsonObject>(path);

      return json.Get<FileDto>("data");
    }


    static internal async Task<FixedList<CashTransactionHolderDto>> GetTransactions(FixedList<long> transactionIds) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/bulk-operation/get-transactions";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(transactionIds, path);

      return json.GetFixedList<CashTransactionHolderDto>("data");
    }


    static internal async Task<FixedList<CashEntryDescriptor>> GetTransactionsEntries(FixedList<long> entriesIds) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/entries/bulk-operation/get-entries";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(entriesIds, path);

      return json.GetFixedList<CashEntryDescriptor>("data");
    }


    static internal async Task<FixedList<CashEntryDescriptor>> SearchEntries(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/entries/search";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(query, path);

      return json.GetFixedList<CashEntryDescriptor>("data");
    }


    static internal async Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(query, path);

      return json.GetFixedList<CashTransactionDescriptor>("data");
    }


    static internal async Task UpdateBulkEntries(FixedList<CashEntryFields> bulkEntries) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/bulk-operation/update-entries";

      await webApiClient.PostAsync<JsonObject>(bulkEntries, path);
    }


    static internal async Task<CashTransactionHolderDto> UpdateEntries(FixedList<CashEntryFields> entries) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/{entries[0].TransactionId}/update-entries";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(entries, path);

      return json.Get<CashTransactionHolderDto>("data");
    }

    #region Methods Sistema Legado

    static internal async Task<FixedList<MovimientoSistemaLegado>> GetMovimientosSistemaLegado(long id) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/sistema-legado/transacciones/{id}";

      JsonObject json = await webApiClient.GetAsync<JsonObject>(path);

      return json.GetFixedList<MovimientoSistemaLegado>("data");
    }

    #endregion Methods Sistema Legado

    #region Helpers

    static private WebApiClient GetWebApiClient() {
      return WebApiClient.GetInstance("SICOFIN");
    }

    #endregion Helpers

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
