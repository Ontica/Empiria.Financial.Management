/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Input Query DTO                         *
*  Type     : PayableEntitiesQuery                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrive payable entities.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Input query DTO used to retrive payable entities.</summary>
  public class PayableEntitiesQuery {

    public string PayableTypeUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class PayableEntitiesQuery


  static public class PayableEntitiesQueryExtensions {

    static public void EnsureIsValid(this PayableEntitiesQuery query) {
      Assertion.Require(query.PayableTypeUID, nameof(query.PayableTypeUID));

      _ = PayableType.Parse(query.PayableTypeUID);
    }

  }

}  // namespace Empiria.Payments.Payables.Adapters
