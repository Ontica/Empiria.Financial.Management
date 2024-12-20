/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Data Layer                              *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Data service                            *
*  Type     : BillData                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for billing.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Globalization;
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


    static internal List<Bill> ValidateIfExistCreditNotesByBill(string cfdiRelated) {

      var sql = $"SELECT * FROM FMS_BILLS " +
                $"WHERE BILL_NO_RELATED = '{cfdiRelated}' " +
                $"AND BILL_STATUS <> 'N' AND BILL_STATUS <> 'X'";
      var op = DataOperation.Parse(sql);
      return DataReader.GetPlainObjectList<Bill>(op);
    }


    static internal void WriteBill(Bill bill, string schemaExtData) {

      var op = DataOperation.Parse("write_FMS_Bill",
        bill.Id, bill.UID, bill.BillType.Id, bill.BillCategory.Id,
        bill.BillNo, bill.BillNoRelated, bill.IssueDate, bill.IssuedBy.Id,
        bill.IssuedTo.Id, bill.ManagedBy.Id, string.Join(" ", bill.Identificators),
        string.Join(" ", bill.Tags), bill.Currency.Id, bill.Subtotal, 
        bill.Discount, bill.Total, schemaExtData, "", "", "",
        bill.PostedBy.Id, bill.PostingTime, (char) bill.Status,
        bill.PayableId, bill.PayableEntityTypeId, bill.PayableEntityId);

      DataWriter.Execute(op);
    }


    static internal void WriteBillConcept(BillConcept concept) {

      var op = DataOperation.Parse("write_FMS_Bill_Concept",
        concept.Id, concept.UID, concept.Bill.Id, concept.Product.Id,
        concept.SATProductID, concept.SATProductCode,
        concept.Description, string.Join(" ", concept.Identificators),
        string.Join(" ", concept.Tags), concept.Quantity,
        concept.QuantityUnit.Id, concept.UnitPrice, concept.Subtotal, concept.Discount,
        "", "", concept.PostedBy.Id, concept.PostingTime, (char) concept.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteBillTaxEntry(BillTaxEntry tax) {

      var op = DataOperation.Parse("write_FMS_Bill_Tax",
        tax.Id, tax.UID, tax.Bill.Id, tax.BillConcept.Id,
        tax.TaxType.Id, (char) tax.TaxMethod, (char) tax.TaxFactorType,
        tax.Factor, tax.BaseAmount, tax.Total, "",
        tax.PostedBy.Id, tax.PostingTime, (char) tax.Status);

      DataWriter.Execute(op);
    }


    #endregion Public methods


  } // class BillData

} // namespace Empiria.Billing.Data
