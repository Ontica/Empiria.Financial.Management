/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentsBroker                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payments broker.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Reflection;

using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Represents a payments broker.</summary>
  internal class PaymentsBroker : GeneralObject {

    #region Constructors and parsers

    static public PaymentsBroker Parse(int id) => ParseId<PaymentsBroker>(id);

    static public PaymentsBroker Parse(string uid) => ParseKey<PaymentsBroker>(uid);

    static public FixedList<PaymentsBroker> GetList() {
      return BaseObject.GetList<PaymentsBroker>()
                       .ToFixedList();
    }

    static public PaymentsBroker Empty => ParseEmpty<PaymentsBroker>();


    static internal PaymentsBroker GetPaymentsBroker(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      return Parse(750);
    }

    #endregion Constructors and parsers

    #region Properties


    private string ServiceAssemblyName {
      get {
        return base.ExtendedDataField.Get<string>("serviceAssemblyName");
      }
    }

    private string ServiceTypeName {
      get {
        return base.ExtendedDataField.Get<string>("serviceTypeName");
      }
    }

    #endregion Properties

    #region Methods

    public IPaymentsBroker GetService() {
      Type brokerServiceType = ObjectFactory.GetType(ServiceAssemblyName,
                                                     ServiceTypeName);

      return (IPaymentsBroker) ObjectFactory.CreateObject(brokerServiceType);
    }

    #endregion Methods

  }  // class PaymentsBroker

}  // namespace Empiria.Payments.Processor
