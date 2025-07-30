/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Data Layer                              *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Data Services web api proxy             *
*  Type     : CashLedgerData                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides cash ledger transactions data services using a web proxy.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;
using Empiria.WebApi.Client;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.Data {

  /// <summary>Provides cash ledger transactions data services using a web proxy.</summary>
  static internal class CashLedgerData {

    static internal FixedList<CashTransactionDescriptor> GetTransactions(CashLedgerQuery query) {
      WebApiClient webApiClient = GetWebApiClient();

      string path = "v2/financial-accounting/cash-ledger";

      JsonObject json = webApiClient.PostAsync<JsonObject>(query, path).Result;

      return json.GetFixedList<CashTransactionDescriptor>("data");
    }

    #region Helpers

    static private WebApiClient GetWebApiClient() {
      return WebApiClient.GetInstance("SICOFIN");
    }

    #endregion Helpers

  }  // class CashLedgerData

}  // namespace Empiria.CashFlow.CashLedger.Data
