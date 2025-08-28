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

using Empiria.Financial;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer {

  internal class CashFlowExplorer {

    private CashFlowExplorerQuery _query;
    private FixedList<CashLedgerTotalEntryDto> _totals;

    internal CashFlowExplorer(CashFlowExplorerQuery query, FixedList<CashLedgerTotalEntryDto> totals) {
      Assertion.Require(query, nameof(query));
      Assertion.Require(totals, nameof(totals));

      _query = query;
      _totals = totals;
    }

    #region Methods

    internal CashFlowExplorerResult Execute() {
      return new CashFlowExplorerResult {
        Query = _query,
        Columns = GetDynamicColumns(),
        Entries = ProcessEntries()
      };
    }


    private FixedList<DataTableColumn> GetDynamicColumns() {
      var columns = new List<DataTableColumn> {
        new DataTableColumn("cashAccountNo", "Concepto presupuestal", "text"),
        new DataTableColumn("conceptDescription", "Descripción", "text"),
        new DataTableColumn("program", "Programa", "text"),
        new DataTableColumn("subprogram", "Subprograma", "text"),
        new DataTableColumn("financingSource", "Fuente", "text"),
        new DataTableColumn("operationType", "Operación", "text"),
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

      return entries.ToFixedList();
    }


    private CashFlowExplorerEntry ProcessTotal(CashLedgerTotalEntryDto total) {
      var entry = new CashFlowExplorerEntry {
        CashAccountId = total.CashAccountId,
        CashAccountNo = total.CashAccountNo,
        CurrencyCode = Currency.Parse(total.CurrencyId).ISOCode,
      };

      entry.Sum(total);

      return entry;
    }

    #endregion Methods

  }  // class CashFlowExplorer

}  // namespace Empiria.CashFlow.Explorer
