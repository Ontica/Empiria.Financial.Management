/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Query DTO                               *
*  Type     : BillsQuery, BillQueryDateType              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve bills.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Billing.Adapters {

  /// <summary>Enumerates the bill dates query options.</summary>
  public enum BillQueryDateType {

    Emission,

    Payment,

    Recording,

    Verification,

    Cancelation,

    None

  }  // enum BillQueryDateType



  /// <summary>Input query DTO used to retrieve bills.</summary>
  public class BillsQuery {

    public string BillTypeUID {
      get; set;
    } = string.Empty;


    public string BillCategoryUID {
      get; set;
    } = string.Empty;


    public string ManagedByUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public string ConceptsKeywords {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public BillQueryDateType BillDateType {
      get; set;
    } = BillQueryDateType.None;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public BillStatus Status {
      get; set;
    } = BillStatus.All;

  }  // class BillsQuery

}  // namespace Empiria.Billing.Adapters
