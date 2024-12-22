/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Services interactor class               *
*  Type     : PaymentService                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides payment services using external services providers.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Adapters;
using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;
using System;
using Empiria.Payments.BanobrasIntegration.IkosCash;
using System.Threading.Tasks;
using Empiria.Data;


namespace Empiria.Payments.Processor.Services {

  internal class PaymentService : Service {

    #region Constructors and parsers

    protected PaymentService() {
      // no-op
    }

    static public PaymentService ServiceInteractor() {
      return CreateInstance<PaymentService>();
    }

    #endregion Constructors and parsers

    #region Services

    internal PaymentInstruction Pay(PaymentOrder paymentOrder) {
      IPaymentInstruction paymentInstruction = PaymentInstructionMapper.Map(paymentOrder);

      IPaymentsBroker brokerProvider = PaymentsBroker.SelectPaymentBroker(paymentInstruction);

      IPaymentResult paymentResult = brokerProvider.Pay(paymentInstruction);

      var broker = PaymentsBroker.Parse("5800f993-2db1-4aa8-aaad-5bd86db24c78");

      //SentTransactonToIkosCash();

      if (paymentResult.Failed) {
        return RejectedPayment(paymentOrder, paymentResult, broker);

      } else {
        return SuccessfullPayment(paymentOrder, paymentResult, broker);
      }
    }
           

    internal PaymentInstruction ValidateIsPaymentInstructionPayed(string paymentInstructionUD) {
      PaymentInstruction paymentInstruction = PaymentInstruction.Parse(paymentInstructionUD);
      WritePaymentLog(paymentInstruction);

      return paymentInstruction;
    }

    


    #endregion Services

    #region Helpers

    private static PaymentInstruction RejectedPayment(PaymentOrder paymentOrder,
                                                 IPaymentResult paymentResult,
                                                 PaymentsBroker broker) {

      var rejectedPaymentData = new RejectedPaymentData(paymentResult);

      var rejectedPayment = new RejectedPayment(broker, paymentOrder, rejectedPaymentData);

      rejectedPayment.Save();

      return rejectedPayment;
    }


    static private PaymentInstruction SuccessfullPayment(PaymentOrder paymentOrder,
                                                    IPaymentResult paymentResult,
                                                    PaymentsBroker broker) {

      var successfulPaymentData = new SuccessfulPaymentData(paymentResult);

      var payment = new SuccessfulPayment(broker, paymentOrder, successfulPaymentData);

      payment.Save();

      return payment;
    }


    static private void WritePaymentLog(PaymentInstruction paymentInstruction) {

      var paymentLog = new PaymentLog(paymentInstruction);
      paymentLog.RequestTime = System.DateTime.Now;
      paymentLog.ApplicationTime = System.DateTime.Now; 
      paymentLog.RecordingTime = System.DateTime.Now;
      paymentLog.Status = 'P';
      paymentLog.Save();  

    }

    #region TemporalIkosCashServices

    internal string GetToken() {
      IkosCashPaymentService paymentService = new IkosCashPaymentService();
     
      return paymentService.GetToken().Result;
    }


    internal string GetFirma() {
      var transaction = SetValuesOntica();

      IkosCashPaymentService paymentService = new IkosCashPaymentService();
      var organizationConcepts = paymentService.GetOrganizationUnitConcepts(1).Result;

      var firma = paymentService.GetFirma(transaction);

      return firma;
    }


    internal FixedList<OrganizationUnitDto> GetIkosOrganizationUnitConcepts() {
      IkosCashPaymentService paymentService = new IkosCashPaymentService();
      var organizationConcepts =  paymentService.GetOrganizationUnitConcepts(1).Result;

      return organizationConcepts.ToFixedList();
    }


    internal PaymentStatusResultDto GetIkosPaymentStatus(string paymentCode) {
      IkosCashPaymentService paymentService = new IkosCashPaymentService();
      var paymentStatus = paymentService.GetPaymentsStatus(paymentCode).Result;

      return paymentStatus;
    }


    internal  ResultadoTransaccionDto SentTransactonToIkosCash() {
      IkosCashPaymentService paymentService = new IkosCashPaymentService();

      var transaction = SetValuesOntica();
      
      var paymentTransaction = paymentService.AddPaymentTransaction(transaction).GetAwaiter().GetResult();

      if (paymentTransaction.Code != 0) {
        Assertion.EnsureNoReachThisCode($"Encontré el siguiente error en el simefin: {paymentTransaction.ErrorMesage}");
      }

      return paymentTransaction;
    }


    private TransaccionFields SetValuesOntica() {
      var transaction = new TransaccionFields();
      transaction.Header = new Header {
        IdSistemaExterno = "2024122050003",
        IdUsuario = 45,
        IdDepartamento = 40,
        IdConcepto = 418,
        ClaveCliente = "VON990614PG4",
        Cuenta = "012180001556696260",
        FechaOperacion = Convert.ToDateTime("2024-12-20T00:00:00"),
        FechaValor = Convert.ToDateTime("2024-12-20T00:00:00"),
        Monto = 10000.00m,
        Referencia = "50003",
        ConceptoPago = "Pago Banobras, S.N.C.",
        Origen = "O",
        Firma = "",
        SerieFirma = "NjcxMTc5ZDE="
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

    #endregion TemporalIkosCashServices


    #endregion Helpers

  }  // class PaymentService

} // namespace Empiria.Payments.Processor.Services
