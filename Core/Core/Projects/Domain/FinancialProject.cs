/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Aggregate type                          *
*  Type     : FinancialProject                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial project that is an aggregate of financial accounts.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Projects.Data;

namespace Empiria.Financial.Projects {

  /// <summary>Represents a financial project that is an aggregate of financial accounts.</summary>
  [PartitionedType(typeof(FinancialProjectType))]
  public class FinancialProject : BaseObject, INamedEntity {

    #region Fields

    static internal readonly string PROJECT_BASE_ACCOUNTS_ROLE = "cash-flow-projection-base-account";
    static internal readonly string STANDARD_ACCOUNTS_ROLE = "financial-project-std-account";

    static internal readonly string TO_ASSIGN_PROJECT_NO = "Por asignar";
    static internal readonly string DELETED_PROJECT_NO = "Eliminado";

    private Lazy<List<FinancialAccount>> _accounts = new Lazy<List<FinancialAccount>>();
    private List<FinancialAccount> _deletedAccounts = new List<FinancialAccount>();

    #endregion Fields

    #region Constructors and parsers

    protected FinancialProject(FinancialProjectType powertype) : base(powertype) {
      // Required by Empiria Framework
    }

    internal FinancialProject(FinancialProjectCategory category, OrganizationalUnit baseOrgUnit,
                              FinancialProgram subprogram, string name) : base(FinancialProjectType.Empty) {
      Assertion.Require(category, nameof(category));
      Assertion.Require(!category.IsEmptyInstance, nameof(category));
      Assertion.Require(baseOrgUnit, nameof(baseOrgUnit));
      Assertion.Require(!baseOrgUnit.IsEmptyInstance, nameof(baseOrgUnit));
      Assertion.Require(subprogram, nameof(subprogram));
      Assertion.Require(name, nameof(name));

      Category = category;
      BaseOrgUnit = baseOrgUnit;
      Subprogram = subprogram;

      Name = EmpiriaString.Clean(name);
      ProjectNo = TO_ASSIGN_PROJECT_NO;
      StartDate = DateTime.Today;
    }

    static public FinancialProject Parse(int id) => ParseId<FinancialProject>(id);

    static public FinancialProject Parse(string uid) => ParseKey<FinancialProject>(uid);

    static public FinancialProject Empty => ParseEmpty<FinancialProject>();

    protected override void OnLoad() {
      Refresh();
    }

    #endregion Constructors and parsers

    #region Properties

    public FinancialProjectType FinancialProjectType {
      get {
        return (FinancialProjectType) base.GetEmpiriaType();
      }
    }


    [DataField("PRJ_CATEGORY_ID")]
    public FinancialProjectCategory Category {
      get; private set;
    }


    [DataField("PRJ_NO")]
    public string ProjectNo {
      get; private set;
    }


    [DataField("PRJ_NAME")]
    public string Name {
      get; private set;
    }


    string INamedEntity.Name {
      get {
        if (ProjectNo.Length != 0 && Status == EntityStatus.Pending) {
          return $"({ProjectNo}) {Name} (Proyectado) [{Program.Name}]";
        } else if (ProjectNo.Length == 0 && Status == EntityStatus.Pending) {
          return $"(S/N) {Name} (Proyectado) [{Program.Name}]";
        } else if (ProjectNo.Length != 0) {
          return $"({ProjectNo}) {Name} [{Program.Name}]";
        } else {
          return $"(S/N) {Name} [{Program.Name}]";
        }
      }
    }


    public FinancialProgram Program {
      get {
        return Subprogram.Parent;
      }
    }


    [DataField("PRJ_SUBPROGRAM_ID")]
    public FinancialProgram Subprogram {
      get; private set;
    }


    [DataField("PRJ_BASE_ORG_UNIT_ID")]
    public Party BaseOrgUnit {
      get; private set;
    }


    [DataField("PRJ_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("PRJ_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("PRJ_IDENTIFIERS")]
    private string _identifiers = string.Empty;

    public FixedList<string> Identifiers {
      get {
        return EmpiriaString.Tagging(_identifiers);
      }
    }


    [DataField("PRJ_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }


    [DataField("PRJ_GOALS_EXT_DATA")]
    private JsonObject _projectGoals = new JsonObject();

    internal protected FinancialProjectGoals ProjectGoals {
      get {
        return new FinancialProjectGoals(_projectGoals);
      }
    }


    [DataField("PRJ_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ProjectNo, Name, _identifiers, _tags, Program.Name,
                                           BaseOrgUnit.Keywords, Category.Keywords, Subprogram.Keywords,
                                           ProjectGoals.Beneficiario, ProjectGoals.Localizacion);
      }
    }


    [DataField("PRJ_PARENT_ID")]
    private int _parentId = -1;

    public FinancialProject Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        return Parse(_parentId);
      }
      private set {
        _parentId = value.Id;
      }
    }


    [DataField("PRJ_ASSIGNEE_ID")]
    public Person Assignee {
      get; private set;
    }


    [DataField("PRJ_RECORDING_TIME")]
    public DateTime RecordingTime {
      get; private set;
    }


    [DataField("PRJ_RECORDED_BY_ID")]
    public Party RecordedBy {
      get; private set;
    }


    [DataField("PRJ_AUTHORIZATION_TIME")]
    public DateTime AuthorizationTime {
      get; private set;
    }


    [DataField("PRJ_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("PRJ_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("PRJ_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("PRJ_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PRJ_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PRJ_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    internal int HistoricId {
      get {
        return this.Id;
      }
    }

    internal FinancialProjectRules Rules {
      get {
        return new FinancialProjectRules(this);
      }
    }


    public FixedList<FinancialAccount> Accounts {
      get {
        return _accounts.Value.ToFixedList();
      }
    }

    public FixedList<FinancialAccount> BaseAccounts {
      get {
        return _accounts.Value.ToFixedList()
                        .FindAll(x => x.FinancialAccountType.PlaysRole(FinancialProject.PROJECT_BASE_ACCOUNTS_ROLE));
      }
    }


    public bool HasProjectNo {
      get {
        return this.ProjectNo.Length != 0 &&
               this.ProjectNo != TO_ASSIGN_PROJECT_NO;
      }
    }

    #endregion Properties

    #region Methods

    internal void Authorize() {
      Assertion.Require(Rules.CanAuthorize, "Current user can not authorize this project.");

      AuthorizedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      AuthorizationTime = DateTime.Now;
      Status = EntityStatus.OnReview;
    }


    internal void Delete() {
      Assertion.Require(Rules.CanDelete,
                        $"Can not delete project. Its status is {Status.GetName()}.");
      this.Status = EntityStatus.Deleted;
    }


    internal void Refresh() {
      _accounts = new Lazy<List<FinancialAccount>>(() => FinancialProjectDataService.GetProjectAccounts(this));
    }


    protected override void OnSave() {
      if (base.IsNew) {
        StartDate = ExecutionServer.DateMaxValue;
        EndDate = ExecutionServer.DateMaxValue;
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
        ProjectNo = FinancialProjectDataService.GetNextProjectNo();
      }
      if (Status == EntityStatus.Pending) {
        RecordedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        RecordingTime = DateTime.Now;
      }

      FinancialProjectDataService.WriteProject(this, this.ExtData.ToString());

      foreach (var account in _accounts.Value) {
        if (account.IsDirty) {
          account.Save();
        }
      }
      foreach (var deletedAccount in _deletedAccounts) {
        deletedAccount.Save();
      }
      _deletedAccounts.Clear();
    }


    internal void SetParent(FinancialProject parent) {
      Assertion.Require(parent, nameof(parent));

      Parent = parent;

      MarkAsDirty();
    }


    internal void Update(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Category = Patcher.Patch(fields.ProjectTypeCategoryUID, this.Category);
      Subprogram = Patcher.Patch(fields.SubprogramUID, Subprogram);
      Name = Patcher.Patch(fields.Name, this.Name);
      ProjectNo = Patcher.Patch(fields.ProjectNo, this.ProjectNo);
      BaseOrgUnit = Patcher.Patch(fields.BaseOrgUnitUID, this.BaseOrgUnit);
      Assignee = Patcher.Patch(fields.AssigneeUID, this.Assignee);
      Description = EmpiriaString.Clean(fields.Description);
      Justification = EmpiriaString.Clean(fields.Justification);
      _projectGoals = JsonObject.Parse(fields.ProjectGoals);

      MarkAsDirty();
    }

    #endregion Methods

    #region Accounts aggregate methods

    internal FinancialAccount AddAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var accountType = FinancialAccountType.Parse(fields.FinancialAccountTypeUID);
      var stdAccount = StandardAccount.Parse(fields.StandardAccountUID);
      var orgUnit = OrganizationalUnit.Parse(fields.OrganizationalUnitUID);

      fields.EnsureValid();

      Assertion.Require(stdAccount.Category.PlaysRole(STANDARD_ACCOUNTS_ROLE),
                        $"stdAccount {stdAccount.Name} can not be added to projects.");

      var account = new FinancialAccount(accountType, stdAccount, orgUnit, this);

      account.Update(fields);

      return account;
    }


    internal FinancialAccount GetAccount(string accountUID) {
      Assertion.Require(accountUID, nameof(accountUID));

      FinancialAccount entry = _accounts.Value.Find(x => x.UID == accountUID);

      Assertion.Require(entry, $"Financial project account with UID '{accountUID}' not found.");

      return FinancialAccount.Parse(accountUID);
    }


    internal FixedList<StandardAccount> GetStandardAccounts() {
      return this.Subprogram.StandardAccount.GetAllChildren()
                                            .FindAll(x =>
                                              x.Category.PlaysRole(STANDARD_ACCOUNTS_ROLE)
                                            );
    }


    internal void RemoveAccount(FinancialAccount account) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this project.");
      Assertion.Require(account, nameof(account));
      Assertion.Require(_accounts.Value.Contains(account),
                        "Entry to remove does not belong to this project.");

      account.Delete();

      var operations = account.GetOperations();

      foreach (var operation in operations) {
        account.RemoveOperation(operation.UID);
      }

      _deletedAccounts.Add(account);
      _accounts.Value.Remove(account);
    }


    internal void UpdateAccount(FinancialAccount account, FinancialAccountFields fields) {
      Assertion.Require(account, nameof(account));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      _ = GetAccount(account.UID);

      account.Update(fields);
    }

    #endregion Accounts aggregate methods

  } // class FinancialProject

} // namespace Empiria.Financial.Projects
