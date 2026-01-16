/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : ICreditSicData                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface with credit data coming from external systems.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Adapters {

  /// <summary>Interface with credit data coming from external systems.</summary>
  public interface ICreditSicData {

    string CreditNo {
      get;
    }

    string SubledgerAccountNo {
      get;
    }

    string BaseInterestRate {
      get;
    }

    string BaseMorInterestRate {
      get;
    }

    string PreviousCredits {
      get;
    }

    string CurrencyNo {
      get;
    }


    string Currency{
      get;
    }


    string FederalTaxPayersReg {
      get;
     }

    int FederalTaxPayersRegNo {
      get;
    }

    string CustomerType { 
      get; 
    }

    string CustomerName {
      get;
    }


    string OrganizationUnitNo {
      get;
    }


    string OrganizationUnit {
      get;
    }


    string CreditType {
      get;
    }

    string CreditStage {
      get;
    }

    string StandardAccount {
      get;
    }

    string ExternalCreditNo {
      get;
    }

    DateTime MaxAvailabilityDate { 
      get; 
    }

    DateTime MaxRefinancingDate {
      get;
    }

    int LineCreditNo {
      get;
    }

    decimal NetFinancedAmount {
      get;
    }

    string ConstructionBuilding {
      get;
    }

    decimal ConstructionBuildingCost { 
      get; 
    }

    decimal LoanAmount { 
      get; 
    }

    decimal CurrentBalance {
      get;
    }

    int InvestmentTerm {
      get;
    }

    int DisbursementPeriod { 
      get; 
    }

    DateTime DisbursementDate {
      get;
    }

    int RepaymentTerm {
      get;
    }

    DateTime RepaymentDate {
      get;
    }

    int InterestRate { 
      get; 
    }

    int InterestGracePeriod {
      get;
    }

    decimal InterestRateFactor {
      get;
    }

    decimal InterestRateFloor {
      get;
    }

    decimal InterestRateCeiling {
      get;
    }


  }  // interface ICreditSictData

} // namespace Empiria.Financial.Adapters
