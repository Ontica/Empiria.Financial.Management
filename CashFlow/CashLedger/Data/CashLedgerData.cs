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
using Empiria.WebApi.Client;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  static public class CashLedgerData {

    static public Task<FixedList<NamedEntityDto>> GetAccountingLedgers() {

      return GetFixedList<NamedEntityDto>("v2/financial-accounting/ledgers/ifrs");
    }


    static internal async Task<FixedList<CashTransactionDescriptor>> GetTransactions(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger/transactions/search";

      return await webApiClient.PostAsync<FixedList<CashTransactionDescriptor>>(query, path);
    }


    static public Task<FixedList<NamedEntityDto>> GetTransactionSources() {

      return GetFixedList<NamedEntityDto>("v2/financial-accounting/vouchers/functional-areas");
    }


    static public Task<FixedList<NamedEntityDto>> GetTransactionTypes() {

      return GetFixedList<NamedEntityDto>("v2/financial-accounting/vouchers/transaction-types");
    }


    static public Task<FixedList<NamedEntityDto>> GetVoucherTypes() {

      return GetFixedList<NamedEntityDto>("v2/financial-accounting/vouchers/voucher-types");
    }

    #region Helpers

    private static async Task<FixedList<T>> GetFixedList<T>(string path) {
      WebApiClient webApiClient = GetWebApiClient();

      return await webApiClient.GetAsync<FixedList<T>>(path);
    }


    static private WebApiClient GetWebApiClient() {
      return WebApiClient.GetInstance("SICOFIN");
    }

    #endregion Helpers

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
