/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillConcept                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a bill concept with its tax entries.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Products.SATMexico;

using Empiria.Billing.Data;

namespace Empiria.Billing {

  /// <summary>Represents a bill concept with its tax entries.</summary>
  internal class BillConcept : BaseObject {

    #region Constructors and parsers

    internal BillConcept() {
      // Required by Empiria Framework.
    }

    public BillConcept(Bill bill, Product product) {
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

    [DataField("BILL_CONCEPT_TYPE_ID")]
    public int BillConceptTypeId {
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
        return _identificators.Split(' ').ToFixedList();
      }
      private set {
        _identificators = string.Join(" ", value);
      }
    }


    [DataField("BILL_CONCEPT_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
      }
      private set {
        _tags = string.Join(" ", value);
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

    public FixedList<BillTaxEntry> TaxEntries {
      get; set;
    } = new FixedList<BillTaxEntry>();


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(SATProductCode, Description, _identificators, _tags,
                                           SATProduct.Keywords, Product.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    internal void Update(BillConceptFields fields) {

      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();
      this.BillConceptTypeId = -1; //TODO ASIGNAR EN BD
      this.SATProduct = PatchField(fields.SATProductUID, SATProducto.Empty);
      this.SATProductCode = fields.SATProductCode;
      this.Product = PatchField(fields.ProductUID, Product);
      this.Description = PatchField(fields.Description, Description);
      this.Identificators = fields.Identificators.ToFixedList();
      this.Tags = fields.Tags.ToFixedList();
      this.Quantity = fields.Quantity;
      this.QuantityUnit = Product.BaseUnit;
      this.UnitPrice = fields.UnitPrice;
      this.Subtotal = fields.Subtotal;
      this.Discount = fields.Discount;

      SchemaData.Update(fields);
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      BillData.WriteBillConcept(this, this.ExtData.ToString());
    }

    #endregion Methods

  } // class BillConcept

} // namespace Empiria.Billing
