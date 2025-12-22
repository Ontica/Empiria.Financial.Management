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
                                        PaymentInstructionEvent instructionEvent) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(instructionEvent != PaymentInstructionEvent.None, nameof(instructionEvent));

      PaymentInstruction = paymentInstruction;
      PaymentOrder = paymentInstruction.PaymentOrder;
      Load(instructionEvent);
    }


    internal PaymentInstructionLogEntry(PaymentInstruction paymentInstruction,
                                        BrokerResponseDto brokerResponse) {

      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(brokerResponse, nameof(brokerResponse));

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
      get;
      private set;
    }


    [DataField("PYMT_LOG_REQUEST_CODE")]
    public string BrokerInstructionNo {
      get; private set;
    } = string.Empty;


    [DataField("PYMT_LOG_REQUEST_TIME")]
    public DateTime RequestTime {
      get; set;
    }


    [DataField("PYMT_LOG_APPLICATION_TIME")]
    public DateTime ApplicationTime {
      get; set;
    }


    [DataField("PYMT_LOG_RECORDING_TIME")]
    public DateTime RecordingTime {
      get; set;
    }


    [DataField("PYMT_LOG_TEXT")]
    public string BrokerMessage {
      get; private set;
    } = string.Empty;


    public string BrokerStatusText {
      get {
        return ExtData.Get("brokerStatusText", string.Empty);
      }
      private set {
        ExtData.SetIfValue("brokerStatusText", value);
      }
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
      RecordingTime = DateTime.Now;
      PaymentInstructionData.WritePaymentLog(this);
    }


    private void Load(PaymentInstructionEvent instructionEvent) {
      var time = DateTime.Now;

      BrokerInstructionNo = string.Empty;
      BrokerMessage = instructionEvent.GetDescription();
      BrokerStatusText = string.Empty;
      Status = PaymentInstruction.Status;
      RequestTime = time;
      ApplicationTime = time;
    }


    private void Load(BrokerResponseDto brokerResponse) {
      var time = DateTime.Now;

      BrokerInstructionNo = brokerResponse.BrokerInstructionNo;
      BrokerMessage = brokerResponse.BrokerMessage;
      BrokerStatusText = brokerResponse.BrokerStatusText;
      Status = brokerResponse.Status;

      RequestTime = time;
      ApplicationTime = time;
    }

    #endregion Methods

  }  // class PaymentLog

}  // namespace Empiria.Payments
