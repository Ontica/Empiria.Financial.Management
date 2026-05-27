/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : ICreditAccountData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface with credit account data coming from external systems.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Financial.Adapters {

  /// <summary>Interface with credit account data coming from external systems.</summary>
  public interface ICreditAccountData {

    string CreditNo {
      get;
    }

    string SubledgerAccountNo {
      get;
    }

    InterestRateType BaseInterestRate {
      get;
    }

    CreditProjectType CreditProjectType {
      get;
    }

    Currency Currency {
      get;
    }

    string CustomerType {
      get;
    }

    string CustomerName {
      get;
    }

    OrganizationalUnit OrganizationalUnit {
      get;
    }

    CreditType CreditType {
      get;
    }

    string StandardAccount {
      get;
    }

    string CreditLineNo {
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

    int RepaymentTerm {
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

    decimal OpeningFee {
      get;
    }

    decimal DisbursementFee {
      get;
    }

    bool CapitalizeFees {
      get;
    }

    bool CapitalizeInterest {
      get;
    }

    CreditRiskStage CreditRiskStage {
      get;
    }


  }  // interface ICreditAccountData

} // namespace Empiria.Financial.Adapters
