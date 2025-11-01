/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PayableItem                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an item of a payable object. A payable can be a bill, a contract supply order,      *
*             a service order, a loan, travel expenses, a fixed fund provision, etc.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Billing;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Data;

namespace Empiria.Payments.Payables {

  /// <summary>Represents an item of a payable object. A payable can be a bill, a contract supply order,
  /// a service order, a loan, travel expenses, a fixed fund provision, etc.</summary>
  internal class PayableItem : BaseObject {

    #region Constructors and parsers

    private PayableItem() {
      // Required by Empiria Framework.
    }


    internal PayableItem(Payable payable) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(!payable.IsEmptyInstance,
                        "payable can not be the empty instance.");

      Payable = payable;
    }

    static public PayableItem Parse(int id) => ParseId<PayableItem>(id);

    static internal PayableItem Parse(string UID) {
      return ParseKey<PayableItem>(UID);
    }

    static public PayableItem Empty => ParseEmpty<PayableItem>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PAYABLE_ITEM_PAYABLE_ID")]
    public Payable Payable {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_ENTITY_ITEM_ID")]
    public int PayableEntityItemId {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_PRODUCT_UNIT_ID")]
    public ProductUnit Unit {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_PRODUCT_QTY")]
    public decimal Quantity {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    public decimal Total {
      get {
        return Math.Round((Quantity * UnitPrice) - Discount, 2);
      }
    }


    [DataField("PAYABLE_ITEM_BUDGET_ACCOUNT_ID")]
    public FormerBudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_BILL_CONCEPT_ID")]
    public BillConcept BillConcept {
      get; private set;
    }


    public bool HasBillConcept {
      get {
        return !BillConcept.IsEmptyInstance;
      }
    }

    [DataField("PAYABLE_ITEM_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("PAYABLE_ITEM_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Pending;


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Product.Keywords, Description, Payable.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }
      PayableData.WritePayableItem(this, this.ExtData.ToString());
    }


    internal void SetBillConcept(BillConcept billConcept) {
      Assertion.Require(billConcept, nameof(billConcept));

      this.BillConcept = billConcept;
    }


    internal void Update(PayableItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.Product = Product.Parse(fields.ProductUID);
      this.Unit = ProductUnit.Parse(fields.UnitUID);

      this.PayableEntityItemId = fields.EntityItemId;
      this.Description = fields.Description;
      this.Quantity = fields.Quantity;
      this.UnitPrice = fields.UnitPrice;
      this.Discount = fields.Discount;
      this.BudgetAccount = FormerBudgetAccount.Parse(fields.BudgetAccountUID);
    }

    #endregion Methods

  }  // class PayableItem

}  // namespace Empiria.Payments.Payables
