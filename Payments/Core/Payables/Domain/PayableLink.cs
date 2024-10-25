/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PayableBill                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents represent the list of bills that make up an payable.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Payments.Payables.Data;
using Empiria.StateEnums;


namespace Empiria.Payments.Payables {

  /// <summary>Represents represent the list of bills that make up an payable.</summary>
  internal class PayableLink : BaseObject {

    #region Constructors and parsers

    private PayableLink() {
      // Required by Empiria Framework.
    }


    internal PayableLink(PayableLinkType linkType, Payable payable, BaseObject linkedObject) {
      Assertion.Require(linkType, nameof(linkType));
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(linkedObject, nameof(linkedObject));
      Assertion.Require(!linkType.IsEmptyInstance,
                        "payable can not be the empty instance.");
      Assertion.Require(!payable.IsEmptyInstance,
                        "payable can not be the empty instance.");
      Assertion.Require(!linkedObject.IsEmptyInstance,
                        "payable can not be the empty instance.");

      Assertion.Require(linkedObject.GetEmpiriaType().Equals(linkType.LinkedObjectType), "Invalid LinkedObjectType.");

      PayableLinkType = linkType;
      Payable = payable;
      LinkedObject = linkedObject;
    }

    static public PayableLink Parse(int id) => ParseId<PayableLink>(id);

    static internal PayableLink Parse(string UID) {
      return ParseKey<PayableLink>(UID);
    }

    static public PayableLink Empty => ParseEmpty<PayableLink>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PAYBLE_LINK_TYPE_ID")]
    public PayableLinkType PayableLinkType {
      get; private set;
    }


    [DataField("PAYABLE_ID")]
    public Payable Payable {
      get; private set;
    }


    [DataField("LINKED_OBJECT_ID")]
    public BaseObject LinkedObject {
      get; private set;
    }


    [DataField("PAYABLE_LINK_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("PAYABLE_LINK_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PAYABLE_LINK_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Active;

    #endregion Properties

    #region Methods


    protected override void OnBeforeSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }
    }
   

    protected override void OnSave() {
      PayableData.WritePayableLink(this);
    }


    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    #endregion Methods

  }  // class PayableBill

}  // namespace Empiria.Payments.Payables
