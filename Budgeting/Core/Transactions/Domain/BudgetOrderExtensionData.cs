/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Information Holder                      *
*  Type     : BudgetOrderExtensionData                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains extended data for budget related orders.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Budgeting.Transactions {

  public class BudgetOrderExtensionData {

    private readonly JsonObject _json;

    public BudgetOrderExtensionData(JsonObject json) {
      _json = json;
    }

    static internal BudgetOrderExtensionData Parse(JsonObject json) {
      Assertion.Require(json, nameof(json));

      return new BudgetOrderExtensionData(json);
    }

    static public BudgetOrderExtensionData Empty {
      get {
        var extData = new BudgetOrderExtensionData(new JsonObject());

        extData.IsEmptyInstance = true;

        return extData;
      }
    }

    public bool IsEmptyInstance {
      get; private set;
    }


    public DateTime RequestedTime {
      get {
        return _json.Get("requestedTime", ExecutionServer.DateMinValue);
      }
    }


    public DateTime StartDate {
      get {
        return _json.Get("startDate", ExecutionServer.DateMinValue);
      }
    }


    public DateTime EndDate {
      get {
        return _json.Get("endDate", ExecutionServer.DateMinValue);
      }
    }

    public int EstimatedMonths {
      get {
        return _json.Get("estimatedMonths", 0);
      }
    }

  }  // class BudgetOrderExtensionData

}  // namespace Empiria.Budgeting.Transactions
