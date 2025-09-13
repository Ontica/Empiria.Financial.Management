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

        AddProcessedEntry(entry, CashAccountStatus.Pending,
          $"{appliedRule} (regla aplicada sobre cuenta sin auxiliar ni concepto directo)");

      } else if (cashAccounts.Count == 0) {

        AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
          $"{appliedRule} (cuenta {entry.SubledgerAccountNumber} no registrada en PYC)");

      } else if (cashAccounts.Count == 1 && cashAccounts[0].FinancialAccountType.Equals(FinancialAccountType.OperationAccount)) {

        AddProcessedEntry(entry, cashAccounts[0], appliedRule);

      } else if (cashAccounts.Count == 1 && !cashAccounts[0].FinancialAccountType.Equals(FinancialAccountType.OperationAccount)) {

        AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
          $"{appliedRule} (la cuenta {entry.SubledgerAccountNumber}) " +
          $"no tiene un concepto relacionado con el tipo de operación {rule.CreditConcept})");

      } else if (cashAccounts.Count > 1) {

        AddProcessedEntry(entry, CashAccountStatus.CashFlowUnassigned,
          $"{appliedRule} (la cuenta {entry.SubledgerAccountNumber} " +
          $"tiene más de un concepto relacionado con el tipo {rule.CreditConcept}.)");

      } else {
        throw Assertion.EnsureNoReachThisCode();
      }
    }


    internal void AddProcessedEntry(CashEntryDto entry,
                                    CashAccountStatus status, string appliedRule) {
      Assertion.Require(status.IsForControl(), nameof(status));

      entry.Processed = true;

      if (entry.CashAccountId == status.ControlValue() &&
          entry.CashAccountAppliedRule == appliedRule) {
        return;
      }

      entry.CashAccountId = status.ControlValue();

      var fields = new CashEntryFields {
        EntryId = entry.Id,
        CashAccountId = status.ControlValue(),
        CashAccountNo = status.Name(),
        TransactionId = _transaction.Id,
        AppliedRule = appliedRule
      };

      _processedEntries.Add(fields);
    }


    internal void AddProcessedEntry(CashEntryDto entry,
                                    FinancialAccount cashAccount, string appliedRule) {
      entry.Processed = true;

      if (entry.CashAccountId == cashAccount.Id &&
          entry.CashAccountAppliedRule == appliedRule) {
        return;
      }

      entry.CashAccountId = cashAccount.Id;

      var fields = new CashEntryFields {
        EntryId = entry.Id,
        CashAccountId = cashAccount.Id,
        CashAccountNo = cashAccount.AccountNo,
        TransactionId = _transaction.Id,
        AppliedRule = appliedRule
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


    internal FixedList<CashEntryDto> TryGetMatchingEntries(FinancialRule rule,
                                                                      CashEntryDto entry) {
      if (entry.Debit != 0) {
        return _entries.FindAll(x => !x.Processed &&
                                      x.Credit > 0 &&
                                      x.CurrencyId == entry.CurrencyId &&
                                      MatchesAccountNumber(x.AccountNumber, rule.CreditAccount, rule) &&
                                      MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber, rule) &&
                                      MatchesSector(x.SectorCode, entry.SectorCode, rule));

      } else {
        return _entries.FindAll(x => !x.Processed &&
                                      x.Debit > 0 &&
                                      x.CurrencyId == entry.CurrencyId &&
                                      MatchesAccountNumber(x.AccountNumber, rule.DebitAccount, rule) &&
                                      MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber, rule) &&
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
                                  MatchesAccountNumber(x.AccountNumber, swapRule ? rule.DebitAccount : rule.CreditAccount, rule) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber, rule) &&
                                  MatchesSector(x.SectorCode, entry.SectorCode, rule));

      } else {
        return _entries.Find(x => !x.Processed &&
                                  x.Debit == entry.Credit &&
                                  x.CurrencyId == entry.CurrencyId &&
                                  MatchesAccountNumber(x.AccountNumber, swapRule ? rule.CreditAccount : rule.DebitAccount, rule) &&
                                  MatchesSubLedgerAccountNumber(x.SubledgerAccountNumber, entry.SubledgerAccountNumber, rule) &&
                                  MatchesSector(x.SectorCode, entry.SectorCode, rule));
      }
    }


    #endregion Methods

    #region Helpers

    static private bool MatchesAccountNumber(string accountNumber, string ruleAccountNumber, FinancialRule rule = null) {

      rule = rule ?? FinancialRule.Empty;

      if (string.IsNullOrWhiteSpace(ruleAccountNumber)) {
        return rule.Category.IsSingleEntry;
      }

      bool isPatternRule = ruleAccountNumber.Contains("*") && ruleAccountNumber.EndsWith("]");

      if (isPatternRule) {
        string startsWith = ruleAccountNumber.Split('*')[0];
        string endsWith = EmpiriaString.TrimAll(ruleAccountNumber.Split('*')[1], "]", string.Empty);

        return accountNumber.StartsWith(startsWith) && accountNumber.EndsWith(endsWith);
      } else {
        return accountNumber.StartsWith(ruleAccountNumber);
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


    static private bool MatchesSubLedgerAccountNumber(string subledgerAccountNumber1,
                                                      string subledgerAccountNumber2,
                                                      FinancialRule rule = null) {

      rule = rule ?? FinancialRule.Empty;

      if (rule.Conditions.Get("skipSubledgerAccountMatch", false)) {
        return true;
      }

      return subledgerAccountNumber1 == subledgerAccountNumber2 ||
            (string.IsNullOrWhiteSpace(subledgerAccountNumber1) && !string.IsNullOrWhiteSpace(subledgerAccountNumber2)) ||
            (!string.IsNullOrWhiteSpace(subledgerAccountNumber1) && string.IsNullOrWhiteSpace(subledgerAccountNumber2));
    }


    static private bool SatisfiesRule(FinancialRule rule, CashEntryDto entry) {
      if (entry.Debit != 0) {
        return MatchesAccountNumber(entry.AccountNumber, rule.DebitAccount, rule) &&
                                    (rule.DebitCurrency.IsEmptyInstance || rule.DebitCurrency.Id == entry.CurrencyId);

      } else {
        return MatchesAccountNumber(entry.AccountNumber, rule.CreditAccount, rule) &&
                                   (rule.CreditCurrency.IsEmptyInstance || rule.CreditCurrency.Id == entry.CurrencyId);
      }
    }


    static private FixedList<FinancialAccount> TryGetCashAccounts(FinancialRule rule, CashEntryDto entry) {

      FixedList<FinancialAccount> directAccounts = TryGetDirectCashAccounts(rule, entry);

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

    static private FixedList<FinancialAccount> TryGetDirectCashAccounts(FinancialRule rule, CashEntryDto entry) {
      if (entry.Debit > 0) {
        return FinancialAccount.GetList(x => x.AccountNo == rule.DebitConcept && rule.DebitConcept.Length >= 4 &&
                                             x.Currency.Id == entry.CurrencyId &&
                                             x.FinancialAccountType.Equals(FinancialAccountType.OperationAccount));
      } else {
        return FinancialAccount.GetList(x => x.AccountNo == rule.CreditConcept && rule.CreditConcept.Length >= 4 &&
                                             x.Currency.Id == entry.CurrencyId &&
                                             x.FinancialAccountType.Equals(FinancialAccountType.OperationAccount));
      }
    }

    #endregion Helpers

  }  //class CashTransactionProcessorHelper

}  // namespace Empiria.CashFlow.CashLedger
