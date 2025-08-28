/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Dynamic Columns Output DTO              *
*  Type     : CashFlowExplorerResultDto                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic columns output DTO with information about a cash flow explorer query result.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

namespace Empiria.CashFlow.Explorer.Adapters {

  /// <summary>Dynamic columns output DTO with information about a cash flow explorer query result.</summary>
  public class CashFlowExplorerResultDto {

    public CashFlowExplorerQuery Query {
      get; internal set;
    }

    public FixedList<DataTableColumn> Columns {
      get; internal set;
    }

    public FixedList<CashFlowExplorerEntry> Entries {
      get; internal set;
    }

  }  // class CashFlowExplorerResultDto

}  // namespace Empiria.CashFlow.Explorer.Adapters
