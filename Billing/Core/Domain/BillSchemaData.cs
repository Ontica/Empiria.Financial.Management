/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillSchemaData                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds additional bill information according to the bill's schema.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Holds additional bill information according to the bill's schema.</summary>
  public class BillSchemaData {

    private readonly JsonObject _schemaData = new JsonObject();

    internal BillSchemaData(JsonObject schemaData) {
      Assertion.Require(schemaData, nameof(schemaData));

      _schemaData = schemaData;
    }


    public string Version {
      get {
        return _schemaData.Get("version", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("version", value);
      }
    }


    public string NombreEmisor {
      get {
        return _schemaData.Get("nombreEmisor", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("nombreEmisor", value);
      }
    }


    public string RFCEmisor {
      get {
        return _schemaData.Get("rfcEmisor", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("rfcEmisor", value);
      }
    }


    public string NombreReceptor {
      get {
        return _schemaData.Get("nombreReceptor", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("nombreReceptor", value);
      }
    }


    public string RFCReceptor {
      get {
        return _schemaData.Get("rfcReceptor", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("rfcReceptor", value);
      }
    }


    public string RegimenFiscalEmisor {
      get {
        return _schemaData.Get<string>("regimenFiscalEmisor", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("regimenFiscalEmisor", value);
      }
    }


    public string DomicilioFiscalReceptor {
      get {
        return _schemaData.Get<string>("domicilioFiscalReceptor", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("domicilioFiscalReceptor", value);
      }
    }


    public string UsoCFDI {
      get {
        return _schemaData.Get("usoCFDI", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("usoCFDI", value);
      }
    }


    public string TipoComprobante {
      get {
        return _schemaData.Get("tipoComprobante", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("tipoComprobante", value);
      }
    }


    public string Folio {
      get {
        return _schemaData.Get("folio", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("folio", value);
      }
    }


    public string Serie {
      get {
        return _schemaData.Get("serie", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("serie", value);
      }
    }


    public string MetodoPago {
      get {
        return _schemaData.Get("metodoPago", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("metodoPago", value);
      }
    }


    public string FormaPago {
      get {
        return _schemaData.Get("formaPago", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("formaPago", value);
      }
    }


    public string Exportacion {
      get {
        return _schemaData.Get("exportacion", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("exportacion", value);
      }
    }


    public string LugarExpedicion {
      get {
        return _schemaData.Get("lugarExpedicion", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("lugarExpedicion", value);
      }
    }


    public string Moneda {
      get {
        return _schemaData.Get<string>("moneda", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("moneda", value);
      }
    }

    internal string ToJsonString() {
      return _schemaData.ToString();
    }

    internal void Update(BillSchemaDataFields schemaData) {
      Assertion.Require(schemaData, nameof(schemaData));

      Version = schemaData.SchemaVersion;
      RFCEmisor = schemaData.IssuedBy.RFC;
      NombreEmisor = schemaData.IssuedBy.Nombre;
      RFCReceptor = schemaData.IssuedTo.RFC;
      NombreReceptor = schemaData.IssuedTo.Nombre;
      RegimenFiscalEmisor =schemaData.IssuedBy.RegimenFiscal;
      DomicilioFiscalReceptor = schemaData.IssuedTo.DomicilioFiscal;
      UsoCFDI = schemaData.IssuedTo.UsoCFDI;
      TipoComprobante = schemaData.TipoComprobante;
      Folio = schemaData.Folio;
      Serie = schemaData.Serie;
      MetodoPago = schemaData.MetodoPago;
      FormaPago = schemaData.FormaPago;
      Exportacion = schemaData.Exportacion;
      LugarExpedicion = schemaData.LugarExpedicion;
      Moneda = schemaData.Moneda;
    }
  }

} // namespace Empiria.Billing
