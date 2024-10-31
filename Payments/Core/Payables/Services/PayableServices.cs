/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Service Layer                           *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service                                 *
*  Type     : PayableServices                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service to provide operations over Payable instances.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Payables.Services {

  /// <summary>Service to provide operations over Payable instances.</summary>
  static internal class PayableServices {

    #region Methods

    static public void SetAsPayed(Payable payable) {
      Assertion.Require(payable, nameof(payable));

      payable.Pay();
      payable.Save();
    }


    static public void SetOnPayment(Payable payable) {
      Assertion.Require(payable, nameof(payable));

      payable.OnPayment();
      payable.Save();
    }

    #endregion Methods

  }  // class PayableServices

}  // namespace Empiria.Payments.Payables.Services
