/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Report Builder                          *
*  Type     : CashFlowConceptsReport                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Builds a cash flow report with detailed concepts.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Builds a cash flow report with detailed concepts.</summary>
  internal class CashFlowConceptsReport {

    private readonly CashFlowExplorerQuery _query;
    private readonly FixedList<CashFlowExplorerEntry> _sourceEntries;

    public CashFlowConceptsReport(CashFlowExplorerQuery query,
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
        new DataTableColumn("cashFlowAcct_01", "Clasificación 1", "text"),
        new DataTableColumn("cashFlowAcct_02", "Clasificación 2", "text"),
        new DataTableColumn("cashFlowAcct_03", "Clasificación 3", "text"),
        new DataTableColumn("cashFlowAcct_04", "Clasificación 4", "text"),
        new DataTableColumn("cashFlowAcct_05", "Clasificación 5", "text"),
        new DataTableColumn("cashFlowAcct_06", "Clasificación 6", "text"),
        new DataTableColumn("program", "Programa", "text"),
        new DataTableColumn("subprogram", "Subprograma", "text"),
        new DataTableColumn("financingSource", "Fuente", "text"),
        new DataTableColumn("operationType", "Operación", "text"),
        new DataTableColumn("cashAccountNo", "Concepto", "text"),
        new DataTableColumn("conceptDescription", "Descripción", "text"),
        new DataTableColumn("organizationalUnit", "Área", "text"),
        new DataTableColumn("currencyCode", "Moneda", "text"),
        new DataTableColumn("inflows", "Entradas", "decimal"),
        new DataTableColumn("outflows", "Salidas", "decimal"),
        new DataTableColumn("total", "Total", "decimal"),
        new DataTableColumn("totalMXN", "Total M.N", "decimal"),
      }.ToFixedList();
    }

  }  // class CashFlowConceptsReport

}  // namespace Empiria.CashFlow.Explorer
