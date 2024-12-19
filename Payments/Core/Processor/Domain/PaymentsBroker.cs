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

      var response = SentToIkosCash(instruction);

      return new PaymentResultDto {
         Failed = EmpiriaMath.GetRandomBoolean(),
      };
    }

    #endregion Methods

    #region Helpers

    internal ResultadoTransaccionDto SentToIkosCash(IPaymentInstruction instruction) {
      IkosCashPaymentService paymentService = new IkosCashPaymentService();
    
      var transaction = LoadValues(instruction);

      var paymentTransaction = paymentService.AddPaymentTransaction(transaction).GetAwaiter().GetResult();

      if (paymentTransaction.Code != 0) {
        Assertion.EnsureNoReachThisCode($"Encontré el siguiente error en el simefin: {paymentTransaction.ErrorMesage}" );
      }

      return paymentTransaction;
    }


    private TransaccionFields LoadValues(IPaymentInstruction instruction) {
      var referencia = GetReferenceNumber();

      var transaction = new TransaccionFields();
      transaction.Header = new Header {
        IdSistemaExterno = "2024121942250",
        IdUsuario = 45,
        IdDepartamento = 40,
        IdConcepto = 418,
        ClaveCliente = "VON990614PG4",
        Cuenta = "012180001556696260",
        FechaOperacion = Convert.ToDateTime("2024-12-19T00:00:00"),
        FechaValor = Convert.ToDateTime("2024-12-19T00:00:00"),
        Monto = instruction.Total,
        Referencia = "42250",
        ConceptoPago = "Pago Banobras, S.N.C.",
        Origen = "O",
        Firma = "",
        SerieFirma = "NjnecxMTc5ZDE="
      };

      transaction.Payload = new Payload {
        InstitucionBen = "040012",
        ClaveRastreo = "",
        NomBen = "LA VIA ONTICA, S.C.",
        RfcBen = "VON990614PG4",
        TipoCtaBen = 40,
        CtaBen = "",
        Iva = 0.00m
      };

      return transaction;
    }

    private string GetReferenceNumber() {
      Random rng = new Random();
      return rng.Next(999999).ToString(); 
    }

    
    #endregion Helpers

  }  // class PaymentsBroker

}  // namespace Empiria.Payments.Processor
