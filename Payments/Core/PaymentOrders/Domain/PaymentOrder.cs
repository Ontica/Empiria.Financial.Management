/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentOrder                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment order.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Represents a payment order.</summary>
  public class PaymentOrder : BaseObject {

    #region Constructors and parsers

    public PaymentOrder() {
      // Required by Empiria Framework.
    }


    public PaymentOrder(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      _payableEntityTypeId = payableEntity.GetEmpiriaType().Id;
      _payableEntityId = payableEntity.Id;
    }


    static internal PaymentOrder Parse(string uid) => ParseKey<PaymentOrder>(uid);

    static internal PaymentOrder Parse(int id) => ParseId<PaymentOrder>(id);

    static public PaymentOrder Empty => ParseEmpty<PaymentOrder>();

    static public FixedList<PaymentOrder> GetListFor(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      return PaymentOrderData.GetPaymentOrders(payableEntity);
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYMT_ORD_NO")]
    public string PaymentOrderNo {
      get; private set;
    }


    [DataField("PYMT_ORD_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PYMT_ORD_OBSERVATIONS")]
    public string Observations {
      get; private set;
    }


    [DataField("PYMT_ORD_PAYABLE_ENTITY_TYPE_ID")]
    private int _payableEntityTypeId = -1;


    [DataField("PYMT_ORD_PAYABLE_ENTITY_ID")]
    private int _payableEntityId = -1;

    public IPayableEntity PayableEntity {
      get {
        return (IPayableEntity) Parse(_payableEntityTypeId, this._payableEntityId);
      }
    }

    [DataField("PYMT_ORD_PAY_TO_ID")]
    public Party PayTo {
      get; private set;
    }


    [DataField("PYMT_ORD_PAYMENT_METHOD_ID")]
    public PaymentMethod PaymentMethod {
      get; private set;
    }


    [DataField("PYMT_ORD_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("PYMT_ORD_PAYMENT_ACCOUNT_ID")]
    public PaymentAccount PaymentAccount {
      get; private set;
    }


    [DataField("PYMT_ORD_DUETIME")]
    public DateTime DueTime {
      get; private set;
    }


    [DataField("PYMT_ORD_SECURITY_EXT_DATA")]
    public JsonObject SecurityExtData {
      get; private set;
    }


    [DataField("PYMT_ORD_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }

    internal string ReferenceNumber {
      get {
        return this.ExtData.Get("referenceNumber", string.Empty);
      }
      private set {
        this.ExtData.SetIfValue("referenceNumber", value);
      }
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.PaymentOrderNo, this.PayTo.Name,
                                           this.RequestedBy.Name, this.PaymentMethod.Name);
      }
    }


    [DataField("PYMT_ORD_REQUESTED_BY_ID")]
    public OrganizationalUnit RequestedBy {
      get; private set;
    }


    [DataField("PYMT_ORD_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("PYMT_ORD_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("PYMT_ORD_AUTHORIZATION_TIME")]
    public DateTime AuthorizedTime {
      get; private set;
    }


    [DataField("PYMT_ORD_PAYED_BY_ID")]
    public OrganizationalUnit PayedBy {
      get; private set;
    }


    [DataField("PYMT_ORD_CLOSING_TIME")]
    public DateTime ClosingTime {
      get; private set;
    }


    [DataField("PYMT_ORD_CLOSED_BY_ID")]
    public Party ClosedBy {
      get; private set;
    }



    [DataField("PYMT_ORD_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PYMT_ORD_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PYMT_ORD_STATUS", Default = PaymentOrderStatus.Pending)]
    public PaymentOrderStatus Status {
      get; private set;
    } = PaymentOrderStatus.Pending;


    public decimal Total {
      get; internal set;
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      Assertion.Require(this.Status == PaymentOrderStatus.Pending,
                  $"No se puede eliminar una orden de pago que está en estado {this.Status.GetName()}.");

      this.Status = PaymentOrderStatus.Deleted;
    }

    protected override void OnSave() {
      if (base.IsNew) {
        this.PaymentOrderNo = GeneratePaymentOrderNo();
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      PaymentOrderData.WritePaymentOrder(this, this.SecurityExtData.ToString(), this.ExtData.ToString());
    }


    internal void Pay() {
      Assertion.Require(this.Status == PaymentOrderStatus.Received,
               $"No se puede realizar el pago debido " +
               $"a que tiene el estado {this.Status.GetName()}.");

      this.Status = PaymentOrderStatus.Payed;
    }


    internal void Reject() {
      Assertion.Require(this.Status == PaymentOrderStatus.Pending || this.Status == PaymentOrderStatus.Received,
               $"No se puede realizar el pago debido " +
               $"a que tiene el estado {this.Status.GetName()}.");
      this.Status = PaymentOrderStatus.Rejected;
    }


    internal void SentToPay() {
      Assertion.Require(this.Status == PaymentOrderStatus.Pending,
               $"No se puede realizar el pago debido " +
               $"a que tiene el estado {this.Status.GetName()}.");

      this.Status = PaymentOrderStatus.Received;
    }


    internal void SetReferenceNumber(string referenceNumber) {

      if ((referenceNumber != null) || (referenceNumber != string.Empty)) {
        this.ReferenceNumber = referenceNumber;
      }

    }


    internal void Suspend(Party suspendedBy, DateTime suspendedUntil) {
      Assertion.Require(suspendedBy, nameof(suspendedBy));

      Assertion.Require(this.Status == PaymentOrderStatus.Received ||
                        this.Status == PaymentOrderStatus.Suspended,
                $"No se puede suspender la orden de pago debido " +
                $"a que tiene el estado {this.Status.GetName()}.");

      this.Status = PaymentOrderStatus.Suspended;
    }


    public void Update(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(this.Status == PaymentOrderStatus.Pending,
                        $"No se pueden actualizar los datos de la orden de pago " +
                        $"debido a que tiene el estado {this.Status.GetName()}.");

      fields.EnsureValid();

      Description = fields.Description;
      Observations = fields.Observations;
      PayTo = Party.Parse(fields.PayToUID);
      PaymentMethod = PaymentMethod.Parse(fields.PaymentMethodUID);
      Currency = Currency.Parse(fields.CurrencyUID);
      PaymentAccount = PaymentAccount.Parse(fields.PaymentAccountUID);
      DueTime = fields.DueTime;
      RequestedTime = fields.RequestedTime;
      RequestedBy = OrganizationalUnit.Parse(fields.RequestedByUID);
      ReferenceNumber = fields.ReferenceNumber;
      Total = fields.Total;
    }

    #endregion Methods

    #region Helpers

    private string GeneratePaymentOrderNo() {
      return "O-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }

    #endregion Helpers

  }  // class PaymentOrder

}  // namespace Empiria.Payments
