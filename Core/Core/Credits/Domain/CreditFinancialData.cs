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
  public class CreditFinancialData {

    #region Constructors and Parsers

    private readonly JsonObject _financialExtData = new JsonObject();

    internal CreditFinancialData(JsonObject creditData) {
      Assertion.Require(creditData, nameof(creditData));

      _financialExtData = creditData;
    }

    #endregion Properties

    #region Properties
        
    public decimal Interes {
      get {
        return _financialExtData.Get("interes", 0);
      }
      private set {
        _financialExtData.SetIfValue("interes", value);
      }
    }


    public int Comision {
      get {
        return _financialExtData.Get("comision", 0);
      }
      private set {
        _financialExtData.SetIfValue("comision", value);
      }
    }


    public decimal Saldo {
      get {
        return _financialExtData.Get("saldo", 0);
      }
      private set {
        _financialExtData.SetIfValue("saldo", value);
      }
    }


    public decimal PlazoInversion {
      get {
        return _financialExtData.Get("plazoInversion", 0);
      }
      private set {
        _financialExtData.SetIfValue("plazoInversion", value);
      }
    }


    public decimal PeriodoGracia {
      get {
        return _financialExtData.Get("periodoGracia", 0);
      }
      private set {
        _financialExtData.SetIfValue("periodoGracia", value);
      }
    }


    public decimal PlazoAmortizacion {
      get {
        return _financialExtData.Get("plazoAmortizacion", 0);
      }
      private set {
        _financialExtData.SetIfValue("plazoAmortizacion", value);
      }
    }


    public decimal Tasa {
      get {
        return _financialExtData.Get("tasa", 0);
      }
      private set {
        _financialExtData.SetIfValue("tasa", value);
      }
    }


    public int FactorTasa {
      get {
        return _financialExtData.Get("factorTasa", 0);
      }
      private set {
        _financialExtData.SetIfValue("factorTasa", value);
      }
    }


    public int TasaPiso {
      get {
        return _financialExtData.Get("tasaPiso", 0);
      }
      private set {
        _financialExtData.SetIfValue("tasaPiso", value);
      }
    }


    public int TasaTecho {
      get {
        return _financialExtData.Get("tasaTecho", 0);
      }
      private set {
        _financialExtData.SetIfValue("tasaTecho", value);
      }
    }


    public DateTime FechaAmortizacion {
      get {
        return _financialExtData.Get("fechaAmortizacion", DateTime.Now);
      }
      private set {
        _financialExtData.SetIfValue("fechaAmortizacion", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _financialExtData.ToString();
    }


    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      Interes = fields.Interes;
      Comision = fields.Comision;
      Saldo = fields.Saldo;
      PlazoInversion = fields.PlazoInversion;
      PeriodoGracia = fields.PlazoGracia;
      PlazoAmortizacion = fields.PlazoAmortizacion;
      Tasa = fields.Tasa;
      TasaPiso = fields.TasaPiso;
      FactorTasa = fields.FactorTasa;
      TasaTecho = fields.TasaTecho;
      FechaAmortizacion = fields.FechaAmortizacion;
    }

    #endregion Helpers

  } // class CreditFinancialData

} // namespace Empiria.Financial
