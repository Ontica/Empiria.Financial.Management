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

      //FixedList<CashEntryFields> temp = ProcessNoCashFlowGroupedEntries();
      //updated.AddRange(temp);

      FixedList<CashEntryFields> temp = ProcessNoCashFlowEntries();
      updated.AddRange(temp);

      temp = ProcessNoCashFlowEntriesAdded();
      updated.AddRange(temp);

      temp = ProcessSameEntries();
      updated.AddRange(temp);

      temp = ProcessCashFlowEntries();
      updated.AddRange(temp);

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessCashFlowEntries() {
      FixedList<CashTransactionEntryDto> entries = GetUnprocessedEntries();

      if (entries.Count == 0) {
        return new FixedList<CashEntryFields>();
      }

      var updated = new List<CashEntryFields>(_entries.Count);

      var rules = TempRule.GetList();

      foreach (var entry in entries) {

        FixedList<string> concepts = rules.FindAll(x => entry.AccountNumber == x.CuentaContable)
                                          .SelectDistinct(x => x.Concepto.ToString());

        if (concepts.Count == 0) {
          AddCashEntryFields(updated, entry, 0);
          continue;
        }

        if (concepts.Count == 1 && entry.SubledgerAccountNumber.Length == 0) {
          AddCashEntryFields(updated, entry, int.Parse(concepts[0]));
          continue;
        }

        if (concepts.Count > 1 && entry.SubledgerAccountNumber.Length == 0) {
          AddCashEntryFields(updated, entry, -2);
          continue;
        }

        var account = FinancialAccount.TryParseWithSubledgerAccount(entry.SubledgerAccountNumber);

        if (account == null && concepts.Count == 1) {
          AddCashEntryFields(updated, entry, int.Parse(concepts[0]));
          continue;
        }

        if (account == null && concepts.Count > 1) {
          AddCashEntryFields(updated, entry, -2);
          continue;
        }

        FixedList<string> accountConcepts = account.GetOperations()
                                                   .FindAll(x => x.Currency.Id == entry.CurrencyId)
                                                   .SelectDistinct(x => x.AccountNo)
                                                   .Intersect(concepts);

        if (accountConcepts.Count == 0 && concepts.Count == 1) {
          AddCashEntryFields(updated, entry, int.Parse(concepts[0]));

        } else if (accountConcepts.Count == 1) {
          AddCashEntryFields(updated, entry, int.Parse(accountConcepts[0]));

        } else {
          AddCashEntryFields(updated, entry, -2);
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessSameEntries() {
      FixedList<CashTransactionEntryDto> debitEntries = GetUnprocessedEntries();

      var updated = new List<CashEntryFields>(_entries.Count);

      if (debitEntries.SelectDistinct(x => x.AccountNumber).Count == 1) {
        foreach (var entry in debitEntries) {
          AddCashEntryFields(updated, entry, -1);
        }
        return updated.ToFixedList();
      }

      debitEntries = GetUnprocessedEntries(x => x.Debit != 0 && !x.AccountNumber.StartsWith("9"));

      if (debitEntries.Count == 0) {
        return new FixedList<CashEntryFields>();
      }

      foreach (var debitGroup in debitEntries.GroupBy(x => $"{x.AccountNumber}|{x.CurrencyId}|{x.SectorCode}|{x.SubledgerAccountNumber}")) {

        CashTransactionEntryDto pivotEntry = debitGroup.First();

        FixedList<CashTransactionEntryDto> creditEntries = GetUnprocessedEntries(x => x.Credit != 0 &&
                                                                                      x.CurrencyId == pivotEntry.CurrencyId &&
                                                                                      x.AccountNumber == pivotEntry.AccountNumber &&
                                                                                      x.SectorCode == pivotEntry.SectorCode &&
                                                                                      x.SubledgerAccountNumber == pivotEntry.SubledgerAccountNumber);
        if (debitGroup.Sum(x => x.Debit) != creditEntries.Sum(x => x.Credit)) {
          continue;
        }

        foreach (var debitEntry in debitGroup) {
          AddCashEntryFields(updated, debitEntry, -1);
        }

        foreach (var creditEntry in creditEntries) {
          AddCashEntryFields(updated, creditEntry, -1);
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessNoCashFlowEntries() {
      FixedList<CashTransactionEntryDto> entries = GetUnprocessedEntries(x => x.Debit != 0);

      if (entries.Count == 0) {
        return new FixedList<CashEntryFields>();
      }

      var updated = new List<CashEntryFields>(_entries.Count);

      FixedList<FinancialRule> rules = FinancialRuleCategory.ParseNamedKey("NO_CASH_FLOW_UNGROUPED")
                                                            .GetFinancialRules(_transaction.AccountingDate);

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = GetApplicableRules(rules, entry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto matchingEntry = TryGetMatchingEntry(rule, entry);

          if (matchingEntry != null) {
            AddCashEntryFields(updated, entry, -1);
            AddCashEntryFields(updated, matchingEntry, -1);
          }
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<CashEntryFields> ProcessNoCashFlowEntriesAdded() {

      FixedList<FinancialRule> rules = FinancialRuleCategory.ParseNamedKey("NO_CASH_FLOW_UNGROUPED")
                                                            .GetFinancialRules(_transaction.AccountingDate);

      FixedList<FinancialRule> applicableRules = GetApplicableRules(rules, GetUnprocessedEntries(x => x.Debit != 0));

      var updated = new List<CashEntryFields>();

      foreach (var rule in applicableRules) {
        FixedList<CashTransactionEntryDto> debitEntries = GetEntriesSatisfyingRule(rule, GetUnprocessedEntries(x => x.Debit != 0));
        FixedList<CashTransactionEntryDto> creditEntries = GetEntriesSatisfyingRule(rule, GetUnprocessedEntries(x => x.Credit != 0));

        var debitEntriesByCurrency = debitEntries.GroupBy(x => x.CurrencyId);

        foreach (var debitCurrencyGroup in debitEntriesByCurrency) {
          var creditEntriesByCurrency = creditEntries.FindAll(x => x.CurrencyId == debitCurrencyGroup.Key);

          decimal debitSum = debitCurrencyGroup.Sum(x => x.Debit);
          decimal creditSum = creditEntriesByCurrency.Sum(x => x.Credit);

          if (debitSum != creditSum) {
            continue;
          }

          foreach (var debitEntry in debitCurrencyGroup) {
            AddCashEntryFields(updated, debitEntry, -1);
          }

          foreach (var creditEntry in creditEntriesByCurrency) {
            AddCashEntryFields(updated, creditEntry, -1);
          }
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules,
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


    private FixedList<CashTransactionEntryDto> GetEntriesSatisfyingRule(FinancialRule rule,
                                                                        FixedList<CashTransactionEntryDto> entries) {
      if (entries.Count == 0) {
        return new FixedList<CashTransactionEntryDto>();
      }

      return entries.FindAll(x => SatisfiesRule(rule, x));
    }


    private bool SatisfiesRule(FinancialRule rule, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return MatchesAccountNumber(entry.AccountNumber, rule.DebitAccount) &&
                                    (rule.DebitCurrency.IsEmptyInstance || rule.DebitCurrency.Id == entry.CurrencyId);

      } else {
        return MatchesAccountNumber(entry.AccountNumber, rule.CreditAccount) &&
                                   (rule.CreditCurrency.IsEmptyInstance || rule.CreditCurrency.Id == entry.CurrencyId);
      }
    }


    private FixedList<CashEntryFields> ProcessNoCashFlowGroupedEntries() {
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

          CashTransactionEntryDto matchingEntry = TryGetMatchingEntry(groupedRule.First(), entry);

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


    private FixedList<CashTransactionEntryDto> GetUnprocessedEntries() {
      return _entries.FindAll(x => !x.Processed);
    }


    private FixedList<CashTransactionEntryDto> GetUnprocessedEntries(System.Func<CashTransactionEntryDto, bool> predicate) {
      return _entries.FindAll(x => !x.Processed && predicate.Invoke(x));
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


    private CashTransactionEntryDto TryGetMatchingEntry(FinancialRule rule, CashTransactionEntryDto entry) {
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


    private FixedList<CashTransactionEntryDto> TryGetMatchingEntries(FinancialRule rule, CashTransactionEntryDto entry) {
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


    private bool MatchesSubLedgerAccountNumber(string subledgerAccountNumber1,
                                               string subledgerAccountNumber2) {
      return subledgerAccountNumber1 == subledgerAccountNumber2 ||
            (string.IsNullOrWhiteSpace(subledgerAccountNumber1) && !string.IsNullOrWhiteSpace(subledgerAccountNumber2)) ||
            (!string.IsNullOrWhiteSpace(subledgerAccountNumber1) && string.IsNullOrWhiteSpace(subledgerAccountNumber2));
    }

    #endregion Helpers

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
