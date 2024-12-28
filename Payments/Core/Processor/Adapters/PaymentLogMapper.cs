/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PaymentLogMapper                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payment log.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Services {

  /// <summary>Provides data mapping services for payment log.</summary>
  static internal class PaymentLogMapper {

    #region Methods

    internal static FixedList<PaymentLogDto> Map(FixedList<PaymentLog> paymentLogs) {
      throw new NotImplementedException();
    }

    #endregion Methods

  }  // class PaymentLogMapper

}  // namespace Empiria.Payments.Processor.Services