/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillExtData                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds additional bill information according to the bill's.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Holds additional bill information according to the bill's.</summary>
  public class BillExtData {

    private readonly JsonObject _extData = new JsonObject();

    internal BillExtData(JsonObject schemaData) {
      Assertion.Require(schemaData, nameof(schemaData));

      _extData = schemaData;
    }


    public string CfdiRelacionado {
      get {
        return _extData.Get("cfdiRelacionado", string.Empty);
      }
      private set {
        _extData.SetIfValue("cfdiRelacionado", value);
      }
    }


    public string TipoRelacion {
      get {
        return _extData.Get("tipoRelacion", string.Empty);
      }
      private set {
        _extData.SetIfValue("tipoRelacion", value);
      }
    }


    public string TipoRelacionNombre {
      get {
        return _extData.Get("tipoRelacionNombre", string.Empty);
      }
      private set {
        _extData.SetIfValue("tipoRelacionNombre", value);
      }
    }


    public string NoEstacion {
      get {
        return _extData.Get("noEstacion", string.Empty);
      }
      private set {
        _extData.SetIfValue("noEstacion", value);
      }
    }


    public string ClavePemex {
      get {
        return _extData.Get("clavePemex", string.Empty);
      }
      private set {
        _extData.SetIfValue("clavePemex", value);
      }
    }


    public decimal TasaIEPS {
      get {
        return _extData.Get("tasaIeps", 0);
      }
      private set {
        _extData.SetIfValue("tasaIeps", value);
      }
    }


    public decimal IEPS {
      get {
        return _extData.Get("ieps", 0);
      }
      private set {
        _extData.SetIfValue("ieps", value);
      }
    }


    public decimal TasaIVA {
      get {
        return _extData.Get("tasaIva", 0);
      }
      private set {
        _extData.SetIfValue("tasaIva", value);
      }
    }


    public decimal IVA {
      get {
        return _extData.Get("iva", 0);
      }
      private set {
        _extData.SetIfValue("iva", value);
      }
    }


    public string NoIdentificacion {
      get {
        return _extData.Get("noIdentificacion", string.Empty);
      }
      private set {
        _extData.SetIfValue("noIdentificacion", value);
      }
    }


    public decimal TasaAIEPS {
      get {
        return _extData.Get("tasaAieps", 0);
      }
      private set {
        _extData.SetIfValue("tasaAieps", value);
      }
    }


    public decimal AIEPS {
      get {
        return _extData.Get("aIeps", 0);
      }
      private set {
        _extData.SetIfValue("aIeps", value);
      }
    }


    public string ComplementVersion {
      get {
        return _extData.Get("complementVersion", string.Empty);
      }
      private set {
        _extData.SetIfValue("complementVersion", value);
      }
    }


    public string TipoOperacion {
      get {
        return _extData.Get("tipoOperacion", string.Empty);
      }
      private set {
        _extData.SetIfValue("tipoOperacion", value);
      }
    }


    public string NumeroDeCuenta {
      get {
        return _extData.Get("numeroDeCuenta", string.Empty);
      }
      private set {
        _extData.SetIfValue("numeroDeCuenta", value);
      }
    }


    public string AddendaLeyendas {
      get {
        return _extData.Get("addendaLeyendas", string.Empty);
      }
      private set {
        _extData.SetIfValue("addendaLeyendas", value);
      }
    }


    public decimal SubTotal {
      get {
        return _extData.Get("subTotal", 0);
      }
      private set {
        _extData.SetIfValue("subTotal", value);
      }
    }


    public decimal Total {
      get {
        return _extData.Get("total", 0);
      }
      private set {
        _extData.SetIfValue("total", value);
      }
    }


    public string Folio {
      get {
        return _extData.Get("folio", string.Empty);
      }
      private set {
        _extData.SetIfValue("folio", value);
      }
    }


    public string Serie {
      get {
        return _extData.Get("serie", string.Empty);
      }
      private set {
        _extData.SetIfValue("serie", value);
      }
    }


    public DateTime FechaEmision {
      get {
        return _extData.Get("fechaEmision", DateTime.MaxValue);
      }
      private set {
        _extData.SetIfValue("fechaEmision", value);
      }
    }


    internal string ToJsonString() {
      return _extData.ToString();
    }


    public string DisposicionFiscal {
      get {
        return _extData.Get("disposicionFiscal", string.Empty);
      }
      private set {
        _extData.SetIfValue("disposicionFiscal", value);
      }
    }


    public string Norma {
      get {
        return _extData.Get("norma", string.Empty);
      }
      private set {
        _extData.SetIfValue("norma", value);
      }
    }


    public string TextoLeyenda {
      get {
        return _extData.Get("textoLeyenda", string.Empty);
      }
      private set {
        _extData.SetIfValue("textoLeyenda", value);
      }
    }


    internal void Update(BillFields fields) {
      Assertion.Require(fields, nameof(fields));

      if (fields.BillRelatedDocument.RelatedCFDI != string.Empty) {
        CfdiRelacionado = fields.BillRelatedDocument.RelatedCFDI;
        TipoRelacion = fields.BillRelatedDocument.TipoRelacion;
        TipoRelacionNombre = fields.BillRelatedDocument.TipoRelacionNombre.ToString();
      }
      if (fields.Addenda.NoEstacion != string.Empty) {
        NoEstacion = fields.Addenda.NoEstacion;
        ClavePemex = fields.Addenda.ClavePemex;
        TasaIEPS = fields.Addenda.TasaIEPS;
        IEPS = fields.Addenda.IEPS;
        TasaIVA = fields.Addenda.TasaIVA;
        IVA = fields.Addenda.IVA;
        NoIdentificacion = fields.Addenda.NoIdentificacion;
        TasaAIEPS = fields.Addenda.TasaAIEPS;
        AIEPS = fields.Addenda.AIEPS;
      }
      if (fields.Addenda.Concepts.Count > 0) {
        Folio = fields.Addenda.Folio;
        Serie = fields.Addenda.Serie;
        FechaEmision = fields.Addenda.FechaEmision;
      }
      if (fields.FiscalLegendsData.DisposicionFiscal != string.Empty) {
        DisposicionFiscal = fields.FiscalLegendsData.DisposicionFiscal;
      }
      if (fields.FiscalLegendsData.Norma != string.Empty) {
        Norma = fields.FiscalLegendsData.Norma;
      }
      if (fields.FiscalLegendsData.TextoLeyenda != string.Empty) {
        TextoLeyenda = fields.FiscalLegendsData.TextoLeyenda;
      }

    }


    internal void UpdateFuelConsumptionComplementData(FuelConsumptionBillFields fields) {
      Assertion.Require(fields, nameof(fields));
      
      string addendaLeyendas = string.Empty;

      foreach (var addendaLabel in fields.Addenda.Labels) {
        addendaLeyendas += $"{addendaLabel}.. ";
      }

      ComplementVersion = fields.ComplementData.Version;
      TipoOperacion = fields.ComplementData.TipoOperacion;
      NumeroDeCuenta = fields.ComplementData.NumeroDeCuenta;
      SubTotal = fields.ComplementData.SubTotal;
      Total = fields.ComplementData.Total;
      AddendaLeyendas = addendaLeyendas;
    }

  } // class BillExtData

} // namespace Empiria.Billing
