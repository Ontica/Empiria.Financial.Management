﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillTaxEntry                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds tax data related to a bill or a bill concept.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Billing.Data;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Billing {

  /// <summary>Holds tax data related to a bill or a bill concept.</summary>
  internal class BillTaxEntry : BaseObject {

    #region Constructors and parsers

    private BillTaxEntry() {
      // Required by Empiria Framework.
    }

    public BillTaxEntry(BillTaxEntryFields fields, string billUID, string billConceptUID) {
      MapFieldsToBillTaxEntry(fields, billUID, billConceptUID);
    }

    static internal BillTaxEntry Parse(int id) => ParseId<BillTaxEntry>(id);

    static internal BillTaxEntry Parse(string uid) => ParseKey<BillTaxEntry>(uid);

    #endregion Constructors and parsers

    #region Properties

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


    [DataField("BILL_TAX_STATUS")]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Private methods

    private void MapFieldsToBillTaxEntry(BillTaxEntryFields fields,
                                         string billUID, string billConceptUID) {
      
      this.Bill = Bill.Parse(billUID);
      this.BillConcept = BillConcept.Parse(billConceptUID);
      this.TaxType = TaxType.Parse(-1);
      this.TaxMethod = fields.TaxType;
      this.TaxFactorType = fields.TaxFactorType;
      this.Factor = fields.Factor;
      this.BaseAmount = fields.BaseAmount;
      this.Total = fields.Total;
      this.ExtData = new JsonObject();
      this.PostedBy = Party.Parse(ExecutionServer.CurrentUserId);
      this.PostingTime = DateTime.Now;
      this.Status = EntityStatus.Active;
    }


    protected override void OnSave() {
      BillData.WriteBillTaxEntry(this);
    }

    #endregion Private methods
  } // class BillTaxEntry

}  // namespace Empiria.Billing
