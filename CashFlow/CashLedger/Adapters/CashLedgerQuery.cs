/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Query DTO                               *
*  Type     : CashLedgerQuery                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve cash ledger transactions.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Integration.CashLedger;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Input query DTO used to retrieve cash ledger transactions.</summary>
  public class CashLedgerQuery : SharedCashLedgerQuery {

    public string PartyUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectTypeUID")]
    public string ProjectTypeCategoryUID {
      get; set;
    } = string.Empty;


    public string ProjectAccountUID {
      get; set;
    } = string.Empty;


    public string EntriesKeywords {
      get; set;
    } = string.Empty;

  }  // class CashLedgerQuery

}  // namespace Empiria.CashFlow.CashLedger.Adapters
