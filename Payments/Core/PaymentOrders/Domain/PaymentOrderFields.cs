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
using Empiria.StateEnums;

namespace Empiria.Payments {

  /// <summary>Fields structure used for create and update payment orders.</summary>
  public class PaymentOrderFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string PaymentTypeUID {
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


    public decimal Total {
      get; set;
    }

    public string Description {
      get; set;
    } = string.Empty;


    public string Observations {
      get; set;
    } = string.Empty;


    public DateTime DueTime {
      get; set;
    } = ExecutionServer.DateMinValue;


    public Priority Priority {
      get; set;
    } = Priority.Normal;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public DateTime RequestedTime {
      get; set;
    } = ExecutionServer.DateMinValue;


    internal void EnsureValid() {

      UID = Patcher.CleanUID(UID);
      PaymentTypeUID = Patcher.CleanUID(PaymentTypeUID);
      PaymentMethodUID = Patcher.CleanUID(PaymentMethodUID);

      if (UID.Length != 0) {
        return;
      }

      Assertion.Require(PaymentTypeUID, "Necesito el tipo de pago.");
      Assertion.Require(PaymentMethodUID, "Necesito el método de pago.");

      var paymentMethod = PaymentMethod.Parse(PaymentMethodUID);

      if (paymentMethod.AccountRelated) {
        Assertion.Require(PaymentAccountUID, "Necesito el número de cuenta.");

        _ = PaymentAccount.Parse(PaymentAccountUID);

      } else {
        PaymentAccountUID = Patcher.CleanUID("Empty");
      }

      Assertion.Require(Total > 0, "El total a pagar debe ser mayor que cero.");
    }

  }  // class PaymentOrderFields

}  // namespace Empiria.Payments.Adapters
