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

    public bool CapitalizeInterest {
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

    static internal readonly decimal FEES_TAX = 0.16m;

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

    private decimal CalculateFixedMonthlyPayment(decimal balance, int month) {

      decimal monthlyRate = CalculateMonthlyInterestRate();

      int adjustedMonth = _params.TotalMonths - month + 1;

      // Formula: P * (r * (1+r)^n) / ((1+r)^n - 1)
      double factor = Math.Pow((double) (1 + monthlyRate), adjustedMonth);

      decimal monthlyPayment = balance * ((monthlyRate * (decimal) factor) / ((decimal) factor - 1));

      return Math.Round(monthlyPayment, 2);
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


    private decimal CalculateMonthlyFixedPrincipalPayment(decimal balance, int month) {

      int adjustedMonth = _params.TotalMonths - month + 1;

      return Math.Round(balance / adjustedMonth, 2);
    }


    private decimal CalculateMonthlyInterestRate() {
      return _params.AnnualInterestRate / 100 / 12;
    }


    private FixedList<AmortizationTableEntry> GetCuotaFijaMonthlyTable() {

      var table = new List<AmortizationTableEntry>(_params.TotalMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal balance = 0;
      decimal interestPayment = 0;
      decimal capitalizedInterest = 0;
      decimal principalPayment = 0;
      decimal feesBalance = 0;
      decimal capitalizedFees = 0;

      decimal monthlyPayment = 0;

      for (int month = 2; month <= _params.TotalMonths; month++) {

        var monthFees = CalculateMonthFees(month - 1);

        feesBalance += monthFees;

        if (GetMonthDisbursement(month - 1) != 0) {

          balance += GetMonthDisbursement(month - 1);

          if (_params.CapitalizeFees) {
            capitalizedFees = monthFees + (monthFees * FEES_TAX);
            balance += capitalizedFees;
          }

          if (_params.CapitalizeInterest) {
            balance += table.Find(x => x.Month == month - 1)?.CapitalizedInterest ?? 0;
          }

          monthlyPayment = CalculateFixedMonthlyPayment(balance, month);

          interestPayment = Math.Round(balance * monthlyRate, 2);

          if (_params.CapitalizeInterest) {
            capitalizedInterest = interestPayment;
          }

          principalPayment = Math.Round(monthlyPayment - interestPayment, 2);

        } else if (_params.CapitalizeInterest) {

          balance += table.Find(x => x.Month == month - 1)?.CapitalizedInterest ?? 0;

          monthlyPayment = CalculateFixedMonthlyPayment(balance, month);

          interestPayment = Math.Round(balance * monthlyRate, 2);

          capitalizedInterest = interestPayment;

          principalPayment = Math.Round(monthlyPayment - interestPayment, 2);

        } else {

          interestPayment = Math.Round(balance * monthlyRate, 2);
          principalPayment = Math.Round(monthlyPayment - interestPayment, 2);
        }

        if (month <= _params.InitialPeriod.Month + _params.GraceMonths) {
          continue;
        }

        if (balance <= 0 && feesBalance <= 0) {
          continue;
        }

        if (month == _params.TotalMonths) {
          principalPayment = balance;
        }

        balance = Math.Round(balance - principalPayment, 2);

        table.Add(new AmortizationTableEntry {
          Month = month,
          Principal = principalPayment,
          Interest = interestPayment,
          Fees = feesBalance,
          CapitalizedInterest = capitalizedInterest,
          CapitalizedFees = capitalizedFees,
          RemainingBalance = balance
        });

        feesBalance = 0;
        capitalizedInterest = 0;
        capitalizedFees = 0;
      }

      return table.ToFixedList();
    }


    private FixedList<AmortizationTableEntry> GetCuotaVariableMonthlyTable() {

      var table = new List<AmortizationTableEntry>(_params.RepaymentMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal balance = 0;
      decimal interestPayment = 0;
      decimal capitalizedInterest = 0;
      decimal principalPayment = 0;
      decimal feesBalance = 0;
      decimal capitalizedFees = 0;

      for (int month = 2; month <= _params.TotalMonths; month++) {

        var monthFees = CalculateMonthFees(month - 1);

        feesBalance += monthFees;

        if (GetMonthDisbursement(month - 1) != 0) {

          balance += GetMonthDisbursement(month - 1);

          if (_params.CapitalizeFees) {
            capitalizedFees = monthFees + (monthFees * FEES_TAX);
            balance += capitalizedFees;
          }

          if (_params.CapitalizeInterest) {
            balance += table.Find(x => x.Month == month - 1)?.CapitalizedInterest ?? 0;
          }

          principalPayment = CalculateMonthlyFixedPrincipalPayment(balance, month);

          interestPayment = Math.Round(balance * monthlyRate, 2);

          if (_params.CapitalizeInterest) {
            capitalizedInterest = interestPayment;
          }

        } else if (_params.CapitalizeInterest) {

          balance += table.Find(x => x.Month == month - 1)?.CapitalizedInterest ?? 0;

          principalPayment = CalculateMonthlyFixedPrincipalPayment(balance, month);

          interestPayment = Math.Round(balance * monthlyRate, 2);

          capitalizedInterest = interestPayment;

        } else {

          interestPayment = Math.Round(balance * monthlyRate, 2);
        }


        if (month <= _params.InitialPeriod.Month + _params.GraceMonths) {
          continue;
        }

        if (balance <= 0 && feesBalance <= 0) {
          continue;
        }

        if (month == _params.TotalMonths) {
          principalPayment = balance;
        }

        balance = Math.Round(balance - principalPayment, 2);

        table.Add(new AmortizationTableEntry {
          Month = month,
          Principal = principalPayment,
          Interest = interestPayment,
          Fees = feesBalance,
          CapitalizedInterest = capitalizedInterest,
          CapitalizedFees = capitalizedFees,
          RemainingBalance = balance
        });

        feesBalance = 0;
        capitalizedInterest = 0;
        capitalizedFees = 0;
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


    public decimal FeesTaxes {
      get {
        return Fees * AmortizationTable.FEES_TAX;
      }
    }

    public decimal CapitalizedInterest {
      get; internal set;
    }

    public decimal CapitalizedFees {
      get; internal set;
    }

    public decimal RemainingBalance {
      get; internal set;
    }

  }  // class AmortizationTableEntry

}  // namespace Empiria.CashFlow.Projections
