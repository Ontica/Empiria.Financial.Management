/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentsBroker                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payments broker.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Represents a payments broker.</summary>
  internal class PaymentsBroker : GeneralObject, IPaymentsBroker {

    #region Constructors and parsers

    static public PaymentsBroker Parse(int id) => ParseId<PaymentsBroker>(id);

    static public PaymentsBroker Parse(string uid) => ParseKey<PaymentsBroker>(uid);

    static public FixedList<PaymentsBroker> GetList() {
      return BaseObject.GetList<PaymentsBroker>()
                       .ToFixedList();
    }

    static public PaymentsBroker Empty => ParseEmpty<PaymentsBroker>();

    static internal IPaymentsBroker SelectPaymentBroker(IPaymentInstruction paymentInstruction) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));

      return new PaymentsBroker();
    }

    #endregion Constructors and parsers

    public IPaymentResult Pay(IPaymentInstruction instruction) {
      return new PaymentResultDto {
         Failed = EmpiriaMath.GetRandomBoolean(),
      };
    }

  }  // class PaymentsBroker

}  // namespace Empiria.Payments.Processor
