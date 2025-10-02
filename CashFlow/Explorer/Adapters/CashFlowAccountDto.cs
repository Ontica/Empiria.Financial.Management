/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Output DTO                              *
*  Type     : CashFlowAccountDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with cash flow account data.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Explorer.Adapters {

  /// <summary>Output DTO with cash flow account data.</summary>
  public class CashFlowAccountDto {

    public string CashAccountNo {
      get; internal set;
    }

    public string CashAccountName {
      get; internal set;
    }

    public string FinancialAccountName {
      get; internal set;
    }

    public string OperationType {
      get; internal set;
    }

    public string OrgUnitName {
      get; internal set;
    }

    public string CurrencyCode {
      get; internal set;
    }

  }  // class CashFlowAccountDto

}  // namespace Empiria.CashFlow.Explorer.Adapters
