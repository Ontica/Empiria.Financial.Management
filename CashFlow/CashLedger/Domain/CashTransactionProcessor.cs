/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Service provider                        *
*  Type     : CashTransactionProcessor                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Process a cash transaction to determine their matching cash accounts.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;
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
                                      FixedList<CashEntryDto> entries) {
      _helper = new CashTransactionProcessorHelper(transaction, entries);
    }

    #endregion Constructors and parsers

    #region Methods

    internal FixedList<CashEntryFields> Execute() {

      ProcessNoCashFlowEntriesOneToOne();

      ProcessNoCashFlowEntriesOneToOneTwoWay();

      ProcessNoCashFlowCreditOrDebitEntries();

      // ProcessNoCashFlowEntriesWithCreditsAndDebitsAdded();

      // ProcessNoCashFlowLonelyEntries();

      // ProcessEqualEntriesAsNoCashFlowEntriesAdded();

      ProcessNoCashFlowDebitOrCreditEntriesRARE();

      ProcessNoCashFlowAccounts();

      ProcessEqualEntriesAsNoCashFlowEntries();

      ProcessNoCashFlowEntriesSwapedFromCashFlowRules();

      ProcessCashFlowEntriesOneToOne();

      ProcessCashFlowDirectEntries();

      ProcessRemainingEntries();

      return _helper.GetProcessedEntries();
    }

    #endregion Methods

    #region Processors

    private void ProcessCashFlowDirectEntries() {
      FixedList<CashEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_ACCOUNTS")
                                              .FindAll(x => x.DebitConcept.Length != 0 ||
                                                            x.CreditConcept.Length != 0);

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        if (applicableRules.Count == 1) {
          _helper.AddCashFlowEntry(applicableRules[0], entry,
            "Regla con flujo directo");
        }
      }
    }


    private void ProcessCashFlowEntriesOneToOne() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (debitEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_ONE_TO_ONE");

      foreach (var debitEntry in debitEntries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry == null) {
            continue;
          }

          _helper.AddCashFlowEntry(rule, debitEntry,
            "Regla con flujo uno a uno");
          _helper.AddCashFlowEntry(rule, creditEntry,
            "Regla con flujo uno a uno");

        } // foreach rule

      } // foreach debitEntry
    }


    private void ProcessCashFlowEntriesTwoWay() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (debitEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_ONE_TO_ONE");

      foreach (var debitEntry in debitEntries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry == null) {
            continue;
          }

          _helper.AddCashFlowEntry(rule, debitEntry,
            "Regla con flujo uno a uno");
          _helper.AddCashFlowEntry(rule, creditEntry,
            "Regla con flujo uno a uno");

        } // foreach rule

      } // foreach debitEntry
    }


    private void ProcessNoCashFlowEntriesSwapedFromCashFlowRules() {
      FixedList<CashEntryDto> creditEntries = _helper.GetUnprocessedEntries(x => x.Credit != 0);

      if (creditEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_ONE_TO_ONE");

      foreach (var creditEntry in creditEntries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, creditEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto debitEntry = _helper.TryGetMatchingEntry(rule, creditEntry, true);

          if (debitEntry == null) {
            continue;
          }

          if (_helper.HaveSameCashAccount(rule, debitEntry, creditEntry)) {

            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo conceptos iguales");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo conceptos iguales");

          }

        } // foreach rule

      } // foreach creditEntry
    }


    private void ProcessEqualEntriesAsNoCashFlowEntries() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit > 0);

      if (debitEntries.Count == 0) {
        return;
      }

      foreach (var debitEntry in debitEntries) {

        CashEntryDto creditEntry = _helper.TryGetMatchingEntry(debitEntry);

        if (creditEntry != null) {
          _helper.AddProcessedEntry(FinancialRule.Empty, debitEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo y abono iguales");
          _helper.AddProcessedEntry(FinancialRule.Empty, creditEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo y abono iguales");
        }
      }
    }


    private void ProcessNoCashFlowAccounts() {
      FixedList<CashEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_ACCOUNTS");

      foreach (var entry in entries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        if (applicableRules.Count == 0) {
          _helper.AddProcessedEntry(FinancialRule.Empty, entry, CashAccountStatus.NoCashFlow,
            "La cuenta contable no maneja operaciones con flujo");
        }
      }
    }

    private void ProcessNoCashFlowCreditOrDebitEntries() {
      FixedList<CashEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_DEBIT_OR_CREDIT");

      foreach (var entry in entries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        if (applicableRules.Count != 0) {
          _helper.AddProcessedEntry(applicableRules[0], entry, CashAccountStatus.NoCashFlow,
            $"Regla sin flujo directa cargo o abono");
        }
      }
    }


    private void ProcessNoCashFlowEntriesOneToOne() {
      FixedList<CashEntryDto> entries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_ONE_TO_ONE");

      foreach (var debitEntry in entries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry != null) {
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo uno a uno");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo uno a uno");
          }
        }
      }
    }


    private void ProcessNoCashFlowEntriesOneToOneTwoWay() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit > 0);

      if (debitEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_TWO_WAY");

      foreach (var debitEntry in debitEntries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry != null) {
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo de ida y vuelta - ida");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo de ida y vuelta - ida");
          }
        }
      }


      foreach (var creditEntry in _helper.GetUnprocessedEntries(x => x.Credit > 0)) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, creditEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto debitEntry = _helper.TryGetMatchingEntry(rule, creditEntry, true);

          if (debitEntry != null) {
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo de ida y vuelta - vuelta");
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla sin flujo de ida y vuelta - vuelta");
          }
        }
      }
    }


    private void ProcessRemainingEntries() {
      FixedList<CashEntryDto> unprocessed = _helper.GetUnprocessedEntries();

      foreach (var entry in unprocessed) {
        _helper.AddProcessedEntry(FinancialRule.Empty, entry, CashAccountStatus.Pending,
          "Regla dejar pendientes las sobrantes");
      }
    }


    #endregion Processors

    #region Unused processors

    private void ProcessNoCashFlowDebitOrCreditEntriesRARE() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (debitEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_DEBIT_OR_CREDIT");

      foreach (var debitEntry in debitEntries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry == null) {
            continue;
          }

          _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
            "Regla RARA sin flujo de ida y vuelta - ida");
          _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
            "Regla RARA sin flujo de ida y vuelta - ida");

        } // foreach rule

      }  // foreach debitEntry


      FixedList<CashEntryDto> creditEntries = _helper.GetUnprocessedEntries(x => x.Credit != 0);

      foreach (var creditEntry in creditEntries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, creditEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto debitEntry = _helper.TryGetMatchingEntry(rule, creditEntry, true);

          if (debitEntry == null) {
            continue;
          }

          if (_helper.HaveSameCashAccount(rule, debitEntry, creditEntry)) {
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla RARA anulación cargo y abono misma cuenta de flujo - vuelta");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla RARA anulación cargo y abono misma cuenta de flujo - vuelta");
          } else {
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla RARA sin flujo de ida y vuelta - vuelta");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla RARA sin flujo de ida y vuelta - vuelta");
          }

        } // foreach rule

      }  // foreach debitEntry

    }


    private void ProcessEqualEntriesAsNoCashFlowEntriesAdded() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (debitEntries.Count == 0) {
        return;
      }

      foreach (var debitGroup in debitEntries.GroupBy(x => $"{x.AccountNumber}|{x.CurrencyId}|{x.SectorCode}|{x.SubledgerAccountNumber}")) {

        CashEntryDto pivotEntry = debitGroup.First();

        FixedList<CashEntryDto> creditEntries = _helper.GetUnprocessedEntries(x => x.Credit != 0 &&
                                                                                              x.CurrencyId == pivotEntry.CurrencyId &&
                                                                                              x.AccountNumber == pivotEntry.AccountNumber &&
                                                                                              x.SectorCode == pivotEntry.SectorCode &&
                                                                                              x.SubledgerAccountNumber == pivotEntry.SubledgerAccountNumber);
        if (debitGroup.Sum(x => x.Debit) != creditEntries.Sum(x => x.Credit)) {
          continue;
        }

        foreach (var debitEntry in debitGroup) {
          _helper.AddProcessedEntry(FinancialRule.Empty, debitEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo/abono sumadas");
        }

        foreach (var creditEntry in creditEntries) {
          _helper.AddProcessedEntry(FinancialRule.Empty, creditEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo/abono sumadas");
        }
      }
    }


    private void ProcessNoCashFlowEntriesWithCreditsAndDebitsAdded() {

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_CREDIT_DEBIT");

      FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules,
                                                                            _helper.GetUnprocessedEntries(x => x.Debit != 0));

      foreach (var rule in applicableRules) {
        FixedList<CashEntryDto> debitEntries = _helper.GetEntriesSatisfyingRule(rule,
                                                                                          _helper.GetUnprocessedEntries(x => x.Debit != 0));
        FixedList<CashEntryDto> creditEntries = _helper.GetEntriesSatisfyingRule(rule,
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
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo sumadas");
          }

          foreach (var creditEntry in creditEntriesByCurrency) {
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo sumadas");
          }
        }
      }
    }


    private void ProcessNoCashFlowLonelyEntries() {
      FixedList<CashEntryDto> unprocessed = _helper.GetUnprocessedEntries();

      if (unprocessed.Count == 0) {
        return;
      }

      decimal debits = unprocessed.Sum(x => x.Debit);
      decimal credits = unprocessed.Sum(x => x.Credit);

      if (debits != 0 && credits != 0) {
        return;
      }

      foreach (var entry in unprocessed) {
        _helper.AddProcessedEntry(FinancialRule.Empty, entry, CashAccountStatus.NoCashFlow,
          $"Regla sin flujo (solo cargos o abonos restantes)");
      }
    }

    #endregion Unused processors

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
