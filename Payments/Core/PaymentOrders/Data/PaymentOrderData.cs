/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PaymentOrderData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for payments instances.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

using Empiria.Financial;

namespace Empiria.Payments.Data {

  /// <summary>Provides data read and write methods for contract instances.</summary>
  static internal class PaymentOrderData {

    #region Methods

    static internal FixedList<PaymentOrder> GetPaymentOrders(IPayableEntity payableEntity) {
      var sql = "SELECT * FROM FMS_PAYMENT_ORDERS " +
               $"WHERE PYMT_ORD_PAYABLE_ENTITY_TYPE_ID = {payableEntity.GetEmpiriaType().Id} AND " +
               $"PYMT_ORD_PAYABLE_ENTITY_ID = {payableEntity.Id} AND " +
               $"PYMT_ORD_STATUS <> 'X' " +
               $"ORDER BY PYMT_ORD_NO DESC";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<PaymentOrder>(op);
    }


    static internal FixedList<PaymentOrder> GetPaymentOrders(string filter, string sortBy) {
      var sql = "SELECT * FROM FMS_PAYMENT_ORDERS";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<PaymentOrder>(dataOperation);
    }


    static internal void WritePaymentOrder(PaymentOrder o) {

      var op = DataOperation.Parse("write_FMS_Payment_Order",
                     o.Id, o.UID, o.PaymentType.Id, o.PaymentOrderNo, o.Description, o.Observations,
                     o.PayableEntity.GetEmpiriaType().Id, o.PayableEntity.Id, o.PayTo.Id,
                     o.PaymentMethod.Id, o.Currency.Id, o.PaymentAccount.Id, o.DueTime,
                     o.SecurityExtData.ToString(), o.ExtData.ToString(), o.Keywords, o.RequestedBy.Id,
                     o.PostingTime, o.AuthorizedBy.Id, o.AuthorizedTime, o.PayedBy.Id,
                     o.ClosingTime, o.ClosedBy.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePaymentOrderItem(PaymentOrderItem o) {
      var op = DataOperation.Parse("write_FMS_Payable_Item",
                     o.Id, o.UID, o.GetEmpiriaType().Id, o.PaymentOrder.Id, o.PayableTypeId, o.PayableId,
                     o.Currency.Id, o.DepositAmount, o.WithdrawAmount, o.ExchangeRate,
                     o.SecurityExtData.ToString(), o.ExtData.ToString(), o.Keywords,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class PaymentOrderData

}  // namespace Empiria.Payments.Data
