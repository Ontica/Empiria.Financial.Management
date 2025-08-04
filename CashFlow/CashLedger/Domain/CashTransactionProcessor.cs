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

using Empiria.Financial.Rules;

using Empiria.CashFlow.CashLedger.Adapters;
using System;

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

    internal FixedList<CashTransactionEntryDto> Execute() {
      var updated = new List<CashTransactionEntryDto>(_entries.Count);

      FixedList<CashTransactionEntryDto> temp = ProcessNoCashFlowEntires(_entries);
      updated.AddRange(temp);

      return updated.ToFixedList();
    }


    private FixedList<CashTransactionEntryDto> ProcessNoCashFlowEntires(FixedList<CashTransactionEntryDto> entries) {
      var updated = new List<CashTransactionEntryDto>(_entries.Count);

      FixedList<FinancialRule> rules = FinancialRuleCategory.ParseNamedKey("NO_CASH_FLOW_UNGROUPED")
                                                            .GetFinancialRules(_transaction.AccountingDate);

      foreach (var entry in entries) {
        FixedList<FinancialRule> applicableRules = GetApplicableRules(rules, entry);

        foreach (var rule in applicableRules) {
          CashTransactionEntryDto matchingEntry = TryGetMatchingEntry(rule, entry);
          if (matchingEntry != null) {
            entry.CashAccountId = -1;
            matchingEntry.CashAccountId = -1;
          }
        }
      }

      return updated.ToFixedList();
    }


    private FixedList<FinancialRule> GetApplicableRules(FixedList<FinancialRule> rules, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return rules.FindAll(x => entry.AccountNumber.StartsWith(x.DebitAccount) &&
                                  (x.DebitCurrency.IsEmptyInstance || x.DebitCurrency.Id == entry.CurrencyId));
      } else {
        return rules.FindAll(x => entry.AccountNumber.StartsWith(x.CreditAccount) &&
                                  (x.CreditCurrency.IsEmptyInstance || x.CreditCurrency.Id == entry.CurrencyId));
      }
    }


    private CashTransactionEntryDto TryGetMatchingEntry(FinancialRule rule, CashTransactionEntryDto entry) {
      if (entry.Debit != 0) {
        return _entries.Find(x => x.CashAccountId == 0 &&
                                  x.Credit == entry.Debit &&
                                  x.AccountNumber.StartsWith(rule.CreditAccount) &&
                                  entry.CurrencyId == rule.CreditCurrency.Id &&
                                  x.SubledgerAccountNumber == entry.SubledgerAccountNumber &&
                                  (x.SectorCode == entry.SectorCode || entry.SectorCode == string.Empty));

      } else {
        return _entries.Find(x => x.CashAccountId == 0 &&
                                  x.Debit == entry.Credit &&
                                  x.AccountNumber.StartsWith(rule.DebitAccount) &&
                                  entry.CurrencyId == rule.DebitCurrency.Id &&
                                  x.SubledgerAccountNumber == entry.SubledgerAccountNumber &&
                                  (x.SectorCode == entry.SectorCode || entry.SectorCode == string.Empty));
      }
    }

    #endregion Methods

    #region Helpers

    #endregion Helpers

  } // class CashTransactionProcessor

} // namespace namespace Empiria.CashFlow.CashLedger
