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

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  static internal class CashTransactionData {

    static internal async Task<CashTransactionHolderDto> GetTransaction(long id, bool returnLegacySystemData) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/{id}/{returnLegacySystemData}";

      return await webApiClient.GetAsync<CashTransactionHolderDto>(path);
    }


    static internal async Task<FileDto> GetTransactionAsPdfFile(long id) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/vouchers/{id}/print";

      return await webApiClient.GetAsync<FileDto>(path);
    }


    static internal async Task<FixedList<CashTransactionHolderDto>> GetTransactions(FixedList<long> transactionIds,
                                                                                    bool returnLegacySystemData) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/bulk-operation/get-transactions/{returnLegacySystemData}";

      return await webApiClient.PostAsync<FixedList<CashTransactionHolderDto>>(transactionIds, path);
    }


    static internal async Task<FixedList<CashEntryDescriptor>> GetTransactionsEntries(FixedList<long> entriesIds) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/entries/bulk-operation/get-entries";

      return await webApiClient.PostAsync<FixedList<CashEntryDescriptor>>(entriesIds, path);
    }


    static internal async Task<FixedList<CashEntryDescriptor>> SearchEntries(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/entries/search";

      return await webApiClient.PostAsync<FixedList<CashEntryDescriptor>>(query, path);
    }


    static internal async Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      return await webApiClient.PostAsync<FixedList<CashTransactionDescriptor>>(query, path);
    }


    static internal async Task UpdateBulkEntries(FixedList<CashEntryFields> bulkEntries) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/bulk-operation/update-entries";

      await webApiClient.PostAsync<JsonObject>(bulkEntries, path);
    }


    static internal async Task<CashTransactionHolderDto> UpdateEntries(FixedList<CashEntryFields> entries) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/{entries[0].TransactionId}/update-entries";

      return await webApiClient.PostAsync<CashTransactionHolderDto>(entries, path);
    }

    #region Helpers

    static private WebApiClient GetWebApiClient() {
      return WebApiClient.GetInstance("SICOFIN");
    }

    #endregion Helpers

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
