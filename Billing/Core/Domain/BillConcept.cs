/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Partitioned Type                        *
*  Type     : BillConcept                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a bill concept with its tax entries.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Products;
using Empiria.Products.SATMexico;

using Empiria.Billing.Data;

namespace Empiria.Billing {

  /// <summary>Partitioned type that represents a bill concept with its tax entries.</summary>
  [PartitionedType(typeof(BillConceptType))]
  internal class BillConcept : BaseObject {

    #region Constructors and parsers

    protected BillConcept(BillConceptType billConceptType) : base(billConceptType) {
      // Required by Empiria Framework.
    }

    public BillConcept(Bill bill, Product product) : this(BillConceptType.Normal, bill, product) {
      // no-op
    }

    public BillConcept(BillConceptType billConceptType, Bill bill, Product product) : base(billConceptType) {
      Assertion.Require(bill, nameof(bill));
      Assertion.Require(!bill.IsEmptyInstance, nameof(bill));
      Assertion.Require(product, nameof(product));

      this.Bill = bill;
      this.Product = product;
    }


    static public BillConcept Parse(int id) => ParseId<BillConcept>(id);

    static public BillConcept Parse(string uid) => ParseKey<BillConcept>(uid);

    static internal FixedList<BillConcept> GetListFor(Bill bill) {
      Assertion.Require(bill, nameof(bill));

      return BillData.GetBillConcepts(bill);
    }

    static public BillConcept Empty => ParseEmpty<BillConcept>();

    #endregion Constructors and parsers

    #region Properties

    public BillConceptType BillConceptType {
      get {
        return (BillConceptType) base.GetEmpiriaType();
      }
    }


    [DataField("BILL_CONCEPT_ID")]
    public int BillConceptId {
      get; private set;
    } = -1;


    [DataField("BILL_CONCEPT_BILL_ID")]
    public Bill Bill {
      get; private set;
    }


    [DataField("BILL_CONCEPT_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("BILL_CONCEPT_SAT_PRODUCT_ID")]
    public SATProducto SATProduct {
      get; private set;
    }


    [DataField("BILL_CONCEPT_SAT_PRODUCT_CODE")]
    public string SATProductCode {
      get; set;
    }


    [DataField("BILL_CONCEPT_DESCRIPTION")]
    public string Description {
      get; set;
    }


    [DataField("BILL_CONCEPT_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("BILL_CONCEPT_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }

    [DataField("BILL_CONCEPT_QTY", ConvertFrom = typeof(float))]
    public decimal Quantity {
      get; private set;
    }


    [DataField("BILL_CONCEPT_UNIT_ID")]
    public ProductUnit QuantityUnit {
      get; private set;
    }


    [DataField("BILL_CONCEPT_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("BILL_CONCEPT_SUBTOTAL")]
    public decimal Subtotal {
      get; private set;
    }


    [DataField("BILL_CONCEPT_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("BILL_CONCEPT_SCHEMA_EXT_DATA")]
    private JsonObject SchemaExtData {
      get; set;
    }


    [DataField("BILL_CONCEPT_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("BILL_CONCEPT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BILL_CONCEPT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BILL_CONCEPT_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }


    public BillConceptSchemaData SchemaData {
      get {
        return new BillConceptSchemaData(this.SchemaExtData);
      }
    }


    public BillConceptExtData ConceptExtData {
      get {
        return new BillConceptExtData(this.ExtData);
      }
    }


    public FixedList<BillTaxEntry> TaxEntries {
      get {
        return BillTaxEntry.GetListFor(this.Bill.BillType.Id, this.BillConceptId);
      }
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(SATProductCode, Description, _identificators, _tags,
                                           SATProduct.Keywords, Product.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      BillData.WriteBillConcept(this, this.ExtData.ToString());
    }


    internal void Update(BillConceptFields fields) {

      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();
      this.SATProduct = Patcher.Patch(fields.SATProductUID, SATProducto.Empty);
      this.SATProductCode = fields.SATProductServiceCode;
      this.Product = Patcher.Patch(fields.ProductUID, Product);
      this.Description = Patcher.Patch(fields.Description, Description);
      _tags = EmpiriaString.Tagging(fields.Tags);
      this.Quantity = fields.Quantity;
      this.QuantityUnit = Product.BaseUnit;
      this.UnitPrice = fields.UnitPrice;
      this.Subtotal = fields.Subtotal;
      this.Discount = fields.Discount;

      SchemaData.Update(fields);
    }


    internal void UpdateComplementConcept(FuelConseptionComplementConceptDataFields fields) {

      fields.EnsureIsValid();
      this.SATProduct = Patcher.Patch(fields.SATProductUID, SATProducto.Empty);
      this.SATProductCode = fields.SATProductServiceCode;
      this.Product = Patcher.Patch(fields.ProductUID, Product);
      this.QuantityUnit = Product.BaseUnit;
      this.Quantity = fields.Cantidad;
      this.UnitPrice = fields.ValorUnitario;
      this.Subtotal = fields.Importe;
      _tags = EmpiriaString.Tagging(fields.Tags);

      SchemaData.UpdateComplementConcept(fields);
      ConceptExtData.Update(fields);
    }

    #endregion Methods

  } // class BillConcept

} // namespace Empiria.Billing
