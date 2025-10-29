/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Use case interactor class               *
*  Type     : CashFlowExplorerUseCases                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cash flow explorer information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.CashFlow.CashLedger.Adapters;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer {

  internal class CashFlowExplorer {

    private CashFlowExplorerQuery _query;
    private FixedList<CashAccountTotalDto> _totals;

    internal CashFlowExplorer(CashFlowExplorerQuery query, FixedList<CashAccountTotalDto> totals) {
      Assertion.Require(query, nameof(query));
      Assertion.Require(totals, nameof(totals));

      _query = query;
      _totals = totals;
    }

    #region Methods

    internal DynamicDto<CashFlowExplorerEntry> Execute() {
      return new DynamicDto<CashFlowExplorerEntry>(
        _query,
        GetDynamicColumns(),
        ProcessEntries()
      );
    }


    private FixedList<DataTableColumn> GetDynamicColumns() {
      var columns = new List<DataTableColumn> {
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
      };

      return columns.ToFixedList();
    }


    private FixedList<CashFlowExplorerEntry> ProcessEntries() {
      var entries = new List<CashFlowExplorerEntry>(_totals.Count);

      foreach (var entry in _totals) {
        entries.Add(ProcessTotal(entry));
      }

      var filteredEntries = ApplyFilters(entries);

      return filteredEntries.Sort(x => $"{x.StandardAccountNo.PadRight(64)}|{x.OperationType.PadRight(64)}|{x.CashAccountNo.PadRight(16)}|" +
                                      $"{x.ConceptDescription.PadRight(300)}|{x.OrganizationalUnit.PadRight(255)}||{x.CurrencyCode}");
    }


    private FixedList<CashFlowExplorerEntry> ApplyFilters(List<CashFlowExplorerEntry> entries) {
      var filtered = entries.ToFixedList()
                            .FindAll(x => x.CashAccountId > 0);

      if (_query.OperationTypeUID.Length != 0) {
        filtered = filtered.FindAll(x => x.CashAccount.OperationType.UID == _query.OperationTypeUID);
      }

      if (_query.PartyUID.Length != 0) {
        filtered = filtered.FindAll(x => x.CashAccount.OrganizationalUnit.UID == _query.PartyUID);
      }

      if (EmpiriaString.BuildKeywords(_query.Keywords).Length != 0) {
        var queryKeywords = EmpiriaString.BuildKeywords(_query.Keywords);

        filtered = filtered.FindAll(x => EmpiriaString.ContainsAny(x.CashAccount.Parent.Keywords, queryKeywords));
      }

      return filtered.ToFixedList();
    }


    private CashFlowExplorerEntry ProcessTotal(CashAccountTotalDto total) {
      var entry = new CashFlowExplorerEntry {
        CashAccountId = total.CashAccountId,
        CashAccountNo = total.CashAccountNo,
        CurrencyCode = total.CurrencyCode
      };

      entry.Sum(total);

      return entry;
    }

    #endregion Methods

  }  // class CashFlowExplorer

}  // namespace Empiria.CashFlow.Explorer
