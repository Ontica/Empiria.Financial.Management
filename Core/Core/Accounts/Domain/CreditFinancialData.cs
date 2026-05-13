/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditFinancialData                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds financial data for credit accounts.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

using Empiria.Financial.Adapters;

namespace Empiria.Financial {

  /// <summary>Holds financial data for credit accounts.</summary>
  public class CreditFinancialData : FinancialData {

    #region Constructors and Parsers

    private readonly JsonObject _financialExtData = new JsonObject();

    public CreditFinancialData(JsonObject financialData) {
      Assertion.Require(financialData, nameof(financialData));

      _financialExtData = financialData;
    }

    public CreditFinancialData(ICreditAccountData account) {
      Assertion.Require(account, nameof(account));

      CurrentBalance = account.CurrentBalance;
      GracePeriod = account.InterestGracePeriod;
      InterestRate = account.InterestRate;
      InvestmentTerm = account.InvestmentTerm;
      InterestRateCeiling = account.InterestRateCeiling;
      InterestRateFactor = account.InterestRateFactor;
      InterestRateFloor = account.InterestRateFloor;
      RepaymentTerm = account.RepaymentTerm;
    }

    #endregion Properties

    #region Properties

    public bool CapitalizeInterest {
      get {
        return _financialExtData.Get("capitalizeInterest", false);
      }
      private set {
        _financialExtData.SetIfValue("capitalizeInterest", value);
      }
    }

    public bool CapitalizeFees {
      get {
        return _financialExtData.Get("capitalizeFees", false);
      }
      private set {
        _financialExtData.SetIfValue("capitalizeFees", value);
      }
    }


    public decimal OpeningFee {
      get {
        return _financialExtData.Get("openingFee", 0m);
      }
      private set {
        _financialExtData.SetIfValue("openingFee", value);
      }
    }

    public decimal DisbursementFee {
      get {
        return _financialExtData.Get("disbursementFee", 0m);
      }
      private set {
        _financialExtData.SetIfValue("disbursementFee", value);
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
        _financialExtData.SetIf("interestRateTypeId", value.Id, !value.IsEmptyInstance);
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

    internal JsonObject ToJson() {
      return _financialExtData;
    }

    #endregion Properties

    #region Helpers

    public override string ToJsonString() {
      return _financialExtData.ToString();
    }

    #endregion Helpers

  } // class CreditFinancialData

} // namespace Empiria.Financial
