/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PaymentsReportsController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to build payments reports and export them to Excel.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Reporting;

namespace Empiria.Payments.WebApi {

  /// <summary>Web API used to build payments reports and export them to Excel.</summary>
  public class PaymentsReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v2/financial-management/reports/payments-bills")]
    public SingleObjectModel BuildPaymentsBillsReport([FromBody] ReportFields fields) {

      using (var service = PaymentsReportingService.ServiceInteractor()) {

        var paymentsBills = service.BuildPaymentsBillsReport(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, paymentsBills);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/payments-concepts")]
    public SingleObjectModel BuildPaymentsConceptsReport([FromBody] ReportFields fields) {

      using (var service = PaymentsReportingService.ServiceInteractor()) {

        var paymentsBills = service.BuildPaymentsConceptsReport(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, paymentsBills);
      }
    }

    #endregion Web apis

  }  // class PaymentsReportsController



  /// <summary>Base input fields for financial reports.</summary>
  public class ReportFields {

    public DateTime FromDate {
      get; set;
    } = DateTime.Today;


    public DateTime ToDate {
      get; set;
    } = DateTime.Today;

  }  // class ReportFields

}  // namespace Empiria.Payments.WebApi
