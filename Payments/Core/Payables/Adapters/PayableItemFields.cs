/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Interface adapters                      *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields Input DTO                        *
*  Type     : PayableItemFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains fields in order to create or update a PayableItem.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

using Empiria.Budgeting;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Contains fields in order to create or update a PayableItem.</summary>
  public class PayableItemFields {

    #region Properties

    public string UID {
      get; set;
    } = string.Empty;


    public string PayableUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string UnitUID {
      get; set;
    } = string.Empty;


    public int EntityItemId {
      get; set;
    } = -1;

    public string Description {
      get; set;
    } = string.Empty;


    public decimal Quantity {
      get; set;
    }


    public decimal UnitPrice {
      get; set;
    }


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public decimal ExchangeRate {
      get; set;
    } = 1;


    public decimal Discount {
      get; set;
    } = 0;


    public string BudgetAccountUID {
      get; set;
    } = string.Empty;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(PayableUID, nameof(PayableUID));
      Assertion.Require(Quantity > 0, "Necesito la cantidad.");
      Assertion.Require(UnitPrice > 0, "Necesito el precio unitario.");

      Assertion.Require(CurrencyUID, "Necesito la moneda.");
      Assertion.Require(BudgetAccountUID, "Necesito el número de cuenta del presupuesto.");

      _ = Currency.Parse(CurrencyUID);
      _ = BudgetAccount.Parse(BudgetAccountUID);
    }

    #endregion Methods

  }  // class PayableItemFields

}  // namespace Empiria.Payments.Payables.Adapters
