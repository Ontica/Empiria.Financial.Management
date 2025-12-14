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

using Empiria.Json;
using Empiria.Parties;

using Empiria.Payments.Processor.Adapters;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Represents a payment instruction.</summary>
  public class PaymentInstruction : BaseObject {


    private BrokerResponseDto _lastBrokerResponse;

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
    }

    static public PaymentInstruction Parse(int id) => ParseId<PaymentInstruction>(id);

    static public PaymentInstruction Parse(string uid) => ParseKey<PaymentInstruction>(uid);

    static public PaymentInstruction Empty => ParseEmpty<PaymentInstruction>();

    static internal FixedList<PaymentInstruction> GetInProgress() {
      return PaymentInstructionData.GetInProgressPaymentInstructions();
    }

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

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (base.IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      if (!IsDirty) {
        return;
      }

      PaymentInstructionData.WritePaymentInstruction(this);

      if (_lastBrokerResponse == null) {
        return;
      }

      var logEntry = new PaymentInstructionLogEntry(this, _lastBrokerResponse);

      logEntry.Save();

      _lastBrokerResponse = null;
    }


    internal void Update(BrokerResponseDto brokerResponse) {
      Assertion.Require(brokerResponse, nameof(brokerResponse));

      if (brokerResponse.Status == Status) {
        return;
      }

      EnsureCanUpdateStatusTo(brokerResponse.Status);

      if (BrokerInstructionNo.Length == 0) {
        BrokerInstructionNo = brokerResponse.BrokerInstructionNo;
      }
      Status = brokerResponse.Status;

      _lastBrokerResponse = brokerResponse;

      MarkAsDirty();
    }

    #endregion Methods

    #region Helpers

    private void EnsureCanUpdateStatusTo(PaymentInstructionStatus newStatus) {

      if (this.Status.IsFinal()) {
        Assertion.RequireFail("El estado de la instrucción de pago no se puede modificar " +
                               $"debido a que está en el estado final: {this.Status.GetName()}.");
      }


      switch (Status) {
        case PaymentInstructionStatus.Programmed:

          if (newStatus == PaymentInstructionStatus.WaitingRequest ||
              newStatus == PaymentInstructionStatus.Canceled ||
              newStatus == PaymentInstructionStatus.Suspended) {

            return;
          }

          break;

        case PaymentInstructionStatus.Suspended:

          if (newStatus == PaymentInstructionStatus.Programmed) {

            return;
          }

          break;

        case PaymentInstructionStatus.WaitingRequest:

          if (newStatus == PaymentInstructionStatus.Requested) {

            return;
          }

          break;

        case PaymentInstructionStatus.Requested:

          if (newStatus == PaymentInstructionStatus.InProgress ||
              newStatus == PaymentInstructionStatus.Failed) {

            return;
          }

          break;

        case PaymentInstructionStatus.InProgress:

          if (newStatus == PaymentInstructionStatus.InProgress ||
              newStatus == PaymentInstructionStatus.PaymentConfirmation ||
              newStatus == PaymentInstructionStatus.Failed) {

            return;
          }

          break;


        case PaymentInstructionStatus.PaymentConfirmation:

          if (newStatus == PaymentInstructionStatus.Payed ||
              newStatus == PaymentInstructionStatus.Failed) {

            return;
          }

          break;

        default:

          throw Assertion.EnsureNoReachThisCode($"Unhandled payment instruction status change from " +
                                                $"{this.Status.GetName()} to {newStatus.GetName()}.");
      }


      Assertion.RequireFail("No es posible cambiar el estado de la instrucción de pago " +
                            $"del estado {this.Status.GetName()} al estado {newStatus.GetName()}.");
    }


    static private string GeneratePaymentInstructionNo() {
      // ToDo: Implement a better way to generate unique payment instruction numbers

      return "PI-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }

    #endregion Helpers

  }  // class PaymentInstruction

} // namespace Empiria.Payments
