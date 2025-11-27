/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields DTO                              *
*  Type     : PaymentOrderFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Fields structure used for create and update payment orders.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;

namespace Empiria.Payments.Orders {

  /// <summary>Fields structure used for create and update payment orders.</summary>
  public class PaymentOrderFields {

    public string PaymentOrderTypeUID {
      get; set;
    } = string.Empty;


    public string PayableEntityTypeUID {
      get; set;
    } = string.Empty;


    public string PayableEntityUID {
      get; set;
    } = string.Empty;


    public string ReferenceNumber {
      get; set;
    } = string.Empty;


    public string PayToUID {
      get; set;
    } = string.Empty;


    public string PaymentMethodUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string PaymentAccountUID {
      get; set;
    } = string.Empty;


    public string Notes {
      get; set;
    } = string.Empty;


    public DateTime DueTime {
      get; set;
    } = ExecutionServer.DateMinValue;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public DateTime RequestedTime {
      get; set;
    } = ExecutionServer.DateMinValue;


    internal void EnsureValid() {

      Assertion.Require(PaymentMethodUID, "Necesito el método de pago.");
      var paymentMethod = PaymentMethod.Parse(PaymentMethodUID);

      if (paymentMethod.LinkedToAccount == true) {
        Assertion.Require(PaymentAccountUID, "Necesito el número de cuenta.");
        _ = PaymentAccount.Parse(PaymentAccountUID);
      } else {
        PaymentAccountUID = "Empty";
      }

      Assertion.Require(CurrencyUID, "Necesito la moneda.");
      _ = Currency.Parse(CurrencyUID);

    }

  }  // class PaymentOrderFields


}  // namespace Empiria.Payments.Orders.Adapters
