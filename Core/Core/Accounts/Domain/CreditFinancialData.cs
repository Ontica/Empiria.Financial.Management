/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditFinancialData                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds financial data for credit accounts.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Financial {

  /// <summary>Holds financial data for credit accounts.</summary>
  public class CreditFinancialData : FinancialData {

    #region Constructors and Parsers

    private readonly JsonObject _financialExtData = new JsonObject();

    internal CreditFinancialData(JsonObject financialData) {
      Assertion.Require(financialData, nameof(financialData));

      _financialExtData = financialData;
    }

    #endregion Properties

    #region Properties

    public decimal Fees {
      get {
        return _financialExtData.Get("fees", 0m);
      }
      private set {
        _financialExtData.SetIfValue("fees", value);
      }
    }


    public decimal CurrentBalance {
      get {
        return _financialExtData.Get("currentBalance", 0m);
      }
      private set {
        _financialExtData.SetIfValue("currentBalance", value);
      }
    }


    public int InvestmentTerm {
      get {
        return _financialExtData.Get("investmentTerm", 0);
      }
      private set {
        _financialExtData.SetIfValue("investmentTerm", value);
      }
    }


    public int GracePeriod {
      get {
        return _financialExtData.Get("gracePeriod", 0);
      }
      private set {
        _financialExtData.SetIfValue("gracePeriod", value);
      }
    }


    public int RepaymentTerm {
      get {
        return _financialExtData.Get("repaymentTerm", 0);
      }
      private set {
        _financialExtData.SetIfValue("repaymentTerm", value);
      }
    }


    public DateTime RepaymentDate {
      get {
        return _financialExtData.Get("repaymentDate", ExecutionServer.DateMaxValue);
      }
      private set {
        _financialExtData.SetIfValue("repaymentDate", value);
      }
    }


    public decimal ExchangeRate {
      get {
        return _financialExtData.Get("exchangeRate", 0m);
      }
      private set {
        _financialExtData.SetIfValue("exchangeRate", value);
      }
    }

    public decimal InterestRate {
      get {
        return _financialExtData.Get("interestRate", 0m);
      }
      private set {
        _financialExtData.SetIfValue("interestRate", value);
      }
    }


    public InterestRateType InterestRateType {
      get {
        return _financialExtData.Get("interestRateTypeId", InterestRateType.Empty);
      }
      private set {
        _financialExtData.SetIfValue("interestRateTypeId", value.Id);
      }
    }


    public decimal InterestRateFactor {
      get {
        return _financialExtData.Get("interestRateFactor", 0m);
      }
      private set {
        _financialExtData.SetIfValue("interestRateFactor", value);
      }
    }


    public decimal InterestRateFloor {
      get {
        return _financialExtData.Get("interestRateFloor", 0m);
      }
      private set {
        _financialExtData.SetIfValue("interestRateFloor", value);
      }
    }


    public decimal InterestRateCeiling {
      get {
        return _financialExtData.Get("interestRateCeiling", 0m);
      }
      private set {
        _financialExtData.SetIfValue("interestRateCeiling", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal override string ToJsonString() {
      return _financialExtData.ToString();
    }

    #endregion Helpers

  } // class CreditFinancialData

} // namespace Empiria.Financial
