/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PaymentOrder                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a payment order.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;

using Empiria.Payments.Orders.Adapters;
using Empiria.Payments.Orders.Data;

namespace Empiria.Payments.Orders {

  /// <summary>Represents a payment order.</summary>
  internal class PaymentOrder : BaseObject {

    #region Constructors and parsers

    private PaymentOrder() {
      // Required by Empiria Framework.
    }

    public PaymentOrder(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));

      Update(fields);
    }


    static internal PaymentOrder Parse(string UID) {
      return BaseObject.ParseKey<PaymentOrder>(UID);
    }


    static internal PaymentOrder Parse(int Id) {
      return BaseObject.ParseId<PaymentOrder>(Id);
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("ORDER_TYPE_ID")]
    public PaymentOrderType PaymentOrderType {
      get; private set;
    }


    [DataField("ORDER_NO")]
    public string PaymentOrderNo {
      get; private set;
    }


    [DataField("ORDER_PAY_TO_ID")]
    public Party PayTo {
      get; private set;
    }


    //[DataField("ORDER_PAYABLE_ID")]
    //public Payable Payable {
    //  get; internal set;
    //}


    public int PayableTypeId {
      get; internal set;
    }


    [DataField("ORDER_PAYMENT_METHOD_ID")]
    public PaymentMethod PaymentMethod {
      get; private set;
    }


    [DataField("ORDER_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    //[DataField("ORDER_PAYMENT_ACCOUNT_ID")]
    //public PaymentAccount PaymentAccount {
    //  get; private set;
    //}


    [DataField("ORDER_NOTES")]
    public string Notes {
      get; private set;
    }


    [DataField("ORDER_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("ORDER_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("ORDER_TOTAL", ConvertFrom = typeof(decimal))]
    public decimal Total {
      get; private set;
    }


    [DataField("ORDER_DUETIME")]
    public DateTime DueTime {
      get; private set;
    }


    [DataField("ORDER_REQUESTED_BY_ID")]
    public OrganizationalUnit RequestedBy {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.PaymentOrderNo, this.PayTo.Name,
                                           this.RequestedBy.Name, this.PaymentOrderType.Name,
                                           this.PaymentMethod.Name);
      }
    }

    [DataField("ORDER_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("ORDER_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ORDER_STATUS", Default = PaymentOrderStatus.Pending)]
    public PaymentOrderStatus Status {
      get; private set;
    } = PaymentOrderStatus.Pending;

    #endregion Properties

    #region Methods


    internal void Delete() {
      Assertion.Require(this.Status == PaymentOrderStatus.Pending,
                  $"No se puede eliminar una orden de pago que está en estado {this.Status.GetName()}.");

      this.Status = PaymentOrderStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PaymentOrderNo = GeneratePaymentOrderNo();
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }

      PaymentOrderData.WritePaymentOrder(this, this.ExtData.ToString());
    }


    internal void Suspend(Contact suspendedBy, DateTime suspendedUntil) {
      Assertion.Require(suspendedBy, nameof(suspendedBy));

      Assertion.Require(this.Status == PaymentOrderStatus.Received ||
                        this.Status == PaymentOrderStatus.Suspended,
                $"No se puede suspender la orden de pago debido " +
                $"a que tiene el estado {this.Status.GetName()}.");

      this.Status = PaymentOrderStatus.Suspended;
    }


    internal void Update(PaymentOrderFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(this.Status == PaymentOrderStatus.Pending,
                        $"No se pueden actualizar los datos de la orden de pago " +
                        $"debido a que tiene el estado {this.Status.GetName()}.");

      fields.EnsureValid();

      this.PaymentOrderType = PaymentOrderType.Parse(fields.PaymentOrderTypeUID);
      this.PayTo = Party.Parse(fields.PayToUID);
      // this.Payable = Payable.Parse(fields.PayableUID);
      this.PayableTypeId = -1;
      this.PaymentMethod = PaymentMethod.Parse(fields.PaymentMethodUID);
      this.Currency = Currency.Parse(fields.CurrencyUID);
      //this.PaymentAccount = PaymentAccount.Parse(fields.PaymentAccountUID);
      this.Notes = fields.Notes;
      this.RequestedTime = fields.RequestedTime;
      this.DueTime = fields.DueTime;
      this.RequestedBy = OrganizationalUnit.Parse(fields.RequestedByUID);
      this.Total = fields.Total;
    }

    #endregion Methods

    #region Helpers

    private string GeneratePaymentOrderNo() {
      return "O-" + EmpiriaString.BuildRandomString(10).ToUpperInvariant();
    }

    #endregion Helpers

  }  // class PaymentOrder

}  // namespace Empiria.Payments.Contracts
