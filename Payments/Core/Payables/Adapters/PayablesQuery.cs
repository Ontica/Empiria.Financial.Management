/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Query Data Transfer Object              *
*  Type     : PayablesQuery                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search payable objects.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Query DTO used to search payable objects.</summary>
  public class PayablesQuery {

    public PayableStatus Status {
      get; set;
    } = PayableStatus.All;


    public string PayableTypeUID {
      get; set;
    } = string.Empty;


    public string RequesterOrgUnitUID {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
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


  }  // class PayablesQuery

}  // namespace Empiria.Payments.Payables.Adapters
