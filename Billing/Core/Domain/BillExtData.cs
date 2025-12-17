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


    public FixedList<BillConceptSchemaData> Concepts {
      get {
        return _extData.GetFixedList<BillConceptSchemaData>("concepts", false);
      }
      private set {
        _extData.SetIfValue("concepts", value);
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


    internal void UpdateFuelConsumptionAddenda(FuelConsumptionBillAddendaFields addenda) {
      Assertion.Require(addenda, nameof(addenda));

      if (addenda.Concepts.Count == 0) {
        return;
      }

      var conceptsExtData = new List<BillConceptSchemaData>();

      foreach (var conceptFields in addenda.Concepts) {

        var billConceptExtData = new BillConceptSchemaData();

        billConceptExtData.Update(conceptFields);

        conceptsExtData.Add(billConceptExtData);
      }
      Concepts = conceptsExtData.ToFixedList();
    }

  } // class BillExtData

} // namespace Empiria.Billing
