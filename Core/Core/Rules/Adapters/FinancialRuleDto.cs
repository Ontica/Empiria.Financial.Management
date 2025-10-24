/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : FinancialRuleDto                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO for a financial rule.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Rules.Adapters {

  /// <summary>Output DTO for a financial rule.</summary>
  public class FinancialRuleDto {
    public string UID {
      get;
      internal set;
    }
    public string Description {
      get;
      internal set;
    }
    public string DebitAccount {
      get;
      internal set;
    }
    public string CreditAccount {
      get;
      internal set;
    }
  }  // class FinancialRuleDto

}  // namespace Empiria.Financial.Rules.Adapters
