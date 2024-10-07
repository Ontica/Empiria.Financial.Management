/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.dll                       Pattern   : Infomation Holder                       *
*  Type     : PaymentOrderType                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment order type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Payments.Orders {

  /// <summary>Represents a payment order type.</summary>
  public class PaymentOrderType : GeneralObject {

    #region Constructors and parsers

    static internal PaymentOrderType Parse(string uid) {
      return BaseObject.ParseKey<PaymentOrderType>(uid);
    }

    static internal PaymentOrderType Parse(int id) => BaseObject.ParseId<PaymentOrderType>(id);

    static internal PaymentOrderType Empty => BaseObject.ParseEmpty<PaymentOrderType>();

    #endregion Constructors and parsers

    #region Public Methods

    static internal FixedList<PaymentOrderType> GetList() {
      return BaseObject.GetList<PaymentOrderType>()
                       .ToFixedList();
    }


    #endregion Public Methods

  }  // class PaymentOrderType

}  // namespace Empiria.Payments.Contracts
