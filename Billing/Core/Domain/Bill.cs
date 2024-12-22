/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Partitioned Type                        *
*  Type     : Bill                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a bill for an invoice, credit note, paycheck, payment reception, etc.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Billing.Data;
using Empiria.Financial;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;

namespace Empiria.Billing {

  /// <summary>Represents a bill for an invoice, credit note, paycheck, payment reception, etc.</summary>
  [PartitionedType(typeof(BillType))]
  internal class Bill : BaseObject {

    #region Constructors and parsers

    protected Bill(BillType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected Bill() {

    }

    static public Bill Parse(int id) => ParseId<Bill>(id);

    static public Bill Parse(string uid) => ParseKey<Bill>(uid);

    public Bill(BillCategory billCategory,
                string billNo) : base(billCategory.BillType) {

      Assertion.Require(billCategory, nameof(billCategory));
      Assertion.Require(billNo, nameof(billNo));

      PayableId = 1;
      BillCategory = billCategory;
      BillNo = billNo;
      PayableTotal = 2170128.00M;
    }

    public Bill(IPayable payable,
                BillCategory billCategory,
                string billNo) : base(billCategory.BillType) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(billCategory, nameof(billCategory));
      Assertion.Require(billNo, nameof(billNo));

      PayableEntityTypeId = ObjectTypeInfo.Parse(payable.PayableEntity.Type.UID).Id;
      PayableEntityId = payable.PayableEntity.Id;
      PayableId = payable.Id;
      BillCategory = billCategory;
      BillNo = billNo;
      PayableTotal = payable.PayableEntity.Items.Sum(x => x.Total);
    }

    static public Bill Empty => ParseEmpty<Bill>();

    #endregion Constructors and parsers

    #region Properties

    public BillType BillType {
      get {
        return (BillType) base.GetEmpiriaType();
      }
    }


    [DataField("BILL_CATEGORY_ID")]
    public BillCategory BillCategory {
      get; private set;
    }


    [DataField("BILL_NO")]
    public string BillNo {
      get; private set;
    }


    [DataField("BILL_RELATED_BILL_NO")]
    public string RelatedBillNo {
      get;
      private set;
    }


    [DataField("BILL_ISSUE_DATE")]
    public DateTime IssueDate {
      get; private set;
    }


    [DataField("BILL_ISSUED_BY_ID")]
    public Party IssuedBy {
      get; private set;
    }


    [DataField("BILL_ISSUED_TO_ID")]
    public Party IssuedTo {
      get; private set;
    }


    [DataField("BILL_MANAGED_BY_ID")]
    public Party ManagedBy {
      get; private set;
    }


    [DataField("BILL_PAYABLE_ENTITY_TYPE_ID")]
    internal int PayableEntityTypeId {
      get; private set;
    }


    [DataField("BILL_PAYABLE_ENTITY_ID")]
    internal int PayableEntityId {
      get; private set;
    }


    [DataField("BILL_PAYABLE_ID")]
    internal int PayableId {
      get; private set;
    }


    [DataField("BILL_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("BILL_SUBTOTAL")]
    public decimal Subtotal {
      get; private set;
    }


    [DataField("BILL_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("BILL_TOTAL")]
    public decimal Total {
      get; private set;
    }


    [DataField("BILL_IDENTIFICATORS")]
    private string _identificators = string.Empty;


    public FixedList<string> Identificators {
      get {
        return _identificators.Split(' ').ToFixedList();
      }
    }


    [DataField("BILL_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
      }
    }



    [DataField("BILL_SCHEMA_EXT_DATA")]
    private JsonObject SchemaExtData {
      get; set;
    }

    public BillSchemaData SchemaData {
      get {
        return new BillSchemaData(this.SchemaExtData);
      }
    }


    [DataField("BILL_SECURITY_EXT_DATA")]
    private JsonObject SecurityExtData {
      get; set;
    }


    public BillSecurityData SecurityData {
      get {
        return new BillSecurityData(this.SecurityExtData);
      }
    }


    [DataField("BILL_PAYMENT_EXT_DATA")]
    private JsonObject PaymentExtData {
      get; set;
    }


    [DataField("BILL_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("BILL_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BILL_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BILL_STATUS", Default = BillStatus.Pending)]
    public BillStatus Status {
      get; private set;
    }


    internal FixedList<BillConcept> Concepts {
      get; set;
    } = new FixedList<BillConcept>();


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BillNo, RelatedBillNo, BillCategory.Keywords,
                                          _identificators, _tags,  IssuedBy.Keywords, IssuedTo.Keywords);
      }
    }


    public decimal PayableTotal {
      get;private set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      BillData.WriteBill(this, ExtData.ToString());
    }


    internal void AssignConcepts() {

      this.Concepts = BillConcept.GetListByBillId(Id);

      foreach (var concept in Concepts) {

        concept.TaxEntries = BillTaxEntry.GetListByBillConceptId(concept.BillConceptId);
      }
    }


    internal void Update(BillFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();
      fields.EnsureIsValidBill(PayableId, PayableTotal, BillCategory);
      fields.EnsureIsValidCreditNote(BillCategory);

      RelatedBillNo = fields.CFDIRelated;
      IssueDate = PatchField(fields.IssueDate, IssueDate);
      IssuedBy = PatchField(fields.IssuedByUID, IssuedBy);
      IssuedTo = PatchField(fields.IssuedToUID, IssuedTo);
      ManagedBy = PatchField(fields.ManagedByUID, ManagedBy);
      _identificators = PatchField(fields.Identificators, _identificators);
      _tags = PatchField(fields.Tags, _tags);
      Currency = PatchField(fields.CurrencyUID, Currency);
      Subtotal = fields.Subtotal;
      Discount = fields.Discount;
      Total = fields.Total;

      SchemaData.Update(fields.SchemaData);
      SecurityData.Update(fields.SecurityData);
    }


    #endregion Methods

  } // class Bill

} // namespace Empiria.Billing
