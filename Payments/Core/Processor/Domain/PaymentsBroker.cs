/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentsBroker                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payments broker.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.BanobrasIntegration.IkosCash;
using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

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

    #region Methods

    public IPaymentResult Pay(IPaymentInstruction instruction) {

      var response = SendToIkosCash(instruction);

      return new PaymentResultDto {
         Failed = EmpiriaMath.GetRandomBoolean(),
      };
    }

    #endregion Methods

    #region Helpers

    internal ResultadoTransaccionDto SendToIkosCash(IPaymentInstruction instruction) {
      var paymentService = new IkosCashPaymentService();

      var transaction = new TransaccionFields();

      var paymentTransaction = paymentService.SendPaymentTransaction(transaction).Result;

      if (paymentTransaction.Code != 0) {
        Assertion.EnsureNoReachThisCode($"Encontré el siguiente error en el simefin: {paymentTransaction.ErrorMesage}" );
      }

      return paymentTransaction;
    }

    #endregion Helpers

  }  // class PaymentsBroker

}  // namespace Empiria.Payments.Processor
