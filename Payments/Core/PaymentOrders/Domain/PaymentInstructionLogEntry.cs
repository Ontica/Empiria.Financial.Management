/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentInstructionLogEntry                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data for a payment instruction log entry.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Json;

using Empiria.Payments.Processor.Adapters;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Holds data for a payment instruction log entry.</summary>
  internal class PaymentInstructionLogEntry : BaseObject {

    #region Constructors and parsers

    private PaymentInstructionLogEntry() {
      // Required by Empiria Framework.
    }


    internal PaymentInstructionLogEntry(PaymentInstruction paymentInstruction,
                                        PaymentInstructionEvent instructionEvent,
                                        string userMessage) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(instructionEvent != PaymentInstructionEvent.None, nameof(instructionEvent));

      userMessage = userMessage ?? string.Empty;

      TimeStamp = DateTime.Now;
      PaymentInstruction = paymentInstruction;
      PaymentOrder = paymentInstruction.PaymentOrder;
      Load(instructionEvent, userMessage);
    }


    internal PaymentInstructionLogEntry(PaymentInstruction paymentInstruction,
                                        BrokerResponseDto brokerResponse) {

      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(brokerResponse, nameof(brokerResponse));

      TimeStamp = DateTime.Now;
      PaymentInstruction = paymentInstruction;
      PaymentOrder = paymentInstruction.PaymentOrder;
      Load(brokerResponse);
    }

    static public PaymentInstructionLogEntry Parse(int id) => ParseId<PaymentInstructionLogEntry>(id);

    static public PaymentInstructionLogEntry Parse(string uid) => ParseKey<PaymentInstructionLogEntry>(uid);

    static public PaymentInstructionLogEntry Empty => ParseEmpty<PaymentInstructionLogEntry>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYMT_LOG_PYMT_INSTRUCTION_ID")]
    public PaymentInstruction PaymentInstruction {
      get; private set;
    }


    [DataField("PYMT_LOG_PYMT_ORDER_ID")]
    public PaymentOrder PaymentOrder {
      get; private set;
    }


    [DataField("PYMT_LOG_TEXT")]
    public string LogText {
      get; private set;
    }


    [DataField("PYMT_LOG_BROKER_RESPONSE")]
    public string BrokerResponse {
      get; private set;
    }


    [DataField("PYMT_LOG_RECORDING_TIME")]
    public DateTime TimeStamp {
      get; private set;
    }


    [DataField("PYMT_LOG_EXT_DATA")]
    internal JsonObject ExtData {
      get; private set;
    }


    [DataField("PYMT_LOG_STATUS", Default = PaymentInstructionStatus.Programmed)]
    public PaymentInstructionStatus Status {
      get; set;
    } = PaymentInstructionStatus.Programmed;


    #endregion Properties

    #region Methods

    protected override void OnSave() {
      PaymentInstructionData.WritePaymentLog(this);
    }


    private void Load(PaymentInstructionEvent instructionEvent, string userMessage) {
      LogText = instructionEvent.GetDescription();

      if (userMessage.Length != 0) {
        LogText += $" {userMessage}";
      }

      BrokerResponse = string.Empty;
      Status = PaymentInstruction.Status;
    }


    private void Load(BrokerResponseDto brokerResponse) {
      var brokerName = PaymentInstruction.BrokerConfigData.Name;
      var brokerInstructionNo = PaymentInstruction.BrokerInstructionNo;

      LogText = $"Solicitud {brokerName} {brokerInstructionNo}. ";

      if (brokerResponse.BrokerMessage.Length != 0) {
        LogText += brokerResponse.BrokerMessage;
      } else {
        LogText += brokerResponse.BrokerStatusText;
      }
      BrokerResponse = brokerResponse.BrokerStatusText;
      Status = brokerResponse.Status;
    }

    #endregion Methods

  }  // class PaymentLog

}  // namespace Empiria.Payments
