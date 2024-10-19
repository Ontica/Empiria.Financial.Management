/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payables Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PayableData                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for Payable instances.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

using Empiria.Contracts;

namespace Empiria.Payments.Payables.Data {

  /// <summary>Provides data read and write methods for Payable instances.</summary>
  static internal class PayableData {

    #region Methods

    static internal FixedList<Payable> GetPayables(string filter, string sortBy) {
      var sql = "SELECT * FROM VW_FMS_PAYABLES";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Payable>(op);
    }


    static internal PayableItem GetPayableItem(Payable payable, string payableItemUID) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(payableItemUID, nameof(payableItemUID));

      var sql = "SELECT * FROM FMS_PAYABLE_ITEMS " +
                $"WHERE PAYABLE_ID = {payable.Id} AND " +
                $"PAYABLE_ITEM_UID = '{payableItemUID}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetObject<PayableItem>(op);
    }


    static internal List<PayableItem> GetPayableItems(Payable payable) {
      Assertion.Require(payable, nameof(payable));

      var sql = $"SELECT * FROM FMS_PAYABLE_ITEMS " +
                $"WHERE PAYABLE_ID = {payable.Id} AND " +
                $"PAYABLE_ITEM_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<PayableItem>(op);
    }


    static internal int GetLastPayableNumber() {
      string sql = "SELECT MAX(PAYABLE_ID) " +
                   "FROM FMS_Payables";

      var op = DataOperation.Parse(sql);

      return (int) DataReader.GetScalar<decimal>(op);
    }


    static internal void WriteContractMilestone(ContractMilestone o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payable",
                     o.Id, o.UID, o.payableNo, o.PayableType.Id, o.Description, o.OrganizationalUnit.Id,
                     o.RequestedTime, o.PayTo.Id, o.Contract.Id,
                     o.Currency.Id, o.BudgetType.Id, o.DueTime,
                     extensionData, o.Keywords, o.PostedBy.Id,
                     o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePayable(Payable o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payable",
                     o.Id, o.UID, o.payableNo, o.PayableType.Id, o.Description, o.OrganizationalUnit.Id,
                     o.RequestedTime, o.PayTo.Id, Contract.Empty.Id,
                     o.Currency.Id, o.BudgetType.Id, o.DueTime,
                     extensionData, o.Keywords, o.PostedBy.Id,
                     o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePayableItem(PayableItem o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payable_Item",
                     o.Id, o.UID, o.Payable.Id, 1, 1, o.Description, o.Quantity, o.UnitPrice,
                     o.Currency.Id, 1, o.ExchangeRate, o.BudgetAccount.Id, 1, extensionData,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class PayableData

}  // namespace Empiria.Payments.Payables.Data
