/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Service provider                        *
*  Type     : CashTransactionAnalyzer                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Analyze a cash transaction to determine the status of its entries.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger {

  /// <summary>Analyze a cash transaction to determine the status of its entries.</summary>
  internal class CashTransactionAnalyzer {

    private readonly FixedList<CashTransactionEntryDto> _entries;

    public CashTransactionAnalyzer(FixedList<CashTransactionEntryDto> entries) {
      Assertion.Require(entries, nameof(entries));

      _entries = entries;
    }


    #region Methods

    internal FixedList<CashTransactionAnalysisEntry> Execute() {
      var entries = new List<CashTransactionAnalysisEntry>(32);

      FixedList<CashTransactionAnalysisEntry> temp =
            AnalyzeEntries(x => x.NoCashFlow,
                           CashAccountStatus.NoCashFlow.Name());
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.CashFlowAssigned,
                            "Con flujo asignado");
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.CashFlowUnassigned,
                           CashAccountStatus.CashFlowUnassigned.Name());
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.Pending, "Pendientes");
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.Id != 0, "Totales");
      entries.AddRange(temp);

      return entries.ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private FixedList<CashTransactionAnalysisEntry> AnalyzeEntries(Func<CashTransactionEntryDto, bool> predicate,
                                                                   string label) {
      var selectedEntries = _entries.FindAll(x => predicate.Invoke(x));

      if (selectedEntries.Count == 0) {
        return new FixedList<CashTransactionAnalysisEntry>();
      }

      var analyzed = new List<CashTransactionAnalysisEntry>(8);

      var currencyGroups = selectedEntries.GroupBy(x => x.CurrencyId).OrderBy(x => x.Key);

      foreach (var currencyGroup in currencyGroups) {
        AddTotalsEntry(analyzed, label, currencyGroup.ToFixedList());
      }

      return analyzed.ToFixedList();
    }


    private void AddTotalsEntry(List<CashTransactionAnalysisEntry> list, string label,
                                FixedList<CashTransactionEntryDto> entries) {

      var totalsEntry = new CashTransactionAnalysisEntry {
        EntryLabel = label,
        Currency = entries[0].CurrencyName,
        TotalEntries = entries.Count(),
        Debits = entries.Sum(x => x.Debit),
        Credits = entries.Sum(x => x.Credit),
      };

      list.Add(totalsEntry);
    }

    #endregion Helpers

  }  // class CashTransactionAnalyzer

}  // namespace Empiria.CashFlow.CashLedger
