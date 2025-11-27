/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PayableItem                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an item of a payment object. It can be a bill, a contract supply order,             *
*             a service order, a loan, travel expenses, a fixed fund provision, etc.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Payments.Orders.Data;

namespace Empiria.Payments.Orders {

  /// <summary>Represents an item of a payment order object.It can be a bill, a contract supply order,
  /// a service order, a loan, travel expenses, a fixed fund provision, etc.</summary>
  internal class PaymentOrderItem : BaseObject {

    #region Constructors and parsers

    private PaymentOrderItem() {
      // Required by Empiria Framework.
    }


    internal PaymentOrderItem(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance,
                        "PaymentOrder can not be the empty instance.");

      PaymentOrder = paymentOrder;
    }

    static public PaymentOrderItem Parse(int id) => ParseId<PaymentOrderItem>(id);

    static internal PaymentOrderItem Parse(string UID) {
      return ParseKey<PaymentOrderItem>(UID);
    }

    static public PaymentOrderItem Empty => ParseEmpty<PaymentOrderItem>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYMT_ORD_ITEM_PYMT_ORDER_ID")]
    public PaymentOrder PaymentOrder {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_PAYABLE_TYPE_ID")]
    public int PayableTypeId {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_PAYABLE_ID")]
    public int PayableId {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_DEPOSIT_AMOUNT")]
    public decimal DepositAmount {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_WITHDRAW_AMOUNT")]
    public decimal WithdrawAmount {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_EXCHANGE_RATE")]
    public decimal ExchangeRate {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_SECURITY_EXT_DATA")]
    private JsonObject SecurityExtData {
      get; set;
    }


    [DataField("PYMT_ORD_ITEM_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(PaymentOrder.Keywords);
      }
    }


    [DataField("PYMT_ORD_ITEM_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PYMT_ORD_ITEM_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Pending;

    #endregion Properties

    #region Methods

    internal void Delete() {
      Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      PaymentOrderData.WritePaymentOrderItem(this, this.SecurityExtData.ToString(), this.ExtData.ToString());
    }


    internal void Update(PaymentOrderItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      PayableId = fields.EntityItemId;
      PayableTypeId = fields.EntityTypeId;
      DepositAmount = fields.InputTotal;
      WithdrawAmount = fields.OutputTotal;
      Currency = Currency.Parse(fields.CurrencyUID);
      ExchangeRate = fields.ExchangeRate;
    }

    #endregion Methods

  }  // class PaymentOrderItem

}  // namespace Empiria.Payments.Orders
