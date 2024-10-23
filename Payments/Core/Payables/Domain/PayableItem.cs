/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PayableItem                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an item of a payable object. A payable can be a bill, a contract milestone,         *
*             a service order, a loan, travel expenses, a fixed fund provision, etc.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Budgeting;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Data;

namespace Empiria.Payments.Payables {

  /// <summary>Represents an item of a payable object. A payable can be a bill, a contract milestone,
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

    [DataField("PAYABLE_ID")]
    public Payable Payable {
      get; private set;
    }


    //[DataField("PAYABLE_ENTITY_ITEM_ID")]
    public int PayableEntityItem {
      get; private set;
    }


    public Product Product {
      get; private set;
    }


    public ProductUnit Unit {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_QTY", ConvertFrom = typeof(decimal))]
    public decimal Quantity {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_EXCH_RATE", ConvertFrom = typeof(decimal))]
    public decimal ExchangeRate {
      get; private set;
    } = 1;


    [DataField("PAYABLE_ITEM_QTY", ConvertFrom = typeof(decimal))]
    public decimal UnitPrice {
      get; private set;
    }


    public decimal Subtotal {
      get {
        return Math.Round(UnitPrice * Quantity * ExchangeRate);
      }
    }


    [DataField("PAYABLE_ITEM_BDG_ACCT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("PAYABLE_ITEM_POSTED_BY_ID")]
    public Contact PostedBy {
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

    #endregion Properties

    #region Methods


    protected override void OnBeforeSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }
    }


    protected override void OnSave() {
      PayableData.WritePayableItem(this, this.ExtData.ToString());
    }


    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    internal void Update(PayableItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      //this.Product = Product..Parse(fields.ProductUID);
      //this.Unit = ProductUnit.Parse(fields.UnitUID);

      this.PayableEntityItem = -1;
      this.Description = fields.Description;
      this.Quantity = fields.Quantity;
      this.Currency = Currency.Parse(fields.CurrencyUID);
      this.ExchangeRate = fields.ExchangeRate;
      this.UnitPrice = fields.UnitPrice;
      this.BudgetAccount = BudgetAccount.Parse(fields.BudgetAccountUID);
    }

    #endregion Methods

  }  // class PayableItem

}  // namespace Empiria.Payments.Payables
