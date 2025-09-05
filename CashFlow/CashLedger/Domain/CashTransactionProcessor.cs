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

      ProcessNoCashFlowEntriesOneToOneTwoWay();

      ProcessNoCashFlowEntriesOneToOne();

      // ProcessNoCashFlowEntriesWithCreditsAndDebitsAdded();

      ProcessEqualEntriesAsNoCashFlowEntries();

      // ProcessNoCashFlowLonelyEntries();

      // ProcessEqualEntriesAsNoCashFlowEntriesAdded();

      ProcessCashFlowEntriesOneToOneTwoWay();

      ProcessCashFlowDebitOrCreditEntries();

      ProcessCashFlowDirectEntries();

      ProcessRemainingEntries();

      return _helper.GetProcessedEntries();
    }

    #endregion Methods

    #region Processors

    private void ProcessCashFlowDebitOrCreditEntries() {

      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries(x => x.Debit > 0);

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_CREDIT_OR_DEBIT");


      foreach (var entry in entries) {

        FixedList<string> concepts;

        if (entry.Debit > 0) {
          concepts = rules.FindAll(x => x.DebitAccount == entry.AccountNumber &&
                                        x.DebitConcept.Length != 0)
                          .SelectDistinct(x => x.DebitConcept);
        } else {
          concepts = rules.FindAll(x => x.CreditAccount == entry.AccountNumber &&
                                        x.CreditConcept.Length != 0)
                          .SelectDistinct(x => x.CreditConcept);
        }

        if (concepts.Count == 0) {
          goto CONTINUE;
        }

        foreach (var concept in concepts.FindAll(x => x.Length >= 4)) {

          var accounts = FinancialAccount.GetList(x => x.AccountNo == concept &&
                                                       x.Currency.Id == entry.CurrencyId &&
                                                       x.FinancialAccountType.Equals(FinancialAccountType.OperationAccount));

          if (accounts.Count == 1) {
            _helper.AddProcessedEntry(entry, accounts[0],
              "Regla con flujo en pares con concepto determinado automáticamente");
            goto CONTINUE;
          }
          if (accounts.Count > 1 && entry.SubledgerAccountNumber.Length == 0) {
            _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
              $"Regla con flujo en pares con concepto determinado automáticamente " +
              $"(Se encontraron varias cuentas con el concepto {concepts[0]}. " +
              $"Faltan reglas StdAccount sin auxiliar)");
            goto CONTINUE;
          }
          if (accounts.Count > 1 && entry.SubledgerAccountNumber.Length != 0) {
            foreach (var acct in accounts) {
              if (acct.Parent.SubledgerAccountNo == entry.SubledgerAccountNumber) {
                _helper.AddProcessedEntry(entry, acct,
                  "Regla con flujo en pares con concepto determinado automáticamente");
                goto CONTINUE;
              } else {
                _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
                  $"Regla con flujo en pares con concepto determinado automáticamente " +
                  $"(cuenta {entry.SubledgerAccountNumber} no registrada en PYC)");
                goto CONTINUE;
              }
            }
          }
        }  // foreach concept

        if (entry.SubledgerAccountNumber.Length == 0) {
          _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
            $"Regla con flujo en pares con concepto (Faltan reglas StdAccount sin auxiliar)");
          goto CONTINUE;
        }

        var account = FinancialAccount.TryParseWithSubledgerAccount(entry.SubledgerAccountNumber);

        if (account == null) {
          _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
            $"Regla con flujo en pares con concepto (cuenta {entry.SubledgerAccountNumber} no registrada en PYC)");
          goto CONTINUE;
        }

        FixedList<FinancialAccount> operations = account.GetOperations()
                                                        .FindAll(x => x.Currency.Id == entry.CurrencyId &&
                                                                      x.StandardAccount.StdAcctNo.EndsWith(concepts[0]));

        if (operations.Count == 0) {
          _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
              $"Regla con flujo en pares con concepto " +
              $"(la cuenta {entry.SubledgerAccountNumber} no tiene concepto {concepts[0]})");

        } else if (operations.Count == 1) {
          _helper.AddProcessedEntry(entry, operations[0],
             "Regla con flujo en pares con concepto determinado automáticamente");

        } else {
          _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
             $"Regla con flujo en pares con concepto (la cuenta {entry.SubledgerAccountNumber} " +
             $"tiene más de un concepto de tipo {concepts[0]})");
        }

CONTINUE:
        ;  // no-op

      }  // foreach entry
    }


    private void ProcessCashFlowDirectEntries() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_DIRECT");

      foreach (var entry in entries) {

        FixedList<string> concepts;

        if (entry.Debit > 0) {
          concepts = rules.FindAll(x => x.DebitAccount == entry.AccountNumber &&
                                        x.DebitConcept.Length != 0)
                          .SelectDistinct(x => x.DebitConcept);
        } else {
          concepts = rules.FindAll(x => x.CreditAccount == entry.AccountNumber &&
                                        x.CreditConcept.Length != 0)
                          .SelectDistinct(x => x.CreditConcept);
        }


        if (concepts.Count == 0) {
          continue;
        }

        foreach (var concept in concepts) {
          var accounts = FinancialAccount.GetList(x => x.AccountNo == concept &&
                                                       x.Currency.Id == entry.CurrencyId &&
                                                       x.FinancialAccountType.Equals(FinancialAccountType.OperationAccount));

          if (accounts.Count == 1) {
            _helper.AddProcessedEntry(entry, accounts[0],
              "Regla con flujo directo");
            continue;
          }

          if (concepts.Count > 1) {
            _helper.AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
              "Regla con flujo directo (existen múltiples conceptos)");
            continue;
          }
        }
      }
    }


    private void ProcessCashFlowEntriesOneToOneTwoWay() {
      FixedList<CashTransactionEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (debitEntries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("CASH_FLOW_TWO_WAY");

      foreach (var debitEntry in debitEntries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, debitEntry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto creditEntry = _helper.TryGetMatchingEntry(rule, debitEntry);

          if (creditEntry == null) {
            continue;
          }

          _helper.AddCashFlowEntry(rule, debitEntry, "Regla con flujo uno a uno");
          _helper.AddCashFlowEntry(rule, creditEntry, "Regla con flujo uno a uno");

        }  // foreach rule

      }  // foreach entry
    }


    private void ProcessEqualEntriesAsNoCashFlowEntries() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      foreach (var debitEntry in entries.FindAll(x => x.Debit > 0)) {

        CashTransactionEntryDto matchingEntry = entries.Find(x => x.Credit == debitEntry.Debit &&
                                                                  x.CurrencyId == debitEntry.CurrencyId &&
                                                                  x.AccountNumber == debitEntry.AccountNumber &&
                                                                  x.SectorCode == debitEntry.SectorCode &&
                                                                  x.SubledgerAccountNumber == debitEntry.SubledgerAccountNumber);

        if (matchingEntry != null) {
          _helper.AddProcessedEntry(debitEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo/abono uno a uno");
          _helper.AddProcessedEntry(matchingEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo/abono uno o uno");
        }
      }
    }


    private void ProcessEqualEntriesAsNoCashFlowEntriesAdded() {
      FixedList<CashTransactionEntryDto> debitEntries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

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
          _helper.AddProcessedEntry(debitEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo/abono sumadas");
        }

        foreach (var creditEntry in creditEntries) {
          _helper.AddProcessedEntry(creditEntry, CashAccountStatus.NoCashFlow,
            "Regla anulación cargo/abono sumadas");
        }
      }
    }


    private void ProcessNoCashFlowEntriesOneToOne() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries(x => x.Debit != 0);

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_CREDIT_DEBIT");

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto matchingEntry = _helper.TryGetMatchingEntry(rule, entry);

          if (matchingEntry != null) {
            _helper.AddProcessedEntry(entry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo uno a uno");
            _helper.AddProcessedEntry(matchingEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo uno a uno");
          }
        }
      }
    }


    private void ProcessNoCashFlowEntriesOneToOneTwoWay() {
      FixedList<CashTransactionEntryDto> entries = _helper.GetUnprocessedEntries();

      if (entries.Count == 0) {
        return;
      }

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_TWO_WAY");

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = _helper.GetApplicableRules(rules, entry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto matchingEntry = _helper.TryGetMatchingEntry(rule, entry);

          if (matchingEntry != null) {
            _helper.AddProcessedEntry(entry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo de ida y vuelta");
            _helper.AddProcessedEntry(matchingEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo de ida y vuelta");
          }
        }
      }
    }


    private void ProcessNoCashFlowEntriesWithCreditsAndDebitsAdded() {

      FixedList<FinancialRule> rules = _helper.GetRules("NO_CASH_FLOW_CREDIT_DEBIT");

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
            _helper.AddProcessedEntry(debitEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo sumadas");
          }

          foreach (var creditEntry in creditEntriesByCurrency) {
            _helper.AddProcessedEntry(creditEntry, CashAccountStatus.NoCashFlow,
              $"Regla sin flujo sumadas");
          }
        }
      }
    }


    private void ProcessNoCashFlowLonelyEntries() {
      FixedList<CashTransactionEntryDto> unprocessed = _helper.GetUnprocessedEntries();

      if (unprocessed.Count == 0) {
        return;
      }

      decimal debits = unprocessed.Sum(x => x.Debit);
      decimal credits = unprocessed.Sum(x => x.Credit);

      if (debits != 0 && credits != 0) {
        return;
      }

      foreach (var entry in unprocessed) {
        _helper.AddProcessedEntry(entry, CashAccountStatus.NoCashFlow,
          $"Regla sin flujo (solo cargos o abonos restantes)");
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
          _helper.AddProcessedEntry(entry, CashAccountStatus.Pending,
            "Regla dejar pendientes las sobrantes");
        }
      }
    }

    #endregion Processors

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
