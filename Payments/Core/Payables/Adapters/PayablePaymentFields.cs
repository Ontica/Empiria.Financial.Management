/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Interface adapters                      *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields Input DTO                        *
*  Type     : PayablePaymentFields                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains fields in order to create or update a Payable payment data.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Contains fields in order to create or update a Payable payment data.</summary>
  public class PayablePaymentFields {

    #region Properties


    public string PaymentMethodUID {
      get; set;
    } = string.Empty;


    public string PaymentAccountUID {
      get; set;
    } = string.Empty;
    
    
    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(PaymentMethodUID,  "Necesito el identificador UID del método de pago");
      
      var paymentMethod = PaymentMethod.Parse(PaymentMethodUID);

      if (paymentMethod.LinkedToAccount) {
        Assertion.Require(PaymentAccountUID, "Necesito el identificador UID de la cuenta donde se debe realizar el pago.");
        _ = PaymentAccount.Parse(PaymentAccountUID);
      }
      
    }

    #endregion Methods

  }  // class PayablePaymentFields

}  // namespace Empiria.Payments.Payables.Adapters
