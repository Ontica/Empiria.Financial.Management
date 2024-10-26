/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : BillSecurityData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds bill security data according to its schema.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Json;

namespace Empiria.Billing {

  /// <summary>Holds bill security data according to its schema.</summary>
  public class BillSecurityData {

    private readonly JsonObject _securityData;

    internal BillSecurityData(JsonObject securityData) {
      Assertion.Require(securityData, nameof(securityData));

      _securityData = securityData;
    }


    public string Certificado {
      get {
        return _securityData.Get("certificado", string.Empty);
      }
      private set {
        _securityData.SetIfValue("certificado", value);
      }
    }


    public string Sello {
      get {
        return _securityData.Get("sello", string.Empty);
      }
      private set {
        _securityData.SetIfValue("sello", value);
      }
    }


    public string SelloCFD {
      get {
        return _securityData.Get("selloCFD", string.Empty);
      }
      private set {
        _securityData.SetIfValue("selloCFD", value);
      }
    }


    public string SelloSAT {
      get {
        return _securityData.Get("selloSAT", string.Empty);
      }
      private set {
        _securityData.SetIfValue("selloSAT", value);
      }
    }

  }  // BillSecurityData

} // namespace Empiria.Billing
