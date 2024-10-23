/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestone Management              Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractMilestone                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract milestone.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contacts;
using Empiria.Contracts.Data;
using Empiria.Json;
using Empiria.Parties;
using Empiria.StateEnums;
using System;

namespace Empiria.Contracts {

  /// <summary>Represents a contract milestone.</summary>
  public class ContractMilestone : BaseObject {

    #region Constructors and parsers

    static internal ContractMilestone Parse(int id) {
      return BaseObject.ParseId<ContractMilestone>(id);
    }

    static internal ContractMilestone Parse(string uid) {
      return BaseObject.ParseKey<ContractMilestone>(uid);
    }

    static internal FixedList<ContractMilestone> GetList() {
      return BaseObject.GetList<ContractMilestone>()
                       .ToFixedList();
    }

    static internal ContractMilestone Empty => BaseObject.ParseEmpty<ContractMilestone>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("CONTRACT_ID")]
    public Contract Contract {
      get; private set;
    }


    [DataField("MILESTONE_PAYMENT_NUMBER")]
    public int PaymentNumber {
      get; private set;
    }


    [DataField("MILESTONE_PAYMENT_DATE")]
    public DateTime PaymentDate {
      get; private set;
    }


    [DataField("MILESTONE_MGMT_ORG_UNIT_ID")]
    public OrganizationalUnit ManagedByOrgUnit {
      get; private set;
    }


    [DataField("CONTRACT_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ManagedByOrgUnit.Name, ManagedByOrgUnit.Code);
      }
    }


    [DataField("MILESTONE_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("MILESTONE_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("MILESTONE_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal void Activate() {
      Assertion.Require(this.Status == EntityStatus.Suspended,
                  $"No se puede activar un entregable que no está suspendido.");

      this.Status = EntityStatus.Active;
    }


    internal void Delete() {

      Assertion.Require(this.Status == EntityStatus.Active || this.Status == EntityStatus.Suspended,
                  $"No se puede eliminar un entregable que está en estado {this.Status.GetName()}.");

      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }

      ContractMilestoneData.WriteMilestone(this, this.ExtData.ToString());
    }


    internal void Suspend() {
      Assertion.Require(this.Status == EntityStatus.Active,
                  $"No se puede suspender un contrato que no está activo.");

      this.Status = EntityStatus.Suspended;
    }

    #endregion Methods

  }  // class ContractMilestone

}  // namespace Empiria.Contracts
