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
using Empiria.Parties;

namespace Empiria.Financial.Accounts {

  /// <summary>Holds financial credit information.</summary>
  public class  CreditExtData {

    #region Constructors and Parsers

    private readonly JsonObject _extData = new JsonObject();

    internal CreditExtData(JsonObject creditData) {
      Assertion.Require(creditData, nameof(creditData));

      _extData = creditData;
    }

    #endregion Properties

    #region Properties

    public string CreditNo {
      get {
        return _extData.Get("CreditNo", string.Empty);
      }
      private set {
        _extData.SetIfValue("CreditNo", value);
      }
    }

    
    public int EtapaCredito {
      get {
        return _extData.Get("EtapaCredito", 0);
      }
      private set {
        _extData.SetIfValue("EtapaCredito", value);
      }
    }
    

    public string Acredited {
      get {
        return _extData.Get("Acredited", string.Empty);
      }
      private set {
        _extData.SetIfValue("Acredited", value);
      }
    }
      

    public int CreditType {
      get {
        return _extData.Get("CreditType", 0);
      }
      private set {
        _extData.SetIfValue("CreditType", value);
      }
    }
    

    public int Currency {
      get {
        return _extData.Get("Currency", 0);
      }
      private set {
        _extData.SetIfValue("Currency", value);
      }
    }


    public decimal Interests {
      get {
        return _extData.Get("Interests", 0);
      }
      private set {
        _extData.SetIfValue("Interests", value);
      }
    }


    public int Commissions {
      get {
        return _extData.Get("Commissions", 0);
      }
      private set {
        _extData.SetIfValue("Commissions", value);
      }
    }


    public decimal Balances {
      get {
        return _extData.Get("Balances", 0);
      }
      private set {
        _extData.SetIfValue("Balances", value);
      }
    }
        

    public decimal InvestmentTerm {
      get {
        return _extData.Get("InvestmentTerm", 0);
      }
      private set {
        _extData.SetIfValue("InvestmentTerm", value);
      }
    }


    public decimal PeriodoGracia {
      get {
        return _extData.Get("PeriodoGracia", 0);
      }
      private set {
        _extData.SetIfValue("PeriodoGracia", value);
      }
    }


    public decimal AmortizationTerm {
      get {
        return _extData.Get("AmortizationTerm", 0);
      }
      private set {
        _extData.SetIfValue("AmortizationTerm", value);
      }
    }


    public decimal Rate {
      get {
        return _extData.Get("Rate", 0);
      }
      private set {
        _extData.SetIfValue("Rate", value);
      }
    }


    public int RateFactor {
      get {
        return _extData.Get("RateFactor", 0);
      }
      private set {
        _extData.SetIfValue("RateFactor", value);
      }
    }


    public int TasaPiso {
      get {
        return _extData.Get("TasaPiso", 0);
      }
      private set {
        _extData.SetIfValue("TasaPiso", value);
      }
    }


    public int TasaTecho {
      get {
        return _extData.Get("TasaTecho", 0);
      }
      private set {
        _extData.SetIfValue("TasaTecho", value);
      }
    }


    public DateTime AmortizationDate {
      get {
        return _extData.Get("AmortizationDate", DateTime.Now);
      }
      private set {
        _extData.SetIfValue("AmortizationDate", value);
      }
    }  

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _extData.ToString();
    }


    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      CreditNo = fields.CreditNo;
      EtapaCredito = fields.EtapaCredito;
      Acredited = fields.Acredited;
      CreditType = fields.CreditType;
      Currency = fields.Currency;
      Interests = fields.Interests;
      Commissions = fields.Commissions;
      Balances = fields.Balances;
      InvestmentTerm = fields.InvestmentTerm;
      PeriodoGracia = fields.PlazoGracia;
      AmortizationTerm = fields.AmortizationTerm;
      Rate = fields.Rate;
      TasaPiso = fields.TasaPiso;
      RateFactor = fields.RateFactor;
      TasaTecho = fields.TasaTecho;
      AmortizationDate = fields.AmortizationDate;
    }

    #endregion Helpers


  } // class CreditExtData

} // namespace Empiria.Financial.Accounts