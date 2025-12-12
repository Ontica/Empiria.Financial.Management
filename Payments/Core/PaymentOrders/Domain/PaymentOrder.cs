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

using Empiria.Payments.Processor;

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

    static internal PaymentOrder Parse(int id) => ParseId<PaymentOrder>(id);

    static public PaymentOrder Parse(string uid) => ParseKey<PaymentOrder>(uid);

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
        return EmpiriaString.BuildKeywords(PaymentOrderNo, PayTo.Name,
                                           RequestedBy.Name, PaymentMethod.Name);
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


    // ToDo: How to deal with this
    public decimal Total {
      get {
        return ExtData.Get("total", 0m);
      }
      private set {
        ExtData.Set("total", value);
      }
    }

    #endregion Properties

    #region Methods

    internal void Cancel() {
      Assertion.Require(Status == PaymentOrderStatus.Pending,
               $"No se puede rechazar el pago debido " +
               $"a que tiene el estado {Status.GetName()}.");
      Status = PaymentOrderStatus.Canceled;
    }


    public bool CanCreatePaymentInstruction() {
      if (Status == PaymentOrderStatus.Pending ||
          Status == PaymentOrderStatus.Failed) {
        return true;
      }
      return false;
    }


    internal PaymentInstruction CreatePaymentInstruction(PaymentsBroker broker) {
      Assertion.Require(broker, nameof(broker));

      Assertion.Require(CanCreatePaymentInstruction(),
               $"No se puede crear la instrucción de pago debido " +
               $"a que tiene el estado {Status.GetName()}.");

      var instruction = new PaymentInstruction(broker, this);

      // ToDo: Create aggregate root  -> _instructions.Add(instruction);

      Status = PaymentOrderStatus.Programmed;

      return instruction;
    }


    internal void Delete() {
      Assertion.Require(Status == PaymentOrderStatus.Pending,
                  $"No se puede eliminar una orden de pago que " +
                  $"está en estado {Status.GetName()}.");

      Status = PaymentOrderStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        PaymentOrderNo = GeneratePaymentOrderNo();
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      PaymentOrderData.WritePaymentOrder(this, SecurityExtData.ToString(), ExtData.ToString());
    }


    internal void SetAsPayed() {
      Assertion.Require(Status == PaymentOrderStatus.InProgress,
               $"No se puede cambiar el estado del pago debido " +
               $"a que tiene el estado {Status.GetName()}.");

      Status = PaymentOrderStatus.Payed;
    }


    internal void SetAsPending() {
      Assertion.Require(Status == PaymentOrderStatus.Suspended ||
                        Status == PaymentOrderStatus.Programmed,
                        $"No se puede cambiar el estado del pago debido " +
                        $"a que tiene el estado {Status.GetName()}.");

      Status = PaymentOrderStatus.Pending;
    }


    internal void SetReferenceNumber(string referenceNumber) {

      if ((referenceNumber != null) || (referenceNumber != string.Empty)) {
        this.ReferenceNumber = referenceNumber;
      }

    }


    internal void Suspend(Party suspendedBy, DateTime suspendedUntil) {
      Assertion.Require(suspendedBy, nameof(suspendedBy));

      Assertion.Require(Status == PaymentOrderStatus.Pending ||
                        Status == PaymentOrderStatus.Programmed,
                $"No se puede suspender la orden de pago debido " +
                $"a que tiene el estado {Status.GetName()}.");

      Status = PaymentOrderStatus.Suspended;
    }


    public void Update(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(Status == PaymentOrderStatus.Pending,
                        $"No se pueden actualizar los datos de la orden de pago " +
                        $"debido a que tiene el estado {Status.GetName()}.");

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
      // ToDo: Generate real pament order number

      return "O-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }


    internal void UpdatePaymentInstruction(PaymentInstruction instruction,
                                           string externalRequestID,
                                           PaymentInstructionStatus initialStatus) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(externalRequestID, nameof(externalRequestID));

      instruction.SetExternalUniqueNo(externalRequestID);
      instruction.UpdateStatus(initialStatus);

      if (initialStatus == PaymentInstructionStatus.InProcess) {
        Status = PaymentOrderStatus.InProgress;

      } else if (initialStatus == PaymentInstructionStatus.Failed) {
        Status = PaymentOrderStatus.Failed;
      }
    }

    #endregion Helpers

  }  // class PaymentOrder

}  // namespace Empiria.Payments
