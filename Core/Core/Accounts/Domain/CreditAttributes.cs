/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditAttributes                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds credit financial account attributes.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Financial {

  /// <summary>Holds credit financial account attributes.</summary>
  public class CreditAttributes : AccountAttributes {

    #region Constructors and Parsers

    private readonly JsonObject _attributes = new JsonObject();

    public CreditAttributes() {

    }

    internal CreditAttributes(JsonObject attributes) {
      Assertion.Require(attributes, nameof(attributes));

      _attributes = attributes;
    }

    #endregion Properties

    #region Properties

    public string NoCredito {
      get {
        return _attributes.Get("noCredito", string.Empty);
      }
      set {
        _attributes.SetIfValue("noCredito", value);
      }
    }


    public string Acreditado {
      get {
        return _attributes.Get("acreditado", string.Empty);
      }
      set {
        _attributes.SetIfValue("acreditado", value);
      }
    }


    public string TipoCredito {
      get {
        return _attributes.Get("tipoCredito", string.Empty);
      }
      set {
        _attributes.SetIfValue("tipoCredito", value);
      }
    }


    public int EtapaCredito {
      get {
        return _attributes.Get("etapaCredito", 0);
      }
      set {
        _attributes.SetIfValue("etapaCredito", value);
      }
    }


    #endregion Properties

    #region Helpers

    internal override string ToJsonString() {
      return _attributes.ToString();
    }

    #endregion Helpers

  } // class CreditAttributes

} // namespace Empiria.Financial
