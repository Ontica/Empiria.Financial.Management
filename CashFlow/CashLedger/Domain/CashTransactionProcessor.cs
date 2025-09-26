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

      ProcessNoCashFlowDebitOrCreditEntries();

      ProcessNoCashFlowDebitOrCreditEntriesRule2();

      ProcessNoCashFlowGroupedEntries();

      ProcessNoCashFlowEntriesSwapedFromCashFlowRules();

      ProcessNoCashFlowAccounts();

      ProcessEqualEntriesAsNoCashFlowEntries();

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


    private void ProcessNoCashFlowDebitOrCreditEntries() {
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


    private void ProcessNoCashFlowDebitOrCreditEntriesRule2() {
      FixedList<CashEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (debitEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_DEBIT_OR_CREDIT_RULE_2");

      foreach (var debitEntry in debitEntries) {

        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry == null) {
            continue;
          }

          _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
            "Regla dos sin flujo de ida y vuelta - ida");
          _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
            "Regla dos sin flujo de ida y vuelta - ida");

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
              "Regla dos anulación cargo y abono misma cuenta de flujo - vuelta");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla dos anulación cargo y abono misma cuenta de flujo - vuelta");
          } else {
            _helper.AddProcessedEntry(rule, debitEntry, CashAccountStatus.NoCashFlow,
              "Regla dos sin flujo de ida y vuelta - vuelta");
            _helper.AddProcessedEntry(rule, creditEntry, CashAccountStatus.NoCashFlow,
              "Regla dos sin flujo de ida y vuelta - vuelta");
          }

        } // foreach rule

      }  // foreach creditEntry
    }


    private void ProcessNoCashFlowGroupedEntries() {
      var entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      var groupedRules = _helper.GetApplicableRules(_helper.GetRules("NO_CASH_FLOW_GROUPED"), entries)
                                .GroupBy(x => x.GroupId);

      foreach (var group in groupedRules) {

        FixedList<FinancialRule> debitRules = _helper.GetApplicableRules(group.ToFixedList(),
                                                                         entries.FindAll(x => x.Debit != 0));

        FixedList<FinancialRule> creditRules = _helper.GetApplicableRules(group.ToFixedList(),
                                                                          entries.FindAll(x => x.Credit != 0));


        FixedList<int> currencies = entries.SelectDistinct(x => x.CurrencyId);


        foreach (int currencyId in currencies) {

          FixedList<CashEntryDto> debitEntries =
                      _helper.GetEntriesSatisfyingRules(debitRules, entries.FindAll(x => x.Debit != 0 &&
                                                                                         x.CurrencyId == currencyId));

          FixedList<CashEntryDto> creditEntries =
                      _helper.GetEntriesSatisfyingRules(creditRules, entries.FindAll(x => x.Credit != 0 &&
                                                                                          x.CurrencyId == currencyId));

          decimal debitSum = debitEntries.Sum(x => x.Debit);
          decimal creditSum = creditEntries.Sum(x => x.Credit);

          if (debitSum != creditSum) {
            continue;
          }

          foreach (var debitEntry in debitEntries) {
            _helper.AddProcessedEntry(FinancialRule.Empty, debitEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo agrupada");
          }

          foreach (var creditEntry in creditEntries) {
            _helper.AddProcessedEntry(FinancialRule.Empty, creditEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo agrupada");
          }

          entries = _helper.GetUnprocessedEntries();

        }   // foreach currencyId

      }  // foreach group

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

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
