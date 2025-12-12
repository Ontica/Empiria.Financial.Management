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

using Empiria.Payments.Adapters;
using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Holds data for a payment instruction log entry.</summary>
  internal class PaymentInstructionLogEntry : BaseObject {

    #region Constructors and parsers

    private PaymentInstructionLogEntry() {
      // Required by Empiria Framework.
    }


    internal PaymentInstructionLogEntry(PaymentInstruction paymentInstruction,
                                        PaymentInstructionResultDto paymentResultDto) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(paymentResultDto, nameof(paymentResultDto));

      PaymentInstruction = paymentInstruction;
      PaymentOrder = paymentInstruction.PaymentOrder;
      Load(paymentResultDto);
    }


    internal PaymentInstructionLogEntry(PaymentInstruction paymentInstruction,
                                        PaymentInstructionStatusDto newStatus) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));
      Assertion.Require(!paymentInstruction.IsEmptyInstance, nameof(paymentInstruction));
      Assertion.Require(newStatus, nameof(newStatus));

      PaymentInstruction = paymentInstruction;
      PaymentOrder = paymentInstruction.PaymentOrder;

      Load(newStatus);
    }


    static public PaymentInstructionLogEntry Parse(int id) => ParseId<PaymentInstructionLogEntry>(id);

    static public PaymentInstructionLogEntry Parse(string uid) => ParseKey<PaymentInstructionLogEntry>(uid);

    static public PaymentInstructionLogEntry Empty => ParseEmpty<PaymentInstructionLogEntry>();

    static internal FixedList<PaymentInstructionLogEntry> GetListFor(PaymentInstruction instruction) {
      return PaymentInstructionData.GetPaymentInstructionLogs(instruction);
    }

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
    public string ExternalRequestID {
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
    public string ExternalResultText {
      get; private set;
    } = string.Empty;


    [DataField("PYMT_LOG_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    } = JsonObject.Empty;


    [DataField("PYMT_LOG_STATUS", Default = PaymentInstructionStatus.Pending)]
    public PaymentInstructionStatus Status {
      get; set;
    } = PaymentInstructionStatus.Pending;


    #endregion Properties

    #region Methods

    protected override void OnSave() {
      RecordingTime = DateTime.Now;
      PaymentInstructionData.WritePaymentLog(this, ExtData.ToString());
    }


    private void Load(PaymentInstructionResultDto dto) {
      var time = DateTime.Now;

      ExternalRequestID = dto.ExternalRequestID;
      ExternalResultText = dto.ExternalResultText;
      Status = PaymentInstruction.Status;

      RequestTime = time;
      ApplicationTime = time;
    }


    private void Load(PaymentInstructionStatusDto newStatus) {
      var time = DateTime.Now;

      ExternalRequestID = newStatus.ExternalRequestID;
      ExternalResultText = newStatus.ExternalStatusName;
      Status = newStatus.Status;

      RequestTime = time;
      ApplicationTime = time;
    }

    #endregion Methods

  }  // class PaymentLog

}  // namespace Empiria.Payments
