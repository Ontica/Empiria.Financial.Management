/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input Query DTO                       *
*  Type     : FinancialRuleQuery                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input query used to search financial rules.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Rules.Adapters {

  /// <summary>Input query used to search financial rules.</summary>
  public class FinancialRuleQuery {

    public string CategoryUID {
      get; set;
    } = string.Empty;


    public DateTime Date {
      get; set;
    } = DateTime.Today;


    public string Keywords {
      get; internal set;
    } = string.Empty;

  }  // class FinancialRuleQuery

}  // namespace Empiria.Financial.Rules.Adapters
