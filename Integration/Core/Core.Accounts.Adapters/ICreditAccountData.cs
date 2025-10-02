/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : ICreditAccountData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface with credit account data coming from external systems.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;

namespace Empiria.Financial.Adapters {

  /// <summary>Interface with credit account data coming from external systems.</summary>
  public interface ICreditAccountData {

    string AccountNo {
      get;
    }

    string Borrower {
      get;
    }

    string SubledgerAccountNo {
      get;
    }

    string CreditStage {
      get;
    }

    string CreditType {
      get;
    }

    string ExternalCreditNo {
      get;
    }

    OrganizationalUnit Area {
      get;
    }

    Currency Currency {
      get;
    }

    string StandardAccount {
      get;
    }

    decimal CurrentBalance {
      get;
    }

    int InvestmentTerm {
      get;
    }

    int GracePeriod {
      get;
    }

    int RepaymentTerm {
      get;
    }

    DateTime RepaymentDate {
      get;
    }

    decimal InterestRate {
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

  }  // interface ICreditAccountData

} // namespace Empiria.Financial.Adapters
