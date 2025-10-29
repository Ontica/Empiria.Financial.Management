/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Input query DTO                         *
*  Type     : RecordsSearchQuery                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to search cash flow related data and entities.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Adapters {

  public enum RecordSearchQueryType {

    CashFlowConcepts,

    CashFlowEntriesBySubledger,

    CreditEntries,

    None
  }


  /// <summary>Input query DTO used to search cash flow related data and entities.</summary>
  public class RecordsSearchQuery : IQuery {

    public RecordSearchQueryType QueryType {
      get; set;
    } = RecordSearchQueryType.None;


    public DateTime FromDate {
      get; set;
    } = DateTime.MinValue;


    public DateTime ToDate {
      get; set;
    } = DateTime.MinValue;


    public string ClassificationUID {
      get; set;
    } = string.Empty;


    public string OperationTypeUID {
      get; set;
    } = string.Empty;


    public string[] Keywords {
      get; set;
    } = new string[0];


    public string[] Ledgers {
      get; set;
    } = new string[0];


    [Newtonsoft.Json.JsonProperty(PropertyName = "Parties")]
    public string OrgUnitUID {
      get; set;
    } = string.Empty;

  }  // class RecordsSearchQuery

}  // namespace Empiria.Financial.Adapters
