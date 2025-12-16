/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Aggregate root information holder       *
*  Type     : PaymentOrder                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment order that serves as an aggregate root of payment instructions.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Represents a payment order that serves as an aggregate root of payment instructions.</summary>
  public class PaymentOrder : BaseObject {

    private Lazy<List<PaymentInstruction>> _paymentInstructions;

    #region Constructors and parsers

    protected PaymentOrder() {
      // Required by Empiria Framework.
    }


    public PaymentOrder(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      _payableEntityTypeId = payableEntity.GetEmpiriaType().Id;
      _payableEntityId = payableEntity.Id;

      RefreshPaymentInstructions();
    }

    static internal PaymentOrder Parse(int id) => ParseId<PaymentOrder>(id);

    static public PaymentOrder Parse(string uid) => ParseKey<PaymentOrder>(uid);

    static public PaymentOrder Empty => ParseEmpty<PaymentOrder>();

    static public FixedList<PaymentOrder> GetListFor(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      return PaymentOrderData.GetPaymentOrders(payableEntity);
    }

    protected override void OnLoad() {
      RefreshPaymentInstructions();
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
    internal JsonObject ExtData {
      get; private set;
    }


    internal string ReferenceNumber {
      get {
        return ExtData.Get("referenceNumber", string.Empty);
      }
      private set {
        ExtData.SetIfValue("referenceNumber", value);
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


    internal PaymentOrderRules Rules {
      get {
        return new PaymentOrderRules(this);
      }
    }

    #endregion Properties

    #region Payment instructions aggregate root

    public FixedList<PaymentInstruction> PaymentInstructions {
      get {
        return _paymentInstructions.Value.ToFixedList();
      }
    }


    public PaymentInstruction LastPaymentInstruction {
      get {
        return _paymentInstructions.Value[_paymentInstructions.Value.Count - 1];
      }
    }


    public void EnsureCanCreateInstruction() {
      if (IsEmptyInstance) {
        Assertion.RequireFail("No se puede crear una instrucción de pago " +
                              "para la instancia Empty.");
      }
      if (IsNew) {
        Assertion.RequireFail("No se puede crear la instrucción de pago " +
                              "debido a que la solicitud no ha sido guardada.");
      }

      if (!PaymentInstructions.All(x => x.Status.IsFinal())) {
        Assertion.RequireFail("No se puede ejecutar la operación debido a que esta " +
                              "solicitud de pago tiene una instrucción de pago " +
                              "que está programada o en proceso.");
      }

      if (Status == PaymentOrderStatus.Pending ||
          Status == PaymentOrderStatus.Failed) {
        return;
      }

      Assertion.RequireFail($"No se puede crear la instrucción de pago debido " +
                            $"a que tiene el estado {Status.GetName()}.");
    }


    internal PaymentInstruction CreatePaymentInstruction() {
      Assertion.Require(Rules.CanGeneratePaymentInstruction(),
                       $"No se puede crear la instrucción de pago por falta de permisos o " +
                       $"debido a que su estado es {Status.GetName()}.");

      EnsureCanCreateInstruction();

      var instruction = new PaymentInstruction(this);

      _paymentInstructions.Value.Add(instruction);

      Status = PaymentOrderStatus.Programmed;

      return instruction;
    }

    #endregion Payment instructions aggregate root

    #region Methods

    internal void Cancel() {
      Assertion.Require(Rules.CanCancel(),
                       $"No se puede cancelar la orden de pago por falta de permisos o " +
                       $"debido a que su estado es {Status.GetName()}.");

      Status = PaymentOrderStatus.Canceled;
    }


    internal void Reset() {
      Assertion.Require(Rules.CanReset(),
                       $"No se puede resetear la orden de pago por falta de permisos o " +
                       $"debido a que su estado es {Status.GetName()}.");

      Status = PaymentOrderStatus.Pending;
    }


    internal void Suspend() {
      Assertion.Require(Rules.CanSuspend(),
                       $"No se puede suspender la orden de pago por falta de permisos o " +
                       $"debido a que su estado es {Status.GetName()}.");

      Status = PaymentOrderStatus.Suspended;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        PaymentOrderNo = GeneratePaymentOrderNo();
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      PaymentOrderData.WritePaymentOrder(this);

      foreach (var instruction in _paymentInstructions.Value) {
        instruction.Save();
      }
    }


    internal void SetReferenceNumber(string referenceNumber) {

      if ((referenceNumber != null) || (referenceNumber != string.Empty)) {
        this.ReferenceNumber = referenceNumber;
      }

    }


    public void Update(PaymentOrderFields fields) {

      Assertion.Require(Rules.CanUpdate(),
                 $"No se puede actualizar la orden de pago por falta de permisos o " +
                 $"debido a que su estado es {Status.GetName()}.");

      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      PayTo = Patcher.Patch(fields.PayToUID, PayTo);
      PaymentMethod = Patcher.Patch(fields.PaymentMethodUID, PaymentMethod);
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      PaymentAccount = Patcher.Patch(fields.PaymentAccountUID, PaymentAccount);
      DueTime = Patcher.Patch(fields.DueTime, DueTime);
      Description = EmpiriaString.Clean(fields.Description);
      Observations = EmpiriaString.Clean(fields.Observations);
      RequestedBy = Patcher.Patch(fields.RequestedByUID, RequestedBy);
      ReferenceNumber = EmpiriaString.Clean(fields.ReferenceNumber);
      Total = fields.Total;
    }

    #endregion Methods

    #region Helpers

    private string GeneratePaymentOrderNo() {
      // ToDo: Generate real pament order number

      return "O-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }


    private void RefreshPaymentInstructions() {
      _paymentInstructions = new Lazy<List<PaymentInstruction>>(() =>
                                          PaymentInstructionData.GetPaymentInstructions(this));
    }

    #endregion Helpers

  }  // class PaymentOrder

}  // namespace Empiria.Payments
