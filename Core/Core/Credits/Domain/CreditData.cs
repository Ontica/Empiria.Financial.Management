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

namespace Empiria.Financial {

  /// <summary>Holds financial credit information.</summary>
  public class CreditData {

    #region Constructors and Parsers

    private readonly JsonObject _extData = new JsonObject();

    internal CreditData(JsonObject creditData) {
      Assertion.Require(creditData, nameof(creditData));

      _extData = creditData;
    }

    #endregion Properties

    #region Properties

    public string CreditoNo {
      get {
        return _extData.Get("creditoNo", string.Empty);
      }
      private set {
        _extData.SetIfValue("creditoNo", value);
      }
    }


    public int EtapaCredito {
      get {
        return _extData.Get("etapaCredito", 0);
      }
      private set {
        _extData.SetIfValue("etapaCredito", value);
      }
    }


    public string Acreditado {
      get {
        return _extData.Get("acreditado", string.Empty);
      }
      private set {
        _extData.SetIfValue("acreditado", value);
      }
    }


    public string TipoCredito {
      get {
        return _extData.Get("tipoCredito", string.Empty);
      }
      private set {
        _extData.SetIfValue("tipoCredito", value);
      }
    }


    public decimal Interes {
      get {
        return _extData.Get("interes", 0);
      }
      private set {
        _extData.SetIfValue("interes", value);
      }
    }


    public int Comision {
      get {
        return _extData.Get("comision", 0);
      }
      private set {
        _extData.SetIfValue("comision", value);
      }
    }


    public decimal Saldo {
      get {
        return _extData.Get("saldo", 0);
      }
      private set {
        _extData.SetIfValue("saldo", value);
      }
    }


    public decimal PlazoInversion {
      get {
        return _extData.Get("plazoInversion", 0);
      }
      private set {
        _extData.SetIfValue("plazoInversion", value);
      }
    }


    public decimal PeriodoGracia {
      get {
        return _extData.Get("periodoGracia", 0);
      }
      private set {
        _extData.SetIfValue("periodoGracia", value);
      }
    }


    public decimal PlazoAmortizacion {
      get {
        return _extData.Get("plazoAmortizacion", 0);
      }
      private set {
        _extData.SetIfValue("plazoAmortizacion", value);
      }
    }


    public decimal Tasa {
      get {
        return _extData.Get("tasa", 0);
      }
      private set {
        _extData.SetIfValue("tasa", value);
      }
    }


    public int FactorTasa {
      get {
        return _extData.Get("factorTasa", 0);
      }
      private set {
        _extData.SetIfValue("factorTasa", value);
      }
    }


    public int TasaPiso {
      get {
        return _extData.Get("tasaPiso", 0);
      }
      private set {
        _extData.SetIfValue("tasaPiso", value);
      }
    }


    public int TasaTecho {
      get {
        return _extData.Get("tasaTecho", 0);
      }
      private set {
        _extData.SetIfValue("tasaTecho", value);
      }
    }


    public DateTime FechaAmortizacion {
      get {
        return _extData.Get("fechaAmortizacion", DateTime.Now);
      }
      private set {
        _extData.SetIfValue("fechaAmortizacion", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _extData.ToString();
    }


    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      CreditoNo = fields.CreditoNo;
      EtapaCredito = fields.EtapaCredito;
      Acreditado = fields.Acreditado;
      TipoCredito = fields.TipoCredito;
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


  } // class CreditExtData

} // namespace Empiria.Financial
