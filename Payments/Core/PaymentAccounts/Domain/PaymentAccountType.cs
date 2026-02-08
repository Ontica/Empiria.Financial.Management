/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Common Storage Type                   *
*  Type     : PaymentAccountType                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the type of a payment account.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Describes the type of a payment account.</summary>
  public class PaymentAccountType : CommonStorage {

    #region Constructors and parsers

    static public PaymentAccountType Parse(int id) => ParseId<PaymentAccountType>(id);

    static public PaymentAccountType Parse(string uid) => ParseKey<PaymentAccountType>(uid);

    static public PaymentAccountType Empty => ParseEmpty<PaymentAccountType>();

    static public FixedList<PaymentAccountType> GetList() {
      return GetStorageObjects<PaymentAccountType>();
    }

    #endregion Constructors and parsers

  }  // class PaymentAccountType

}  // namespace Empiria.Payments
