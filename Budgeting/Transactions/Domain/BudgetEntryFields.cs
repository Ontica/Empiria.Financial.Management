/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Imput Fields                            *
*  Type     : BudgetEntryFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create and update budget entries.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Products;

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


    public decimal Deposit {
      get; set;
    }


    public decimal Withdrawal {
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

      if (fields.BudgetAccountUID.Length == 0) {
        _ = BudgetAccount.Parse(fields.BudgetAccountUID);
      }

      if (fields.BalanceColumnUID.Length == 0) {
        _ = BudgetAccount.Parse(fields.BalanceColumnUID);
      }

      if (fields.ProductUID.Length == 0) {
        _ = Product.Parse(fields.ProductUID);
      }

      if (fields.CurrencyUID.Length == 0) {
        _ = Currency.Parse(fields.CurrencyUID);
      }

      Assertion.Require(fields.Deposit > 0 ^ fields.Withdrawal > 0,
                        "Uno de los importes de ingreso o egreso debe ser mayor a cero.");

    }

  }  // class BudgetEntryFieldsExtensions

}  // namespace Empiria.Budgeting.Transactions
