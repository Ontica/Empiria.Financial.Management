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

using Empiria.Documents;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;

using Empiria.Billing.Data;

namespace Empiria.Billing {

  /// <summary>Represents a bill for an invoice, credit note, paycheck, payment reception, etc.</summary>
  [PartitionedType(typeof(BillType))]
  public class Bill : BaseObject {

    #region Constructors and parsers

    protected Bill(BillType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public Bill Parse(int id) => ParseId<Bill>(id);

    static public Bill Parse(string uid) => ParseKey<Bill>(uid);

    static public Bill TryParseWithBillNo(string billNo) =>
                            TryParse<Bill>($"BILL_NO = '{billNo}' AND BILL_STATUS <> 'X'");

    static public Bill Empty => ParseEmpty<Bill>();

    static public FixedList<Bill> GetListFor(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      return GetFullList<Bill>($"BILL_PAYABLE_ENTITY_ID = {payableEntity.Id} " +
                               $"AND BILL_STATUS <> 'X'", "BILL_ID")
            .ToFixedList();
    }


    static public FixedList<Bill> GetListFor(FixedList<IPayableEntity> payableEntities) {
      Assertion.Require(payableEntities, nameof(payableEntities));

      if (payableEntities.Count == 0) {
        return FixedList<Bill>.Empty;
      }

      return GetFullList<Bill>($"BILL_PAYABLE_ENTITY_ID IN " +
                               $"({string.Join(",", payableEntities.Select(x => x.Id))}) " +
                               $"AND BILL_STATUS <> 'X'", "BILL_ID")
            .ToFixedList();
    }


    public Bill(IPayableEntity payable, BillCategory billCategory,
                string billNo) : base(billCategory.BillType) {

      Assertion.Require(payable, nameof(payable));
      Assertion.Require(billCategory, nameof(billCategory));
      Assertion.Require(billNo, nameof(billNo));

      PayableEntityTypeId = payable.GetEmpiriaType().Id;
      PayableEntityId = payable.Id;
      PayableId = payable.Id;
      ManagedBy = payable.OrganizationalUnit;
      Currency = payable.Currency;

      BillCategory = billCategory;
      BillNo = billNo;
      PayableTotal = payable.Total;
    }


    public Bill(IPayableEntity payable, BillCategory billCategory,
                DocumentFields fields) : this(payable, billCategory, fields.DocumentNumber) {

      Assertion.Require(fields.Total > 0, "Total must be positive.");

      Currency = payable.Currency;
      Description = fields.Name;
      IssueDate = DateTime.Now;
      Subtotal = fields.Total;
      Total = fields.Total;
    }


    static public FixedList<Bill> GetListFor(Party party) {
      return GetList<Bill>($"BILL_ISSUED_BY_ID = {party.Id} " +
                           $"AND BILL_STATUS <> 'X'")
            .ToFixedList();
    }

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


    public string Name {
      get {
        if (BillType.Name.Contains("Voucher")) {
          return Description;
        } else if (Concepts.Count != 0) {
          return EmpiriaString.DivideLongString(Concepts[0].Description, 95, " ");
        } else {
          return "La factura no tiene conceptos";
        }
      }
    }


    public string Description {
      get {
        return ExtData.Get("description", string.Empty);
      }
      private set {
        ExtData.SetIfValue("description", value);
      }
    }


    [DataField("BILL_RELATED_BILL_NO")]
    public string RelatedBillNo {
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


    public decimal Taxes {
      get {
        return BillTaxes.Sum(x => x.Total);
      }
    }


    [DataField("BILL_TOTAL")]
    public decimal Total {
      get; private set;
    }


    public string PaymentMethod {
      get {
        if (SchemaData.MetodoPago.Length != 0) {
          return SchemaData.MetodoPago;
        } else {
          return "ND";
        }
      }
    }


    [DataField("BILL_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("BILL_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
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


    public BillExtData BillExtData {
      get {
        return new BillExtData(this.ExtData);
      }
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
      get {
        return BillConcept.GetListFor(this);
      }
    }


    internal FixedList<BillRelatedBill> BillRelatedBills {
      get {
        return BillRelatedBill.GetListFor(this);
      }
    }


    internal FixedList<BillTaxEntry> BillTaxes {
      get {
        return BillTaxEntry.GetListByBill(this);
      }
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BillNo, RelatedBillNo, BillCategory.Keywords,
                                          _identificators, _tags, IssuedBy.Keywords, IssuedTo.Keywords);
      }
    }


    public decimal PayableTotal {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal void AddBillRelatedBill(ComplementRelatedPayoutDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      var billRelated = new BillRelatedBill(this, fields);

      billRelated.Save();

      foreach (var taxFields in fields.Taxes) {
        billRelated.AddBillRelatedTaxes(this, taxFields);
      }
    }


    internal void AddBillTaxes(BillTaxEntryFields taxFields, int billTaxRelatedObjectId) {
      Assertion.Require(taxFields, nameof(taxFields));

      BillTaxEntry taxEntry = new BillTaxEntry(this, billTaxRelatedObjectId, taxFields);
      taxEntry.Save();
    }


    internal void AddComplementConcepts(FuelConsumptionComplementConceptDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      BillConcept billConcept = new BillConcept(BillConceptType.Complement, this, fields);

      billConcept.Save();

      foreach (var taxFields in fields.TaxEntries) {
        AddBillTaxes(taxFields, billConcept.Id);
      }
    }


    public void AddConcept(BillConceptType billConceptType, BillConceptFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      BillConcept billConcept = new BillConcept(billConceptType, this, fields);

      billConcept.Save();

      foreach (var taxFields in fields.TaxEntries) {
        AddBillTaxes(taxFields, billConcept.Id);
      }
    }


    public void Delete() {

      this.Status = BillStatus.Deleted;

      foreach (var concept in Concepts) {
        concept.Delete();
        concept.Save();
      }
      foreach (var tax in BillTaxes) {
        tax.Delete();
        tax.Save();
      }
      foreach (var relatedBill in BillRelatedBills) {
        relatedBill.Delete();
        relatedBill.Save();
      }
    }


    private void GetTotals(FixedList<ComplementRelatedPayoutDataFields> payoutData) {

      Subtotal = payoutData.Select(x => x.Taxes.Sum(y => y.BaseAmount)).Sum();
      Total = payoutData.Sum(x => x.Monto);
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      BillData.WriteBill(this, ExtData.ToString());
    }


    internal void Update(BillFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValidDocument();
      fields.EnsureIsValidNote(BillCategory);

      RelatedBillNo = fields.BillRelatedDocument.RelatedCFDI;
      IssueDate = Patcher.Patch(fields.SchemaData.Fecha, IssueDate);
      IssuedBy = Patcher.Patch(fields.IssuedByUID, IssuedBy);
      IssuedTo = Patcher.Patch(fields.IssuedToUID, IssuedTo);
      _tags = EmpiriaString.Tagging(fields.Tags);
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      Subtotal = fields.Subtotal;
      Discount = fields.Discount;
      Total = fields.Total;

      SchemaData.Update(fields.SchemaData);
      SecurityData.Update(fields.SecurityData);
      BillExtData.Update(fields);
    }


    internal void UpdateFuelConsumptionBill(FuelConsumptionBillFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValidDocument();
      fields.EnsureIsValidFuelConsumption(BillCategory);

      RelatedBillNo = fields.CFDIRelated;
      IssueDate = Patcher.Patch(fields.SchemaData.Fecha, IssueDate);
      IssuedBy = Patcher.Patch(fields.IssuedByUID, IssuedBy);
      IssuedTo = Patcher.Patch(fields.IssuedToUID, IssuedTo);
      _tags = EmpiriaString.Tagging(fields.Tags);
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      Subtotal = fields.Subtotal;
      //Discount = fields.Discount;
      Total = fields.Total;

      SchemaData.Update(fields.SchemaData);
      SecurityData.Update(fields.SecurityData);
      BillExtData.UpdateFuelConsumptionComplementData(fields);
    }


    internal void UpdatePaymentComplement(BillPaymentComplementFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValidDocument();
      fields.EnsureIsValidPaymentComplement(BillCategory, PayableId, PayableTotal);

      RelatedBillNo = fields.CFDIRelated;
      IssueDate = Patcher.Patch(fields.SchemaData.Fecha, IssueDate);
      IssuedBy = Patcher.Patch(fields.IssuedByUID, IssuedBy);
      IssuedTo = Patcher.Patch(fields.IssuedToUID, IssuedTo);
      _tags = EmpiriaString.Tagging(fields.Tags);
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      Discount = fields.Discount;
      GetTotals(fields.ComplementRelatedPayoutData);

      SchemaData.Update(fields.SchemaData);
      SecurityData.Update(fields.SecurityData);
    }

    #endregion Methods

  } // class Bill

} // namespace Empiria.Billing
