/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Common Storage Type                     *
*  Type     : PaymentType                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a payment type related to a payment order.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Describes a payment type related to a payment order.</summary>
  public class PaymentType : CommonStorage {

    #region Constructors and parsers

    static public PaymentType Parse(int id) => ParseId<PaymentType>(id);

    static public PaymentType Parse(string uid) => ParseKey<PaymentType>(uid);

    static public PaymentType Empty => ParseEmpty<PaymentType>();

    static public FixedList<PaymentType> GetList() {
      return GetStorageObjects<PaymentType>();
    }

    #endregion Constructors and parsers

    #region Properties

    public string Prefix {
      get {
        return ExtData.Get("prefix", "PG");
      }
    }

    #endregion Properties

  }  // class PaymentType

}  // namespace Empiria.Payments
