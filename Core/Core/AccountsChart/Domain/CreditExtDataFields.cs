/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                            Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : CreditExtDataFields                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial credit information.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Accounts {

  /// <summary>Input fields with financial credit information.</summary>
  public class CreditExtDataFields {

    #region Properties

    public int Year {
      get; set;
    } = 0;


    public  string OrganizationUnitUID {
      get; set;
    } = string.Empty;


    public string CreditNo {
      get; set;
    } = string.Empty;


    public int StageCredit {
      get; set;
    } = 0;


    public int PublicWorkNo {
      get; set;
    } = 0;


    public string Acredited {
      get; set;
    } = string.Empty;


    public string Muncipality {
      get; set;
    } = string.Empty;


    public int CreditType {
      get; set;
    } = 0;


    public string PublicWorkType {
      get; set;
    } = string.Empty;


    public string ProyectDescription {
      get; set;
    } = string.Empty;


    public int DirectJobs {
      get; set;
    } = 0;


    public int BeneficiaryUnit {
      get; set;
    } = 0;

    public int TotalBeneficiaries {
      get; set;
    } = 0;


    public int IndirectJobs {
      get; set;
    } = 0;


    public int Currency {
      get; set;
    } = 0;


    public int Disbursements {
      get; set;
    } = 0;


    public decimal Interests {
      get; set;
    } = 0;


    public int Commissions {
      get; set;
    } = 0;


    public decimal Balances {
      get; set;
    } = 0;


    public decimal WorkAmount {
      get; set;
    } = 0;


    public decimal InvestmentTerm {
      get; set;
    } = 0;


    public decimal DisbursementPeriod {
      get; set;
    } = 0;


    public decimal GracePeriod {
      get; set;
    } = 0;


    public decimal AmortizationTerm {
      get; set;
    } = 0;


    public decimal Rate {
      get; set;
    } = 0;


    public int RateFactor {
      get; set;
    } = 0;


    public int Floorrate {
      get; set;
    } = 0;


    public int Ceilingrate {
      get; set;
    } = 0;


    public int OpeningCommission {
      get; set;
    } = 0;
  
  
    public int CommissionAvailability {
      get; set;
    } = 0;


    public DateTime DisbursementDate {
      get; set;
    } = DateTime.Now;


    public DateTime AmortizationDate {
      get; set;
    } = DateTime.Now;


    public int Clasification {
      get; set;
    } = 0;


    /*
    internal string ToJsonString() {
      return _extData.ToString();
    }
    */

    #endregion Properties

    #region Methods

    #endregion Methods

  }  // class CreditExtDataFields

}  // namespace Empiria.Financial.Accounts
