/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Abstract Information Holder             *
*  Type     : PaymentInstruction                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract class that represents an actual payment or a rejected payment instruction.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;

using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Data;


namespace Empiria.Payments.Processor {

  /// <summary>Abstract class that represents an actual payment or a rejected payment instruction.</summary>
  abstract internal class PaymentInstruction : BaseObject {

    internal PaymentInstruction(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      Broker = broker;
      PaymentOrder = paymentOrder;
    }

    static public PaymentInstruction Parse(int id) => ParseId<PaymentInstruction>(id);

    static public PaymentInstruction Parse(string uid) => ParseKey<PaymentInstruction>(uid);

    static public PaymentInstruction Empty => ParseEmpty<PaymentInstruction>();

    #region Properties

    [DataField("PAYMENT_INSTRUCTION_TYPE_ID")]
    public int TypeId {
      get; private set;
    }


    [DataField("PAYMENT_INSTRUCTION_BROKER_ID")]
    public PaymentsBroker Broker {
      get; private set;
    }


    [DataField("PAYMENT_INSTRUCTION_ORDER_ID")]
    public PaymentOrder PaymentOrder {
      get; private set;
    }


    [DataField("PAYMENT_INSTRUCTION_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PAYMENT_INSTRUCTION_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    } = JsonObject.Empty;


    [DataField("PAYMENT_INSTRUCTION_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("PAYMENT_INSTRUCTION_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PAYMENT_INSTRUCTION_STATUS", Default = PaymentInstructionStatus.Capture)]
    public PaymentInstructionStatus Status {
      get; private set;
    } = PaymentInstructionStatus.Capture;

    #endregion Properties


    #region Methods

    protected override void OnBeforeSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }
    }


    protected override void OnSave() {
      ProcessorData.WritePaymentInstruction(this, this.ExtData.ToString());
    }


    public void SetFailPayment() {
      this.TypeId = 3751;
      this.Status = PaymentInstructionStatus.Rejected;
    }


    public void SetSuccessfulPayment() {
      this.TypeId = 3752;
      this.Status = PaymentInstructionStatus.Payed;
    }


    #endregion Methods

  }  // class PaymentInstruction

} // namespace Empiria.Payments.Processor
