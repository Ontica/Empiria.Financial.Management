/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentLog                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Store the payment instruction log.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Adapters;
using Empiria.Payments.Processor.Data;

namespace Empiria.Payments.Processor {

  /// <summary>Store the payment instruction log.</summary>
  internal class PaymentLog : BaseObject {

    #region Constructors and parsers

    public PaymentLog(PaymentInstruction paymentInstruction) {

      Assertion.Require(paymentInstruction, nameof(paymentInstruction));

      this.PaymentInstruction = paymentInstruction;
    }

    static public PaymentLog Parse(int id) => ParseId<PaymentLog>(id);

    static public PaymentLog Parse(string uid) => ParseKey<PaymentLog>(uid);

    static public PaymentLog Empty => ParseEmpty<PaymentLog>();

    static internal PaymentLog TryGetFor(PaymentInstruction paymentInstruction) {
      return BaseObject.TryParse<PaymentLog>($"PYMT_LOG_PYMT_INSTRUCTION_ID = {paymentInstruction.Id} AND PYMT_LOG_STATUS <> 'X' ");
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYMT_LOG_PYMT_INSTRUCTION_ID")]
    public PaymentInstruction PaymentInstruction {
      get; private set;
    }


    [DataField("PYMT_LOG_TEXT")]
    public string Text {
      get; private set;
    } = string.Empty;


    [DataField("PYMT_LOG_REQUEST_CODE")]
    public string RequestCode {
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


    [DataField("PYMT_LOG_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    } = JsonObject.Empty;


    [DataField("PYMT_LOG_STATUS", Default = PaymentLogStatus.Pending)]
    public PaymentLogStatus Status {
      get; set;
    } = PaymentLogStatus.Pending;


    #endregion Properties

    #region Methods

    internal void Update(PaymentResultDto paymentResultDto) {
      Assertion.Require(paymentResultDto, nameof(paymentResultDto));
      this.Text = paymentResultDto.Text;
      this.RequestCode = paymentResultDto.RequestID;
      this.RequestTime = DateTime.Now;
      this.ApplicationTime = DateTime.Now;
      this.RecordingTime = DateTime.Now;
      this.Status = (PaymentLogStatus) this.PaymentInstruction.Status;
    }


    protected override void OnSave() {
      ProcessorData.WritePaymentLog(this, this.ExtData.ToString());
    }


    #endregion Methods

  }  // class PaymentLog

}  // namespace Empiria.Payments.Processor
