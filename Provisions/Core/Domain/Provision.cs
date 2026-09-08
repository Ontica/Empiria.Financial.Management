/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Provisions Management                      Component : Domain Layer                            *
*  Assembly : Empiria.Provisions.Core.dll                Pattern   : Information Holder                      *
*  Type     : Provision                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents the recognition of an economic obligation that may later result in a payment order. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;

using Empiria.Financial;
using Empiria.Payments;

namespace Empiria.Provisions {

  /// <summary>Represents the recognition of an economic obligation that may
  /// later result in a payment order.</summary>
  public class Provision : BaseObject {

    #region Constructors and parsers

    protected Provision() {
      // Required by Empiria Framework.
    }


    internal Provision(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      _payableEntityTypeId = payableEntity.GetEmpiriaType().Id;
      _payableEntityId = payableEntity.Id;

      PaymentOrder = PaymentOrder.Empty;

      Status = ProvisionStatus.Programmed;
    }


    static public Provision Parse(int id) => ParseId<Provision>(id);

    static public Provision Parse(string uid) => ParseKey<Provision>(uid);

    static public Provision Empty => ParseEmpty<Provision>();

    #endregion Constructors and parsers


    #region Properties

    [DataField("PRV_PAYABLE_ENTITY_TYPE_ID")]
    private int _payableEntityTypeId = -1;


    [DataField("PRV_PAYABLE_ENTITY_ID")]
    private int _payableEntityId = -1;


    public IPayableEntity PayableEntity {
      get {
        return (IPayableEntity) Parse(_payableEntityTypeId,
                                      _payableEntityId);
      }
    }


    [DataField("PRV_PAYMENT_ORDER_ID")]
    public PaymentOrder PaymentOrder {
      get; private set;
    }


    [DataField("PRV_EXT_DATA")]
    internal JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return PayableEntity.EntityNo + EmpiriaString.BuildKeywords(PayableEntity.Keywords,
                                                                    PaymentOrder.Keywords);
      }
    }


    [DataField("PRV_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PRV_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PRV_STATUS", Default = ProvisionStatus.Programmed)]
    public ProvisionStatus Status {
      get; private set;
    } = ProvisionStatus.Programmed;

    #endregion Properties

    #region Methods

    internal void Cancel() {
      Assertion.Require(Status == ProvisionStatus.Programmed,
                        "Only a programmed provision can be canceled.");

      Status = ProvisionStatus.Canceled;
    }


    internal void SetAsProvisioned() {
      Assertion.Require(Status == ProvisionStatus.Programmed,
                        "Only a programmed provision can be provisioned.");

      Status = ProvisionStatus.Provisioned;
    }


    internal void SetPaymentOrder(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));

      PaymentOrder = paymentOrder;
    }


    internal void RemovePaymentOrder() {
      Assertion.Require(!PaymentOrder.InProgress && PaymentOrder.Status != PaymentOrderStatus.Payed,
                        "Cannot remove the payment order because it is in progress or payed.");

      PaymentOrder = PaymentOrder.Empty;
    }


    protected override void OnSave() {
      if (IsNew) {
        PostingTime = DateTime.Now;
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      }

      ProvisionsData.WriteProvision(this);
    }

    #endregion Methods

  }  // class Provision

}  // namespace Empiria.Provisions
