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

      FixedList<CashTransactionAnalysisEntry> temp = AnalyzeEntries(x => x.CashAccountId == 0, "Pendientes");
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.CashAccountId == -1, "Sin flujo");
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.CashAccountId > 0, "Con flujo");
      entries.AddRange(temp);

      temp = AnalyzeEntries(x => x.CashAccountId == 2, "Con flujo pendiente");
      entries.AddRange(temp);

      return entries.ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private FixedList<CashTransactionAnalysisEntry> AnalyzeEntries(Func<CashTransactionEntryDto, bool> predicate,
                                                                   string label) {
      var selectedEntries = _entries.FindAll(x => predicate.Invoke(x));

      var analyzed = new List<CashTransactionAnalysisEntry>(8);

      AddTotalsEntry(analyzed, label, selectedEntries);

      var currencyGroups = selectedEntries.GroupBy(x => x.CurrencyId);

      if (currencyGroups.Count() == 1) {
        return analyzed.ToFixedList();
      }

      foreach (var currencyGroup in currencyGroups) {
        AddTotalsEntry(analyzed, label, currencyGroup.ToFixedList());
      }

      return analyzed.ToFixedList();
    }


    private void AddTotalsEntry(List<CashTransactionAnalysisEntry> list, string label,
                                FixedList<CashTransactionEntryDto> entries) {

      var currencies = entries.SelectDistinct(x => x.CurrencyName);

      var totalsEntry = new CashTransactionAnalysisEntry {
        EntryLabel = label,
        Currency = currencies.Count == 1 ? currencies[0] : "Todas",
        TotalEntries = entries.Count(),
        Debits = entries.Sum(x => x.Debit),
        Credits = entries.Sum(x => x.Credit),
      };
    }

    #endregion Helpers

  }  // class CashTransactionAnalyzer

}  // namespace Empiria.CashFlow.CashLedger
