/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PaymentsTimeWindowController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update the payments time window.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.Payments.WebApi {

  /// <summary>Web API used to retrieve and update the payments time window.</summary>
  public class PaymentsTimeWindowController : WebApiController {

    #region Web apis

    [HttpGet]
    [Route("v2/payments-management/time-control/time-window")]
    public SingleObjectModel GetPaymentsTimeWindow() {

      var timeWindow = PaymentsTimeWindow.Instance;

      return new SingleObjectModel(base.Request, TimeWindowDto.MapToDto(timeWindow));
    }


    [HttpPost]
    [Route("v2/payments-management/time-control/time-window")]
    public SingleObjectModel UpdatePaymentsTimeWindow([FromBody] TimeWindowDto fields) {

      var timeWindow = PaymentsTimeWindow.Instance;

      timeWindow.Update(TimeSpan.Parse(fields.StartTime), TimeSpan.Parse(fields.EndTime));

      return new SingleObjectModel(base.Request, TimeWindowDto.MapToDto(timeWindow));
    }

    #endregion Web apis

  }  // class PaymentsTimeWindowController



  /// <summary>DTO used to return and update the payments time window.</summary>
  public class TimeWindowDto {

    public string StartTime {
      get; set;
    } = string.Empty;


    public string EndTime {
      get; set;
    } = string.Empty;


    static internal TimeWindowDto MapToDto(PaymentsTimeWindow timeWindow) {
      return new TimeWindowDto {
        StartTime = timeWindow.StartTime.ToString(PaymentsTimeWindow.FORMAT),
        EndTime = timeWindow.EndTime.ToString(PaymentsTimeWindow.FORMAT)
      };
    }

  }  // class TimeWindowDto

}  // namespace Empiria.Payments.WebApi
