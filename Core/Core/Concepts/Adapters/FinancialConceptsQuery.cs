/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Input Query DTO                         *
*  Type     : FinancialConceptsQuery                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search financial concepts.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.Financial.Concepts.Adapters {

  /// <summary>Query DTO used to search financial concepts.</summary>
  public class FinancialConceptsQuery {

    public string GroupUID {
      get; set;
    } = string.Empty;


    public DateTime Date {
      get; set;
    } = DateTime.Today;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;

  }  // class FinancialConceptsQuery

}  // namespace Empiria.Financial.Concepts.Adapters
