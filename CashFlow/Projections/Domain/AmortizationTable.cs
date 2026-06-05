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
using System.Linq;

using Empiria.Time;

namespace Empiria.CashFlow.Projections {

  /// <summary>Enumerates amortization methods.</summary>
  public enum AmortizationMethod {

    CuotaFija,

    CuotaVariable,

    None,

  }


  /// <summary>Holds parameters used to build amortization tables.</summary>
  public class AmortizationParameters {

    public FixedList<(int Month, decimal Amount)> Disbursements {
      get; set;
    }


    public decimal TotalAmount {
      get {
        return Disbursements.Sum(x => x.Amount);
      }
    }


    public YearMonth InitialPeriod {
      get; set;
    }

    public int RepaymentMonths {
      get; set;
    }


    public int TotalMonths {
      get {
        return InitialPeriod.Month + GraceMonths + RepaymentMonths;
      }
    }


    public decimal AnnualInterestRate {
      get; set;
    }

    public int GraceMonths {
      get; set;
    }

    public decimal DisbursementFee {
      get; internal set;
    }

    public decimal OpeningFee {
      get; set;
    }

    public bool CapitalizeFees {
      get; set;
    }

    internal void EnsureValid() {
      Assertion.Require(Disbursements, "Se requiere el campo con las variables mensuales de otorgamiento de crédito.");
      Assertion.Require(TotalAmount > 0, "El importe total del otorgamiento de crédito debe ser mayor a cero.");
      Assertion.Require(AnnualInterestRate > 0, "La tasa de interés debe ser mayor a cero.");
      Assertion.Require(RepaymentMonths > 0, "El plazo de amortización en meses debe ser mayor a cero.");
      Assertion.Require(GraceMonths >= 0, "Los meses de gracia deben ser mayores o iguales a cero.");
      Assertion.Require(DisbursementFee >= 0, "La comisión por desembolso debe ser un porcentaje mayor o igual a cero.");
      Assertion.Require(OpeningFee >= 0, "La comisión por apertura debe ser un porcentaje mayor o igual a cero.");
    }

  }  // class AmortizationParameters



  /// <summary>Service used to generate an amortization table for a given loan or credit.</summary>
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

      decimal balance = 0;

      for (int month = 1; month <= _params.GraceMonths; month++) {

        balance += GetMonthDisbursement(month);

        decimal monthInterest = Math.Round(balance * monthlyRate, 2);

        interest += monthInterest;
        balance += monthInterest;
      }

      return interest;
    }


    private decimal CalculateMonthFees(int month) {

      decimal amount = GetMonthDisbursement(month);

      int firstDisbursementMonth = _params.Disbursements
                                          .First(x => x.Amount != 0m)
                                          .Month;

      if (month == firstDisbursementMonth) {
        return Math.Round(amount * _params.OpeningFee / 100, 2);
      }

      return Math.Round(amount * _params.DisbursementFee / 100, 2);
    }


    private decimal CalculateMonthlyFixedPrincipalPayment(decimal amount, int month) {
      return Math.Round(amount / (_params.RepaymentMonths - month + 1), 2);
    }


    private decimal CalculateMonthlyInterestRate() {
      return _params.AnnualInterestRate / 100 / 12;
    }


    private FixedList<AmortizationTableEntry> GetCuotaFijaMonthlyTable() {

      var table = new List<AmortizationTableEntry>(_params.TotalMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal graceMonthsInterest = CalculateGraceMonthsInterest();

      decimal balance = graceMonthsInterest;

      for (int month = 1; month <= _params.TotalMonths; month++) {

        balance += GetMonthDisbursement(month);

        decimal monthlyPayment = CalculateFixedMonthlyPayment(balance);

        if (balance <= 0) {
          continue;
        }

        decimal interest = Math.Round(balance * monthlyRate, 2);
        decimal principal = Math.Round(monthlyPayment - interest, 2);
        decimal fees = CalculateMonthFees(month);

        if (month == _params.TotalMonths) {
          principal = balance;
        }

        balance = Math.Round(balance - principal, 2);

        table.Add(new AmortizationTableEntry {
          Month = month,
          Principal = principal,
          Interest = interest,
          Fees = fees,
          RemainingBalance = balance
        });

      }

      return table.ToFixedList();
    }


    private FixedList<AmortizationTableEntry> GetCuotaVariableMonthlyTable() {

      var table = new List<AmortizationTableEntry>(_params.RepaymentMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal balance = 0;
      decimal feesBalance = 0;

      decimal principalPayment = 0;

      for (int month = 2; month <= _params.TotalMonths; month++) {

        if (GetMonthDisbursement(month - 1) != 0) {

          balance += GetMonthDisbursement(month - 1);

          feesBalance += CalculateMonthFees(month - 1);

          if (_params.CapitalizeFees) {
            balance += feesBalance;
            feesBalance = 0;
          }

          principalPayment = CalculateMonthlyFixedPrincipalPayment(balance, month - 1);
        }

        if (month < _params.InitialPeriod.Month + _params.GraceMonths) {
          continue;
        }

        if (balance <= 0 && feesBalance <= 0) {
          continue;
        }

        decimal interest = Math.Round(balance * monthlyRate, 2);

        if (month == _params.TotalMonths) {
          principalPayment = balance;
        }

        balance = Math.Round(balance - principalPayment, 2);

        table.Add(new AmortizationTableEntry {
          Month = month,
          Principal = principalPayment,
          Interest = interest,
          Fees = feesBalance,
          RemainingBalance = balance
        });

        feesBalance = 0;
      }

      return table.ToFixedList();
    }


    private decimal GetMonthDisbursement(int month) {

      if (_params.Disbursements.Exists(x => x.Month == month)) {
        return _params.Disbursements.First(x => x.Month == month).Amount;
      }

      return 0;
    }


    #endregion Helpers

  }  // class AmortizationTable



  /// <summary>Information holder for an amortization table month entry.</summary>
  public class AmortizationTableEntry {

    public int Month {
      get; internal set;
    }


    public decimal MonthPayment {
      get {
        return Principal + Interest + Fees;
      }
    }

    public decimal Principal {
      get; internal set;
    }

    public decimal Interest {
      get; internal set;
    }

    public decimal Fees {
      get; internal set;
    }

    public decimal RemainingBalance {
      get; internal set;
    }


    internal void Sum(AmortizationTableEntry entry) {

      Principal += entry.Principal;
      Interest += entry.Interest;
      Fees += entry.Fees;
      RemainingBalance += entry.RemainingBalance;
    }

  }  // class AmortizationTableEntry

}  // namespace Empiria.CashFlow.Projections
