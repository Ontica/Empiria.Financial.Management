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
using Empiria.Json;

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
    public Char Status {
      get; set;
    } = 'P';


    #endregion Properties

    #region Methods

    protected override void OnSave() {
      ProcessorData.WritePaymentLog(this, this.ExtData.ToString());
    }

    #endregion Methods

  }  // class PaymentLog

}  // namespace Empiria.Payments.Processor
