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
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Data;
using Empiria.Financial;

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


    [DataField("PAYABLE_ITEM_ENTITY_TYPE_ID")]
    public int PayableEntityTypeId {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_INPUT_TOTAL")]
    public decimal InputTotal {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_OUTPUT_TOTAL")]
    public decimal OutputTotal {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_EXCHANGE_RATE")]
    public decimal ExchangeRate {
      get; private set;
    }


    [DataField("PAYABLE_ITEM_CONTROL_EXT_DATA")]
    private JsonObject ControlExtData {
      get; set;
    }


    [DataField("PAYABLE_ITEM_SECURITY_EXT_DATA")]
    private JsonObject SecurityExtData {
      get; set;
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
        return EmpiriaString.BuildKeywords(Payable.Keywords);
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
      PayableData.WritePayableItem(this, this.ControlExtData.ToString(), this.SecurityExtData.ToString(), this.ExtData.ToString());
    }


    internal void Update(PayableItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.PayableEntityItemId = fields.EntityItemId;
      this.PayableEntityTypeId = fields.EntityTypeId;
      this.InputTotal = fields.InputTotal;
      this.OutputTotal = fields.OutputTotal;
      this.Currency = Currency.Parse(fields.CurrencyUID);
      this.ExchangeRate = fields.ExchangeRate;
    }

    #endregion Methods

  }  // class PayableItem

}  // namespace Empiria.Payments.Payables
