/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Interface adapters                      *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Input Fields DTO                        *
*  Type     : PaymentOrderItemFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains fields in order to create or update a payment order items.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Contains fields in order to create or update a payment order item.</summary>
  public class PaymentOrderItemFields {

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
    } = string.Empty;


    public decimal ExchangeRate {
      get; set;
    } = 0;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(PayableUID, nameof(PayableUID));

    }

    #endregion Methods

  }  // class PaymentOrderItemFields

}  // namespace Empiria.Payments
