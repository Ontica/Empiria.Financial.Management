/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : PaymentsReportingService                      License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate payments reports.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Storage;

namespace Empiria.Payments.Reporting {

  /// <summary>Provides services used to generate payments reports.</summary>
  public class PaymentsReportingService : Service {

    #region Constructors and parsers

    private PaymentsReportingService() {
      // no-op
    }

    static public PaymentsReportingService ServiceInteractor() {
      return CreateInstance<PaymentsReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportPaymentOrderToPdf(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      var templateUID = $"{GetType().Name}.ExportPaymentOrderToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new PaymentOrderVoucherBuilder(paymentOrder, templateConfig);

      return exporter.CreatePdf("payment.transactions", paymentOrder.PaymentOrderNo);
    }

    #endregion Services

  } // class PaymentsReportingService

} // namespace Empiria.Payments.Reporting
