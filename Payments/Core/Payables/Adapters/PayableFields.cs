/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields DTO                              *
*  Type     : PayableFields                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for create and update payable objects.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Input fields DTO used for create and update payable objects.</summary>
  public class PayableFields {

    #region Properties

    public string PayableTypeUID {
      get; set;
    } = string.Empty;


    public string PayableEntityUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string ExchangeRateTypeUID {
      get; set;
    } = string.Empty;


    public decimal ExchangeRate {
      get; set;
    } = 1;


    public string PaymentMethodUID {
      get; set;
    } = string.Empty;


    public string PaymentAccountUID {
      get; set;
    } = string.Empty;


    public DateTime DueTime {
      get; set;
    } = ExecutionServer.DateMaxValue;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {

      Assertion.Require(PaymentMethodUID, "Necesito el método de pago.");

      var paymentMethod = PaymentMethod.Parse(PaymentMethodUID);

      if (paymentMethod.LinkedToAccount) {
        Assertion.Require(PaymentAccountUID, "Necesito se proporcione la cuenta donde se realizará el pago.");
        _ = PaymentAccount.Parse(PaymentAccountUID);
      }
    }

    #endregion Methods

  }  // class PayableFields

}  // namespace Empiria.Payments.Payables.Adapters
