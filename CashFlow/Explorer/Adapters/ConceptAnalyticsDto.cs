/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Output DTO                              *
*  Type     : ConceptAnalyticsDto                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for a cash ledger entry with detailed financial account information.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Explorer.Adapters {

  /// <summary>Output DTO for a cash ledger entry with detailed financial account information.</summary>
  public class ConceptAnalyticsDto : CashEntryExtendedDto {

    public string ConceptDescription {
      get; internal set;
    } = string.Empty;


    public string FinancialAcctOrgUnit {
      get; internal set;
    } = string.Empty;


    public string ProjectType {
      get; internal set;
    } = string.Empty;


    public string FinancialAcctType {
      get; internal set;
    } = string.Empty;


    public string FinancialAcctName {
      get; internal set;
    } = string.Empty;

  }  // class ConceptAnalyticsDto

} // namespace Empiria.CashFlow.Explorer.Adapters
