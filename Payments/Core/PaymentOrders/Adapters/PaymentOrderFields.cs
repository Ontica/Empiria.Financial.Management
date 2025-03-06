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
      Assertion.Require(PaymentOrderTypeUID, "Necesito el tipo de orden de pago.");
      _ = PaymentOrderType.Parse(PaymentOrderTypeUID);

      //Assertion.Require(PayableUID, "Necesito el objeto ligado al pago.");
      //_ = Payable.Parse(PayableUID);

      Assertion.Require(PaymentMethodUID, "Necesito el método de pago.");
      _ = PaymentMethod.Parse(PaymentMethodUID);

      Assertion.Require(CurrencyUID, "Necesito la moneda.");
      _ = Currency.Parse(CurrencyUID);

      //Assertion.Require(PaymentAccountUID, "Necesito el número de cuenta.");
    //  _ = PaymentAccount.Parse(PaymentAccountUID); ToDo

      Assertion.Require(Total > 0, "Necesito que el importe a pagar sea mayor a cero.");
    }

  }  // class PaymentOrderFields

  public class ManualPaymentOrderFields : PaymentOrderFields {

    public string ReferenceNumber1 {
      get; set;
    } = string.Empty;

  }

  }  // namespace Empiria.Payments.Orders.Adapters
