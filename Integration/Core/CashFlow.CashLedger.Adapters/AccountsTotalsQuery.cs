/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Input query DTO                         *
*  Type     : AccountsTotalsQuery                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to search accounts totals in a date period.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.DataTypes;

namespace Empiria.Financial.Adapters {

  /// <summary>Input query DTO used to search information about accounts in a date period.</summary>
  public class AccountsTotalsQuery : IQuery {

    public string QueryType {
      get; set;
    } = string.Empty;


    public DateTime FromDate {
      get; set;
    } = DateTime.MaxValue;


    public DateTime ToDate {
      get; set;
    } = DateTime.MaxValue;


    public string[] Accounts {
      get; set;
    } = new string[0];


    public string[] Ledgers {
      get; set;
    } = new string[0];


    public KeyValue[] CustomFields {
      get; set;
    } = new KeyValue[0];

  }  // class AccountsTotalsQuery

}  // namespace Empiria.Financial.Adapters
