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

using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;

using Empiria.Financial;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Input fields used to create and update budget entries.</summary>
  public class BudgetEntryFields {

    public string BudgetAccountUID {
      get; set;
    } = string.Empty;


    public string BalanceColumnUID {
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


    public decimal ProductQty {
      get; set;
    }

    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public int OperationTypeId {
      get; set;
    } = -1;


    public int OperationId {
      get; set;
    } = -1;


    public int BaseEntityItemId {
      get; set;
    } = -1;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public decimal OriginalAmount {
      get; set;
    }

    public decimal Amount {
      get; set;
    }

    public decimal ExchangeRate {
      get; internal set;
    } = 1m;


  }  // class BudgetEntryFields



  /// <summary>Extension methods for BudgetEntryFields type.</summary>
  internal static class BudgetEntryFieldsExtensions {

    static public void EnsureIsValid(this BudgetEntryFields fields) {
      fields.Description = EmpiriaString.Clean(fields.Description);
      fields.Justification = EmpiriaString.Clean(fields.Justification);

      _ = BudgetAccount.Parse(fields.BudgetAccountUID);
      _ = BudgetAccount.Parse(fields.BalanceColumnUID);

      Assertion.Require(fields.Amount != 0, "El importe debe ser distinto a cero.");

      if (fields.OriginalAmount == 0) {
        fields.OriginalAmount = Math.Abs(fields.Amount);
      }

      if (fields.ProductUID.Length != 0) {
        _ = Product.Parse(fields.ProductUID);
      }

      if (fields.ProductUID.Length != 0) {
        Assertion.Require(fields.ProductUnitUID.Length != 0, "Se requiere la unida de medida del producto.");
        Assertion.Require(fields.ProductQty > 0, "La cantidad del producto debe ser mayor a cero.");
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

    }

  }  // class BudgetEntryFieldsExtensions

}  // namespace Empiria.Budgeting.Transactions
