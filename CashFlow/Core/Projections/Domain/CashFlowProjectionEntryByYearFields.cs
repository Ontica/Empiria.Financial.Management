/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Imput Fields                            *
*  Type     : CashFlowProjectionEntryByYearFields        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create and update a cash flow projection's account for a whole year.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Products;

namespace Empiria.CashFlow.Projections {

  /// <summary>Input fields used to create and update a cash flow
  /// projection's account for a whole year.</summary>
  public class CashFlowProjectionEntryByYearFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string ProjectionUID {
      get; set;
    } = string.Empty;


    public string ProjectionColumnUID {
      get; set;
    } = string.Empty;


    public string CashFlowAccountUID {
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


    public int Year {
      get; set;
    }

    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public CashFlowProjectionMonthEntryFields[] Amounts {
      get; set;
    } = new CashFlowProjectionMonthEntryFields[0];

  }  // CashFlowProjectionEntryByYearFields



  /// <summary>Extension methods for CashFlowProjectionEntryByYearFields type.</summary>
  static internal class CashFlowProjectionEntryByYearFieldsExtensions {

    static public void EnsureValid(this CashFlowProjectionEntryByYearFields fields) {
      fields.Description = EmpiriaString.Clean(fields.Description);

      _ = CashFlowProjection.Parse(fields.ProjectionUID);
      _ = FinancialAccount.Parse(fields.CashFlowAccountUID);
      _ = CashFlowProjectionColumn.Parse(fields.ProjectionColumnUID);

      if (fields.ProductUID.Length != 0) {
        _ = Product.Parse(fields.ProductUID);

        Assertion.Require(fields.ProductUnitUID.Length != 0,
                          "Se requiere la unidad de medida del producto.");

        _ = ProductUnit.Parse(fields.ProductUnitUID);
      }


      if (fields.CurrencyUID.Length != 0) {
        _ = Currency.Parse(fields.CurrencyUID);
      }

      Assertion.Require(fields.Amounts.Length > 0,
                        "Requiero se proporcione cuando menos el importe de un mes.");

      foreach (var amount in fields.Amounts) {
        amount.EnsureValid(fields.ProductUID.Length > 0);
      }
    }

  }  // class CashFlowProjectionEntryByYearFieldsExtensions



  /// <summary>Input fields used to create and update a cash flow projection entry amount for a month.</summary>
  public class CashFlowProjectionMonthEntryFields {

    public string EntryUID {
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

  }  // class CashFlowProjectionMonthEntryFields



  /// <summary>Extension methods for CashFlowProjectionMonthEntryFields type.</summary>
  static internal class CashFlowProjectionMonthEntryFieldsExtensions {

    static public void EnsureValid(this CashFlowProjectionMonthEntryFields fields, bool hasProduct) {

      Assertion.Require(1 <= fields.Month && fields.Month <= 12,
                        $"No reconozco el valor para el mes {fields.Month}.");

      string monthName = EmpiriaString.MonthName(fields.Month);

      Assertion.Require(fields.Amount >= 0,
                        $"El importe asignado a {monthName} debe ser mayor o igual a cero.");

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

  }  // class CashFlowProjectionMonthEntryFieldsExtensions

}  // namespace Empiria.CashFlow.Projections
