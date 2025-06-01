/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Partitioned Aggregate Root              *
*  Type     : CashFlowProjection                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a cash flow projection with its entries.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Financial.Projects;

using Empiria.CashFlow.Projections.Data;

namespace Empiria.CashFlow.Projections {

  /// <summary>Partitioned type that represents a cash flow projection with its entries.</summary>
  [PartitionedType(typeof(CashFlowProjectionType))]
  public class CashFlowProjection : BaseObject, INamedEntity {

    #region Fields

    static internal readonly string BASE_ACCOUNT_ROLE = "cash-flow-projection-base-account";

    static internal readonly string DELETED_PROJECTION_NO = "Eliminada";
    static internal readonly string TO_ASSIGN_PROJECTION_NO = "Por asignar";

    #endregion Fields

    #region Constructors and parsers

    protected CashFlowProjection(CashFlowProjectionType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    internal CashFlowProjection(CashFlowPlan plan, CashFlowProjectionCategory category,
                                FinancialAccount baseAccount) : this(category.ProjectionType) {
      Assertion.Require(plan, nameof(plan));
      Assertion.Require(!plan.IsEmptyInstance, nameof(plan));
      Assertion.Require(category, nameof(category));
      Assertion.Require(!category.IsEmptyInstance, nameof(baseAccount));
      Assertion.Require(baseAccount, nameof(category));
      Assertion.Require(!baseAccount.IsEmptyInstance, nameof(baseAccount));
      Assertion.Require(baseAccount.FinancialAccountType.PlaysRole(BASE_ACCOUNT_ROLE),
                $"{baseAccount.Name} is not enabled to be used as baseAccount for cash flow projections.");

      Plan = plan;
      Category = category;
      BaseAccount = baseAccount;

      BaseProject = baseAccount.Project;
      BaseParty = baseAccount.OrganizationalUnit;

      OperationSource = OperationSource.Default;
      ProjectionNo = TO_ASSIGN_PROJECTION_NO;
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

      AuthorizedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      AuthorizationTime = DateTime.Now;
      Status = TransactionStatus.Authorized;
    }


    internal void Close() {
      Assertion.Require(Rules.CanClose, "Current user can not close this cash flow projection.");

      AppliedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      if (ApplicationDate == ExecutionServer.DateMaxValue) {
        ApplicationDate = DateTime.Now;
      }
      Status = TransactionStatus.Closed;
    }


    internal void DeleteOrCancel() {
      Assertion.Require(Rules.CanDelete, "Current user can not delete or cancel this cash flow projection.");

      Assertion.Require(this.Status == TransactionStatus.Pending,
                       $"Can not delete or cancel this cash flow projection. Its status is {Status.GetName()}.");

      if (HasProjectionNo) {
        Status = TransactionStatus.Canceled;
      } else {
        ProjectionNo = DELETED_PROJECTION_NO;
        Status = TransactionStatus.Deleted;
      }
    }


    internal FixedList<FinancialAccount> GetCashFlowAccounts() {
      return CashFlowProjectionDataService.GetCashFlowAccounts(this);
    }


    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      if (Status == TransactionStatus.Pending) {
        RecordedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        RecordingTime = DateTime.Now;
      }

      CashFlowProjectionDataService.WriteProjection(this);

      SaveEntries();
    }


    internal void Reject() {
      Assertion.Require(Rules.CanReject,
                        $"Can not reject this cash flow projection. Its status is {Status.GetName()}.");

      RequestedBy = Party.Empty;
      RequestedTime = ExecutionServer.DateMaxValue;

      AuthorizedBy = Party.Empty;
      AuthorizationTime = ExecutionServer.DateMaxValue;

      Status = TransactionStatus.Pending;
    }


    internal void SendToAuthorization() {
      Assertion.Require(Rules.CanSendToAuthorization,
                        "Current user can not send this cash flow projection to authorization.");

      if (!HasProjectionNo) {
        ProjectionNo = CashFlowProjectionDataService.GetNextProjectionNo(this);
      }

      RequestedBy = PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      RequestedTime = DateTime.Now;
      Status = TransactionStatus.OnAuthorization;
    }


    internal void Update(CashFlowProjectionFields fields) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this cash flow projection.");
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      BaseParty = PatchField(fields.PartyUID, BaseParty);
      BaseProject = PatchField(fields.ProjectUID, BaseProject);
      BaseAccount = PatchField(fields.AccountUID, BaseAccount);
      OperationSource = PatchField(fields.SourceUID, OperationSource);

      Description = EmpiriaString.Clean(fields.Description);
      Justification = EmpiriaString.Clean(fields.Justification);
      Tags = string.Join(" ", fields.Tags);

      ApplicationDate = PatchField(fields.ApplicationDate, ApplicationDate);
    }

    #endregion Methods

    #region Cash flow entries aggregate

    private Lazy<List<CashFlowProjectionEntry>> _entries = new Lazy<List<CashFlowProjectionEntry>>();
    private List<CashFlowProjectionEntry> _deletedEntries = new List<CashFlowProjectionEntry>();

    protected override void OnLoad() {
      _entries = new Lazy<List<CashFlowProjectionEntry>>(() =>
                        CashFlowProjectionDataService.GetProjectionEntries(this)
                     );
    }

    public FixedList<CashFlowProjectionEntry> Entries {
      get {
        return _entries.Value.ToFixedList();
      }
    }

    [DataField("CFW_PJC_TOTAL")]
    private decimal _total = 0;

    public decimal GetTotal() {
      if (_entries.IsValueCreated) {
        return Math.Abs(_entries.Value.Sum(x => x.InflowAmount - x.OutflowAmount));
      } else {
        return _total;
      }
    }


    internal CashFlowProjectionEntry AddEntry(CashFlowProjectionEntryFields fields) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this transaction.");

      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      if (TryGetEntry(fields) != null) {
        Assertion.RequireFail("Ya existe un movimiento con la misma información para el " +
                              "mismo mes y año en esta proyección de flujo de efectivo.");
      }

      var projectionColumn = CashFlowProjectionColumn.Parse(fields.ProjectionColumnUID);
      var cashflowAccount = FinancialAccount.Parse(fields.CashFlowAccountUID);

      var entry = new CashFlowProjectionEntry(this, projectionColumn, cashflowAccount,
                                              fields.Year.Value, fields.Month.Value, fields.Amount.Value);

      entry.Update(fields);

      _entries.Value.Add(entry);

      return entry;
    }


    internal FixedList<FinancialAccount> AvailableCashFlowAccounts() {
      return GetCashFlowAccounts()
            .FindAll(x => !Entries.Select(y => y.CashFlowAccount)
                                  .ToFixedList()
                                  .Contains(x)
            );
    }


    internal CashFlowProjectionEntry GetEntry(string projectionEntryUID) {
      Assertion.Require(projectionEntryUID, nameof(projectionEntryUID));

      CashFlowProjectionEntry entry = _entries.Value.Find(x => x.UID == projectionEntryUID);

      Assertion.Require(entry, $"Cash flow projection entry with UID '{projectionEntryUID}' not found.");

      return entry;
    }


    internal void RemoveEntry(CashFlowProjectionEntry entry) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this cash flow projection.");
      Assertion.Require(entry, nameof(entry));
      Assertion.Require(_entries.Value.Contains(entry),
                        "Entry to remove does not belong to this projection.");

      entry.Delete();

      _deletedEntries.Add(entry);
      _entries.Value.Remove(entry);
    }


    private void SaveEntries() {
      foreach (var entry in _entries.Value) {
        if (entry.IsDirty) {
          entry.Save();
        }
      }
      foreach (var deletedEntry in _deletedEntries) {
        deletedEntry.Save();
      }
      _deletedEntries.Clear();
    }


    internal CashFlowProjectionEntry TryGetEntry(CashFlowProjectionEntryFields fields) {
      var column = FieldPatcher.PatchField(fields.ProjectionColumnUID, CashFlowProjectionColumn.Empty);
      var account = FieldPatcher.PatchField(fields.CashFlowAccountUID, FinancialAccount.Empty);
      var product = FieldPatcher.PatchField(fields.ProductUID, Product.Empty);
      var productUnit = FieldPatcher.PatchField(fields.ProductUnitUID, ProductUnit.Empty);
      var currency = FieldPatcher.PatchField(fields.CurrencyUID, Plan.BaseCurrency);

      return _entries.Value.Find(x => x.ProjectionColumn.Equals(column) &&
                                      x.CashFlowAccount.Equals(account) &&
                                      x.Product.Equals(product) &&
                                      x.ProductUnit.Equals(productUnit) &&
                                      x.Currency.Equals(currency) &&
                                      x.Year == fields.Year &&
                                      x.Month == fields.Month);

    }


    internal void UpdateEntry(CashFlowProjectionEntry projectionEntry,
                              CashFlowProjectionEntryFields fields) {
      Assertion.Require(projectionEntry, nameof(projectionEntry));
      Assertion.Require(fields, nameof(fields));

      var currentEntry = TryGetEntry(fields);

      if (currentEntry != null && !projectionEntry.Equals(currentEntry)) {
        Assertion.RequireFail("Ya existe un movimiento con la misma información para el mismo " +
                              "mes y año en esta proyección de flujo de efectivo.");
      }

      projectionEntry.Update(fields);
    }


    internal void UpdateEntries(FixedList<CashFlowProjectionEntry> entries) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this transaction.");
      Assertion.Require(entries, nameof(entries));

      Assertion.Require(entries.CountAll(x => x.Projection.Equals(this)) == entries.Count,
                        "All entries must belong to this cash flow projection.");

      foreach (var entry in entries) {
        if (entry.IsNew) {
          _entries.Value.Add(entry);
        } else if (entry.Status == TransactionStatus.Deleted) {
          _deletedEntries.Add(entry);
          _entries.Value.Remove(entry);
        }
      }
    }

    #endregion Cash flow entries aggregate

  }  // class CashFlowProjection

}  // namespace Empiria.CashFlow.Projections
