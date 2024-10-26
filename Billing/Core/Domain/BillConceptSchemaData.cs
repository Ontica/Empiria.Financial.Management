/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillConceptSchemaData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds schema-related data for a bill concept.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Holds schema-related data for a bill concept.</summary>
  public class BillConceptSchemaData {

    private readonly JsonObject _schemaData;

    public BillConceptSchemaData(JsonObject billConceptSchemaData) {
      Assertion.Require(billConceptSchemaData, nameof(billConceptSchemaData));

      _schemaData = billConceptSchemaData;
    }


    public string ClaveProdServ {
      get {
        return _schemaData.Get("claveProdServ", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("claveProdServ", value);
      }
    }


    public string ClaveUnidad {
      get {
        return _schemaData.Get("claveUnidad", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("claveUnidad", value);
      }
    }


    public string Unidad {
      get {
        return _schemaData.Get("unidad", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("unidad", value);
      }
    }


    public string NoIdentificacion {
      get {
        return _schemaData.Get("noIdentificacion", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("noIdentificacion", value);
      }
    }


    public string ObjetoImp {
      get {
        return _schemaData.Get("objetoImp", string.Empty);
      }
      private set {
        _schemaData.SetIfValue("objetoImp", value);
      }
    }

  }  // class BillConceptSchemaData

} // namespace Empiria.Billing
