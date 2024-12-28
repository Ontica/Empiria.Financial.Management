﻿/* Empiria Financial *****************************************************************************************
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

using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Data;

namespace Empiria.Payments.Processor {

  /// <summary>Represents a payment instruction.</summary>
  internal class PaymentInstruction : BaseObject {

    private PaymentInstruction() {
      // Required by Empira Framework
    }

    internal PaymentInstruction(PaymentsBroker broker, PaymentOrder paymentOrder) {
      Assertion.Require(broker, nameof(broker));
      Assertion.Require(!broker.IsEmptyInstance, nameof(broker));
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));

      Broker = broker;
      PaymentOrder = paymentOrder;
      PaymentInstructionNo = GeneratePaymentInstructionNo();
    }

    static public PaymentInstruction Parse(int id) => ParseId<PaymentInstruction>(id);

    static public PaymentInstruction Parse(string uid) => ParseKey<PaymentInstruction>(uid);

    static internal FixedList<PaymentInstruction> GetListFor(PaymentOrder paymentOrder) {
      return PaymentInstructionData.GetPaymentOrderInstructions(paymentOrder);
    }

    static public PaymentInstruction Empty => ParseEmpty<PaymentInstruction>();

    #region Properties

    [DataField("PYMT_INSTRUCTION_NO")]
    public string PaymentInstructionNo {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_BROKER_ID")]
    public PaymentsBroker Broker {
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
    public string ExternalRequestUniqueNo {
      get;
      private set;
    }


    [DataField("PYMT_INSTRUCTION_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    }


    [DataField("PYMT_INSTRUCTION_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PYMT_INSTRUCTION_STATUS", Default = PaymentInstructionStatus.Pending)]
    public PaymentInstructionStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }
      PaymentInstructionData.WritePaymentInstruction(this, this.ExtData.ToString());
    }

    internal void SetExternalUniqueNo(string uniqueNo) {
      Assertion.Require(uniqueNo, nameof(uniqueNo));
      Assertion.Require(this.IsNew,
                        "SetExternalUniqueNo() can not be called on already stored payment instructions.");

      ExternalRequestUniqueNo = uniqueNo;
    }

    internal void UpdateStatus(PaymentInstructionStatus status) {
      EnsureCanUpdateStatusTo(status);

      this.Status = status;
    }


    #endregion Methods

    #region Helpers

    private void EnsureCanUpdateStatusTo(PaymentInstructionStatus newStatus) {
      if (this.Status.IsFinal()) {
        Assertion.RequireFail("La instrucción de pago no se puede modificar " +
                              $"debido a que está en el estado final: {this.Status.GetName()}.");
      }
      if (!this.Status.IsFinal() && newStatus.IsFinal()) {
        return;
      }
      if (this.Status == PaymentInstructionStatus.InProcess &&
          newStatus == PaymentInstructionStatus.Pending) {
        Assertion.RequireFail("No es posible cambiar el estado de la instrucción de pago " +
                              "de en proceso a pendiente.");
      }
    }


    private string GeneratePaymentInstructionNo() {
      return "PI-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }

    #endregion Helpers

  }  // class PaymentInstruction

} // namespace Empiria.Payments.Processor
