/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.dll                       Pattern   : Information Holder                      *
*  Type     : PaymentOrderType                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment order type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Orders {

  /// <summary>Represents a payment order type.</summary>
  public class PaymentOrderItemType : GeneralObject {

    #region Constructors and parsers

    static internal PaymentOrderItemType Parse(string uid) {
      return BaseObject.ParseKey<PaymentOrderItemType>(uid);
    }

    static internal PaymentOrderItemType Parse(int id) => BaseObject.ParseId<PaymentOrderItemType>(id);

    static internal PaymentOrderItemType Empty => BaseObject.ParseEmpty<PaymentOrderItemType>();

    #endregion Constructors and parsers

    #region Public Methods

    #endregion Public Methods

  }  // class PaymentOrderType

}  // namespace Empiria.Payments.Orders
