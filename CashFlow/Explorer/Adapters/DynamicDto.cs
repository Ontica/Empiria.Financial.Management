/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Dynamic Columns Output DTO              *
*  Type     : DynamicDto                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dynamic columns output DTO with variable entries information related to a query result.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

namespace Empiria.CashFlow.Explorer.Adapters {

  /// <summary>Dynamic columns output DTO with variable entries information related to a query result.</summary>
  public class DynamicDto<T> {

    public CashFlowExplorerQuery Query {
      get; internal set;
    }

    public FixedList<DataTableColumn> Columns {
      get; internal set;
    }

    public FixedList<T> Entries {
      get; internal set;
    }

  }  // class DynamicDto

}  // namespace Empiria.CashFlow.Explorer.Adapters
