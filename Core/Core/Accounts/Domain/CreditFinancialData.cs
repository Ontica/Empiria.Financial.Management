/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditFinancialData                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds financial data for credit accounts.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Financial {

  /// <summary>Holds financial data for credit accounts.</summary>
  public class CreditFinancialData : FinancialData {

    #region Constructors and Parsers

    private readonly JsonObject _financialExtData = new JsonObject();

    internal CreditFinancialData(JsonObject financialData) {
      Assertion.Require(financialData, nameof(financialData));

      _financialExtData = financialData;
    }

    #endregion Properties

    #region Properties

    public decimal Interes {
      get {
        return _financialExtData.Get("interes", 0m);
      }
      private set {
        _financialExtData.SetIfValue("interes", value);
      }
    }


    public decimal Comision {
      get {
        return _financialExtData.Get("comision", 0m);
      }
      private set {
        _financialExtData.SetIfValue("comision", value);
      }
    }


    public decimal Saldo {
      get {
        return _financialExtData.Get("saldo", 0m);
      }
      private set {
        _financialExtData.SetIfValue("saldo", value);
      }
    }


    public int PlazoInversion {
      get {
        return _financialExtData.Get("plazoInversion", 0);
      }
      private set {
        _financialExtData.SetIfValue("plazoInversion", value);
      }
    }


    public int PeriodoGracia {
      get {
        return _financialExtData.Get("periodoGracia", 0);
      }
      private set {
        _financialExtData.SetIfValue("periodoGracia", value);
      }
    }


    public int PlazoAmortizacion {
      get {
        return _financialExtData.Get("plazoAmortizacion", 0);
      }
      private set {
        _financialExtData.SetIfValue("plazoAmortizacion", value);
      }

    }


    public DateTime FechaAmortizacion {
      get {
        return _financialExtData.Get("fechaAmortizacion", ExecutionServer.DateMaxValue);
      }
      private set {
        _financialExtData.SetIfValue("fechaAmortizacion", value);
      }
    }


    public decimal TipoCambio {
      get {
        return _financialExtData.Get("tipoCambio", 0m);
      }
      private set {
        _financialExtData.SetIfValue("tipoCambio", value);
      }
    }


    public decimal Tasa {
      get {
        return _financialExtData.Get("tasa", 0m);
      }
      private set {
        _financialExtData.SetIfValue("tasa", value);
      }
    }


    public decimal FactorTasa {
      get {
        return _financialExtData.Get("factorTasa", 0m);
      }
      private set {
        _financialExtData.SetIfValue("factorTasa", value);
      }
    }


    public decimal TasaPiso {
      get {
        return _financialExtData.Get("tasaPiso", 0m);
      }
      private set {
        _financialExtData.SetIfValue("tasaPiso", value);
      }
    }


    public decimal TasaTecho {
      get {
        return _financialExtData.Get("tasaTecho", 0m);
      }
      private set {
        _financialExtData.SetIfValue("tasaTecho", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal override string ToJsonString() {
      return _financialExtData.ToString();
    }

    #endregion Helpers

  } // class CreditFinancialData

} // namespace Empiria.Financial
