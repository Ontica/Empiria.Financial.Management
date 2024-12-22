/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Data Layer                              *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Data service                            *
*  Type     : BillData                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for billing.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using Empiria.Data;

namespace Empiria.Billing.Data {

  /// <summary>Provides data read and write methods for billing.</summary>
  static internal class BillData {


    #region Public methods


    internal static FixedList<BillConcept> GetBillConceptsByBillId(int billId) {

      var sql = $"SELECT * FROM FMS_BILL_CONCEPTS WHERE BILL_CONCEPT_BILL_ID = {billId}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<BillConcept>(op);
    }


    internal static List<BillTaxEntry> GetBillTaxesByBillConceptId(int billConceptId) {

      var sql = $"SELECT * FROM FMS_BILL_TAXES WHERE BILL_TAX_BILL_CONCEPT_ID = {billConceptId}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectList<BillTaxEntry>(op);
    }


    static internal FixedList<Bill> GetBillList(string filtering, string sorting) {

      var sql = "SELECT * FROM FMS_BILLS WHERE BILL_ID > 0 ";

      if (!string.IsNullOrWhiteSpace(filtering)) {
        sql += $"AND {filtering} ";
      }

      if (!string.IsNullOrWhiteSpace(sorting)) {
        sql += $"ORDER BY {sorting} ";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<Bill>(op);
    }


    internal static void SetBillAsPayed(string billUID) {

      Bill bill = Bill.Parse(billUID);

      var sql = $"UPDATE FMS_BILLS SET BILL_STATUS = '{(char) BillStatus.Payed}' WHERE BILL_ID = {bill.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal List<Bill> ValidateIfExistBill(string billNo) {

      var sql = $"SELECT * FROM FMS_BILLS WHERE BILL_NO = '{billNo}' " +
                $"AND BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectList<Bill>(op);
    }


    static internal List<Bill> ValidateIfExistBillsByPayable(int payableId) {

      var sql = $"SELECT * FROM FMS_BILLS WHERE BILL_PAYABLE_ID = {payableId} " +
                $"AND BILL_CATEGORY_ID = 701" +
                $"AND BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectList<Bill>(op);
    }


    static internal List<Bill> ValidateIfExistCreditNotesByBill(string relatedCFDI) {

      var sql = $"SELECT * FROM FMS_BILLS " +
                $"WHERE BILL_RELATED_BILL_NO = '{relatedCFDI}' " +
                $"AND BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectList<Bill>(op);
    }


    static internal void WriteBill(Bill o, string extData) {

      var op = DataOperation.Parse("write_FMS_Bill",
          o.Id, o.UID, o.BillType.Id, o.BillCategory.Id,
          o.BillNo, o.RelatedBillNo, o.IssueDate, o.IssuedBy.Id, o.IssuedTo.Id,
          o.ManagedBy.Id, o.PayableEntityTypeId, o.PayableEntityId, o.PayableId,
          o.Currency.Id, o.Subtotal, o.Discount, o.Total,
          string.Join(" ", o.Identificators), string.Join(" ", o.Tags),
          o.SchemaData.ToJsonString(), o.SecurityData.ToJsonString(), string.Empty,
          extData, o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteBillConcept(BillConcept o, string schemaExtData) {

      var op = DataOperation.Parse("write_FMS_Bill_Concept",
        o.Id, o.UID, o.Bill.Id, o.Product.Id,
        o.SATProductID, o.SATProductCode,
        o.Description, string.Join(" ", o.Identificators),
        string.Join(" ", o.Tags), o.Quantity,
        o.QuantityUnit.Id, o.UnitPrice, o.Subtotal, o.Discount,
        schemaExtData, "", o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteBillTaxEntry(BillTaxEntry o) {

      var op = DataOperation.Parse("write_FMS_Bill_Tax",
          o.Id, o.UID, o.Bill.Id, o.BillConcept.Id,
          o.TaxType.Id, (char) o.TaxMethod, (char) o.TaxFactorType,
          o.Factor, o.BaseAmount, o.Total, "",
          o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    #endregion Public methods


  } // class BillData

} // namespace Empiria.Billing.Data
