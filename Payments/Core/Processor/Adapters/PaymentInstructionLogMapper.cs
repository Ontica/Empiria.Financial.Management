/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentInstructionLogMapper                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payment instruction log.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Billing;
using System.Collections.Generic;

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Provides data mapping services for payment log.</summary>
  static internal class PaymentInstructionLogMapper {

    #region Methods

    static internal FixedList<PaymentInstructionLogDescriptorDto> Map(FixedList<PaymentInstructionLogEntry> paymentInstructionLogs) {
      List<PaymentInstructionLogDescriptorDto> logs = new List<PaymentInstructionLogDescriptorDto>();

      foreach (var conceppaymentInstructionLog in paymentInstructionLogs) {
        var paymentInstructionLogdDto = Map(conceppaymentInstructionLog);
        logs.Add(paymentInstructionLogdDto);
      }

      return logs.ToFixedList();
    }


    static internal PaymentInstructionLogDescriptorDto Map(PaymentInstructionLogEntry paymentInstructionLog) {

      return new PaymentInstructionLogDescriptorDto {
        UID = paymentInstructionLog.UID,
        PaymentOrdeNo = paymentInstructionLog.PaymentOrder.PaymentOrderNo,
        PaymentMethod = paymentInstructionLog.PaymentOrder.PaymentMethod.Name,
        Total = paymentInstructionLog.PaymentOrder.Total,
        Currency = paymentInstructionLog.PaymentOrder.Currency.Name,
        RequestTime = paymentInstructionLog.RequestTime,
        RequestCode = paymentInstructionLog.ExternalRequestID,
        Description = paymentInstructionLog.ExternalResultText,
        StatusName = paymentInstructionLog.Status.GetName(),
      };

    }



    #endregion Methods

  }  // class PaymentLogMapper

}  // namespace Empiria.Payments.Processor.Services