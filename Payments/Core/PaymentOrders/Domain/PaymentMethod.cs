/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : Payment Methods                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment method a service order.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;


namespace Empiria.Payments.Orders {

  /// <summary>Represents a payment method a service order.</summary>
  internal class PaymentMethod : GeneralObject {

    #region Constructors and parsers

    static internal PaymentMethod Parse(string uid) {
      return BaseObject.ParseKey<PaymentMethod>(uid);
    }


    static internal PaymentMethod Parse(int id) => BaseObject.ParseId<PaymentMethod>(id);


    static internal PaymentMethod Empty => BaseObject.ParseEmpty<PaymentMethod>();


    #endregion Constructors and parsers


    #region Public Methods

    static internal FixedList<PaymentMethod> GetList() {
      return BaseObject.GetList<PaymentMethod>()
                       .ToFixedList();
    }


    #endregion Public Methods

  }  // class Contract

}  // namespace Empiria.Payments.Contracts
