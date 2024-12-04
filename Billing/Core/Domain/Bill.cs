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

    public Bill(IPayable payable,
                BillCategory billCategory,
                string billNo) : base(billCategory.BillType) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(billCategory, nameof(billCategory));
      Assertion.Require(billNo, nameof(billNo));

      PayableId = payable.Id;
      BillCategory = billCategory;
      BillNo = billNo;
    }

    static public Bill Empty => ParseEmpty<Bill>();

    #endregion Constructors and parsers

    #region Properties

    public BillType BillType {
      get {
        return (BillType) base.GetEmpiriaType();
      }
    }


    [DataField("BILL_UID")]
    public string BillUID {
      get; private set;
    }


    [DataField("BILL_CATEGORY_ID")]
    public BillCategory BillCategory {
      get; private set;
    }


    [DataField("BILL_NO")]
    public string BillNo {
      get; private set;
    }


    [DataField("BILL_PAYABLE_ID")]
    public int PayableId {
      get; private set;
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


    [DataField("BILL_SCHEMA_VERSION")]
    public string SchemaVersion {
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


    [DataField("BILL_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("BILL_SUBTOTAL", ConvertFrom = typeof(float))]
    public decimal Subtotal {
      get; private set;
    }


    [DataField("BILL_DISCOUNT", ConvertFrom = typeof(float))]
    public decimal Discount {
      get; private set;
    }


    [DataField("BILL_TOTAL", ConvertFrom = typeof(float))]
    public decimal Total {
      get; private set;
    }


    [DataField("BILL_SCHEMA_EXT_DATA")]
    private JsonObject SchemaExtData {
      get; set;
    }


    [DataField("BILL_SECURITY_EXT_DATA")]
    private JsonObject SecurityExtData {
      get; set;
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


    public BillSchemaData SchemaData {
      get {
        return new BillSchemaData(this.SchemaExtData);
      }
    }


    public BillSecurityData SecurityData {
      get {
        return new BillSecurityData(this.SecurityExtData);
      }
    }


    internal FixedList<BillConcept> Concepts {
      get; set;
    } = new FixedList<BillConcept>();


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BillNo, IssuedBy.Name);
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      BillData.WriteBill(this);
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

      IssueDate = PatchField(fields.IssueDate, IssueDate);
      IssuedBy = PatchField(fields.IssuedByUID, IssuedBy);
      IssuedTo = PatchField(fields.IssuedToUID, IssuedTo);
      ManagedBy = PatchField(fields.ManagedByUID, ManagedBy);
      SchemaVersion = PatchField(fields.SchemaVersion, SchemaVersion);
      _identificators = PatchField(fields.Identificators, _identificators);
      _tags = PatchField(fields.Tags, _tags);
      Currency = PatchField(fields.CurrencyUID, Currency);
      Subtotal = fields.Subtotal;
      Discount = fields.Discount;
      Total = fields.Total;
    }

    #endregion Methods

  } // class Bill

} // namespace Empiria.Billing
