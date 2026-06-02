/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Service provider                        *
*  Type     : AmortizationTable                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates an amortization table for a given loan or credit.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

namespace Empiria.CashFlow.Projections {


  public enum AmortizationMethod {

    CuotaFija,

    CuotaVariable,

    None,

  }



  public class AmortizationParameters {

    public decimal Amount {
      get; set;
    }

    public decimal AnnualInterestRate {
      get; set;
    }

    public int RepaymentMonths {
      get; set;
    }

    public int GraceMonths {
      get; set;
    }

    internal void EnsureValid() {
      Assertion.Require(Amount > 0, "El importe debe ser mayor a cero.");
      Assertion.Require(AnnualInterestRate > 0, "La tasa de interés debe ser mayor a cero.");
      Assertion.Require(RepaymentMonths > 0, "El plazo de amortización en meses debe ser mayor a cero.");
    }

  }

  /// <summary>Generates an amortization table for a given loan or credit.</summary>
  public class AmortizationTable {

    private AmortizationParameters _params;

    public AmortizationTable(AmortizationParameters parameters) {
      Assertion.Require(parameters, nameof(parameters));

      parameters.EnsureValid();

      _params = parameters;
    }

    #region Methods

    public FixedList<AmortizationTableEntry> GetMonthlyTable(AmortizationMethod method) {
      Assertion.Require(method != AmortizationMethod.None, nameof(method));

      switch (method) {

        case AmortizationMethod.CuotaFija:
          return GetCuotaFijaMonthlyTable();

        case AmortizationMethod.CuotaVariable:
          return GetCuotaVariableMonthlyTable();

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized amortization method {method}.");
      }
    }

    #endregion Methods

    #region Helpers

    private decimal CalculateFixedMonthlyPayment(decimal amount) {

      decimal monthlyRate = CalculateMonthlyInterestRate();

      // Formula: P * (r * (1+r)^n) / ((1+r)^n - 1)
      double factor = Math.Pow((double) (1 + monthlyRate), _params.RepaymentMonths);

      decimal monthlyPayment = amount * ((monthlyRate * (decimal) factor) / ((decimal) factor - 1));

      return Math.Round(monthlyPayment, 2);
    }


    private decimal CalculateGraceMonthsInterest() {

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal interest = 0;

      decimal balance = _params.Amount;

      for (int month = 1; month <= _params.GraceMonths; month++) {

        decimal monthInterest = Math.Round(balance * monthlyRate, 2);

        interest += monthInterest;
        balance += monthInterest;
      }

      return interest;
    }


    private decimal CalculateMonthlyFixedPrincipalPayment(decimal amount) {
      return Math.Round(amount / _params.RepaymentMonths, 2);
    }


    private decimal CalculateMonthlyInterestRate() {
      return _params.AnnualInterestRate / 100 / 12;
    }


    private FixedList<AmortizationTableEntry> GetCuotaFijaMonthlyTable() {

      var table = new List<AmortizationTableEntry>(_params.RepaymentMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal graceMonthsInterest = CalculateGraceMonthsInterest();

      decimal balance = _params.Amount + graceMonthsInterest;

      decimal monthlyPayment = CalculateFixedMonthlyPayment(balance);

      for (int month = 1; month <= _params.RepaymentMonths; month++) {

        if (balance <= 0) {
          break;
        }

        decimal interest = Math.Round(balance * monthlyRate, 2);
        decimal principal = Math.Round(monthlyPayment - interest, 2);

        if (month == _params.RepaymentMonths) {
          principal = balance;
        }

        balance = Math.Round(balance - principal, 2);

        table.Add(new AmortizationTableEntry {
          Month = month,
          MonthlyPayment = principal + interest,
          Principal = principal,
          Interest = interest,
          RemainingBalance = balance
        });

      }

      return table.ToFixedList();
    }


    private FixedList<AmortizationTableEntry> GetCuotaVariableMonthlyTable() {

      var table = new List<AmortizationTableEntry>(_params.RepaymentMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal graceMonthsInterest = CalculateGraceMonthsInterest();

      decimal balance = _params.Amount + graceMonthsInterest;

      decimal principalPayment = CalculateMonthlyFixedPrincipalPayment(balance);

      for (int month = 1; month <= _params.RepaymentMonths; month++) {

        if (balance <= 0) {
          break;
        }

        decimal interest = Math.Round(balance * monthlyRate, 2);
        decimal principal = principalPayment;

        if (month == _params.RepaymentMonths) {
          principal = balance;
        }

        balance = Math.Round(balance - principal, 2);

        table.Add(new AmortizationTableEntry {
          Month = month,
          MonthlyPayment = principal + interest,
          Principal = principal,
          Interest = interest,
          RemainingBalance = balance
        });
      }

      return table.ToFixedList();
    }

    #endregion Helpers

  }  // class AmortizationTable



  /// <summary>Information holder for an amortization table month entry.</summary>
  public class AmortizationTableEntry {

    public int Month {
      get; internal set;
    }

    public decimal MonthlyPayment {
      get; internal set;
    }

    public decimal Principal {
      get; internal set;
    }

    public decimal Interest {
      get; internal set;
    }

    public decimal RemainingBalance {
      get; internal set;
    }

  }  // class AmortizationTableEntry

}  // namespace Empiria.CashFlow.Projections
