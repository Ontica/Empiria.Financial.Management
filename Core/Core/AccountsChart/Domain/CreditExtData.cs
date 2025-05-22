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

    private readonly JsonObject _extData = new JsonObject();

    internal CreditExtData(JsonObject creditData) {
      Assertion.Require(creditData, nameof(creditData));

      _extData = creditData;
    }


    public int Year {
      get {
        return _extData.Get("year", 0);
      }
      private set {
        _extData.SetIfValue("year", value);
      }
    }
        

    public string OrganizationUnit {
      get {
        return _extData.Get("OrganizationalUnit", string.Empty);
      }
      private set {
        _extData.SetIfValue("OrganizationalUnit", value);
      }
    }

    public string CreditNo {
      get {
        return _extData.Get("CreditNo", string.Empty);
      }
      private set {
        _extData.SetIfValue("CreditNo", value);
      }
    }

    
    public int StageCredit {
      get {
        return _extData.Get("StageCredit", 0);
      }
      private set {
        _extData.SetIfValue("StageCredit", value);
      }
    }


    public int PublicWorkNo {
      get {
        return _extData.Get("PublicWorkNo", 0);
      }
      private set {
        _extData.SetIfValue("PublicWorkNo", value);
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


    public string Muncipality {
      get {
        return _extData.Get("Muncipality", string.Empty);
      }
      private set {
        _extData.SetIfValue("Muncipality", value);
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


    public string PublicWorkType {
      get {
        return _extData.Get("PublicWorkType", string.Empty);
      }
      private set {
        _extData.SetIfValue("PublicWorkType", value);
      }
    }


    public string ProyectDescription {
      get {
        return _extData.Get("ProyectDescription", string.Empty);
      }
      private set {
        _extData.SetIfValue("ProyectDescription", value);
      }
    }


    public int TotalBeneficiaries {
      get {
        return _extData.Get("TotalBeneficiaries", 0);
      }
      private set {
        _extData.SetIfValue("TotalBeneficiaries", value);
      }
    }


    public int BeneficiaryUnit {
      get {
        return _extData.Get("BeneficiaryUnit", 0);
      }
      private set {
        _extData.SetIfValue("BeneficiaryUnit", value);
      }
    }


    public int DirectJobs {
      get {
        return _extData.Get("DirectJobs", 0);
      }
      private set {
        _extData.SetIfValue("DirectJobs", value);
      }
    }


    public int IndirectJobs {
      get {
        return _extData.Get("IndirectJobs", 0);
      }
      private set {
        _extData.SetIfValue("IndirectJobs", value);
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


    public int Disbursements {
      get {
        return _extData.Get("Disbursements", 0);
      }
      private set {
        _extData.SetIfValue("Disbursements", value);
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


    public decimal WorkAmount {
      get {
        return _extData.Get("WorkAmount", 0);
      }
      private set {
        _extData.SetIfValue("WorkAmount", value);
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


    public decimal DisbursementPeriod {
      get {
        return _extData.Get("DisbursementPeriod", 0);
      }
      private set {
        _extData.SetIfValue("DisbursementPeriod", value);
      }
    }


    public decimal GracePeriod {
      get {
        return _extData.Get("GracePeriod", 0);
      }
      private set {
        _extData.SetIfValue("GracePeriod", value);
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


    public int Floorrate {
      get {
        return _extData.Get("Floorrate", 0);
      }
      private set {
        _extData.SetIfValue("Floorrate", value);
      }
    }


    public int Ceilingrate {
      get {
        return _extData.Get("Ceilingrate", 0);
      }
      private set {
        _extData.SetIfValue("Ceilingrate", value);
      }
    }


    public int OpeningCommission {
      get {
        return _extData.Get("OpeningCommission", 0);
      }
      private set {
        _extData.SetIfValue("OpeningCommission", value);
      }
    }


    public int CommissionAvailability {
      get {
        return _extData.Get("CommissionAvailability", 0);
      }
      private set {
        _extData.SetIfValue("CommissionAvailability", value);
      }
    }


    public DateTime DisbursementDate {
      get {
        return _extData.Get("DisbursementDate", DateTime.Now);
      }
      private set {
        _extData.SetIfValue("DisbursementDate", value);
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


    internal string ToJsonString() {
      return _extData.ToString();
    }


    public int Clasification {
      get {
        return _extData.Get("Clasification", 0);
      }
      private set {
        _extData.SetIfValue("Clasification", value);
      }
    }


    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      Year = fields.Year;
      OrganizationUnit = OrganizationalUnit.Parse(fields.OrganizationUnitUID).Name;
      CreditNo = fields.CreditNo;
      StageCredit = fields.StageCredit;
      PublicWorkNo = fields.PublicWorkNo;
      Acredited = fields.Acredited;
      Muncipality = fields.Muncipality;
      CreditType = fields.CreditType;
      PublicWorkType = fields.PublicWorkType;
      ProyectDescription = fields.ProyectDescription;
      TotalBeneficiaries = fields.TotalBeneficiaries;
      BeneficiaryUnit = fields.BeneficiaryUnit;
      DirectJobs = fields.DirectJobs;
      IndirectJobs = fields.IndirectJobs;
      Currency = fields.Currency;
      Disbursements = fields.Disbursements;
      Interests = fields.Interests;
      Commissions = fields.Commissions;
      Balances = fields.Balances;
      WorkAmount = fields.WorkAmount;
      InvestmentTerm = fields.InvestmentTerm;
      DisbursementPeriod = fields.DisbursementPeriod;
      GracePeriod = fields.GracePeriod;
      AmortizationTerm = fields.AmortizationTerm;
      Rate = fields.Rate;
      RateFactor = fields.RateFactor;
      Floorrate = fields.Floorrate;
      Ceilingrate = fields.Ceilingrate;
      OpeningCommission = fields.OpeningCommission;
      CommissionAvailability = fields.CommissionAvailability;
      DisbursementDate = fields.DisbursementDate;
      AmortizationDate = fields.AmortizationDate;
      Clasification = fields.Clasification;
    }

  } // class CreditExtData

} // namespace Empiria.Financial.Accounts