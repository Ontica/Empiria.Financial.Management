/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditExtData                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds financial credit information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

namespace Empiria.Financial.Accounts {

  /// <summary>Holds financial credit information.</summary>
  public class  CreditExtData {

    private readonly JsonObject _extData = new JsonObject();

    internal CreditExtData(JsonObject creditData) {
      Assertion.Require(creditData, nameof(creditData));

      _extData = creditData;
    }


    public string Plazo {
      get {
        return _extData.Get("plazo", string.Empty);
      }
      private set {
        _extData.SetIfValue("plazo", value);
      }
    }
        

    public decimal Monto {
      get {
        return _extData.Get("monto", 0);
      }
      private set {
        _extData.SetIfValue("monto", value);
      }
    }


    internal string ToJsonString() {
      return _extData.ToString();
    }


    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      Plazo = fields.Plazo;
      Monto = fields.Monto;
    }

  } // class CreditExtData

} // namespace Empiria.Financial.Accounts