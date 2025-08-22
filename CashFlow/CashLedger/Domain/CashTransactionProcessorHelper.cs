/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Service provider                        *
*  Type     : CashTransactionProcessorHelper             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Helping methods used by CashTransactionProcessor.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Financial.Rules;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger {

  /// <summary>Helping methods used by CashTransactionProcessor.</summary>
  internal class CashTransactionProcessorHelper {

    private CashTransactionDescriptor _transaction;
    private FixedList<CashTransactionEntryDto> _entries;
    private List<CashEntryFields> _processedEntries;

    internal CashTransactionProcessorHelper(CashTransactionDescriptor transaction,
                                            FixedList<CashTransactionEntryDto> entries) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(entries, nameof(entries));

      _transaction = transaction;
      _entries = entries;
      _processedEntries = new List<CashEntryFields>(_entries.Count);
    }


    #region Methods

    internal void AddProcessedEntry(CashTransactionEntryDto entry, int cashAccountId) {
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

      _processedEntries.Add(fields);
    }


    internal FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules,
                                                         FixedList<CashTransactionEntryDto> entries) {
      if (rules.Count == 0 || entries.Count == 0) {
        return new FixedList<FinancialRule>();
      }

      var applicableRules = new List<FinancialRule>(rules.Count);

      foreach (var rule in rules) {
        if (entries.Exists(x => SatisfiesRule(rule, x))) {
          applicableRules.Add(rule);
        }
      }

      return applicableRules.ToFixedList();
    }


    internal FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return rules.FindAll(x => MatchesAccountNumber(entry.AccountNumber, x.DebitAccount) &&
                                  (x.DebitCurrency.IsEmptyInstance || x.DebitCurrency.Id == entry.CurrencyId))
                    .Sort((x, y) => x.DebitAccount.CompareTo(y.DebitAccount))
                    .Reverse();
      } else {
        return rules.FindAll(x => MatchesAccountNumber(entry.AccountNumber, x.CreditAccount) &&
                                  (x.CreditCurrency.IsEmptyInstance || x.CreditCurrency.Id == entry.CurrencyId))
                    .Sort((x, y) => x.CreditAccount.CompareTo(y.CreditAccount))
                    .Reverse();
      }
    }


    internal FixedList<CashTransactionEntryDto> GetEntriesSatisfyingRule(FinancialRule rule,
                                                                         FixedList<CashTransactionEntryDto> entries) {
      if (entries.Count == 0) {
        return new FixedList<CashTransactionEntryDto>();
      }

      return entries.FindAll(x => SatisfiesRule(rule, x));
    }


    internal FixedList<CashEntryFields> GetProcessedEntries() {
      return _processedEntries.ToFixedList();
    }


    internal FixedList<FinancialRule> GetRules(string ruleCategory) {
      return FinancialRuleCategory.ParseNamedKey(ruleCategory)
                                  .GetFinancialRules(_transaction.AccountingDate);
    }


    internal FixedList<CashTransactionEntryDto> GetUnprocessedEntries() {
      return _entries.FindAll(x => !x.Processed);
    }


    internal FixedList<CashTransactionEntryDto> GetUnprocessedEntries(System.Func<CashTransactionEntryDto, bool> predicate) {
      return _entries.FindAll(x => !x.Processed && predicate.Invoke(x));
    }


    internal FixedList<CashTransactionEntryDto> TryGetMatchingEntries(FinancialRule rule, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return _entries.FindAll(x => !x.Processed &&
                                      x.Credit > 0 &&
                                      MatchesAccountNumber(x.AccountNumber, rule.CreditAccount) &&
                                      (entry.CurrencyId == x.CurrencyId) &&
                                      MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber) &&
                                      (x.SectorCode == entry.SectorCode || x.SectorCode == "00" || entry.SectorCode == "00"));

      } else {
        return _entries.FindAll(x => !x.Processed &&
                                      x.Debit > 0 &&
                                      MatchesAccountNumber(x.AccountNumber, rule.DebitAccount) &&
                                      (entry.CurrencyId == x.CurrencyId) &&
                                      MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber) &&
                                      (x.SectorCode == entry.SectorCode || x.SectorCode == "00" || entry.SectorCode == "00"));
      }
    }


    internal CashTransactionEntryDto TryGetMatchingEntry(FinancialRule rule, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return _entries.Find(x => !x.Processed &&
                                  x.Credit == entry.Debit &&
                                  MatchesAccountNumber(x.AccountNumber, rule.CreditAccount) &&
                                  (entry.CurrencyId == x.CurrencyId) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber) &&
                                  (x.SectorCode == entry.SectorCode || x.SectorCode == "00" || entry.SectorCode == "00"));

      } else {
        return _entries.Find(x => !x.Processed &&
                                  x.Debit == entry.Credit &&
                                  MatchesAccountNumber(x.AccountNumber, rule.DebitAccount) &&
                                  (entry.CurrencyId == x.CurrencyId) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber) &&
                                  (x.SectorCode == entry.SectorCode || x.SectorCode == "00" || entry.SectorCode == "00"));
      }
    }


    #endregion Methods

    #region Helpers

    static private bool MatchesAccountNumber(string accountNumber, string accountRuleNumber) {

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


    static private bool MatchesSubLedgerAccountNumber(string subledgerAccountNumber1,
                                                      string subledgerAccountNumber2) {

      return subledgerAccountNumber1 == subledgerAccountNumber2 ||
            (string.IsNullOrWhiteSpace(subledgerAccountNumber1) && !string.IsNullOrWhiteSpace(subledgerAccountNumber2)) ||
            (!string.IsNullOrWhiteSpace(subledgerAccountNumber1) && string.IsNullOrWhiteSpace(subledgerAccountNumber2));
    }


    static private bool SatisfiesRule(FinancialRule rule, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return MatchesAccountNumber(entry.AccountNumber, rule.DebitAccount) &&
                                    (rule.DebitCurrency.IsEmptyInstance || rule.DebitCurrency.Id == entry.CurrencyId);

      } else {
        return MatchesAccountNumber(entry.AccountNumber, rule.CreditAccount) &&
                                   (rule.CreditCurrency.IsEmptyInstance || rule.CreditCurrency.Id == entry.CurrencyId);
      }
    }

    #endregion Helpers

  }  //class CashTransactionProcessorHelper

}  // namespace Empiria.CashFlow.CashLedger
