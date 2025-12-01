/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.dll                       Pattern   : Information Holder                      *
*  Type     : PaymentOrderItemType                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment order item type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Represents a payment order item type.</summary>
  public class PaymentOrderItemType : GeneralObject {

    #region Constructors and parsers

    static internal PaymentOrderItemType Parse(string uid) => ParseKey<PaymentOrderItemType>(uid);

    static internal PaymentOrderItemType Parse(int id) => ParseId<PaymentOrderItemType>(id);

    static internal PaymentOrderItemType Empty => ParseEmpty<PaymentOrderItemType>();

    #endregion Constructors and parsers

  }  // class PaymentOrderItemType

}  // namespace Empiria.Payments
