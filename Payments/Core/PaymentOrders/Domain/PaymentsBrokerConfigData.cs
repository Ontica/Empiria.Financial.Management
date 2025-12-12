/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Common Storage Type                     *
*  Type     : PaymentsBrokerConfigData                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a payments broker's configuration data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Reflection;

using Empiria.Payments.Processor;

namespace Empiria.Payments {

  /// <summary>Holds a payments broker's configuration data.</summary>
  public class PaymentsBrokerConfigData : CommonStorage {

    #region Constructors and parsers

    static public PaymentsBrokerConfigData Parse(int id) => ParseId<PaymentsBrokerConfigData>(id);

    static public PaymentsBrokerConfigData Parse(string uid) => ParseKey<PaymentsBrokerConfigData>(uid);

    static public FixedList<PaymentsBrokerConfigData> GetList() {
      return GetStorageObjects<PaymentsBrokerConfigData>();
    }

    static public PaymentsBrokerConfigData Default {
      get {
        return GetList().Find(broker => broker.IsDefault)
                            ?? throw new InvalidOperationException("No default payments broker is defined.");
      }
    }

    static internal PaymentsBrokerConfigData GetPaymentsBroker(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      // ToDo: For now, we only have one broker. Substitute this logic when multiple brokers are supported.
      return Default;
    }

    static public PaymentsBrokerConfigData Empty => ParseEmpty<PaymentsBrokerConfigData>();

    #endregion Constructors and parsers

    #region Properties

    public bool IsDefault {
      get {
        return ExtData.Get<bool>("isDefault", false);
      }
    }


    private string ServiceAssemblyName {
      get {
        return ExtData.Get<string>("serviceAssemblyName");
      }
    }


    private string ServiceTypeName {
      get {
        return ExtData.Get<string>("serviceTypeName");
      }
    }

    #endregion Properties

    #region Methods

    public IPaymentsBrokerService GetService() {
      Type brokerServiceType = ObjectFactory.GetType(ServiceAssemblyName,
                                                     ServiceTypeName);

      return (IPaymentsBrokerService) ObjectFactory.CreateObject(brokerServiceType);
    }

    #endregion Methods

  }  // class PaymentsBrokerConfigData

}  // namespace Empiria.Payments
