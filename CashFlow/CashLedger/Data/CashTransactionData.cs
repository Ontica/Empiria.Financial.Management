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

using Empiria.Storage;
using Empiria.WebApi.Client;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  internal class CashTransactionData {

    private readonly WebApiClient _webApiClient;

    internal CashTransactionData() {
      _webApiClient = WebApiClient.GetInstance("SICOFIN");
    }


    internal async Task<CashTransactionHolderDto> GetTransaction(long id) {

      string path = $"v2/financial-accounting/cash-ledger/transactions/{id}";

      return await _webApiClient.GetAsync<CashTransactionHolderDto>(path);
    }


    internal async Task<FileDto> GetTransactionAsPdfFile(long id) {

      string path = $"v2/financial-accounting/vouchers/{id}/print";

      return await _webApiClient.GetAsync<FileDto>(path);
    }


    internal async Task<FixedList<CashTransactionHolderDto>> GetTransactions(FixedList<long> transactionIds) {

      string path = $"v2/financial-accounting/cash-ledger/transactions/bulk-operation/get-transactions";

      return await _webApiClient.PostAsync<FixedList<CashTransactionHolderDto>>(transactionIds, path);
    }


    internal async Task<FixedList<CashEntryDescriptor>> GetTransactionsEntries(FixedList<long> entriesIds) {

      string path = $"v2/financial-accounting/cash-ledger/entries/bulk-operation/get-entries";

      return await _webApiClient.PostAsync<FixedList<CashEntryDescriptor>>(entriesIds, path);
    }


    internal async Task<FixedList<CashEntryDescriptor>> SearchEntries(CashLedgerQuery query) {

      string path = "v2/financial-accounting/cash-ledger/entries/search";

      return await _webApiClient.PostAsync<FixedList<CashEntryDescriptor>>(query, path);
    }


    internal async Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      return await _webApiClient.PostAsync<FixedList<CashTransactionDescriptor>>(query, path);
    }


    internal async Task UpdateBulkEntries(FixedList<CashEntryFields> bulkEntries) {

      string path = $"v2/financial-accounting/cash-ledger/transactions/bulk-operation/update-entries";

      await _webApiClient.PostAsync(bulkEntries, path);
    }


    internal async Task<CashTransactionHolderDto> UpdateEntries(FixedList<CashEntryFields> entries) {

      string path = $"v2/financial-accounting/cash-ledger/transactions/{entries[0].TransactionId}/update-entries";

      return await _webApiClient.PostAsync<CashTransactionHolderDto>(entries, path);
    }

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
