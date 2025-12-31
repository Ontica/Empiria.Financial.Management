/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentInstruction                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment instruction.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Payments.Processor.Adapters;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Represents a payment instruction.</summary>
  public class PaymentInstruction : BaseObject {

    private Lazy<List<PaymentInstructionLogEntry>> _logEntries;

    private BrokerResponseDto _lastBrokerResponse;
    private PaymentInstructionEvent _lastEvent = PaymentInstructionEvent.None;

    private readonly object _locker = new object();

    #region Constructors and parsers

    protected PaymentInstruction() {
      // Required by Empira Framework
    }

    internal PaymentInstruction(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));

      PaymentsBrokerConfigData brokerConfigData = PaymentsBrokerConfigData.GetPaymentsBroker(paymentOrder);

      Assertion.Require(brokerConfigData, nameof(brokerConfigData));
      Assertion.Require(!brokerConfigData.IsEmptyInstance, nameof(brokerConfigData));

      BrokerConfigData = brokerConfigData;
      PaymentOrder = paymentOrder;
      PaymentInstructionNo = GeneratePaymentInstructionNo();

      LoadLogEntries();
    }

    static public PaymentInstruction Parse(int id) => ParseId<PaymentInstruction>(id);

    static public PaymentInstruction Parse(string uid) => ParseKey<PaymentInstruction>(uid);

    static public PaymentInstruction Empty => ParseEmpty<PaymentInstruction>();

    static internal FixedList<PaymentInstruction> GetInProgress() {
      return PaymentInstructionData.GetInProgressPaymentInstructions();
    }


    static internal FixedList<PaymentInstruction> GetReadyToBeRequested() {
      FixedList<PaymentInstruction> instructions = PaymentInstructionData.GetWaitingRequestInstructions();

      return instructions.FindAll(x => x.Rules.IsReadyToBeRequested);
    }


    protected override void OnLoad() {
      LoadLogEntries();
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYMT_INSTRUCTION_NO")]
    public string PaymentInstructionNo {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_BROKER_ID")]
    public PaymentsBrokerConfigData BrokerConfigData {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_PYMT_ORDER_ID")]
    public PaymentOrder PaymentOrder {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_EXTERNAL_REQUEST_NO")]
    public string BrokerInstructionNo {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_EXT_DATA")]
    internal JsonObject ExtData {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_STATUS", Default = PaymentInstructionStatus.Programmed)]
    public PaymentInstructionStatus Status {
      get; private set;
    }


    public DateTime EffectiveDate {
      get {
        return PostingTime;
      }
    }


    public DateTime ProgrammedDate {
      get {
        return PostingTime;
      }
    }


    public bool IsUrgent {
      get {
        return PaymentOrder.Priority == Priority.Urgent;
      }
    }


    internal FixedList<PaymentInstructionLogEntry> LogEntries {
      get {
        return _logEntries.Value.ToFixedList();
      }
    }


    internal PaymentInstructionRules Rules {
      get {
        return new PaymentInstructionRules(this);
      }
    }


    internal PaymentsTimeControl TimeControl {
      get {
        return new PaymentsTimeControl(this);
      }
    }


    public bool WasSent {
      get {
        return BrokerInstructionNo.Length != 0;
      }
    }

    #endregion Properties

    #region Methods

    internal void EventHandler(PaymentInstructionEvent instructionEvent) {
      lock (_locker) {
        EventHandlerInternal(instructionEvent);
      }
    }


    protected override void OnSave() {
      lock (_locker) {
        SaveInternal();
      }
    }


    internal void Sent(BrokerResponseDto brokerResponse) {
      lock (_locker) {
        SentInternal(brokerResponse);
        SaveInternal();
      }
    }


    internal void UpdateStatus(BrokerResponseDto brokerResponse) {
      lock (_locker) {

        UpdateStatusInternal(brokerResponse);

        SaveInternal();

        if (Status == PaymentInstructionStatus.Payed) {
          PaymentOrder.SetAsPayed(this);
        }
      }
    }

    #endregion Methods

    #region Helpers

    private void EventHandlerInternal(PaymentInstructionEvent instructionEvent) {

      switch (instructionEvent) {

        case PaymentInstructionEvent.Cancel:

          Assertion.Require(Rules.CanCancel(), "La instrucción de pago no puede ser cancelada.");

          Status.EnsureCanUpdateTo(PaymentInstructionStatus.Canceled);
          Status = PaymentInstructionStatus.Canceled;

          break;

        case PaymentInstructionEvent.CancelPaymentRequest:

          Assertion.Require(Rules.CanCancelPaymentRequest(),
                            "La solicitud de pago de la instrucción no puede ser cancelada.");

          Status.EnsureCanUpdateTo(PaymentInstructionStatus.Programmed);
          Status = PaymentInstructionStatus.Programmed;
          break;

        case PaymentInstructionEvent.RequestPayment:

          if (!PaymentOrder.PaymentMethod.IsElectronic) {
            Status = PaymentInstructionStatus.Payed;
            break;
          }

          Assertion.Require(Rules.CanRequestPayment(),
                            "No se puede enviar a pago esta instrucción de pago.");

          TimeControl.EnsureCanRequestPayment();

          Status.EnsureCanUpdateTo(PaymentInstructionStatus.WaitingRequest);
          Status = PaymentInstructionStatus.WaitingRequest;
          break;

        case PaymentInstructionEvent.Reset:

          Assertion.Require(Rules.CanReset(), "La instrucción de pago no puede ser reiniciada.");

          Status.EnsureCanUpdateTo(PaymentInstructionStatus.Programmed);
          Status = PaymentInstructionStatus.Programmed;
          break;

        case PaymentInstructionEvent.Suspend:

          Assertion.Require(Rules.CanSuspend(), "La instrucción de pago no puede ser suspendida.");

          Status.EnsureCanUpdateTo(PaymentInstructionStatus.Suspended);
          Status = PaymentInstructionStatus.Suspended;
          break;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment instruction event: {instructionEvent}.");
      }

      _lastEvent = instructionEvent;

      MarkAsDirty();
    }


    static private string GeneratePaymentInstructionNo() {
      // ToDo: Implement a better way to generate unique payment instruction numbers

      return "PI-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }


    private void LoadLogEntries() {
      _logEntries = new Lazy<List<PaymentInstructionLogEntry>>(() =>
                                                  PaymentInstructionData.GetPaymentInstructionLog(this));
    }

    private void SaveInternal() {

      if (!IsDirty) {
        return;
      }

      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      PaymentInstructionData.WritePaymentInstruction(this);

      if (_lastBrokerResponse != null) {
        SaveBrokerResponseLogEntry();
      }
      if (_lastEvent != PaymentInstructionEvent.None) {
        SaveEventLogEntry();
      }

    }

    private void SaveEventLogEntry() {
      var logEntry = new PaymentInstructionLogEntry(this, _lastEvent);

      logEntry.Save();

      _logEntries.Value.Add(logEntry);

      _lastEvent = PaymentInstructionEvent.None;
    }


    private void SaveBrokerResponseLogEntry() {
      var logEntry = new PaymentInstructionLogEntry(this, _lastBrokerResponse);

      logEntry.Save();

      _logEntries.Value.Add(logEntry);

      _lastBrokerResponse = null;
    }


    private void SentInternal(BrokerResponseDto brokerResponse) {
      Assertion.Require(brokerResponse, nameof(brokerResponse));
      Assertion.Require(!WasSent, "PaymentInstruction was sent.");
      Assertion.Require(brokerResponse.BrokerInstructionNo.Length != 0, nameof(brokerResponse.BrokerInstructionNo));

      BrokerInstructionNo = brokerResponse.BrokerInstructionNo;

      Status = brokerResponse.Status;

      _lastBrokerResponse = brokerResponse;

      MarkAsDirty();
    }


    private void UpdateStatusInternal(BrokerResponseDto brokerResponse) {
      Assertion.Require(brokerResponse, nameof(brokerResponse));
      Assertion.Require(WasSent || brokerResponse.Status == PaymentInstructionStatus.Exception,
                        "PaymentInstruction was not sent.");

      if (Status == brokerResponse.Status) {
        return;
      }

      Status = brokerResponse.Status;

      _lastBrokerResponse = brokerResponse;

      MarkAsDirty();
    }

    #endregion Helpers

  }  // class PaymentInstruction

} // namespace Empiria.Payments
