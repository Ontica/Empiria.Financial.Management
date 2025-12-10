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


    public decimal NoIdentificacion {
      get {
        return _extData.Get("noIdentificacion", 0);
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


    public FixedList<BillConceptExtData> Concepts {
      get {
        return _extData.GetFixedList<BillConceptExtData>("Concepts", true);
      }
      private set {
        _extData.SetIfValue("Concepts", value);
      }
    }


    internal string ToJsonString() {
      return _extData.ToString();
    }


    internal void Update(BillAddendaFields addendaFields) {
      Assertion.Require(addendaFields, nameof(addendaFields));

      if (addendaFields.NoEstacion != string.Empty) {
        NoEstacion = addendaFields.NoEstacion;
        ClavePemex = addendaFields.ClavePemex;
        TasaIEPS = addendaFields.TasaIEPS;
        IEPS = addendaFields.IEPS;
        TasaIVA = addendaFields.TasaIVA;
        IVA = addendaFields.IVA;
        NoIdentificacion = addendaFields.NoIdentificacion;
        TasaAIEPS = addendaFields.TasaAIEPS;
        AIEPS = addendaFields.AIEPS;
      }
    }


    internal void Update(FuelConsumptionBillAddendaFields addenda) {
      Assertion.Require(addenda, nameof(addenda));

      if (addenda.Concepts.Count==0) {
        return;
      }

      var conceptsExtData = new List<BillConceptExtData>();

      foreach (var conceptFields in addenda.Concepts) {

        var billConceptExtData = new BillConceptExtData();

        billConceptExtData.Update(conceptFields);

        conceptsExtData.Add(billConceptExtData);
      }
      Concepts = conceptsExtData.ToFixedList();
    }

  } // class BillExtData


  public class BillConceptExtData {

    private readonly JsonObject _conceptExtData = new JsonObject();

    public BillConceptExtData() {

    }

    internal BillConceptExtData(JsonObject schemaData) {
      Assertion.Require(schemaData, nameof(schemaData));

      _conceptExtData = schemaData;
    }


    public string BillUID {
      get {
        return _conceptExtData.Get("billUID", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("billUID", value);
      }
    }


    public string ProductUID {
      get {
        return _conceptExtData.Get("productUID", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("productUID", value);
      }
    }


    public string SATProductUID {
      get {
        return _conceptExtData.Get("satProductUID", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("satProductUID", value);
      }
    }


    public string SATProductServiceCode {
      get {
        return _conceptExtData.Get("satProductServiceCode", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("satProductServiceCode", value);
      }
    }


    public string UnitKey {
      get {
        return _conceptExtData.Get("unitKey", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("unitKey", value);
      }
    }


    public string Unit {
      get {
        return _conceptExtData.Get("unit", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("unit", value);
      }
    }


    public string IdentificationNo {
      get {
        return _conceptExtData.Get("identificationNo", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("identificationNo", value);
      }
    }


    public string ObjectImp {
      get {
        return _conceptExtData.Get("objectImp", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("objectImp", value);
      }
    }


    public string Description {
      get {
        return _conceptExtData.Get("description", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("description", value);
      }
    }


    public string Tags {
      get {
        return _conceptExtData.Get("tags", string.Empty);
      }
      private set {
        _conceptExtData.SetIfValue("tags", value);
      }
    }


    public decimal Quantity {
      get {
        return _conceptExtData.Get("quantity", 0);
      }
      private set {
        _conceptExtData.SetIfValue("quantity", value);
      }
    }


    public decimal UnitPrice {
      get {
        return _conceptExtData.Get("unitPrice", 0);
      }
      private set {
        _conceptExtData.SetIfValue("unitPrice", value);
      }
    }


    public decimal Subtotal {
      get {
        return _conceptExtData.Get("subtotal", 0);
      }
      private set {
        _conceptExtData.SetIfValue("subtotal", value);
      }
    }


    public decimal Discount {
      get {
        return _conceptExtData.Get("discount", 0);
      }
      private set {
        _conceptExtData.SetIfValue("discount", value);
      }
    }


    internal string ToJsonString() {
      return _conceptExtData.ToString();
    }


    internal void Update(BillConceptWithTaxFields conceptFields) {
      Assertion.Require(conceptFields, nameof(conceptFields));

      BillUID = conceptFields.BillUID;
      ProductUID = conceptFields.ProductUID;
      SATProductUID = conceptFields.SATProductUID;
      SATProductServiceCode = conceptFields.SATProductServiceCode;
      UnitKey= conceptFields.UnitKey;
      Unit = conceptFields.Unit;
      IdentificationNo = conceptFields.IdentificationNo;
      ObjectImp = conceptFields.ObjectImp;
      Description = conceptFields.Description;
      Tags = EmpiriaString.Tagging(conceptFields.Tags);
      Quantity = conceptFields.Quantity;
      UnitPrice = conceptFields.UnitPrice;
      Subtotal = conceptFields.Subtotal;
      Discount = conceptFields.Discount;
    }

  } // class BillConceptFields

} // namespace Empiria.Billing
