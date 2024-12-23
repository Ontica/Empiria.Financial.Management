/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Data Layer                              *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Data service                            *
*  Type     : BillData                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for billing.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Billing.Data {

  /// <summary>Provides data read and write methods for billing.</summary>
  static internal class BillData {

    #region Methods

    internal static FixedList<BillConcept> GetBillConcepts(Bill bill) {

      var sql = $"SELECT * FROM FMS_BILL_CONCEPTS " +
                $"WHERE BILL_CONCEPT_BILL_ID = {bill.Id} AND " +
                $"BILL_CONCEPT_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<BillConcept>(op);
    }


    static internal FixedList<BillTaxEntry> GetBillConceptTaxEntries(BillConcept billConcept) {

      var sql = $"SELECT * FROM FMS_BILL_TAXES " +
                $"WHERE BILL_TAX_BILL_CONCEPT_ID = {billConcept.Id} AND " +
                "BILL_TAX_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BillTaxEntry>(op);
    }



    static internal FixedList<Bill> GetBillCreditNotes(string billNo) {

      var sql = $"SELECT * FROM FMS_BILLS " +
                $"WHERE BILL_RELATED_BILL_NO = '{billNo}' AND " +
                $"BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Bill>(op);
    }


    static internal FixedList<Bill> GetBillsForPayable(int payableId) {

      var sql = $"SELECT * FROM FMS_BILLS " +
                $"WHERE BILL_PAYABLE_ID = {payableId} AND " +
                $"BILL_CATEGORY_ID = {BillCategory.FacturaProveedores.Id} AND " +
                $"BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Bill>(op);
    }


    static internal FixedList<Bill> SearchBills(string filter, string sort) {

      var sql = "SELECT * FROM FMS_BILLS";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" AND {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sort)) {
        sql += $" ORDER BY {sort}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Bill>(op);
    }


    static internal void SetBillAsPayed(string billUID) {

      Bill bill = Bill.Parse(billUID);

      var sql = $"UPDATE FMS_BILLS " +
                $"SET BILL_STATUS = '{(char) BillStatus.Payed}' " +
                $"WHERE BILL_ID = {bill.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal Bill TryGetBillWithBillNo(string billNo) {
      Assertion.Require(billNo, nameof(billNo));

      var sql = $"SELECT * FROM FMS_BILLS " +
                $"WHERE BILL_NO = '{billNo}' AND " +
                $"BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetObject<Bill>(op, null);
    }


    static internal void WriteBill(Bill o, string extData) {

      var op = DataOperation.Parse("write_FMS_Bill",
          o.Id, o.UID, o.BillType.Id, o.BillCategory.Id,
          o.BillNo, o.RelatedBillNo, o.IssueDate, o.IssuedBy.Id, o.IssuedTo.Id,
          o.ManagedBy.Id, o.PayableEntityTypeId, o.PayableEntityId, o.PayableId,
          o.Currency.Id, o.Subtotal, o.Discount, o.Total, o.PaymentMethod,
          string.Join(" ", o.Identificators), string.Join(" ", o.Tags),
          o.SchemaData.ToJsonString(), o.SecurityData.ToJsonString(), string.Empty,
          extData, o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteBillConcept(BillConcept o, string extensionData) {

      var op = DataOperation.Parse("write_FMS_Bill_Concept",
        o.Id, o.UID, o.BillConceptTypeId, o.Bill.Id, o.Product.Id,
        o.SATProduct.Id, o.SATProductCode, o.Description,
        string.Join(" ", o.Identificators), string.Join(" ", o.Tags),
        o.Quantity, o.QuantityUnit.Id, o.UnitPrice, o.Subtotal, o.Discount,
        o.SchemaData.ToJsonString(), extensionData, o.Keywords,
        o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteBillTaxEntry(BillTaxEntry o, string extensionData) {

      var op = DataOperation.Parse("write_FMS_Bill_Tax",
          o.Id, o.UID, o.TaxType.Id, o.Bill.Id, o.BillConcept.Id,
          (char) o.TaxMethod, (char) o.TaxFactorType, o.Factor,
          o.BaseAmount, o.Total, extensionData,
          o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  } // class BillData

} // namespace Empiria.Billing.Data
