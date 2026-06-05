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

using Empiria.DynamicData.ExternalData;

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

      FinancialAccount principalAcct = TryGetPrincipalAccount();
      FinancialAccount interestAcct = TryGetInterestAccount();
      FinancialAccount feesAcct = TryGetFeesAccount();
      FinancialAccount capitalizedInterestAcct = TryGetCapitalizedInterestAccount();

      if (principalAcct == null || interestAcct == null) {
        return FixedList<CashFlowProjectionEntryFields>.Empty;
      }

      FixedList<CashFlowProjectionEntry> disbursements = GetDisbursementEntries();

      if (disbursements.Count == 0) {
        return FixedList<CashFlowProjectionEntryFields>.Empty;
      }

      var list = new List<CashFlowProjectionEntryFields>();

      var financialData = (CreditFinancialData) _projection.FinancialData;

      CashFlowProjectionEntry firstDisbursement = disbursements[0];

      var yearMonth = new YearMonth(firstDisbursement.Year, firstDisbursement.Month);

      decimal interestRate = GetInterestRate(yearMonth, financialData);

      var amortizationParams = new AmortizationParameters {

        Disbursements = disbursements.Select(x => (x.Month, x.Amount))
                                     .ToFixedList(),
        InitialPeriod = yearMonth,
        AnnualInterestRate = interestRate,
        RepaymentMonths = financialData.RepaymentTerm,
        GraceMonths = financialData.GracePeriod,
        DisbursementFee = financialData.DisbursementFee,
        OpeningFee = financialData.OpeningFee,
        CapitalizeFees = financialData.CapitalizeFees,
        CapitalizeInterest = financialData.CapitalizeInterest
      };

      var amortizationTable = new AmortizationTable(amortizationParams);

      var projectionColumn = firstDisbursement.ProjectionColumn;

      foreach (var amortizationEntry in amortizationTable.GetMonthlyTable(method)
                                                         .FindAll(x => x.Month <= 12)) {

        var month = amortizationEntry.Month;

        if (month > 12) {
          break;
        }

        yearMonth = new YearMonth(firstDisbursement.Year, month);

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

        if (amortizationEntry.CapitalizedInterest != 0) {
          var entry = BuildEntry(projectionColumn, yearMonth,
                                 capitalizedInterestAcct, amortizationEntry.CapitalizedInterest);

          list.Add(entry);
        }

        if (feesAcct != null && amortizationEntry.Fees != 0) {
          var entry = BuildEntry(projectionColumn, yearMonth,
                                 feesAcct, amortizationEntry.Fees);

          list.Add(entry);

          var taxesEntry = BuildEntry(projectionColumn, yearMonth,
                                      GetFeesTaxesAccount(), amortizationEntry.Fees * 0.16m);

          list.Add(taxesEntry);
        }
      }

      return list.ToFixedList();
    }


    internal FixedList<CashFlowProjectionEntry> GetEntriesToBeRemoved() {
      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("01") ||
                                              x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("03") ||
                                              x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("05") ||
                                              x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("06") ||
                                              x.CashFlowAccount.Id == GetFeesTaxesAccount().Id);
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


    private decimal GetInterestRate(YearMonth yearMonth, CreditFinancialData financialData) {

      decimal interestRate = financialData.InterestRate;

      var intrestRateTypeVariable = ExternalVariable.TryParseWithCode(financialData.InterestRateType.Code);

      if (intrestRateTypeVariable == null) {
        return interestRate;
      }

      var variableInterestRate = FinancialVariables.GetFinancialValue(yearMonth.Year, yearMonth.Month,
                                                                      intrestRateTypeVariable);

      return variableInterestRate + interestRate;
    }


    private FixedList<CashFlowProjectionEntry> GetDisbursementEntries() {
      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("07"))
                                .OrderBy(x => x.Year)
                                .ThenBy(x => x.Month)
                                .ToFixedList();
    }


    private FinancialAccount GetFeesTaxesAccount() {
      return FinancialAccount.Parse(222834);
    }


    private FinancialAccount TryGetCapitalizedInterestAccount() {
      return _projection.BaseAccount.GetOperations()
                                    .Find(x => x.StandardAccount.StdAcctNo.EndsWith("05"));
    }


    private FinancialAccount TryGetFeesAccount() {
      return _projection.BaseAccount.GetOperations()
                                    .Find(x => x.StandardAccount.StdAcctNo.EndsWith("06"));
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
