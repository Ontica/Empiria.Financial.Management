/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Imput Fields                            *
*  Type     : BudgetEntryByYearFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create and update a budget entries for a whole year.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Products;
using Empiria.Projects;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Input fields used to create and update a budget entries for a whole year.</summary>
  public class BudgetEntryByYearFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string TransactionUID {
      get; set;
    } = string.Empty;


    public string BalanceColumnUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string ProductUnitUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public int Year {
      get; set;
    }

    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public BudgetMonthEntryFields[] Amounts {
      get; set;
    }

  }  // BudgetEntryByYearFields



  /// <summary>Input fields used to create and update a budget entry amounts for a month.</summary>
  public class BudgetMonthEntryFields {

    public string BudgetEntryUID {
      get; set;
    }

    public int Month {
      get; set;
    }

    public decimal ProductQty {
      get; set;
    }

    public decimal Amount {
      get; set;
    }

  }  // class BudgetMonthEntryFields



  /// <summary>Extension methods for BudgetEntryByYearFields type.</summary>
  static internal class BudgetEntryByYearFieldsExtensions {

    static public void EnsureIsValid(this BudgetEntryByYearFields fields) {
      fields.Description = EmpiriaString.Clean(fields.Description);

      _ = BudgetTransaction.Parse(fields.TransactionUID);
      _ = BudgetAccount.Parse(fields.BudgetAccountUID);
      _ = BalanceColumn.Parse(fields.BalanceColumnUID);

      if (fields.ProductUID.Length != 0) {
        _ = Product.Parse(fields.ProductUID);

        Assertion.Require(fields.ProductUnitUID.Length != 0, "Se requiere la unidad de medida del producto.");

        _ = ProductUnit.Parse(fields.ProductUnitUID);
      }


      if (fields.ProjectUID.Length != 0) {
        _ = Project.Parse(fields.ProjectUID);
      }


      if (fields.CurrencyUID.Length != 0) {
        _ = Currency.Parse(fields.CurrencyUID);
      }

      Assertion.Require(fields.Amounts.Length > 0, "Requiero se proporcione cuando menos el importe de un mes.");

      foreach (var amount in fields.Amounts) {
        amount.EnsureIsValid(fields.ProductUID.Length > 0);
      }
    }

  }  // class BudgetEntryByYearFieldsExtensions



  /// <summary>Extension methods for BudgetMonthEntryFields type.</summary>
  static internal class BudgetMonthEntryFieldsExtensions {

    static public void EnsureIsValid(this BudgetMonthEntryFields fields, bool hasProduct) {

      Assertion.Require(1 <= fields.Month && fields.Month <= 12,
                        $"No reconozco el valor para el mes {fields.Month}.");

      string monthName = EmpiriaString.MonthName(fields.Month);

      Assertion.Require(fields.Amount >= 0, $"El importe asignado a {monthName} debe ser mayor o igual a cero.");

      if (fields.Amount == 0 && fields.ProductQty == 0) {
        return;
      }

      if (hasProduct) {
        Assertion.Require(fields.Amount > 0 && fields.ProductQty > 0,
            $"Requiero se proporcione la cantidad del producto o servicio para el mes de {monthName}.");

      } else {
        Assertion.Require(fields.Amount > 0 && fields.ProductQty == 0,
            $"Este registro no tiene asociado un producto, por lo que requiero que " +
            $"la cantidad del producto o servicio asignada al mes de {monthName} sea cero.");

      }
    }

  }  // class BudgetMonthEntryFieldsExtensions

}  // namespace Empiria.Budgeting.Transactions
