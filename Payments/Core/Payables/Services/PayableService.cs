/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Service Layer                           *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service                                 *
*  Type     : PayableService                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service to provide payment operations to payable.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Payments.Orders.Adapters;
using Empiria.Payments.Orders.UseCases;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Data;
using Empiria.Services;

namespace Empiria.Payments.Payables.Services {

  /// <summary>Service to provide payment operations to payable.</summary>

  static internal class PayableService  {


    #region Methods

    static public void SetOnPayment(Payable payable) {

      payable.OnPayment();
      payable.Save();
    }


    static public void SetPay(Payable payable) {

      payable.Pay();
      payable.Save();
    }


    #endregion Methods


  }  // class PayableService

}  // namespace Empiria.Payments.Payables.Services
