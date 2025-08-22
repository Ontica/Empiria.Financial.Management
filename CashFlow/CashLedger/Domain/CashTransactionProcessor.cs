/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Service provider                        *
*  Type     : CashTransactionProcessor                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Process a cash transaction to determine their matching cash accounts.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;
using Empiria.Financial.Rules;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger {

  /// <summary>Process a cash transaction to determine their matching cash accounts.</summary>
  public class CashTransactionProcessor {

    #region Fields

    private readonly CashTransactionProcessorHelper _helper;

    #endregion Fields

    #region Constructors and parsers

    internal CashTransactionProcessor(CashTransactionDescriptor transaction,
                                      FixedList<CashTransactionEntryDto> entries) {
      _helper = new CashTransactionProcessorHelper(transaction, entries);
    }

    #endregion Constructors and parsers

    #region Methods

    internal FixedList<CashEntryFields> Execute() {

      //ProcessNoCashFlowGroupedEntries();

      ProcessNoCashFlowEntriesOneToOne();

      ProcessNoCashFlowEntriesWithCreditsOrDebitsAdded();

      ProcessSameEntries();

      ProcessCashFlowDirectEntries();

      ProcessCashFlowCreditOrDirectEntries();

      ProcessRemainingEntries();

      return _helper.GetProcessedEntries();
    }

    #endregion Methods

    #region Processors

    private void ProcessCashFlowCreditOrDirectEntries() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_CREDIT_OR_DEBIT");


      foreach (var entry in entries) {

        FixedList<string> concepts = rules.FindAll(x => ((entry.Debit > 0 && x.DebitAccount == entry.AccountNumber) ||
                                                         ((entry.Credit > 0 && x.CreditAccount == entry.AccountNumber)) &&
                                                         x.ConceptAccount.Length != 0))
                                          .SelectDistinct(x => x.ConceptAccount.ToString());

        if (concepts.Count == 0) {
          continue;
        }

        if (concepts.Count == 1 && concepts[0].Length >= 4) {
          _helper.AddProcessedEntry(entry, int.Parse(concepts[0]));
          continue;
        }

        if (concepts.Count > 1 && entry.SubledgerAccountNumber.Length == 0) {
          _helper.AddProcessedEntry(entry, -2);
          continue;
        }

        var account = FinancialAccount.TryParseWithSubledgerAccount(entry.SubledgerAccountNumber);

        if (account == null) {
          _helper.AddProcessedEntry(entry, -2);
          continue;
        }

        FixedList<FinancialAccount> operations = account.GetOperations()
                                                        .FindAll(x => x.Currency.Id == entry.CurrencyId &&
                                                                      EmpiriaString.StartsWith(x.AccountNo, concepts[0]));

        if (operations.Count == 0) {
          _helper.AddProcessedEntry(entry, -2);

        } else if (operations.Count == 1) {
          _helper.AddProcessedEntry(entry, int.Parse(operations[0].AccountNo));

        } else {
          _helper.AddProcessedEntry(entry, -2);
        }
      }
    }


    private void ProcessCashFlowDirectEntries() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_DIRECT");

      foreach (var entry in entries) {

        FixedList<string> concepts = rules.FindAll(x => x.DebitAccount == entry.AccountNumber &&
                                                        x.ConceptAccount.Length != 0)
                                          .SelectDistinct(x => x.ConceptAccount.ToString());

        if (concepts.Count == 0) {
          continue;
        }

        if (concepts.Count == 1) {
          _helper.AddProcessedEntry(entry, int.Parse(concepts[0]));
          continue;
        }

        if (concepts.Count > 1) {
          _helper.AddProcessedEntry(entry, -2);
          continue;
        }
      }
    }


    private void ProcessNoCashFlowEntriesOneToOne() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_UNGROUPED");

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto matchingEntry = _helper.TryGetMatchingEntry(rule, entry);

          if (matchingEntry != null) {
            _helper.AddProcessedEntry(entry, -1);
            _helper.AddProcessedEntry(matchingEntry, -1);
          }
        }
      }
    }


    private void ProcessNoCashFlowEntriesWithCreditsOrDebitsAdded() {

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_UNGROUPED");

      FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules,
                                                                            _helper.GetUnprocessedEntries(x => x.Debit != 0));

      foreach (var rule in applicableRules) {
        FixedList<CashTransactionEntryDto> debitEntries = _helper.GetEntriesSatisfyingRule(rule,
                                                                                          _helper.GetUnprocessedEntries(x => x.Debit != 0));
        FixedList<CashTransactionEntryDto> creditEntries = _helper.GetEntriesSatisfyingRule(rule,
                                                                                           _helper.GetUnprocessedEntries(x => x.Credit != 0));

        var debitEntriesByCurrency = debitEntries.GroupBy(x => x.CurrencyId);

        foreach (var debitCurrencyGroup in debitEntriesByCurrency) {
          var creditEntriesByCurrency = creditEntries.FindAll(x => x.CurrencyId == debitCurrencyGroup.Key);

          decimal debitSum = debitCurrencyGroup.Sum(x => x.Debit);
          decimal creditSum = creditEntriesByCurrency.Sum(x => x.Credit);

          if (debitSum != creditSum) {
            continue;
          }

          foreach (var debitEntry in debitCurrencyGroup) {
            _helper.AddProcessedEntry(debitEntry, -1);
          }

          foreach (var creditEntry in creditEntriesByCurrency) {
            _helper.AddProcessedEntry(creditEntry, -1);
          }
        }
      }
    }


    private void ProcessNoCashFlowGroupedEntries() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries();

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_GROUPED");

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        foreach (var groupedRule in applicableRules.GroupBy(x => x.GroupId)) {
          if (groupedRule.Count() > 1) {
            continue;
          }

          CashTransactionEntryDto matchingEntry = _helper.TryGetMatchingEntry(groupedRule.First(), entry);

          if (matchingEntry != null) {
            _helper.AddProcessedEntry(entry, -1);
            _helper.AddProcessedEntry(matchingEntry, -1);
          }
        }
      }
    }


    private void ProcessRemainingEntries() {
      FixedList<CashEntryFields> processed = _helper.GetProcessedEntries();
      FixedList<CashTransactionEntryDto> unprocessed = _helper.GetUnprocessedEntries();

      foreach (var entry in unprocessed) {
        if (processed.Exists(x => x.EntryId == entry.Id)) {
          continue;
        }
        if (entry.CashAccountId != 0) {
          _helper.AddProcessedEntry(entry, 0);
        }
      }
    }


    private void ProcessSameEntries() {
      FixedList<CashTransactionEntryDto> debitEntries = _helper.GetUnprocessedEntries();

      var updated = new List<CashEntryFields>(debitEntries.Count);

      if (debitEntries.SelectDistinct(x => x.AccountNumber).Count == 1) {
        foreach (var entry in debitEntries) {
          _helper.AddProcessedEntry(entry, -1);
        }
        return;
      }

      debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0 && !x.AccountNumber.StartsWith("9"));

      if (debitEntries.Count == 0) {
        return;
      }

      foreach (var debitGroup in debitEntries.GroupBy(x => $"{x.AccountNumber}|{x.CurrencyId}|{x.SectorCode}|{x.SubledgerAccountNumber}")) {

        CashTransactionEntryDto pivotEntry = debitGroup.First();

        FixedList<CashTransactionEntryDto> creditEntries = _helper.GetUnprocessedEntries(x => x.Credit != 0 &&
                                                                                              x.CurrencyId == pivotEntry.CurrencyId &&
                                                                                              x.AccountNumber == pivotEntry.AccountNumber &&
                                                                                              x.SectorCode == pivotEntry.SectorCode &&
                                                                                              x.SubledgerAccountNumber == pivotEntry.SubledgerAccountNumber);
        if (debitGroup.Sum(x => x.Debit) != creditEntries.Sum(x => x.Credit)) {
          continue;
        }

        foreach (var debitEntry in debitGroup) {
          _helper.AddProcessedEntry(debitEntry, -1);
        }

        foreach (var creditEntry in creditEntries) {
          _helper.AddProcessedEntry(creditEntry, -1);
        }
      }
    }

    #endregion Processors

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
