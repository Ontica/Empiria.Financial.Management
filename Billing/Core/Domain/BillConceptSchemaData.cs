/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillConceptSchemaData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds schema-related data for a bill concept.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Holds schema-related data for a bill concept.</summary>
  public class BillConceptSchemaData {

    private readonly JsonObject _conceptSchemaData;

    public BillConceptSchemaData() {

    }

    public BillConceptSchemaData(JsonObject billConceptSchemaData) {
      Assertion.Require(billConceptSchemaData, nameof(billConceptSchemaData));

      _conceptSchemaData = billConceptSchemaData;
    }


    public string ClaveProdServ {
      get {
        return _conceptSchemaData.Get("claveProdServ", string.Empty);
      }
      private set {
        _conceptSchemaData.SetIfValue("claveProdServ", value);
      }
    }


    public decimal Cantidad {
      get {
        return _conceptSchemaData.Get<decimal>("cantidad", 0);
      }
      private set {
        _conceptSchemaData.SetIfValue("cantidad", value);
      }
    }


    public string ClaveUnidad {
      get {
        return _conceptSchemaData.Get("claveUnidad", string.Empty);
      }
      private set {
        _conceptSchemaData.SetIfValue("claveUnidad", value);
      }
    }


    public string Unidad {
      get {
        return _conceptSchemaData.Get("unidad", string.Empty);
      }
      private set {
        _conceptSchemaData.SetIfValue("unidad", value);
      }
    }


    public string Descripcion {
      get {
        return _conceptSchemaData.Get("descripcion", string.Empty);
      }
      private set {
        _conceptSchemaData.SetIfValue("descripcion", value);
      }
    }


    public decimal ValorUnitario {
      get {
        return _conceptSchemaData.Get<decimal>("valorUnitario", 0);
      }
      private set {
        _conceptSchemaData.SetIfValue("valorUnitario", value);
      }
    }


    public decimal Importe {
      get {
        return _conceptSchemaData.Get<decimal>("importe", 0);
      }
      private set {
        _conceptSchemaData.SetIfValue("importe", value);
      }
    }


    public decimal Descuento {
      get {
        return _conceptSchemaData.Get<decimal>("descuento", 0);
      }
      private set {
        _conceptSchemaData.SetIfValue("descuento", value);
      }
    }


    public string NoIdentificacion {
      get {
        return _conceptSchemaData.Get("noIdentificacion", string.Empty);
      }
      private set {
        _conceptSchemaData.SetIfValue("noIdentificacion", value);
      }
    }


    public string ObjetoImp {
      get {
        return _conceptSchemaData.Get("objetoImp", string.Empty);
      }
      private set {
        _conceptSchemaData.SetIfValue("objetoImp", value);
      }
    }


    public bool IsBonusConcept {
      get {
        return _conceptSchemaData.Get<bool>("isBonusConcept", false);
      }
      private set {
        _conceptSchemaData.SetIfValue("isBonusConcept", value);
      }
    }


    internal string ToJsonString() {
      return _conceptSchemaData.ToString();
    }


    internal void Update(BillConceptFields fields) {
      Assertion.Require(fields, nameof(fields));

      ClaveProdServ = fields.SATProductServiceCode;
      Cantidad = fields.Quantity;
      ClaveUnidad = fields.UnitKey;
      Unidad = fields.Unit;
      Descripcion = fields.Description;
      ValorUnitario = fields.UnitPrice;
      Importe = fields.Subtotal;
      Descuento = fields.Discount;
      NoIdentificacion = fields.IdentificationNo;
      ObjetoImp = fields.ObjectImp;
      if (fields.IsBonusConcept) {
        IsBonusConcept = true;
      }
    }


    internal void UpdateComplementConcept(FuelConsumptionComplementConceptDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      ClaveProdServ = Patcher.Patch(fields.SATProductServiceCode, "ND");
      NoIdentificacion = fields.Identificador;
      ClaveUnidad = fields.ClaveEstacion;
      Unidad = fields.Unidad;
      Cantidad = fields.Cantidad;
      ValorUnitario = fields.ValorUnitario;
      Importe = fields.Importe;
    }

  }  // class BillConceptSchemaData


  /// <summary>Holds schema-related data for a bill concept.</summary>
  public class BillConceptExtData {

    private readonly JsonObject _conceptExtData = new JsonObject();

    public BillConceptExtData() {

    }


    internal BillConceptExtData(JsonObject schemaData) {
      Assertion.Require(schemaData, nameof(schemaData));

      _conceptExtData = schemaData;
    }


    public string Identificador {
      get {
        return _conceptExtData.Get("identificador", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("identificador", value);
      }
    }


    public string Rfc {
      get {
        return _conceptExtData.Get("rfc", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("rfc", value);
      }
    }


    public string ClaveEstacion {
      get {
        return _conceptExtData.Get("claveEstacion", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("claveEstacion", value);
      }
    }


    public string TipoCombustible {
      get {
        return _conceptExtData.Get("tipoCombustible", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("tipoCombustible", value);
      }
    }


    public string Unidad {
      get {
        return _conceptExtData.Get("unidad", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("unidad", value);
      }
    }


    public string NombreCombustible {
      get {
        return _conceptExtData.Get("nombreCombustible", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("nombreCombustible", value);
      }
    }


    public string FolioOperacion {
      get {
        return _conceptExtData.Get("folioOperacion", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("folioOperacion", value);
      }
    }


    public DateTime Fecha {
      get {
        return _conceptExtData.Get("fecha", DateTime.MaxValue);
      }
      private set {
        _conceptExtData.SetIfValue("fecha", value);
      }
    }


    public decimal Cantidad {
      get {
        return _conceptExtData.Get("cantidad", 0);
      }
      private set {
        _conceptExtData.SetIfValue("cantidad", value);
      }
    }


    public decimal ValorUnitario {
      get {
        return _conceptExtData.Get("valorUnitario", 0);
      }
      private set {
        _conceptExtData.SetIfValue("valorUnitario", value);
      }
    }


    public decimal Importe {
      get {
        return _conceptExtData.Get("importe", 0);
      }
      private set {
        _conceptExtData.SetIfValue("importe", value);
      }
    }


    internal string ToJsonString() {
      return _conceptExtData.ToString();
    }


    internal void Update(FuelConsumptionComplementConceptDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      Identificador = fields.Identificador;
      Rfc = fields.Rfc;
      ClaveEstacion = fields.ClaveEstacion;
      TipoCombustible = fields.TipoCombustible;
      Unidad = fields.Unidad;
      NombreCombustible = fields.NombreCombustible;
      FolioOperacion = fields.FolioOperacion;
      Fecha = fields.Fecha;
      Cantidad = fields.Cantidad;
      ValorUnitario = fields.ValorUnitario;
      Importe = fields.Importe;
    }

  } // class BillConceptExtData

} // namespace Empiria.Billing
