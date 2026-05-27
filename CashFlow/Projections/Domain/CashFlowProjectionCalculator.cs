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

namespace Empiria.CashFlow.Projections {

  /// <summary>Generates cash flow projection entries with their values using financial data.</summary>
  internal class CashFlowProjectionCalculator {

    private CashFlowProjection _projection;

    internal CashFlowProjectionCalculator(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      _projection = projection;
    }


    internal FixedList<CashFlowProjectionEntryFields> CalculateEntries(string method) {
      Assertion.Require(method, nameof(method));

      var principalAccount = _projection.BaseAccount.GetOperations()
                                                  .Find(x => x.StandardAccount.StdAcctNo.EndsWith("01"));
      var interestAccount = _projection.BaseAccount.GetOperations()
                                                   .Find(x => x.StandardAccount.StdAcctNo.EndsWith("03"));

      if (principalAccount == null || interestAccount == null) {
        return FixedList<CashFlowProjectionEntryFields>.Empty;
      }

      var disbursements = GetDisbursementEntries();

      if (disbursements.Count == 0) {
        return FixedList<CashFlowProjectionEntryFields>.Empty;
      }

      var list = new List<CashFlowProjectionEntryFields>();

      var financialData = (CreditFinancialData) _projection.FinancialData;

      var calculadora = new AmortizationTable(disbursements.Sum(x => x.Amount),
                                              financialData.InterestRate,
                                              financialData.RepaymentTerm);


      foreach (var amortizacion in calculadora.GetMonthlyTable().FindAll(x => x.Month <= 12)) {

        var mes = amortizacion.Month + disbursements[0].Month;

        if (mes > 12) {
          continue;
        }

        var entry = BuildEntryFrom(disbursements[0], mes, principalAccount, amortizacion.Principal);

        list.Add(entry);

        entry = BuildEntryFrom(disbursements[0], mes, interestAccount, amortizacion.Interest);

        list.Add(entry);
      }

      return list.ToFixedList();
    }

    internal FixedList<CashFlowProjectionEntry> GetEntriesToBeRemoved(string method) {
      Assertion.Require(method, nameof(method));

      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("01") ||
                                              x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("03"));
    }

    #region Helpers

    private CashFlowProjectionEntryFields BuildEntryFrom(CashFlowProjectionEntry baseEntry,
                                                         int mes, FinancialAccount newAccount,
                                                         decimal amount) {
      return new CashFlowProjectionEntryFields {
        ProjectionUID = baseEntry.Projection.UID,
        ProjectionColumnUID = baseEntry.ProjectionColumn.UID,
        CashFlowAccountUID = newAccount.UID,
        Month = mes,
        Year = baseEntry.Year,
        CurrencyUID = baseEntry.Currency.UID,
        Amount = amount
      };
    }


    private FixedList<CashFlowProjectionEntry> GetDisbursementEntries() {
      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("07"))
                                .Sort((x, y) => x.Month.CompareTo(y.Month));
    }

    #endregion Helpers

  }  // class CashFlowProjectionCalculator

}  // namespace Empiria.CashFlow.Projections
