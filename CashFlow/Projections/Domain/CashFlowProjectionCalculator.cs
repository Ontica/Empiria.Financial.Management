/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Service provider                        *
*  Type     : CashFlowProjectionCalculator               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates cash flow projection entries with their values using financial data.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;
using Empiria.Time;

namespace Empiria.CashFlow.Projections {

  /// <summary>Generates cash flow projection entries with their values using financial data.</summary>
  internal class CashFlowProjectionCalculator {

    private CashFlowProjection _projection;

    internal CashFlowProjectionCalculator(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      _projection = projection;
    }


    internal FixedList<CashFlowProjectionEntryFields> CalculateEntries(AmortizationMethod method) {
      Assertion.Require(method, nameof(method));

      var principalAcct = TryGetPrincipalAccount();
      var interestAcct = TryGetInterestAccount();

      if (principalAcct == null || interestAcct == null) {
        return FixedList<CashFlowProjectionEntryFields>.Empty;
      }

      var disbursements = GetDisbursementEntries();

      if (disbursements.Count == 0) {
        return FixedList<CashFlowProjectionEntryFields>.Empty;
      }

      var list = new List<CashFlowProjectionEntryFields>();

      var financialData = (CreditFinancialData) _projection.FinancialData;

      var amortizationTable = new AmortizationTable(disbursements.Sum(x => x.Amount),
                                                    financialData.InterestRate,
                                                    financialData.RepaymentTerm);

      var projectionColumn = disbursements[0].ProjectionColumn;

      foreach (var amortizationEntry in amortizationTable.GetMonthlyTable()
                                                         .FindAll(x => x.Month <= 12)) {

        var month = amortizationEntry.Month + disbursements[0].Month;

        if (month > 12) {
          break;
        }
        var yearMonth = new YearMonth(disbursements[0].Year, month);


        if (amortizationEntry.Principal != 0) {
          var entry = BuildEntry(projectionColumn, yearMonth,
                                 principalAcct, amortizationEntry.Principal);

          list.Add(entry);
        }

        if (amortizationEntry.Interest != 0) {
          var entry = BuildEntry(projectionColumn, yearMonth,
                                 interestAcct, amortizationEntry.Interest);

          list.Add(entry);
        }
      }

      return list.ToFixedList();
    }


    internal FixedList<CashFlowProjectionEntry> GetEntriesToBeRemoved() {
      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("01") ||
                                              x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("03"));
    }

    #region Helpers

    private CashFlowProjectionEntryFields BuildEntry(CashFlowProjectionColumn projectionColumn,
                                                     YearMonth yearMonth,
                                                     FinancialAccount newAccount,
                                                     decimal amount) {
      return new CashFlowProjectionEntryFields {
        ProjectionUID = _projection.UID,
        ProjectionColumnUID = projectionColumn.UID,
        CashFlowAccountUID = newAccount.UID,
        Year = yearMonth.Year,
        Month = yearMonth.Month,
        CurrencyUID = newAccount.Currency.UID,
        Amount = amount
      };
    }


    private FixedList<CashFlowProjectionEntry> GetDisbursementEntries() {
      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("07"))
                                .Sort((x, y) => x.Month.CompareTo(y.Month));
    }


    private FinancialAccount TryGetInterestAccount() {
      return _projection.BaseAccount.GetOperations()
                                    .Find(x => x.StandardAccount.StdAcctNo.EndsWith("03"));
    }


    private FinancialAccount TryGetPrincipalAccount() {
      return _projection.BaseAccount.GetOperations()
                                    .Find(x => x.StandardAccount.StdAcctNo.EndsWith("01"));
    }

    #endregion Helpers

  }  // class CashFlowProjectionCalculator

}  // namespace Empiria.CashFlow.Projections
