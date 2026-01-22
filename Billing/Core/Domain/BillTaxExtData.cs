/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillSchemaData                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds bill tax information.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Holds bill tax information.</summary>
  internal class BillTaxExtData {


    private readonly JsonObject _extData = new JsonObject();

    internal BillTaxExtData(JsonObject extData) {
      Assertion.Require(extData, nameof(extData));

      _extData = extData;
    }


    public string ImpuestoMetodo {
      get {
        return _extData.Get("impuestoMetodo", string.Empty);
      }
      private set {
        _extData.SetIfValue("impuestoMetodo", value);
      }
    }


    public decimal Base {
      get {
        return _extData.Get<decimal>("base", 0);
      }
      private set {
        _extData.SetIfValue("base", value);
      }
    }


    public string Impuesto {
      get {
        return _extData.Get("impuesto", string.Empty);
      }
      private set {
        _extData.SetIfValue("impuesto", value);
      }
    }


    public string TipoFactor {
      get {
        return _extData.Get("tipoFactor", string.Empty);
      }
      private set {
        _extData.SetIfValue("tipoFactor", value);
      }
    }


    public decimal Factor {
      get {
        return _extData.Get<decimal>("factor", 0);
      }
      private set {
        _extData.SetIfValue("factor", value);
      }
    }


    public decimal TasaOCuota {
      get {
        return _extData.Get<decimal>("tasaOCuota", 0);
      }
      private set {
        _extData.SetIfValue("tasaOCuota", value);
      }
    }


    public decimal Importe {
      get {
        return _extData.Get<decimal>("importe", 0);
      }
      private set {
        _extData.SetIfValue("importe", value);
      }
    }


    internal string ToJsonString() {
      return _extData.ToString();
    }


    internal void Update(BillTaxEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      ImpuestoMetodo = fields.TaxMethod.ToString();
      TipoFactor = fields.TaxFactorType.ToString();
      Factor = fields.Factor;
      Base = fields.BaseAmount;
      Impuesto = fields.Impuesto;
      Importe = fields.Total;
    }
  } // class BillTaxExtData
}
