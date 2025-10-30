/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : FinancialRuleDto                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO for a financial rule.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Rules.Adapters {

  /// <summary>Output DTO for a financial rule.</summary>
  public class FinancialRuleDto {

    public string UID {
      get; internal set;
    }

    public string DebitAccount {
      get; internal set;
    }

    public string CreditAccount {
      get; internal set;
    }

    public string DebitConcept {
      get; internal set;
    }

    public string CreditConcept {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class FinancialRuleDto



  /// <summary>Output DTO for a financial rule for use in lists.</summary>
  public class FinancialRuleDescriptor {

    public string UID {
      get; internal set;
    }

    public string DebitAccount {
      get; internal set;
    }

    public string CreditAccount {
      get; internal set;
    }

    public string DebitConcept {
      get; internal set;
    }

    public string CreditConcept {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class FinancialRuleDescriptor

}  // namespace Empiria.Financial.Rules.Adapters
