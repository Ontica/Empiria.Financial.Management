/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditExtData                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds financial credit information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Json;

namespace Empiria.Financial {

  /// <summary>Holds financial credit information.</summary>
  public class CreditData {

    #region Constructors and Parsers

    private readonly JsonObject _extData = new JsonObject();

    internal CreditData(JsonObject creditData) {
      Assertion.Require(creditData, nameof(creditData));

      _extData = creditData;
    }

    #endregion Properties

    #region Properties

    public string CreditoNo {
      get {
        return _extData.Get("creditoNo", string.Empty);
      }
      private set {
        _extData.SetIfValue("creditoNo", value);
      }
    }


    public int EtapaCredito {
      get {
        return _extData.Get("etapaCredito", 0);
      }
      private set {
        _extData.SetIfValue("etapaCredito", value);
      }
    }


    public string Acreditado {
      get {
        return _extData.Get("acreditado", string.Empty);
      }
      private set {
        _extData.SetIfValue("acreditado", value);
      }
    }


    public string TipoCredito {
      get {
        return _extData.Get("tipoCredito", string.Empty);
      }
      private set {
        _extData.SetIfValue("tipoCredito", value);
      }
    }    

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _extData.ToString();
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
