/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Managem ent                       Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PaymentOrderData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for payments instances.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Payments.Orders.Data {

  /// <summary>Provides data read and write methods for contract instances.</summary>
  static internal class PaymentOrderData {

    #region Methods

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


    static internal void WritePaymentOrder(PaymentOrder o, string securityData, string extensionData) {

      var op = DataOperation.Parse("write_FMS_Payment_Order",
                     o.Id, o.UID, o.GetEmpiriaType().Id, o.PaymentOrderNo, o.Description, o.Observations,
                     o.PayableEntity.GetEmpiriaType().Id, o.PayableEntity.Id, o.PayTo.Id,
                     o.PaymentMethod.Id, o.Currency.Id, o.PaymentAccount.Id, o.DueTime,
                     securityData, extensionData, o.Keywords, o.RequestedBy.Id,
                     o.RequestedTime, o.AuthorizedBy.Id, o.AuthorizedTime, o.PayedBy.Id,
                     o.ClosingTime, o.ClosedBy.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePaymentOrderItem(PaymentOrderItem o, string securityData, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payable_Item",
                     o.Id, o.UID, o.GetEmpiriaType().Id, o.PaymentOrder.Id, o.PayableTypeId, o.PayableId,
                     o.Currency.Id, o.DepositAmount, o.WithdrawAmount, o.ExchangeRate,
                     securityData, extensionData, o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    #endregion Methods

  }  // class ContractData

}  // namespace Empiria.Payments.Orders.Data
