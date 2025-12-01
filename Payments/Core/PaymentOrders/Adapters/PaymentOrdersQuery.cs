/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Query Data Transfer Object              *
*  Type     : PaymentOrdersQuery                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search payment orders.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Adapters {

  /// <summary>Query DTO used to search payment orders.</summary>
  public class PaymentOrdersQuery {

    public PaymentOrderStatus Status {
      get; set;
    } = PaymentOrderStatus.All;


    public string RequesterOrgUnitUID {
      get; set;
    } = string.Empty;


    public string PaymentOrderTypeUID {
      get; set;
    } = string.Empty;


    public string PaymentMethodUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class PaymentOrdersQuery

}  // namespace Empiria.Payments.Adapters
