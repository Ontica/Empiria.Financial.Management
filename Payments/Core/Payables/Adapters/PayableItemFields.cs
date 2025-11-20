/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Interface adapters                      *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields Input DTO                        *
*  Type     : PayableItemFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains fields in order to create or update a PayableItem.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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


    public int EntityItemId {
      get; set;
    } = -1;


    public int EntityTypeId {
      get; set;
    } = -1;

    public decimal InputTotal {
      get; set;
    }


    public decimal OutputTotal {
      get; set;
    }


    public string CurrencyUID {
      get; set;
    }


    public decimal ExchangeRate {
      get; set;
    } = 0;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(PayableUID, nameof(PayableUID));
      Assertion.Require(CurrencyUID, "Necesito la moneda.");

      _ = FormerBudgetAccount.Parse(CurrencyUID);
    }

    #endregion Methods

  }  // class PayableItemFields

}  // namespace Empiria.Payments.Payables.Adapters
