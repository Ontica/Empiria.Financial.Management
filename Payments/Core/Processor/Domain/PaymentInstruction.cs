/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Abstract Information Holder             *
*  Type     : PaymentInstruction                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract class that represents an actual payment or a rejected payment instruction.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Orders;

namespace Empiria.Payments.Processor {

  /// <summary>Abstract class that represents an actual payment or a rejected payment instruction.</summary>
  abstract internal class PaymentInstruction : BaseObject {

    protected PaymentInstruction(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      Broker = broker;
      PaymentOrder = paymentOrder;
    }

    #region Properties

    public PaymentsBroker Broker {
      get; private set;
    }

    public PaymentOrder PaymentOrder {
      get; private set;
    }

    #endregion Properties

  }  // class PaymentInstruction

} // namespace Empiria.Payments.Processor
