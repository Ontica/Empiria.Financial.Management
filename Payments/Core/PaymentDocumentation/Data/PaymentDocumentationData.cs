/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PaymentDocumentationData                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for payments.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;

namespace Empiria.Payments.Data {

  /// <summary>Provides data read and write methods for payments.</summary>
  static public class PaymentDocumentationData {

    #region Methods

    static internal FixedList<PaymentOrder> GetPaymentOrders(DateTime fromDate, DateTime toDate) {
      var sql = "SELECT * FROM FMS_PAYMENT_ORDERS WHERE " +
                $"{DataCommonMethods.FormatSqlDbDate(fromDate)} <= PYMT_ORD_POSTING_TIME AND " +
                $"PYMT_ORD_POSTING_TIME < {DataCommonMethods.FormatSqlDbDate(toDate.Date.AddDays(1))} " +
                $"AND PYMT_ORD_STATUS = 'Y'";

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<PaymentOrder>(dataOperation);
    }

    #endregion Methods

  }  // class PaymentDocumentationData

}  // namespace Empiria.Payments.Data
