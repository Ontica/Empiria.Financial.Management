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
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Represents a payment order that serves as an aggregate root of payment instructions.</summary>
  public class PaymentOrder : BaseObject {

    private Lazy<List<PaymentInstruction>> _paymentInstructions;

    #region Constructors and parsers

    protected PaymentOrder() {
      // Required by Empiria Framework.
    }


    public PaymentOrder(PaymentType paymentType, Party payTo, IPayableEntity payableEntity, decimal total) {
      Assertion.Require(paymentType, nameof(paymentType));
      Assertion.Require(!paymentType.IsEmptyInstance, nameof(paymentType));
      Assertion.Require(payTo, nameof(payTo));
      Assertion.Require(!payTo.IsEmptyInstance, nameof(payTo));
      Assertion.Require(payableEntity, nameof(payableEntity));
      Assertion.Require(total > 0, "total must be a positive number");

      PaymentType = paymentType;
      PayTo = payTo;
      Total = total;

      _payableEntityTypeId = payableEntity.GetEmpiriaType().Id;
      _payableEntityId = payableEntity.Id;

      RefreshPaymentInstructions();
    }

    static public PaymentOrder Parse(int id) => ParseId<PaymentOrder>(id);

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

    [DataField("PYMT_ORD_PAYMENT_TYPE_ID")]
    public PaymentType PaymentType {
      get; private set;
    }


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


    [DataField("PYMT_ORD_DEBTOR_ID")]
    public Party Debtor {
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


    public decimal ExchangeRate {
      get {
        return ExtData.Get("exchangeRate", decimal.One);
      }
      private set {
        ExtData.SetIf("exchangeRate", value, value != decimal.One);
      }
    }


    [DataField("PYMT_ORD_TOTAL")]
    public decimal Total {
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


    public Priority Priority {
      get {
        return ExtData.Get("priority", Priority.Normal);
      }
      private set {
        ExtData.SetIf("priority", value.ToString(), value != Priority.Normal);
      }
    }


    [DataField("PYMT_ORD_EXT_DATA")]
    internal JsonObject ExtData {
      get; private set;
    }


    public string ReferenceNumber {
      get {
        return ExtData.Get("referenceNumber", string.Empty);
      }
      private set {
        ExtData.SetIfValue("referenceNumber", value);
      }
    }


    public string AccountingVoucher {
      get {
        return ExtData.Get("accountingVoucher", string.Empty);
      }
      private set {
        ExtData.SetIfValue("accountingVoucher", value);
      }
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(PaymentOrderNo, PayTo.Keywords,
                                           RequestedBy.Keywords, PaymentMethod.Name,
                                           ReferenceNumber, PayableEntity.Name,
                                           PayableEntity.Description, Description, Observations,
                                           Debtor.Keywords);
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


    public PaymentOrderRules Rules {
      get {
        return new PaymentOrderRules(this);
      }
    }


    public bool InProgress {
      get {
        return Status != PaymentOrderStatus.Canceled &&
               Status != PaymentOrderStatus.Failed &&
               Status != PaymentOrderStatus.Payed &&
               !IsEmptyInstance;
      }
    }


    public bool Payed {
      get {
        return Status == PaymentOrderStatus.Payed;
      }
    }

    #endregion Properties

    #region Payment instructions aggregate root

    public bool HasActivePaymentInstruction {
      get {
        return PaymentInstructions.Any(x => x.Status.IsActive());
      }
    }


    public PaymentInstruction LastPaymentInstruction {
      get {
        if (_paymentInstructions.Value.Count == 0) {
          return PaymentInstruction.Empty;
        }
        return _paymentInstructions.Value[_paymentInstructions.Value.Count - 1];
      }
    }


    public FixedList<PaymentInstruction> PaymentInstructions {
      get {
        return _paymentInstructions.Value.ToFixedList();
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

      Budget budget = (Budget) PayableEntity.Budget;

      if (!budget.BudgetType.Equals(BudgetType.None)) {

        var bdgTxn = TryGetApprovedBudget();

        if (bdgTxn == null || bdgTxn.InProcess) {
          Assertion.RequireFail("No se puede crear la instrucción de pago debido " +
                                "a que la solicitud de pago no tiene el presupuesto aprobado.");
        }

      }

      if (HasActivePaymentInstruction) {
        Assertion.RequireFail("No se puede ejecutar la operación debido a que la " +
                              "solicitud de pago tiene una instrucción de pago " +
                              "que está programada o en proceso.");
      }

      if (Currency.Distinct(Currency.Default) && ExchangeRate == decimal.One) {
        Assertion.RequireFail("No se puede crear la instrucción de pago debido a que " +
                              "no se ha proporcionado el tipo de cambio.");
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


    internal void SetAsPayed(PaymentInstruction instruction) {
      Assertion.Require(instruction.Status == PaymentInstructionStatus.Payed,
                       "La instrucción de pago debe estar en estado 'Pagada'.");

      Status = PaymentOrderStatus.Payed;

      Save();
    }


    internal void Suspend() {
      Assertion.Require(Rules.CanSuspend(),
                       $"No se puede suspender la orden de pago por falta de permisos o " +
                       $"debido a que su estado es {Status.GetName()}.");

      Status = PaymentOrderStatus.Suspended;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        PaymentOrderNo = PaymentOrderData.GeneratePaymentOrderNo(this);
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      PaymentOrderData.WritePaymentOrder(this);

      foreach (var instruction in _paymentInstructions.Value) {
        instruction.Save();
      }
    }


    public BudgetTransaction TryGetApprovedBudget() {
      var payableEntity = (IBudgetable) PayableEntity;

      var budgetTxns = BudgetTransaction.GetFor(payableEntity)
                                        .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment);

      var approvedTransaction = budgetTxns.Find(x => x.InProcess || x.IsClosed);

      return approvedTransaction;
    }


    public void Update(PaymentOrderFields fields) {

      Assertion.Require(Rules.CanUpdate(),
                 $"No se puede actualizar la orden de pago por falta de permisos o " +
                 $"debido a que su estado es {Status.GetName()}.");

      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Debtor = Patcher.Patch(fields.DebtorUID, Debtor);
      PaymentMethod = Patcher.Patch(fields.PaymentMethodUID, PaymentMethod);
      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      ExchangeRate = Currency.Equals(Currency.Default) ? decimal.One : fields.ExchangeRate;
      PaymentAccount = Patcher.Patch(fields.PaymentAccountUID, PaymentAccount);
      DueTime = Patcher.Patch(fields.DueTime, DueTime);
      Priority = fields.Priority;
      Description = EmpiriaString.Clean(fields.Description);
      Observations = EmpiriaString.Clean(fields.Observations);
      RequestedBy = Patcher.Patch(fields.RequestedByUID, RequestedBy);
      ReferenceNumber = EmpiriaString.Clean(fields.ReferenceNumber);
    }

    #endregion Methods

    #region Helpers

    private void RefreshPaymentInstructions() {
      _paymentInstructions = new Lazy<List<PaymentInstruction>>(() =>
                                          PaymentInstructionData.GetPaymentInstructions(this));
    }

    #endregion Helpers

  }  // class PaymentOrder

}  // namespace Empiria.Payments
