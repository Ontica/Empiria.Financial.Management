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
using Empiria.Payments.Payables;

namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Fields structure used for create and update payment orders.</summary>
  public class PaymentOrderFields {

    public string ControlNo {
      get; set;
    } = string.Empty;


    public string ReferenceNumber {
      get; set;
    } = string.Empty;


    public string PaymentOrderTypeUID {
      get; set;
    } = string.Empty;


    public string PayableUID {
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


    public Decimal Total {
      get; set;
    }


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

      Assertion.Require(ControlNo, "Necesito el método de pago.");

      var paymentOrder = PaymentOrder.TryGetForControlNo(ControlNo);
      if (paymentOrder != null) {
        Assertion.EnsureNoReachThisCode($"Ya existe una orden de pago con el Número de Control: {ControlNo}");
      }

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


      Assertion.Require(Total > 0, "Necesito que el importe a pagar sea mayor a cero.");
    }

  }  // class PaymentOrderFields


  }  // namespace Empiria.Payments.Orders.Adapters
