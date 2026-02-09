/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Common Storage Type                   *
*  Type     : PaymentMethod                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a payment method.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Represents a payment method.</summary>
  public class PaymentMethod : CommonStorage {

    #region Constructors and parsers

    static public PaymentMethod Parse(int id) => ParseId<PaymentMethod>(id);

    static public PaymentMethod Parse(string uid) => ParseKey<PaymentMethod>(uid);

    static public PaymentMethod Empty => ParseEmpty<PaymentMethod>();

    static public FixedList<PaymentMethod> GetList() {
      return GetStorageObjects<PaymentMethod>();
    }

    #endregion Constructors and parsers

    #region Properties

    public bool AccountRelated {
      get {
        return ExtData.Get("accountRelated", false);
      }
    }


    public string BrokerCode {
      get {
        return ExtData.Get("brokerCode", string.Empty);
      }
    }


    public bool IsElectronic {
      get {
        return BrokerCode.Length != 0;
      }
    }

    #endregion Properties

  }  // class PaymentMethod

}  // namespace Empiria.Payments
