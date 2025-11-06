/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Service provider                        *
*  Type     : CashFlowExplorer                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Process cash account totals and return them filtered and with complete information.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.CashFlow.CashLedger.Adapters;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Process cash account totals and return them filtered and with complete information.</summary>
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

    internal FixedList<CashFlowExplorerEntry> GetEntries() {
      var entries = ProcessEntries();

      entries = ApplyFilters(entries);

      entries = SortEntries(entries);

      return entries.ToFixedList();
    }


    private List<CashFlowExplorerEntry> ProcessEntries() {
      var entries = new List<CashFlowExplorerEntry>(_totals.Count);

      foreach (var entry in _totals) {
        entries.Add(ProcessTotal(entry));
      }

      return entries;
    }


    private List<CashFlowExplorerEntry> ApplyFilters(List<CashFlowExplorerEntry> entries) {
      var filtered = entries.FindAll(x => x.CashAccountId > 0);

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

      return filtered;
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


    private List<CashFlowExplorerEntry> SortEntries(List<CashFlowExplorerEntry> entries) {
      return new List<CashFlowExplorerEntry>(
                    entries.ToFixedList()
                    .Sort(x => $"{x.StandardAccountNo.PadRight(64)}|{x.OperationType.PadRight(64)}|{x.CashAccountNo.PadRight(16)}|" +
                               $"{x.ConceptDescription.PadRight(300)}|{x.OrganizationalUnit.PadRight(255)}||{x.CurrencyCode}"));
    }

    #endregion Methods

  }  // class CashFlowExplorer

}  // namespace Empiria.CashFlow.Explorer
