﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Data Layer                              *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Data service                            *
*  Type     : BillData                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for billing.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Globalization;
using Empiria.Data;

namespace Empiria.Billing.Data {

  /// <summary>Provides data read and write methods for billing.</summary>
  static internal class BillData {


    #region Public methods


    static internal FixedList<Bill> GetBillList(string filtering, string sorting) {

      var sql = "SELECT * FROM FMS_BILLS ";

      if (!string.IsNullOrWhiteSpace(filtering)) {
        sql += $"WHERE {filtering} ";
      }

      if (!string.IsNullOrWhiteSpace(sorting)) {
        sql += $"ORDER BY {sorting} ";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<Bill>(op);
    }


    static public void WriteBill(Bill bill) {

      var op = DataOperation.Parse("WRITE_FMS_BILL",
        bill.Id, bill.UID, bill.BillType.Id, bill.BillCategory.Id,
        bill.BillNo, bill.IssueDate, bill.IssuedBy.Id, bill.IssuedTo.Id,
        bill.ManagedBy.Id, bill.SchemaVersion, bill.Identificators, bill.Tags,
        bill.Currency.Id, bill.Subtotal, bill.Discount, bill.Total,
        bill.SchemaData, bill.SecurityData, "", "",
        bill.PostedBy, bill.PostingTime, (char) bill.Status);

      DataWriter.Execute(op);
    }


    static public void WriteBillConcept(BillConcept concept) {

      var op = DataOperation.Parse("WRITE_FMS_BILL",
        concept.Id, concept.UID, concept.Bill.Id, concept.Product.Id,
        concept.Description, concept.Identificators, concept.Tags, concept.Quantity,
        concept.QuantityUnit.Id, concept.UnitPrice, concept.Subtotal, concept.Discount,
        concept.SchemaData, "", concept.PostedBy, concept.PostingTime);

      DataWriter.Execute(op);
    }


    static public void WriteBillTaxEntry(BillTaxEntry tax) {

      var op = DataOperation.Parse("WRITE_FMS_BILL_TAX",
        tax.Id, tax.UID, tax.Bill.Id, tax.BillConcept.Id,
        tax.TaxType.Id, (char)tax.TaxMethod, (char)tax.TaxFactorType,
        tax.Factor, tax.BaseAmount, tax.Total, "",
        tax.PostedBy, tax.PostingTime, (char) tax.Status);

      DataWriter.Execute(op);
    }


    #endregion Public methods


  } // class BillData

} // namespace Empiria.Billing.Data