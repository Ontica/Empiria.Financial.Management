﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Partitioned Aggregate Root              *
*  Type     : CashFlowProjection                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a cash flow projection with its entries.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Financial.Projects;

using Empiria.CashFlow.Projections.Data;

namespace Empiria.CashFlow.Projections {

  /// <summary>Partitioned type that represents a cash flow projection with its entries.</summary>
  [PartitionedType(typeof(CashFlowProjectionType))]
  public class CashFlowProjection : BaseObject, INamedEntity {

    #region Fields

    private readonly string TO_ASSIGN_PROJECTION_NO = "Por asignar";

    #endregion Fields

    #region Constructors and parsers

    protected CashFlowProjection(CashFlowProjectionType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal protected CashFlowProjection(CashFlowProjectionType projectionType,
                                          CashFlowPlan plan) : base(projectionType) {
      Assertion.Require(plan, nameof(plan));

      this.Plan = plan;
    }

    static public CashFlowProjection Parse(int id) => ParseId<CashFlowProjection>(id);

    static public CashFlowProjection Parse(string uid) => ParseKey<CashFlowProjection>(uid);

    static public CashFlowProjection Empty => ParseEmpty<CashFlowProjection>();

    #endregion Constructors and parsers

    #region Properties

    public CashFlowProjectionType ProjectionType {
      get {
        return (CashFlowProjectionType) base.GetEmpiriaType();
      }
    }

    [DataField("CFW_PJC_CATEGORY_ID")]
    public CashFlowProjectionCategory Category {
      get; private set;
    }


    [DataField("CFW_PJC_PLAN_ID")]
    public CashFlowPlan Plan {
      get; private set;
    }


    [DataField("CFW_PJC_NO")]
    public string ProjectionNo {
      get; private set;
    }


    [DataField("CFW_PJC_BASE_PARTY_ID")]
    public Party BaseParty {
      get; private set;
    }

    [DataField("CFW_PJC_BASE_PROJECT_ID")]
    public FinancialProject BaseProject {
      get; private set;
    }


    [DataField("CFW_PJC_BASE_ACCOUNT_ID")]
    public FinancialAccount BaseAccount {
      get; private set;
    }


    [DataField("CFW_PJC_SOURCE_ID")]
    public OperationSource OperationSource {
      get; private set;
    }


    [DataField("CFW_PJC_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("CFW_PJC_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("CFW_PJC_IDENTIFICATORS")]
    public string Identificators {
      get; private set;
    }


    [DataField("CFW_PJC_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("CFW_PJC_ATTRIBUTES_DATA")]
    public JsonObject AttributesData {
      get; private set;
    }


    [DataField("CFW_PJC_FINANCIAL_DATA")]
    public JsonObject FinancialData {
      get; private set;
    }


    [DataField("CFW_PJC_CONFIG_DATA")]
    public JsonObject ConfigData {
      get; private set;
    }


    [DataField("CFW_PJC_EXT_DATA")]
    internal protected JsonObject ExtData {
      get; private set;
    }


    [DataField("CFW_PJC_APPLICATION_DATE")]
    public DateTime ApplicationDate {
      get; private set;
    }


    [DataField("CFW_PJC_APPLIED_BY_ID")]
    public Party AppliedBy {
      get; private set;
    }


    [DataField("CFW_PJC_RECORDING_TIME")]
    public DateTime RecordingTime {
      get; private set;
    }


    [DataField("CFW_PJC_RECORDED_BY_ID")]
    public Party RecordedBy {
      get; private set;
    }


    [DataField("CFW_PJC_AUTHORIZATION_TIME")]
    public DateTime AuthorizationTime {
      get; private set;
    }


    [DataField("CFW_PJC_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("CFW_PJC_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get; private set;
    }


    [DataField("CFW_PJC_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }

    [DataField("CFW_PJC_ADJUSTMENT_OF_ID")]
    private int _adjustmentOfId = -1;

    public CashFlowProjection AdjustmentOf {
      get {
        if (IsEmptyInstance) {
          return this;
        }
        return Parse(_adjustmentOfId);
      } set {
        _adjustmentOfId = value.Id;
      }
    }


    [DataField("CFW_PJC_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("CFW_PJC_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CFW_PJC_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get; private set;
    }


    public virtual string Name {
      get {
        return this.ProjectionNo;
      }
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ProjectionNo, Description, Identificators, Tags,
                                           Category.Keywords, BaseProject.Keywords, BaseAccount.Keywords,
                                           BaseParty.Keywords, AdjustmentOf.ProjectionNo, Plan.Keywords);
      }
    }


    public bool HasProjectionNo {
      get {
        return this.ProjectionNo.Length != 0 &&
               this.ProjectionNo != TO_ASSIGN_PROJECTION_NO;
      }
    }

    internal CashFlowProjectionRules Rules {
      get {
        return new CashFlowProjectionRules(this);
      }
    }

    #endregion Properties

    #region Methods

    internal void Authorize() {
      Assertion.Require(Rules.CanAuthorize, "Current user can not authorize this cash flow projection.");

      if (!HasProjectionNo) {
        ProjectionNo = CashFlowProjectionDataService.GetNextProjectionNo(this);
      }

      this.AuthorizedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      this.AuthorizationTime = DateTime.Now;
      this.Status = TransactionStatus.Authorized;
    }


    internal void Close() {
      Assertion.Require(Rules.CanClose, "Current user can not close this cash flow projection.");

      this.AppliedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      if (ApplicationDate == ExecutionServer.DateMaxValue) {
        ApplicationDate = DateTime.Now;
      }
      this.Status = TransactionStatus.Closed;
    }


    internal void DeleteOrCancel() {
      Assertion.Require(Rules.CanDelete, "Current user can not delete or cancel this cash flow projection.");

      Assertion.Require(this.Status == TransactionStatus.Pending,
                       $"Can not delete or cancel this cash flow projection. Its status is {Status.GetName()}.");

      if (HasProjectionNo) {
        this.Status = TransactionStatus.Canceled;
      } else {
        this.ProjectionNo = "Eliminada";
        this.Status = TransactionStatus.Deleted;
      }
    }


    protected override void OnSave() {
      if (IsNew) {
        ProjectionNo = TO_ASSIGN_PROJECTION_NO;
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      if (Status == TransactionStatus.Pending) {
        RecordedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        RecordingTime = DateTime.Now;
      }

      CashFlowProjectionDataService.WriteProjection(this);
    }


    internal void Reject() {
      Assertion.Require(Rules.CanReject,
                        $"Can not reject this cash flow projection. Its status is {Status.GetName()}.");

      this.RequestedBy = Party.Empty;
      this.RequestedTime = ExecutionServer.DateMaxValue;

      this.AuthorizedBy = Party.Empty;
      this.AuthorizationTime = ExecutionServer.DateMaxValue;

      this.Status = TransactionStatus.Pending;
    }


    internal void SendToAuthorization() {
      Assertion.Require(Rules.CanSendToAuthorization,
                        "Current user can not send this cash flow projection to authorization.");

      if (!HasProjectionNo) {
        ProjectionNo = CashFlowProjectionDataService.GetNextProjectionNo(this);
      }

      this.RequestedBy = PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      this.RequestedTime = DateTime.Now;
      this.Status = TransactionStatus.OnAuthorization;
    }

    #endregion Methods

  }  // class CashFlowProjection

}  // namespace Empiria.CashFlow.Projections
