/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Imput Fields                            *
*  Type     : BudgetEntryFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create and update budget entries.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Input fields used to create and update budget entries.</summary>
  public class BudgetEntryFields {

    public string TransactionUID {
      get; set;
    } = string.Empty;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string BalanceColumnUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string ProductCode {
      get; set;
    } = string.Empty;


    public string ProductName {
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


    public decimal ProductQty {
      get; set;
    }

    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public int EntityTypeId {
      get; set;
    } = -1;


    public int EntityId {
      get; set;
    } = -1;


    public string OperationNo {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public int Year {
      get; set;
    }

    public int Month {
      get; set;
    }

    public int Day {
      get; set;
    }

    public decimal OriginalAmount {
      get; set;
    }

    public decimal Amount {
      get; set;
    }

    public decimal ExchangeRate {
      get; set;
    } = 1m;


    public string RelatedEntryUID {
      get; set;
    } = string.Empty;

  }  // class BudgetEntryFields


  /// <summary>Extension methods for BudgetEntryFields type.</summary>
  static internal class BudgetEntryFieldsExtensions {

    static public void EnsureIsValid(this BudgetEntryFields fields) {

      fields.TransactionUID = Patcher.CleanUID(fields.TransactionUID);
      fields.BudgetAccountUID = Patcher.CleanUID(fields.BudgetAccountUID);
      fields.BalanceColumnUID = Patcher.CleanUID(fields.BalanceColumnUID);
      fields.ProductUID = Patcher.CleanUID(fields.ProductUID);
      fields.ProductUnitUID = Patcher.CleanUID(fields.ProductUnitUID);
      fields.PartyUID = Patcher.CleanUID(fields.PartyUID);
      fields.ProjectUID = Patcher.CleanUID(fields.ProjectUID);
      fields.CurrencyUID = Patcher.CleanUID(fields.CurrencyUID);

      fields.Description = EmpiriaString.Clean(fields.Description);
      fields.Justification = EmpiriaString.Clean(fields.Justification);

      _ = BudgetAccount.Parse(fields.BudgetAccountUID);
      _ = BalanceColumn.Parse(fields.BalanceColumnUID);

      Assertion.Require(1 <= fields.Month && fields.Month <= 12,
                       "El mes debe estar entre 1 y 12.");

      Assertion.Require(fields.Amount != 0,
                        "El importe debe ser distinto a cero.");

      if (fields.OriginalAmount == 0) {
        fields.OriginalAmount = Math.Abs(fields.Amount);
      }

      if (fields.ProductUID.Length != 0) {
        _ = Product.Parse(fields.ProductUID);

        Assertion.Require(fields.ProductUnitUID.Length != 0,
                          "Se requiere la unidad de medida del producto.");
        Assertion.Require(fields.ProductQty > 0,
                          "La cantidad del producto debe ser mayor a cero.");
      }


      if (fields.ProductUnitUID.Length != 0) {
        _ = ProductUnit.Parse(fields.ProductUnitUID);
      }

      if (fields.ProjectUID.Length != 0) {
        _ = Project.Parse(fields.ProjectUID);
      }

      if (fields.PartyUID.Length != 0) {
        _ = Party.Parse(fields.PartyUID);
      }

      if (fields.CurrencyUID.Length != 0) {
        _ = Currency.Parse(fields.CurrencyUID);
      }

      if (fields.ExchangeRate == 0) {
        fields.ExchangeRate = 1;
      }

      if (fields.TransactionUID.Length == 0) {
        return;
      }

      var txn = BudgetTransaction.Parse(fields.TransactionUID);

      if (txn.TransactionType.AllowsMultiYearEntries) {
        Assertion.Require(fields.Year >= txn.BaseBudget.Year,
                        $"El año debe ser mayor o igual a {txn.BaseBudget.Year}.");

        Assertion.Require(txn.TransactionType.AvailableYears.Contains(fields.Year),
                          $"El año debe estar en la lista de años disponibles.");

      } else {
        Assertion.Require(fields.Year == txn.BaseBudget.Year,
                         $"El año debe ser igual a {txn.BaseBudget.Year}.");
      }
    }

  }  // class BudgetEntryFieldsExtensions

}  // namespace Empiria.Budgeting.Transactions
