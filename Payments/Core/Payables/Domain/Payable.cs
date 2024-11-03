/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Aggregate root, Partitioned type        *
*  Type     : Payable                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payable object that is an aggregate root of PayableItem objects.                  *
*             A payable can be a bill, a contract milestone, a service order, a loan, travel expenses,       *
*             fixed fund provision, etc.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;

using Empiria.Budgeting;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Data;

namespace Empiria.Payments.Payables {

  /// <summary>Represents a payable object that is an aggregate root of PayableItem objects.
  /// A payable can be a bill, a contract milestone, a service order, a loan, travel expenses,
  /// fixed fund provision, etc.</summary>
  [PartitionedType(typeof(PayableType))]
  internal class Payable : BaseObject {

    #region Fields

    private Lazy<List<PayableItem>> _items = new Lazy<List<PayableItem>>();

    #endregion Fields

    #region Constructors and parsers

    private Payable(PayableType payableType) : base(payableType) {
      // Required by Empiria Framework for all partitioned types.
    }


    internal Payable(PayableType payableType, IPayableEntity entity) : base(payableType) {
      Assertion.Require(entity, nameof(entity));

      this.PayableEntity = entity;
    }

    static public Payable Parse(int id) => ParseId<Payable>(id);

    static internal Payable Parse(string UID) {
      return ParseKey<Payable>(UID);
    }

    static public Payable Empty => ParseEmpty<Payable>();

    protected override void OnLoad() {
      Reload();
    }

    #endregion Constructors and parsers

    #region Properties

    public PayableType PayableType {
      get {
        return (PayableType) GetEmpiriaType();
      }
    }

    [DataField("PAYABLE_NO")]
    public string payableNo {
      get; private set;
    }


    [DataField("PAYABLE_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PAYABLE_ENTITY_ID")]
    private int _payableEntityId = -1;


    public IPayableEntity PayableEntity {
      get {
        return PayableType.ParsePayableEntity(_payableEntityId);
      }
      private set {
        _payableEntityId = value.Id;
      }
    }


    [DataField("PAYABLE_ORG_UNIT_ID")]
    public OrganizationalUnit OrganizationalUnit {
      get; private set;
    }


    [DataField("PAYABLE_PAY_TO_ID")]
    public Party PayTo {
      get; private set;
    }


    [DataField("PAYABLE_BUDGET_TYPE_ID")]
    public BudgetType BudgetType {
      get; private set;
    }


    [DataField("PAYABLE_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PAYABLE_TOTAL")]
    public decimal Total {
      get; private set;
    }


    [DataField("PAYABLE_DUETIME")]
    public DateTime DueTime {
      get; private set;
    } = ExecutionServer.DateMaxValue;


    [DataField("PAYABLE_PAYMENT_METHOD_ID")]
    public PaymentMethod PaymentMethod {
      get; private set;
    } = PaymentMethod.Empty;


    [DataField("PAYABLE_PAYMENT_ACCOUNT_ID")]
    public PaymentAccount PaymentAccount {
      get; private set;
    } = PaymentAccount.Empty;


    [DataField("PAYABLE_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    } = JsonObject.Empty;


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.payableNo, this.PayTo.Name, this.PayableType.Name);
      }
    }


    [DataField("PAYABLE_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("PAYABLE_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("PAYABLE_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PAYABLE_STATUS", Default = PayableStatus.Capture)]
    public PayableStatus Status {
      get; private set;
    } = PayableStatus.Capture;

    #endregion Properties

    #region Methods

    public BaseObject GetPayableEntity() {
      return PayableType.PayableEntityType.ParseObject(_payableEntityId);
    }

    protected override void OnBeforeSave() {
      if (base.IsNew) {
        this.payableNo = GeneratePayableNo();
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }
    }

    protected override void OnSave() {
      PayableData.WritePayable(this, this.ExtData.ToString());
    }


    internal void Delete() {
      Assertion.Require(this.Status == PayableStatus.Capture,
                 $"No se puede eliminar una obligación de pago que está en estado {this.Status.GetName()}.");

      this.Status = PayableStatus.Deleted;
    }


    internal void OnPayment() {
      Assertion.Require(this.Status == PayableStatus.Capture,
                 $"No se puede generar la instrucción de pago de una obligación de pago que está en estado {this.Status.GetName()}.");

      this.Status = PayableStatus.OnPayment;
    }


    internal void Pay() {
      Assertion.Require(this.Status == PayableStatus.OnPayment,
                 $"No se puede cambiar a pagada una obligación de pago que está en estado {this.Status.GetName()}.");

      this.Status = PayableStatus.Payed;
    }

    internal void Update(PayableFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Description = GetDescription(fields.Description);
      OrganizationalUnit = OrganizationalUnit.Parse(fields.OrganizationalUnitUID);
      PayTo = Party.Parse(fields.PayToUID);
      BudgetType = BudgetType.Parse(fields.BudgetTypeUID);
      Currency = Currency.Parse(fields.CurrencyUID);
      DueTime = fields.DueTime;
      RequestedTime = fields.RequestedTime;

      UpdatePaymentData(fields);
    }


    private string GetDescription(string description) {
      description = EmpiriaString.Clean(description);

      if (description.Length != 0) {
        return description;
      } else {
        return PayableEntity.Description;
      }
    }


    #endregion Methods

    #region Aggregate root methods

    internal void AddItem(PayableItem payableItem) {
      Assertion.Require(payableItem, nameof(payableItem));
      Assertion.Require(payableItem.Payable.Equals(this),
                       "wrong payableItem.Payable instance");

      _items.Value.Add(payableItem);

      Total += payableItem.Total;
    }


    internal PayableItem GetItem(string payableItemUID) {
      Assertion.Require(payableItemUID, nameof(payableItemUID));

      PayableItem payableItem = _items.Value.Find(x => x.UID == payableItemUID);

      Assertion.Require(payableItem, "PayableItem not found.");

      return payableItem;
    }


    internal FixedList<PayableItem> GetItems() {
      return _items.Value.ToFixedList();
    }

    internal void Reload() {
      _items = new Lazy<List<PayableItem>>(() => PayableData.GetPayableItems(this));
    }

    internal PayableItem RemoveItem(string payableItemUID) {
      Assertion.Require(payableItemUID, nameof(payableItemUID));

      PayableItem payableItem = GetItem(payableItemUID);

      _items.Value.Remove(payableItem);

      payableItem.Delete();

      Total -= payableItem.Total;

      return payableItem;
    }


    internal PayableItem UpdateItem(string payableItemUID, PayableItemFields fields) {
      Assertion.Require(payableItemUID, nameof(payableItemUID));
      Assertion.Require(fields, nameof(fields));

      PayableItem payableItem = GetItem(payableItemUID);

      _items.Value.Remove(payableItem);

      Total -= payableItem.Total;

      payableItem.Update(fields);

      _items.Value.Add(payableItem);

      Total += payableItem.Total;

      return payableItem;
    }

    #endregion Aggregate root methods

    #region Helpers

    static public string GeneratePayableNo() {
      int current = PayableData.GetLastPayableNumber();

      current++;

      return $"OP-2024-{current:00000}";
    }


    private void UpdatePaymentData(PayableFields fields) {
      this.PaymentMethod = PaymentMethod.Parse(fields.PaymentMethodUID);

      if (this.PaymentMethod.LinkedToAccount) {
        this.PaymentAccount = PaymentAccount.Parse(fields.PaymentAccountUID);
      } else {
        this.PaymentAccount = PaymentAccount.Empty;
      }
    }

    #endregion Helpers

  }  // class Payable

}  // namespace Empiria.Payments.Payables
