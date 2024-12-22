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
using System.Collections.Generic;

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

    public BillTaxEntry(Bill bill, BillConcept billConcept) {
      Assertion.Require(bill, nameof(bill));
      Assertion.Require(billConcept, nameof(billConcept));

      this.Bill = bill;
      this.BillConcept = billConcept;
    }

    static internal BillTaxEntry Parse(int id) => ParseId<BillTaxEntry>(id);

    static internal BillTaxEntry Parse(string uid) => ParseKey<BillTaxEntry>(uid);

    static public BillTaxEntry Empty => ParseEmpty<BillTaxEntry>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("BILL_TAX_ENTRY_ID")]
    public int BillTaxId {
      get; private set;
    }


    [DataField("BILL_TAX_ENTRY_UID")]
    public string BillTaxUID {
      get; private set;
    }


    [DataField("BILL_TAX_BILL_ID")]
    public Bill Bill {
      get; private set;
    }


    [DataField("BILL_TAX_BILL_CONCEPT_ID")]
    public BillConcept BillConcept {
      get; private set;
    }


    [DataField("BILL_TAX_TYPE_ID")]
    public TaxType TaxType {
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

    #endregion Properties

    #region Private methods

    internal static FixedList<BillTaxEntry> GetListByBillConceptId(int billConceptId) {

      List<BillTaxEntry> taxList = BillData.GetBillTaxesByBillConceptId(billConceptId);
      if (taxList.Count == 0) {
        return new FixedList<BillTaxEntry>();
      }

      return taxList.ToFixedList();
    }


    internal void Update (BillTaxEntryFields fields) {
      this.TaxType = TaxType.Empty;
      this.TaxMethod = fields.TaxMethod;
      this.TaxFactorType = fields.TaxFactorType;
      this.Factor = fields.Factor;
      this.BaseAmount = fields.BaseAmount;
      this.Total = fields.Total;
      this.ExtData = new JsonObject();
    }


    protected override void OnSave() {
      if (IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }
      BillData.WriteBillTaxEntry(this);
    }


    #endregion Private methods

  } // class BillTaxEntry

}  // namespace Empiria.Billing
