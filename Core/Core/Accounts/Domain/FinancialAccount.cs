/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Aggregate Partitioned Type              *
*  Type     : FinancialAccount                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a financial account as an aggregate of account operations.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Data;
using Empiria.Financial.Projects;

namespace Empiria.Financial {

  /// <summary>Partitioned type that represents a financial account as
  /// an aggregate of account operations.</summary>
  [PartitionedType(typeof(FinancialAccountType))]
  public class FinancialAccount : BaseObject, INamedEntity {

    #region Fields

    private Lazy<List<FinancialAccount>> _operations = new Lazy<List<FinancialAccount>>();
    private List<FinancialAccount> _deletedOperations = new List<FinancialAccount>();

    static readonly private List<FinancialAccount> _cache = null;

    #endregion Fields

    #region Constructors and parsers

    static FinancialAccount() {
      var allAccounts = GetFullList<FinancialAccount>("ACCT_STATUS <> 'X'");

      _cache = new List<FinancialAccount>(allAccounts);
    }


    protected FinancialAccount(FinancialAccountType powertype) : base(powertype) {
      // Required by Empiria Framework
    }


    public FinancialAccount(FinancialAccountType accountType, StandardAccount stdAccount,
                            OrganizationalUnit orgUnit) : base(accountType) {

      Assertion.Require(stdAccount, nameof(stdAccount));
      Assertion.Require(!stdAccount.IsEmptyInstance, nameof(stdAccount));
      Assertion.Require(orgUnit, nameof(orgUnit));
      Assertion.Require(!orgUnit.IsEmptyInstance, nameof(orgUnit));

      this.StandardAccount = stdAccount;
      this.Currency = Currency.Default;

      this.Description = stdAccount.Description;

      this.OrganizationalUnit = orgUnit;

      this.StartDate = DateTime.Today;
    }


    public FinancialAccount(FinancialAccountType accountType, StandardAccount stdAccount,
                            OrganizationalUnit orgUnit, FinancialProject project)
                    : this(accountType, stdAccount, orgUnit) {

      Assertion.Require(project, nameof(project));
      Assertion.Require(!project.IsEmptyInstance, nameof(project));

      Project = project;
      AccountNo = (Project.BaseAccounts.Count + 1).ToString("000");
    }

    static public FinancialAccount Parse(int id) => ParseId<FinancialAccount>(id);

    static public FinancialAccount Parse(string uid) => ParseKey<FinancialAccount>(uid);

    static public FinancialAccount Empty => ParseEmpty<FinancialAccount>();

    static public FixedList<FinancialAccount> GetList() {
      return _cache.ToFixedList();
    }


    protected override void OnLoad() {
      _operations = new Lazy<List<FinancialAccount>>(() =>
                      _cache.FindAll(x => x._parentId == this.Id &&
                                          x.Project.Equals(this.Project) &&
                                          x.FinancialAccountType.Equals(FinancialAccountType.OperationAccount)
                      ));
    }

    #endregion Constructors and parsers

    #region Properties

    public FinancialAccountType FinancialAccountType {
      get {
        return (FinancialAccountType) base.GetEmpiriaType();
      }
    }


    [DataField("ACCT_STD_ACCT_ID")]
    public StandardAccount StandardAccount {
      get; private set;
    }


    [DataField("ACCT_ORG_ID")]
    public Party Organization {
      get; private set;
    }


    [DataField("ACCT_ORG_UNIT_ID")]
    public OrganizationalUnit OrganizationalUnit {
      get; private set;
    }


    [DataField("ACCT_PROJECT_ID")]
    public FinancialProject Project {
      get; private set;
    }


    [DataField("ACCT_LEDGER_ID")]
    internal int LedgerId {
      get; private set;
    } = -1;


    [DataField("ACCT_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("ACCT_NUMBER")]
    public string AccountNo {
      get; private set;
    }


    public string Code {
      get {
        if (AccountNo.Length == 0) {
          return StandardAccount.StdAcctNo;
        }
        return AccountNo;
      }
    }


    [DataField("ACCT_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    public string Name {
      get {
        string name = string.Empty;

        if (Description.Length == 0) {
          name = StandardAccount.Description;

        } else {
          name = Description;
        }

        if (Status == EntityStatus.Pending) {
          name += FinancialAccountType.FemaleGenre ? " [Proyectada]" : " [Proyectado]";
        }
        return name;
      }
    }


    string INamedEntity.Name {
      get {
        return $"({Code}) {Name}";
      }
    }


    [DataField("ACCT_IDENTIFIERS")]
    private string _identifiers = string.Empty;

    public FixedList<string> Identifiers {
      get {
        return _identifiers.Split(' ')
                           .ToFixedList();
      }
    }


    [DataField("ACCT_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ')
                    .ToFixedList();
      }
    }


    [DataField("ACCT_ATTRIBUTES")]
    private JsonObject _attributes = new JsonObject();

    public AccountAttributes Attributes {
      get {
        return new CreditAttributes(_attributes);
      }
    }


    [DataField("ACCT_FINANCIAL_DATA")]
    private JsonObject _financialData = new JsonObject();

    internal protected FinancialData FinancialData {
      get {
        return new CreditFinancialData(_financialData);
      }
    }


    [DataField("ACCT_CONFIG_DATA")]
    internal protected JsonObject ConfigData {
      get; private set;
    }


    [DataField("ACCT_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Code, Name, _identifiers, _tags,
                                           Project.Keywords, OrganizationalUnit.Keywords,
                                           StandardAccount.Keywords);
      }
    }


    [DataField("ACCT_PARENT_ID")]
    private int _parentId = -1;

    public FinancialAccount Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        return Parse(_parentId);
      }
      private set {
        if (this.IsEmptyInstance) {
          return;
        }
        _parentId = value.Id;
      }
    }


    [DataField("ACCT_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("ACCT_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("ACCT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("ACCT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ACCT_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Pending;


    public bool IsInflowAccount {
      get {
        string[] segments = StandardAccount.StdAcctSegments;

        int lastSegmentAsInteger = int.Parse(segments[segments.Length - 1]);

        if (1 <= lastSegmentAsInteger && lastSegmentAsInteger <= 5) {
          return true;
        }
        return false;
      }
    }

    #endregion Properties

    #region Methods

    internal FinancialAccount AddOperation(StandardAccount stdAccount) {
      Assertion.Require(stdAccount, nameof(stdAccount));

      Assertion.Require(!HasOperation(stdAccount),
                        $"Esta cuenta ya contiene la operación {stdAccount.Name}.");

      var operation = new FinancialAccount(FinancialAccountType.OperationAccount,
                                           stdAccount, this.OrganizationalUnit, this.Project);

      operation.AccountNo =
                 $"{stdAccount.StdAcctSegments[stdAccount.StdAcctSegments.Length - 1]}-" +
                 $"{this.Project.ProjectNo}-" +
                 $"{this.AccountNo}";

      operation.Parent = this;

      _operations.Value.Add(operation);

      return operation;
    }


    internal void Delete() {
      this.Status = EntityStatus.Deleted;

      MarkAsDirty();
    }


    internal FixedList<StandardAccount> GetAvailableOperations() {
      return StandardAccount.GetChildren()
                            .FindAll(x => !GetOperations().Contains(y => y.StandardAccount.Equals(x)));
    }


    internal FixedList<FinancialAccount> GetOperations() {
      return _operations.Value.ToFixedList();
    }


    private bool HasOperation(StandardAccount stdAccount) {
      return GetOperations()
            .Contains(x => x.StandardAccount.Equals(stdAccount));
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.StartDate = ExecutionServer.DateMinValue;
        this.EndDate = ExecutionServer.DateMaxValue;

        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;

        _cache.Add(this);
      }

      FinancialAccountDataService.WriteAccount(this, this.ExtData.ToString());

      if (this.Status == EntityStatus.Deleted) {
        _cache.Remove(this);
      }

      foreach (var operation in _operations.Value) {
        if (operation.IsDirty) {
          operation.Save();
        }
      }
      foreach (var deletedOperation in _deletedOperations) {
        deletedOperation.Save();
      }
      _deletedOperations.Clear();

      if (!this.Project.IsEmptyInstance) {
        Project.Refresh();
      }
    }


    internal FinancialAccount RemoveOperation(string operationAccountUID) {
      Assertion.Require(operationAccountUID, nameof(operationAccountUID));

      FinancialAccount operation = GetOperations().Find(x => x.UID == operationAccountUID);

      Assertion.Require(operation, "La operación que se intentó remover no existe.");

      Assertion.Require(operation.Status == EntityStatus.Pending,
                        $"No se puede eliminar la operación {operation.Name} debido a que ya está activa");
      operation.Delete();

      _deletedOperations.Add(operation);

      return operation;
    }


    internal void Update(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      if (AccountNo.Length == 0) {
        AccountNo = PatchField(fields.AccountNo, AccountNo);
      }
      Description = PatchField(fields.Description, Description);
      StandardAccount = PatchField(fields.StandardAccountUID, StandardAccount);
      Currency = PatchField(fields.CurrencyUID, Currency);
      OrganizationalUnit = PatchField(fields.OrganizationalUnitUID, OrganizationalUnit);
      _attributes = JsonObject.Parse(fields.Attributes);
      _financialData = JsonObject.Parse(fields.FinancialData);
      _tags = string.Join(" ", fields.Tags);

      MarkAsDirty();
    }

    #endregion Methods

  } // class FinancialAccount

} // namespace Empiria.Financial
