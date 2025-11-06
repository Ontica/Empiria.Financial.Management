/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Report Builder                          *
*  Type     : CashFlowTotalsReport                      License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Builds a cash flow totals report according to a cash flow accounts tree.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer.UseCases {

  /// <summary>Builds a cash flow totals report according to a cash flow accounts tree.</summary>
  internal class CashFlowTotalsReport {

    private CashFlowExplorerQuery _query;
    private FixedList<CashFlowExplorerEntry> _sourceEntries;

    public CashFlowTotalsReport(CashFlowExplorerQuery query,
                                FixedList<CashFlowExplorerEntry> sourceEntries) {
      _query = query;
      _sourceEntries = sourceEntries;
    }

    internal void Build() {

    }

    internal DynamicDto<CashFlowExplorerEntry> ToDynamicDto() {
      return new DynamicDto<CashFlowExplorerEntry>(_query, GetColumns(), _sourceEntries);
    }

    private FixedList<DataTableColumn> GetColumns() {
      return new List<DataTableColumn> {
        new DataTableColumn("account", "Cuenta", "text"),
        new DataTableColumn("subprogram", "Descripción", "text"),
        new DataTableColumn("currencyCode", "Moneda", "text"),
        new DataTableColumn("inflows", "Entradas", "decimal"),
        new DataTableColumn("outflows", "Salidas", "decimal"),
      }.ToFixedList();
    }

  }  // class CashFlowTotalsReport

}  // namespace Empiria.CashFlow.Explorer.UseCases
