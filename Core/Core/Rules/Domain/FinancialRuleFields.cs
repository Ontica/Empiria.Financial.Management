/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields DTO                      *
*  Type     : FinancialRuleFields                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields used to update and append financial rules.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Rules {

  public class FinancialRuleFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string DebitAccount {
      get; set;
    } = string.Empty;


    public string CreditAccount {
      get; set;
    } = string.Empty;


    public string DebitConcept {
      get; set;
    } = string.Empty;


    public string CreditConcept {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public DateTime StartDate {
      get; set;
    } = DateTime.Today;


    public DateTime EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;

  }  // class FinancialRuleFields

}  // namespace Empiria.Financial.Rules
