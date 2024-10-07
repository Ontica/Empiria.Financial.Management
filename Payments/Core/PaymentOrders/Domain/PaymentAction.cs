/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : Contract                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract or a service order.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Payments.Orders.Adapters;
using System.Diagnostics.Contracts;
using System;


namespace Empiria.Payments.Orders {

  /// <summary>Represents a contract or a service order.</summary>
  internal class PaymentAction : BaseObject {

    #region Constructors and parsers

    private PaymentAction() {
      // Required by Empiria Framework.
    }

    //public PaymentAction(PaymentActionFields fields) {
    //  Assertion.Require(fields, nameof(fields));

    //  Load(fields);
    //}


    static internal PaymentAction Parse(string paymentActionUID) {
      return BaseObject.ParseKey<PaymentAction>(paymentActionUID);
    }

    #endregion Constructors and parsers

    #region Properties


    public PaymentOrder Order {
      get; private set;
    }


    public DateTime ScheduledDate {
      get; private set;
    }


    public Contact ScheduledBy {
      get; private set;
    }


    public string ScheduledNotes {
      get; private set;
    }


    public Contact RequestedBy {
      get; private set;
    }


    public DateTime RequestTime {
      get; private set;
    }


    public JsonObject RequestData {
      get; private set;
    }


    public DateTime ResponseTime {
      get; private set;
    }


    public JsonObject ResponseData {
      get; private set;
    }

    // public PaymentActionStatus Status

    
    public EntityStatus Status {
      get;  private set;
    }


    #endregion Properties

    #region Methods


    internal void Delete() {

      //Assertion.Require(this.Status == EntityStatus.Active || this.Status == EntityStatus.Suspended,
      //            $"No se puede eliminar una orden de pago que está en estado {this.Status.GetName()}.");

      //this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
       //to do
      }

    }


    internal void Suspend() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede suspender una orden de pago que no este activa.");

      this.Status = EntityStatus.Suspended;
    }

    #endregion Methods

    #region Helpers

    private void Load() {

    }

    #endregion Helpers

  } // class PaymentAction

} // namespace Empiria.Payments.Orders
