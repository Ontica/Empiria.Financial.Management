/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillRelatedBillSchemaData                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds additional bill related bill information according to the bill related schema.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Services;

namespace Empiria.Billing {

  /// <summary>Holds additional bill related bill information according to the bill related schema.</summary>
  internal class BillRelatedBillSchemaData {

    private readonly JsonObject _schemaData = new JsonObject();

    internal BillRelatedBillSchemaData(JsonObject schemaData) {
      Assertion.Require(schemaData, nameof(schemaData));

      _schemaData = schemaData;
    }


    #region Properties

    
    public DateTime FechaPago {
      get {
        return _schemaData.Get("fechaPago", DateTime.MaxValue);
      }
      private set {
        _schemaData.SetIfValue("fechaPago", value);
      }
    }


    public string FormaDePagoP {
      get {
        return _schemaData.Get("formaDePagoP", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("formaDePagoP", value);
      }
    }


    public string MonedaP {
      get {
        return _schemaData.Get("monedaP", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("monedaP", value);
      }
    }


    public string TipoCambioP {
      get {
        return _schemaData.Get("tipoCambioP", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("tipoCambioP", value);
      }
    }


    public decimal Monto {
      get {
        return _schemaData.Get<decimal>("monto", 0);
      }
      private set {
        _schemaData.SetIfValue("monto", value);
      }
    }


    public string NumOperacion {
      get {
        return _schemaData.Get("numOperacion", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("numOperacion", value);
      }
    }


    public string IdDocumento {
      get {
        return _schemaData.Get("idDocumento", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("idDocumento", value);
      }
    }


    public string MonedaDR {
      get {
        return _schemaData.Get("monedaDR", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("monedaDR", value);
      }
    }


    public string EquivalenciaDR {
      get {
        return _schemaData.Get("equivalenciaDR", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("equivalenciaDR", value);
      }
    }


    public string NumParcialidad {
      get {
        return _schemaData.Get("numParcialidad", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("numParcialidad", value);
      }
    }


    public decimal ImpSaldoAnt {
      get {
        return _schemaData.Get<decimal>("impSaldoAnt", 0);
      }
      private set {
        _schemaData.SetIfValue("impSaldoAnt", value);
      }
    }


    public decimal ImpPagado {
      get {
        return _schemaData.Get<decimal>("impPagado", 0);
      }
      private set {
        _schemaData.SetIfValue("impPagado", value);
      }
    }


    public decimal ImpSaldoInsoluto {
      get {
        return _schemaData.Get<decimal>("impSaldoInsoluto", 0);
      }
      private set {
        _schemaData.SetIfValue("impSaldoInsoluto", value);
      }
    }


    public string ObjetoImpDR {
      get {
        return _schemaData.Get("objetoImpDR", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("objetoImpDR", value);
      }
    }


    internal string ToJsonString() {
      return _schemaData.ToString();
    }

    #endregion Properties


    internal void Update(ComplementRelatedPayoutDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      this.FechaPago = fields.FechaPago;
      this.FormaDePagoP = fields.FormaDePagoP;
      this.MonedaP = fields.MonedaP;
      this.TipoCambioP = fields.TipoCambioP;
      this.Monto = fields.Monto;
      this.NumOperacion = fields.NumOperacion;
      this.IdDocumento = fields.IdDocumento;
      this.MonedaDR = fields.MonedaDR;
      this.EquivalenciaDR = fields.EquivalenciaDR;
      this.NumParcialidad = fields.NumParcialidad;
      this.ImpSaldoAnt = fields.ImpSaldoAnt;
      this.ImpPagado = fields.ImpPagado;
      this.ImpSaldoInsoluto = fields.ImpSaldoInsoluto;
      this.ObjetoImpDR = fields.ObjetoImpDR;
    }

  } // class BillRelatedBillSchemaData

} // namespace Empiria.Billing
