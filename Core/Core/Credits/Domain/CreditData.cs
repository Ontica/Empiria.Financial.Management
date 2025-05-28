/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditExtData                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds financial credit information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Financial {

  /// <summary>Holds financial credit information.</summary>
  public class CreditData {

    #region Constructors and Parsers

    private readonly JsonObject _attributes = new JsonObject();

    internal CreditData(JsonObject attributes) {
      Assertion.Require(attributes, nameof(attributes));

      _attributes = attributes;
    }

    #endregion Properties

    #region Properties

    public string CreditoNo {
      get {
        return _attributes.Get("creditoNo", string.Empty);
      }
      private set {
        _attributes.SetIfValue("creditoNo", value);
      }
    }


    public int EtapaCredito {
      get {
        return _attributes.Get("etapaCredito", 0);
      }
      private set {
        _attributes.SetIfValue("etapaCredito", value);
      }
    }


    public string Acreditado {
      get {
        return _attributes.Get("acreditado", string.Empty);
      }
      private set {
        _attributes.SetIfValue("acreditado", value);
      }
    }


    public string TipoCredito {
      get {
        return _attributes.Get("tipoCredito", string.Empty);
      }
      private set {
        _attributes.SetIfValue("tipoCredito", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _attributes.ToString();
    }


    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      CreditoNo = fields.CreditoNo;
      EtapaCredito = fields.EtapaCredito;
      Acreditado = fields.Acreditado;
      TipoCredito = fields.TipoCredito;
    }

    #endregion Helpers


  } // class CreditExtData

} // namespace Empiria.Financial
