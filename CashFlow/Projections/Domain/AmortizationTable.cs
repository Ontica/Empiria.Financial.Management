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


  /// <summary>Generates an amortization table for a given loan or credit.</summary>
  public class AmortizationTable {

    public AmortizationTable(decimal amount, decimal annualInterestRate, int repaymentMonths, int graceMonths = 0) {
      Assertion.Require(amount > 0, "El importe debe ser mayor a cero.");
      Assertion.Require(annualInterestRate > 0, "La tasa de interés debe ser mayor a cero.");
      Assertion.Require(repaymentMonths > 0, "El plazo de amortización en meses debe ser mayor a cero.");

      Amount = amount;
      AnnualInterestRate = annualInterestRate;
      RepaymentMonths = repaymentMonths;
      GraceMonths = graceMonths;
    }


    #region Properties

    public decimal Amount {
      get; private set;
    }

    public decimal AnnualInterestRate {
      get; private set;
    }

    public int GraceMonths {
      get; private set;
    }

    public int RepaymentMonths {
      get; private set;
    }

    public int TotalMonths {
      get {
        return RepaymentMonths + GraceMonths;
      }
    }

    #endregion Properties

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
      double factor = Math.Pow((double) (1 + monthlyRate), RepaymentMonths);

      decimal monthlyPayment = amount * ((monthlyRate * (decimal) factor) / ((decimal) factor - 1));

      return Math.Round(monthlyPayment, 2);
    }


    private decimal CalculateGraceMonthsInterest() {

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal interest = 0;

      decimal balance = Amount;

      for (int month = 1; month <= GraceMonths; month++) {

        decimal monthInterest = Math.Round(balance * monthlyRate, 2);

        interest += monthInterest;
        balance += monthInterest;
      }

      return interest;
    }


    private decimal CalculateMonthlyFixedPrincipalPayment(decimal amount) {
      return Math.Round(amount / RepaymentMonths, 2);
    }


    private decimal CalculateMonthlyInterestRate() {
      return AnnualInterestRate / 100 / 12;
    }


    private FixedList<AmortizationTableEntry> GetCuotaFijaMonthlyTable() {

      var table = new List<AmortizationTableEntry>(RepaymentMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal graceMonthsInterest = CalculateGraceMonthsInterest();

      decimal balance = Amount + graceMonthsInterest;

      decimal monthlyPayment = CalculateFixedMonthlyPayment(balance);

      for (int month = 1; month <= RepaymentMonths; month++) {

        if (balance <= 0) {
          break;
        }

        decimal interest = Math.Round(balance * monthlyRate, 2);
        decimal principal = Math.Round(monthlyPayment - interest, 2);

        if (month == RepaymentMonths) {
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

      var table = new List<AmortizationTableEntry>(RepaymentMonths);

      decimal monthlyRate = CalculateMonthlyInterestRate();

      decimal graceMonthsInterest = CalculateGraceMonthsInterest();

      decimal balance = Amount + graceMonthsInterest;

      decimal principalPayment = CalculateMonthlyFixedPrincipalPayment(balance);

      for (int month = 1; month <= RepaymentMonths; month++) {

        if (balance <= 0) {
          break;
        }

        decimal interest = Math.Round(balance * monthlyRate, 2);
        decimal principal = principalPayment;

        if (month == RepaymentMonths) {
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
