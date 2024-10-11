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

    internal protected Payable(PayableType payableType) : base(payableType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public Payable Parse(int id) => ParseId<Payable>(id);

    static internal Payable Parse(string UID) {
      return ParseKey<Payable>(UID);
    }

    static public Payable Empty => ParseEmpty<Payable>();

    protected override void OnLoad() {
      _items = new Lazy<List<PayableItem>>(() => PayableData.GetPayableItems(this));
    }

    #endregion Constructors and parsers

    #region Properties

    public PayableType PayableType {
      get {
        return (PayableType) GetEmpiriaType();
      }
    }

    [DataField("PYM_PYB_NO")]
    public string payableNo {
      get; private set;
    }


    [DataField("PYM_PYB_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PYM_PYB_ORG_UNIT_ID")]
    public OrganizationalUnit OrganizationalUnit {
      get; private set;
    }


    [DataField("PYM_PYB_PAY_TO_ID")]
    public Party PayTo {
      get; private set;
    }


    [DataField("PYM_PYB_BUDGET_TYPE_ID")]
    public BudgetType BudgetType {
      get; private set;
    }


    [DataField("PYM_PYB_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PYM_PYB_TOTAL")]
    public decimal Total {
      get; private set;
    }


    [DataField("PYM_PYB_DUETIME")]
    public DateTime DueTime {
      get; private set;
    } = ExecutionServer.DateMaxValue;


    [DataField("PYM_PYB_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    } = JsonObject.Empty;


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.payableNo, this.PayTo.Name, this.PayableType.Name);
      }
    }


    [DataField("PYM_PYB_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("PYM_PYB_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("PYM_PYB_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PYM_PYB_STATUS", Default = PayableStatus.Capture)]
    public PayableStatus Status {
      get; private set;
    } = PayableStatus.Capture;

    #endregion Properties

    #region Methods

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


    internal void Update(PayableFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.Description = fields.Description;
      this.OrganizationalUnit = OrganizationalUnit.Parse(fields.OrganizationalUnitUID);
      this.PayTo = Party.Parse(fields.PayToUID);
      this.BudgetType = BudgetType.Parse(fields.BudgetTypeUID);
      this.Currency = Currency.Parse(fields.CurrencyUID);
      this.DueTime = fields.DueTime;
      this.RequestedTime = fields.RequestedTime;
    }

    #endregion Methods

    #region Aggregate root methods

    internal void AddItem(PayableItem payableItem) {
      Assertion.Require(payableItem, nameof(payableItem));
      Assertion.Require(payableItem.Payable.Equals(this),
                       "wrong payableItem.Payable instance");

      _items.Value.Add(payableItem);

      Total += payableItem.Subtotal;
    }


    internal PayableItem CreateItem() {
      return new PayableItem(this);
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


    internal PayableItem RemoveItem(string payableItemUID) {
      Assertion.Require(payableItemUID, nameof(payableItemUID));

      PayableItem payableItem = GetItem(payableItemUID);

      _items.Value.Remove(payableItem);

      payableItem.Delete();

      Total -= payableItem.Subtotal;

      return payableItem;
    }


    internal PayableItem UpdateItem(string payableItemUID, PayableItemFields fields) {
      Assertion.Require(payableItemUID, nameof(payableItemUID));
      Assertion.Require(fields, nameof(fields));

      PayableItem payableItem = GetItem(payableItemUID);
     
      _items.Value.Remove(payableItem);

      Total -= payableItem.Subtotal;

      payableItem.Update(fields);

      _items.Value.Add(payableItem);
      
      Total += payableItem.Subtotal;

      return payableItem;
    }

    #endregion Aggregate root methods

    #region Helpers

    static public string GeneratePayableNo() {
      int current = PayableData.GetLastPayableNumber();

      current++;

      return $"OP-2024-{current:00000}";
    }

    #endregion Helpers

  }  // class Payable

}  // namespace Empiria.Payments.Payables
