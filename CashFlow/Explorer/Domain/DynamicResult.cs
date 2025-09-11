/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Information Holder                      *
*  Type     : CashFlowExplorerResult                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds the dynamic result of a cash flow explorer execution.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Holds the dynamic result of a cash flow explorer execution.</summary>
  internal class DynamicResult<T> {

    public CashFlowExplorerQuery Query {
      get; internal set;
    }

    public FixedList<DataTableColumn> Columns {
      get; internal set;
    }

    public FixedList<T> Entries {
      get; internal set;
    }

  }  // class DynamicResult

}  // namespace Empiria.CashFlow.Explorer
