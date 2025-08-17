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

    private readonly CashTransactionDescriptor _transaction;
    private readonly FixedList<CashTransactionEntryDto> _entries;

    #endregion Fields

    #region Constructors and parsers

    internal CashTransactionProcessor(CashTransactionDescriptor transaction,
                                      FixedList<CashTransactionEntryDto> entries) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(entries, nameof(entries));

      _transaction = transaction;
      _entries = entries;
    }

    #endregion Constructors and parsers

    #region Methods

    internal FixedList<CashEntryFields> Execute() {
      var updated = new List<CashEntryFields>(_entries.Count);

      FixedList<CashEntryFields> temp = ProcessNoCashFlowGroupedEntries(true);
      updated.AddRange(temp);

      temp = ProcessNoCashFlowGroupedEntries(false);
      updated.AddRange(temp);

      temp = ProcessNoCashFlowEntries(true);
      updated.AddRange(temp);

      temp = ProcessNoCashFlowEntries(false);
      updated.AddRange(temp);

      temp = ProcessCashFlowEntries();
      updated.AddRange(temp);

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessCashFlowEntries() {
      FixedList<CashTransactionEntryDto> entries = GetUnprocessedEntries();

      var updated = new List<CashEntryFields>(_entries.Count);

      var rules = TempRule.GetList();

      foreach (var entry in entries) {

        FixedList<string> concepts = rules.FindAll(x => entry.AccountNumber == x.CuentaContable)
                                          .SelectDistinct(x => x.Concepto.ToString());

        if (concepts.Count == 0) {
          AddCashEntryFields(updated, entry, 0);
          continue;
        }

        if (concepts.Count == 1) {
          AddCashEntryFields(updated, entry, int.Parse(concepts[0]));
          continue;
        }

        if (concepts.Count > 1 && entry.SubledgerAccountNumber.Length == 0) {
          AddCashEntryFields(updated, entry, -2);
          continue;
        }

        var account = FinancialAccount.TryParseWithSubledgerAccount(entry.SubledgerAccountNumber);

        if (account == null) {
          AddCashEntryFields(updated, entry, -2);
          continue;
        }

        FixedList<string> accountConcepts = account.GetOperations()
                                                   .SelectDistinct(x => x.AccountNo)
                                                   .Intersect(concepts);

        if (accountConcepts.Count == 1) {
          AddCashEntryFields(updated, entry, int.Parse(accountConcepts[0]));
        } else {
          AddCashEntryFields(updated, entry, -2);
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessNoCashFlowEntries(bool matchSubledgerAccounts) {
      FixedList<CashTransactionEntryDto> entries = GetUnprocessedEntries();

      var updated = new List<CashEntryFields>(_entries.Count);

      FixedList<FinancialRule> rules = FinancialRuleCategory.ParseNamedKey("NO_CASH_FLOW_UNGROUPED")
                                                            .GetFinancialRules(_transaction.AccountingDate);

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = GetApplicableRules(rules, entry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto matchingEntry = TryGetMatchingEntry(rule, entry, matchSubledgerAccounts);

          if (matchingEntry != null) {
            AddCashEntryFields(updated, entry, -1);
            AddCashEntryFields(updated, matchingEntry, -1);
          }
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessNoCashFlowGroupedEntries(bool matchSubledgerAccounts) {
      FixedList<CashTransactionEntryDto> entries = GetUnprocessedEntries();

      var updated = new List<CashEntryFields>(_entries.Count);

      FixedList<FinancialRule> rules = FinancialRuleCategory.ParseNamedKey("NO_CASH_FLOW_GROUPED")
                                                            .GetFinancialRules(_transaction.AccountingDate);

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = GetApplicableRules(rules, entry);

        foreach (var groupedRule in applicableRules.GroupBy(x => x.GroupId)) {
          if (groupedRule.Count() > 1) {
            continue;
          }

          CashTransactionEntryDto matchingEntry = TryGetMatchingEntry(groupedRule.First(), entry, matchSubledgerAccounts);

          if (matchingEntry != null) {
            AddCashEntryFields(updated, entry, -1);
            AddCashEntryFields(updated, matchingEntry, -1);
          }
        }
      }

      return updated.ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private void AddCashEntryFields(List<CashEntryFields> list, CashTransactionEntryDto entry, int cashAccountId) {
      entry.Processed = true;

      if (entry.CashAccountId == cashAccountId) {
        return;
      }

      entry.CashAccountId = cashAccountId;

      var fields = new CashEntryFields {
        EntryId = entry.Id,
        CashAccountId = cashAccountId,
        TransactionId = _transaction.Id,
      };

      list.Add(fields);
    }

    private FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return rules.FindAll(x => MatchesAccountNumber(entry.AccountNumber, x.DebitAccount) &&
                                  (x.DebitCurrency.IsEmptyInstance || x.DebitCurrency.Id == entry.CurrencyId));
      } else {
        return rules.FindAll(x => MatchesAccountNumber(entry.AccountNumber, x.CreditAccount) &&
                                  (x.CreditCurrency.IsEmptyInstance || x.CreditCurrency.Id == entry.CurrencyId));
      }
    }


    private FixedList<CashTransactionEntryDto> GetUnprocessedEntries() {
      return _entries.FindAll(x => !x.Processed);
    }


    private bool MatchesAccountNumber(string accountNumber, string accountRuleNumber) {

      if (string.IsNullOrWhiteSpace(accountRuleNumber)) {
        return true;
      }

      bool isPatternRule = accountRuleNumber.Contains("*") && accountRuleNumber.EndsWith("]");

      if (isPatternRule) {
        string startsWith = accountRuleNumber.Split('*')[0];
        string endsWith = EmpiriaString.TrimAll(accountRuleNumber.Split('*')[1], "]", string.Empty);

        return accountNumber.StartsWith(startsWith) && accountNumber.EndsWith(endsWith);
      } else {
        return accountNumber.StartsWith(accountRuleNumber);
      }
    }


    private CashTransactionEntryDto TryGetMatchingEntry(FinancialRule rule, CashTransactionEntryDto entry,
                                                        bool matchSubledgerAccounts) {
      if (entry.Debit != 0) {
        return _entries.Find(x => !x.Processed &&
                                  x.Credit == entry.Debit &&
                                  MatchesAccountNumber(x.AccountNumber, rule.CreditAccount) &&
                                  (rule.CreditCurrency.IsEmptyInstance || entry.CurrencyId == rule.CreditCurrency.Id) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber, matchSubledgerAccounts) &&
                                  (x.SectorCode == entry.SectorCode || x.SectorCode == "00" || entry.SectorCode == "00"));

      } else {
        return _entries.Find(x => !x.Processed &&
                                  x.Debit == entry.Credit &&
                                  MatchesAccountNumber(x.AccountNumber, rule.DebitAccount) &&
                                  (rule.DebitCurrency.IsEmptyInstance || entry.CurrencyId == rule.DebitCurrency.Id) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber, matchSubledgerAccounts) &&
                                  (x.SectorCode == entry.SectorCode || x.SectorCode == "00" || entry.SectorCode == "00"));
      }
    }


    private bool MatchesSubLedgerAccountNumber(string subledgerAccountNumber1,
                                               string subledgerAccountNumber2,
                                               bool matchSubledgerAccounts) {
      if (matchSubledgerAccounts) {
        return subledgerAccountNumber1 == subledgerAccountNumber2;
      }

      return subledgerAccountNumber1 == subledgerAccountNumber2 ||
            (string.IsNullOrWhiteSpace(subledgerAccountNumber1) && !string.IsNullOrWhiteSpace(subledgerAccountNumber2)) ||
            (!string.IsNullOrWhiteSpace(subledgerAccountNumber1) && string.IsNullOrWhiteSpace(subledgerAccountNumber2));
    }

    #endregion Helpers

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
