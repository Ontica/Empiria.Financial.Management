/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Query DTO                               *
*  Type     : CashFlowProjectionsQuery                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve cash flow projections.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Input query DTO used to retrieve cash flow projections.</summary>
  public class CashFlowProjectionsQuery {

    public string PlanUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string BasePartyUID {
      get; set;
    } = string.Empty;


    public string BaseProjectUID {
      get; set;
    } = string.Empty;


    [Newtonsoft.Json.JsonProperty(PropertyName = "ClassificationUID")]
    public string BaseProjectCategoryUID {
      get; set;
    } = string.Empty;


    public string BaseAccountUID {
      get; set;
    } = string.Empty;


    public string SourceUID {
      get; set;
    } = string.Empty;


    public string[] ProjectionsNo {
      get; set;
    } = new string[0];


    public string Keywords {
      get; set;
    } = string.Empty;


    public string EntriesKeywords {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public TransactionDateType DateType {
      get; set;
    } = TransactionDateType.None;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public TransactionPartyType PartyType {
      get; set;
    } = TransactionPartyType.None;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public TransactionStatus Status {
      get; set;
    } = TransactionStatus.All;


    public TransactionStage Stage {
      get; set;
    } = TransactionStage.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class CashFlowProjectionsQuery

}  // namespace Empiria.CashFlow.Projections.Adapters
