/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Service provider                        *
*  Type     : CashTransactionProcessorHelper             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Helping methods used by CashTransactionProcessor.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Financial;
using Empiria.Financial.Rules;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger {

  /// <summary>Helping methods used by CashTransactionProcessor.</summary>
  internal class CashTransactionProcessorHelper {

    private CashTransactionDescriptor _transaction;
    private FixedList<CashEntryDto> _entries;

    private List<CashEntryFields> _processedEntries;

    internal CashTransactionProcessorHelper(CashTransactionDescriptor transaction,
                                            FixedList<CashEntryDto> entries) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(entries, nameof(entries));

      _transaction = transaction;
      _entries = entries;
      _processedEntries = new List<CashEntryFields>(_entries.Count);
    }

    #region Methods

    internal void AddCashFlowEntry(FinancialRule rule, CashEntryDto entry, string appliedRule) {
      FixedList<FinancialAccount> cashAccounts = TryGetCashAccounts(rule, entry);

      if (cashAccounts == null) {

        AddProcessedEntry(rule, entry, CashAccountStatus.Pending,
          $"{appliedRule} (regla aplicada sobre cuenta sin auxiliar ni concepto directo)");

      } else if (cashAccounts.Count == 0) {

        AddProcessedEntry(rule, entry, CashAccountStatus.Pending,
          $"{appliedRule} (cuenta con auxiliar no registrado en PYC)");

      } else if (cashAccounts.Count == 1 && cashAccounts[0].IsOperationAccount) {

        AddProcessedEntry(rule, entry, cashAccounts[0], appliedRule);

      } else if (cashAccounts.Count == 1 && !cashAccounts[0].IsOperationAccount) {

        AddProcessedEntry(rule, entry, CashAccountStatus.Pending,
          $"{appliedRule} (la cuenta no tiene un concepto relacionado con el tipo de operación)");

      } else if (cashAccounts.Count > 1) {

        AddProcessedEntry(rule, entry, CashAccountStatus.Pending,
          $"{appliedRule} (la cuenta tiene más de un concepto relacionado con el tipo de operación.)");

      } else {
        throw Assertion.EnsureNoReachThisCode();
      }
    }


    internal void AddProcessedEntry(FinancialRule rule, CashEntryDto entry,
                                    CashAccountStatus status, string appliedRuleText) {
      Assertion.Require(status.IsForControl(), nameof(status));

      entry.Processed = true;

      if (entry.CashAccountId == status.ControlValue() &&
          entry.CashFlowAppliedRuleId == rule.Id &&
          entry.CashFlowAppliedRuleText == appliedRuleText) {
        return;
      }

      entry.CashAccountId = status.ControlValue();

      var fields = new CashEntryFields {
        EntryId = entry.Id,
        CashAccountId = status.ControlValue(),
        CashAccountNo = status.Name(),
        TransactionId = _transaction.Id,
        AppliedRuleId = rule.Id,
        AppliedRuleText = appliedRuleText
      };

      _processedEntries.Add(fields);
    }


    internal void AddProcessedEntry(FinancialRule rule, CashEntryDto entry,
                                    FinancialAccount cashAccount, string appliedRuleText) {
      entry.Processed = true;

      if (entry.CashAccountId == cashAccount.Id &&
          entry.CashFlowAppliedRuleId == rule.Id &&
          entry.CashFlowAppliedRuleText == appliedRuleText) {
        return;
      }

      entry.CashAccountId = cashAccount.Id;

      var fields = new CashEntryFields {
        EntryId = entry.Id,
        CashAccountId = cashAccount.Id,
        CashAccountNo = cashAccount.AccountNo,
        TransactionId = _transaction.Id,
        AppliedRuleId = rule.Id,
        AppliedRuleText = appliedRuleText
      };

      _processedEntries.Add(fields);
    }


    internal FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules,
                                                         FixedList<CashEntryDto> entries) {
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


    internal FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules,
                                                         CashEntryDto entry) {
      if (entry.Debit != 0) {
        return rules.FindAll(x => MatchesAccountNumber(entry.AccountNumber, x.DebitAccount, x) &&
                                  (x.DebitCurrency.IsEmptyInstance || x.DebitCurrency.Id == entry.CurrencyId));
      } else {
        return rules.FindAll(x => MatchesAccountNumber(entry.AccountNumber, x.CreditAccount, x) &&
                                  (x.CreditCurrency.IsEmptyInstance || x.CreditCurrency.Id == entry.CurrencyId));
      }
    }


    internal FixedList<CashEntryDto> GetEntriesSatisfyingRule(FinancialRule rule,
                                                              FixedList<CashEntryDto> entries) {
      if (entries.Count == 0) {
        return new FixedList<CashEntryDto>();
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


    internal FixedList<CashEntryDto> GetUnprocessedEntries() {
      return _entries.FindAll(x => !x.Processed);
    }


    internal FixedList<CashEntryDto> GetUnprocessedEntries(Func<CashEntryDto, bool> predicate) {
      return _entries.FindAll(x => !x.Processed && predicate.Invoke(x));
    }


    internal bool HaveSameCashAccount(FinancialRule rule, CashEntryDto debitEntry, CashEntryDto creditEntry) {
      var debitCashAccount = GetDirectCashAccounts(rule, debitEntry);
      var creditCashAccount = GetDirectCashAccounts(rule, creditEntry);

      if (debitCashAccount == null || creditCashAccount == null ||
          debitCashAccount.Count != 1 || creditCashAccount.Count != 1) {
        return false;
      }

      return debitCashAccount[0].AccountNo == creditCashAccount[0].AccountNo;
    }


    internal void ReplaceProcessedEntry(CashEntryFields entry,
                                        CashAccountStatus newStatus,
                                        string appliedRule) {

      entry.CashAccountId = newStatus.ControlValue();
      entry.CashAccountNo = newStatus.Name();
      entry.AppliedRuleText = appliedRule;
    }


    internal FixedList<CashEntryDto> TryGetMatchingEntries(FinancialRule rule,
                                                           CashEntryDto entry) {
      if (entry.Debit != 0) {
        return _entries.FindAll(x => !x.Processed &&
                                      x.Credit > 0 &&
                                      x.CurrencyId == entry.CurrencyId &&
                                      MatchesAccountNumber(x.AccountNumber, rule.CreditAccount, rule) &&
                                      MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber,
                                                                    entry.SubledgerAccountNumber, rule) &&
                                      MatchesSector(x.SectorCode, entry.SectorCode, rule));

      } else {
        return _entries.FindAll(x => !x.Processed &&
                                      x.Debit > 0 &&
                                      x.CurrencyId == entry.CurrencyId &&
                                      MatchesAccountNumber(x.AccountNumber, rule.DebitAccount, rule) &&
                                      MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber,
                                                                    entry.SubledgerAccountNumber, rule) &&
                                      MatchesSector(x.SectorCode, entry.SectorCode, rule));
      }
    }


    internal CashEntryDto TryGetMatchingEntry(CashEntryDto entry) {

      if (entry.Debit > 0) {
        return _entries.Find(x => !x.Processed &&
                                  x.Credit == entry.Debit &&
                                  x.CurrencyId == entry.CurrencyId &&
                                  MatchesAccountNumber(x.AccountNumber, entry.AccountNumber) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber) &&
                                  MatchesSector(x.SectorCode, entry.SectorCode));
      } else {
        return _entries.Find(x => !x.Processed &&
                                  x.Debit == entry.Credit &&
                                  x.CurrencyId == entry.CurrencyId &&
                                  MatchesAccountNumber(x.AccountNumber, entry.AccountNumber) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber) &&
                                  MatchesSector(x.SectorCode, entry.SectorCode));
      }
    }


    internal CashEntryDto TryGetMatchingEntry(FinancialRule rule, CashEntryDto entry,
                                              bool swapRule = false) {

      if (entry.Debit != 0) {
        return _entries.Find(x => !x.Processed &&
                                  x.Credit == entry.Debit &&
                                  x.CurrencyId == entry.CurrencyId &&
                                  MatchesAccountNumber(x.AccountNumber,
                                                       swapRule ? rule.DebitAccount : rule.CreditAccount, rule) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber,
                                                                entry.SubledgerAccountNumber, rule) &&
                                  MatchesSector(x.SectorCode, entry.SectorCode, rule));

      } else {
        return _entries.Find(x => !x.Processed &&
                                  x.Debit == entry.Credit &&
                                  x.CurrencyId == entry.CurrencyId &&
                                  MatchesAccountNumber(x.AccountNumber,
                                                       swapRule ? rule.CreditAccount : rule.DebitAccount, rule) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber,
                                                                entry.SubledgerAccountNumber, rule) &&
                                  MatchesSector(x.SectorCode, entry.SectorCode, rule));
      }
    }


    #endregion Methods

    #region Helpers

    static private bool MatchesAccountNumber(string accountNo,
                                             string patternAccountNo,
                                             FinancialRule rule = null) {

      rule = rule ?? FinancialRule.Empty;

      if (string.IsNullOrWhiteSpace(patternAccountNo)) {
        return rule.Category.IsSingleEntry;
      }

      bool isPatternRule = patternAccountNo.Contains("*") && patternAccountNo.EndsWith("]");

      if (isPatternRule) {
        string startsWith = patternAccountNo.Split('*')[0];
        string endsWith = EmpiriaString.TrimAll(patternAccountNo.Split('*')[1], "]", string.Empty);

        return accountNo.StartsWith(startsWith) && accountNo.EndsWith(endsWith);
      } else {
        return accountNo.StartsWith(patternAccountNo);
      }
    }


    static private bool MatchesSector(string sectorCode1, string sectorCode2,
                                      FinancialRule rule = null) {

      rule = rule ?? FinancialRule.Empty;

      if (rule.Conditions.Get("skipSectorCodeMatch", true)) {
        return true;
      }

      return (sectorCode1 == sectorCode2 || sectorCode1 == "00" || sectorCode2 == "00");
    }


    static private bool MatchesSubLedgerAccountNumber(string subledgerAcctNo1,
                                                      string subledgerAcctNo2,
                                                      FinancialRule rule = null) {

      rule = rule ?? FinancialRule.Empty;

      if (rule.Conditions.Get("skipSubledgerAccountMatch", false)) {
        return true;
      }

      return subledgerAcctNo1 == subledgerAcctNo2 ||
            (string.IsNullOrWhiteSpace(subledgerAcctNo1) && !string.IsNullOrWhiteSpace(subledgerAcctNo2)) ||
            (!string.IsNullOrWhiteSpace(subledgerAcctNo1) && string.IsNullOrWhiteSpace(subledgerAcctNo2));
    }


    static private bool SatisfiesRule(FinancialRule rule, CashEntryDto entry) {
      if (entry.Debit != 0) {
        return MatchesAccountNumber(entry.AccountNumber, rule.DebitAccount, rule) &&
                                    (rule.DebitCurrency.IsEmptyInstance ||
                                     rule.DebitCurrency.Id == entry.CurrencyId);

      } else {
        return MatchesAccountNumber(entry.AccountNumber, rule.CreditAccount, rule) &&
                                   (rule.CreditCurrency.IsEmptyInstance ||
                                    rule.CreditCurrency.Id == entry.CurrencyId);
      }
    }


    private FixedList<FinancialAccount> TryGetCashAccounts(FinancialRule rule, CashEntryDto entry) {

      FixedList<FinancialAccount> directAccounts = GetDirectCashAccounts(rule, entry);

      if (directAccounts.Count > 0) {
        return directAccounts;
      }

      if (entry.SubledgerAccountNumber.Length == 0) {
        return null;
      }

      FinancialAccount account = FinancialAccount.TryParseWithSubledgerAccount(entry.SubledgerAccountNumber);

      if (account == null) {
        return new FixedList<FinancialAccount>();
      }

      if (entry.Debit > 0) {

        return account.GetOperations()
                      .FindAll(x => x.Currency.Id == entry.CurrencyId &&
                                    x.StandardAccount.StdAcctNo.EndsWith(rule.DebitConcept));
      } else {

        return account.GetOperations()
              .FindAll(x => x.Currency.Id == entry.CurrencyId &&
                            x.StandardAccount.StdAcctNo.EndsWith(rule.CreditConcept));
      }
    }


    private FixedList<FinancialAccount> GetDirectCashAccounts(FinancialRule rule,
                                                              CashEntryDto entry) {
      string directConcept = TryGetDirectAccountConcept(rule, entry);

      if (directConcept != null && directConcept.Length < 4) {
        return new FixedList<FinancialAccount>();
      }

      return FinancialAccount.GetList(x => x.AccountNo == directConcept &&
                                           x.Currency.Id == entry.CurrencyId &&
                                           x.IsOperationAccount);
    }

    private string TryGetDirectAccountConcept(FinancialRule rule, CashEntryDto entry) {

      if (entry.Debit > 0 && rule.DebitConcept.Length >= 4) {
        return rule.DebitConcept;

      } else if (rule.CreditConcept.Length >= 4) {
        return rule.CreditConcept;

      }

      var rules = GetRules("CASH_FLOW_ACCOUNTS");

      if (entry.Debit > 0) {
        return rules.Find(x => x.DebitAccount == entry.AccountNumber)
                    ?.DebitConcept;
      } else {
        return rules.Find(x => x.CreditAccount == entry.AccountNumber)
                    ?.CreditConcept;
      }
    }


    #endregion Helpers

  }  //class CashTransactionProcessorHelper

}  // namespace Empiria.CashFlow.CashLedger
