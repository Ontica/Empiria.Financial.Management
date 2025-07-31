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

    static internal async Task<CashTransactionHolderDto> ExecuteCommand(long id, CashEntriesCommand command) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = $"v2/financial-accounting/cash-ledger/transactions/{id}/execute-command";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(command, path);

      return json.Get<CashTransactionHolderDto>("data");
    }


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


    static internal async Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(query, path);

      return json.GetFixedList<CashTransactionDescriptor>("data");
    }

    #region Helpers

    static private WebApiClient GetWebApiClient() {
      return WebApiClient.GetInstance("SICOFIN");
    }

    #endregion Helpers

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
