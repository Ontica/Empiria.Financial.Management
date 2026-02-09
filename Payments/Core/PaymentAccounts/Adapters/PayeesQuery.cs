/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Query DTO                               *
*  Type     : PayeesQuery                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search payees.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Payments.Adapters {

  /// <summary>Query DTO used to search payees.</summary>
  public class PayeesQuery {

    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class PayeesQuery

}  // namespace Empiria.Payments.Adapters
