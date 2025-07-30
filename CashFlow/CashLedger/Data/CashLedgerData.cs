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
using Empiria.WebApi.Client;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  static internal class CashLedgerData {

    static internal Task<FixedList<NamedEntityDto>> GetAccountingLedgers() {
      return GetFixedList<NamedEntityDto>("v2/financial-accounting/ledgers/ifrs");
    }


    static internal async Task<FixedList<CashTransactionDescriptor>> GetTransactions(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      JsonObject json = await webApiClient.PostAsync<JsonObject>(query, path);

      return json.GetFixedList<CashTransactionDescriptor>("data");
    }


    static internal Task<FixedList<NamedEntityDto>> GetTransactionSources() {
      return GetFixedList<NamedEntityDto>("v2/financial-accounting/vouchers/functional-areas");
    }


    static internal Task<FixedList<NamedEntityDto>> GetTransactionTypes() {

      return GetFixedList<NamedEntityDto>("v2/financial-accounting/vouchers/transaction-types");
    }


    static internal Task<FixedList<NamedEntityDto>> GetVoucherTypes() {

      return GetFixedList<NamedEntityDto>("v2/financial-accounting/vouchers/voucher-types");
    }

    #region Helpers

    private static async Task<FixedList<T>> GetFixedList<T>(string path) {
      WebApiClient webApiClient = GetWebApiClient();

      JsonObject json = await webApiClient.GetAsync<JsonObject>(path);

      return json.GetFixedList<T>("data");
    }


    static private WebApiClient GetWebApiClient() {
      return WebApiClient.GetInstance("SICOFIN");
    }

    #endregion Helpers

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
