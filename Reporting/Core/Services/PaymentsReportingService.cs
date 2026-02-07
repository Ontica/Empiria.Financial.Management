/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : PaymentsReportingService                      License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate payments reports.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.DynamicData;
using Empiria.Office;
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

    public DynamicDto<PaymentBillDto> BuildPaymentsBillsReport(DateTime fromDate, DateTime toEndDate) {

      var builder = new PaymentsBillsReportBuilder(fromDate, toEndDate);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<PaymentBillDto>(columns, entries);
    }


    public DynamicDto<PaymentConceptDto> BuildPaymentsConceptsReport(DateTime fromDate, DateTime toDate) {

      var builder = new PaymentsConceptsReportBuilder(fromDate, toDate);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<PaymentConceptDto>(columns, entries);
    }


    public FileDto ExportPaymentOrderToPdf(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      var templateUID = $"{GetType().Name}.ExportPaymentOrderToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new PaymentOrderVoucherBuilder(paymentOrder, templateConfig);

      return exporter.CreatePdf("payment.transactions", paymentOrder.PaymentOrderNo);
    }


    public FileDto ExportPaymentOrdersToExcel(FixedList<PaymentOrder> paymentOrders) {

      var templateUID = $"{GetType().Name}.ExportPaymentOrdersToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new PaymentOrdersToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(paymentOrders);

      return excelFile.ToFileDto();
    }

    #endregion Services

  } // class PaymentsReportingService

} // namespace Empiria.Payments.Reporting
