/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : Bill                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represent a bill according to SAT Mexico standard.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Represent a bill according to SAT Mexico standard.</summary>
  internal class Bill : BaseObject {

    #region Constructors and parsers

    private Bill() {
      // Required by Empiria Framework.
    }

    static public Bill Parse(int id) => ParseId<Bill>(id);

    static public Bill Parse(string uid) => ParseKey<Bill>(uid);

    #endregion Constructors and parsers

    #region Public properties

    [DataField("RFC_EMISOR")]
    internal string RFCEmisor {
      get; private set;
    }


    [DataField("REGIMEN_FISCAL_EMISOR")]
    internal string RegimenFiscalEmisor {
      get; private set;
    } = string.Empty;


    [DataField("RFC_RECEPTOR")]
    internal string RFCReceptor {
      get; private set;
    }


    [DataField("REGIMEN_FISCAL_RECEPTOR")]
    internal string RegimenFiscalReceptor {
      get; private set;
    } = string.Empty;


    [DataField("DOMICILIO_FISCAL_RECEPTOR")]
    internal string DomicilioFiscalReceptor {
      get; private set;
    }


    [DataField("USO_CFDI")]
    internal string UsoCFDI {
      get; private set;
    }



    [DataField("CFDI_VERSION")]
    internal string CFDIVersion {
      get; private set;
    }


    [DataField("EXTDATA")]
    internal JsonObject ExtData {
      get; private set;
    }


    internal FixedList<BillConcept> Conceptos {
      get; set;
    } = new FixedList<BillConcept>();


    public int Folio {
      get {
        return this.ExtData.Get("folio", 0);
      }
      private set {
        this.ExtData.Set("folio", value);
      }
    }


    public DateTime Fecha {
      get {
        return this.ExtData.Get("fecha", ExecutionServer.DateMaxValue);
      }
      private set {
        this.ExtData.Set("fecha", value);
      }
    }


    private string Sello {
      get {
        return this.ExtData.Get<string>("sello", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("sello", value);
      }
    }


    private string FormaPago {
      get {
        return this.ExtData.Get<string>("formaPago", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("formaPago", value);
      }
    }


    private string NoCertificado {
      get {
        return this.ExtData.Get<string>("noCertificado", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("noCertificado", value);
      }
    }


    private string TipoDeComprobante {
      get {
        return this.ExtData.Get<string>("tipoDeComprobante", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("tipoDeComprobante", value);
      }
    }


    private string Exportacion {
      get {
        return this.ExtData.Get<string>("exportacion", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("exportacion", value);
      }
    }


    private string MetodoPago {
      get {
        return this.ExtData.Get<string>("metodoPago", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("metodoPago", value);
      }
    }


    private string LugarExpedicion {
      get {
        return this.ExtData.Get<string>("lugarExpedicion", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("lugarExpedicion", value);
      }
    }


    private string Moneda {
      get {
        return this.ExtData.Get<string>("moneda", string.Empty);
      }
      set {
        this.ExtData.SetIfValue("moneda", value);
      }
    }


    public decimal Subtotal {
      get {
        return this.ExtData.Get<decimal>("subtotal", 0);
      }
      set {
        this.ExtData.SetIfValue("subtotal", value);
      }
    }


    public decimal Total {
      get {
        return this.ExtData.Get<decimal>("total", 0);
      }
      set {
        this.ExtData.SetIfValue("total", value);
      }
    }

    #endregion Public properties

  } // class Bill

} // namespace Empiria.Billing
