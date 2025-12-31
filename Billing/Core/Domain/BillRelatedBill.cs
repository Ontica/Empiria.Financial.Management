/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Partitioned Type                        *
*  Type     : BillRelatedBill                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment complement related bill for an invoice.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Billing.Data;

namespace Empiria.Billing {

  /// <summary>Represents a payment complement related bill for an invoice.</summary>
  internal class BillRelatedBill : BaseObject {


    #region Constructors and parsers

    private BillRelatedBill() {
      // Required by Empiria Framework
    }

    public BillRelatedBill(Bill bill) {
      Assertion.Require(bill, nameof(bill));

      this.Bill = bill;
    }

    static public BillRelatedBill Parse(int id) => ParseId<BillRelatedBill>(id);

    static public BillRelatedBill Parse(string uid) => ParseKey<BillRelatedBill>(uid);

    static internal FixedList<BillRelatedBill> GetListFor(Bill bill) {
      Assertion.Require(bill, nameof(bill));

      return BillData.GetBillRelatedBills(bill);
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("BILL_RELATED_BILL_ID")]
    public int BillRelatedBillId {
      get; private set;
    }


    [DataField("BILL_RELATED_BILL_UID")]
    public string BillRelatedBillUID {
      get; private set;
    }


    [DataField("BILL_RELATED_BILL_BILL_ID")]
    public Bill Bill {
      get; private set;
    }


    [DataField("RELATED_DOCUMENT")]
    public string RelatedDocument {
      get; private set;
    }


    [DataField("BILL_RELATED_BILL_SCHEMA_EXT_DATA")]
    private JsonObject SchemaExtData {
      get; set;
    }


    [DataField("BILL_RELATED_BILL_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("BILL_RELATED_BILL_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BILL_RELATED_BILL_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BILL_RELATED_BILL_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public BillRelatedBillSchemaData BillRelatedSchemaExtData {
      get {
        return new BillRelatedBillSchemaData(this.SchemaExtData);
      }
    }


    internal FixedList<BillTaxEntry> TaxEntries {
      get {
        return BillTaxEntry.GetListFor(this.Bill.BillType.Id, this.BillRelatedBillId);
      }
    }


    #endregion Properties

    #region Private methods

    internal void Update(ComplementRelatedPayoutDataFields fields) {
      this.RelatedDocument = fields.IdDocumento;
      this.BillRelatedSchemaExtData.Update(fields);
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      BillData.WriteBillRelatedBillEntry(this, ExtData.ToString());
    }


    #endregion Private methods

  }
}
