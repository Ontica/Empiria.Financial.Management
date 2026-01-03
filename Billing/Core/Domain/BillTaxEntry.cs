/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillTaxEntry                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds tax data related to a bill or a bill concept.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Billing.Data;

namespace Empiria.Billing {

  /// <summary>Holds tax data related to a bill or a bill concept.</summary>
  internal class BillTaxEntry : BaseObject {

    #region Constructors and parsers

    private BillTaxEntry() {
      // Required by Empiria Framework.
    }

    public BillTaxEntry(Bill bill, int billTaxRelatedObjectId, BillTaxEntryFields fields) {
      Assertion.Require(bill, nameof(bill));
      Assertion.Require(!bill.IsEmptyInstance, nameof(bill));
      Assertion.Require(billTaxRelatedObjectId, nameof(billTaxRelatedObjectId));

      Bill = bill;
      BillTaxRelatedObjectTypeId = Bill.BillType.Id;
      BillTaxRelatedObjectId = billTaxRelatedObjectId;
      TaxType = TaxType.Parse(101);  // ToDo: Fix hardcoded tax type id.

      TaxMethod = fields.TaxMethod;
      TaxFactorType = fields.TaxFactorType;
      Factor = fields.Factor;
      BaseAmount = fields.BaseAmount;
      Total = fields.Total;
      BillTaxExtData.Update(fields);
    }

    static internal BillTaxEntry Parse(int id) => ParseId<BillTaxEntry>(id);

    static internal BillTaxEntry Parse(string uid) => ParseKey<BillTaxEntry>(uid);

    static internal FixedList<BillTaxEntry> GetListFor(int relatedObjectTypeId, int relatedObjectId) {
      Assertion.Require(relatedObjectTypeId, nameof(relatedObjectTypeId));
      Assertion.Require(relatedObjectId, nameof(relatedObjectId));

      return BillData.GetBillTaxEntriesByRelatedObject(relatedObjectTypeId, relatedObjectId);
    }

    static public BillTaxEntry Empty => ParseEmpty<BillTaxEntry>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("BILL_TAX_TYPE_ID")]
    public TaxType TaxType {
      get; private set;
    }


    [DataField("BILL_TAX_BILL_ID")]
    public Bill Bill {
      get; private set;
    }


    [DataField("BILL_TAX_RELATED_OBJECT_TYPE_ID")]
    public int BillTaxRelatedObjectTypeId {
      get; private set;
    }


    [DataField("BILL_TAX_RELATED_OBJECT_ID")]
    public int BillTaxRelatedObjectId {
      get; private set;
    }


    [DataField("BILL_TAX_METHOD", Default = BillTaxMethod.Traslado)]
    public BillTaxMethod TaxMethod {
      get; private set;
    }


    [DataField("BILL_TAX_FACTOR_TYPE", Default = BillTaxFactorType.Tasa)]
    public BillTaxFactorType TaxFactorType {
      get; private set;
    }


    [DataField("BILL_TAX_FACTOR")]
    public decimal Factor {
      get; private set;
    }


    [DataField("BILL_TAX_BASE_AMOUNT")]
    public decimal BaseAmount {
      get; private set;
    }


    [DataField("BILL_TAX_TOTAL")]
    public decimal Total {
      get; private set;
    }


    [DataField("BILL_TAX_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("BILL_TAX_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BILL_TAX_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BILL_TAX_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public BillTaxExtData BillTaxExtData {
      get {
        return new BillTaxExtData(this.ExtData);
      }
    }

    #endregion Properties

    #region Private methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      BillData.WriteBillTaxEntry(this, ExtData.ToString());
    }

    #endregion Private methods

  } // class BillTaxEntry

}  // namespace Empiria.Billing
