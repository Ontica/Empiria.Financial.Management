/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Common Storage Type                     *
*  Type     : PaymentsBroker                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payments broker.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Reflection;

namespace Empiria.Payments.Processor {

  /// <summary>Represents a payments broker.</summary>
  internal class PaymentsBroker : CommonStorage {

    #region Constructors and parsers

    static public PaymentsBroker Parse(int id) => ParseId<PaymentsBroker>(id);

    static public PaymentsBroker Parse(string uid) => ParseKey<PaymentsBroker>(uid);

    static public FixedList<PaymentsBroker> GetList() {
      return GetStorageObjects<PaymentsBroker>();
    }

    static public PaymentsBroker Default {
      get {
        return GetList().Find(broker => broker.IsDefault)
                            ?? throw new InvalidOperationException("No default payments broker is defined.");
      }
    }

    static internal PaymentsBroker GetPaymentsBroker(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      // ToDo: For now, we only have one broker. Substitute this logic when multiple brokers are supported.
      return Default;
    }

    static public PaymentsBroker Empty => ParseEmpty<PaymentsBroker>();

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

  }  // class PaymentsBroker

}  // namespace Empiria.Payments.Processor
