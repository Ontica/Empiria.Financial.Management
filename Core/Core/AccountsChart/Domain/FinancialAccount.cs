/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : FinancialAccount                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a financial account.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Projects;

using Empiria.Financial.Data;

namespace Empiria.Financial {

  /// <summary>Partitioned type that represents a financial account.</summary>
  [PartitionedType(typeof(FinancialAccountType))]
  public class FinancialAccount : BaseObject, INamedEntity {

    #region Fields

    static internal readonly string PROJECTED_ACCOUNT_NO = "Proyectada";

    #endregion Fields

    #region Constructors and parsers

    protected FinancialAccount(FinancialAccountType powertype) : base(powertype) {
      // Required by Empiria FrameWork
    }


    public FinancialAccount(FinancialAccountType accountType, StandardAccount stdAccount,
                            OrganizationalUnit orgUnit) : base(accountType) {

      Assertion.Require(stdAccount, nameof(stdAccount));
      Assertion.Require(!stdAccount.IsEmptyInstance, nameof(stdAccount));
      Assertion.Require(orgUnit, nameof(orgUnit));
      Assertion.Require(!orgUnit.IsEmptyInstance, nameof(orgUnit));

      this.StandardAccount = stdAccount;
      this.AccountNo = PROJECTED_ACCOUNT_NO;
      this.Description = stdAccount.Description;

      this.OrganizationalUnit = orgUnit;

      this.StartDate = DateTime.Today;
    }

    static public FinancialAccount Parse(int id) => ParseId<FinancialAccount>(id);

    static public FinancialAccount Parse(string uid) => ParseKey<FinancialAccount>(uid);

    static public FinancialAccount Empty => ParseEmpty<FinancialAccount>();

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
    }


    [DataField("ACCT_NUMBER")]
    public string AccountNo {
      get; protected set;
    }


    [DataField("ACCT_DESCRIPTION")]
    public string Description {
      get; protected set;
    }


    public string Name {
      get {
        if (AccountNo.Length != 0) {
          return $"({AccountNo}) {Description}";
        }
        return Description;
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
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(AccountNo, Description, _identifiers, _tags, Project.Keywords,
                                           OrganizationalUnit.Keywords, StandardAccount.Keywords);
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


    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.StartDate = ExecutionServer.DateMinValue;
        this.EndDate = ExecutionServer.DateMaxValue;

        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      FinancialAccountDataService.WriteAccount(this, this.ExtData.ToString());
    }


    internal void Update(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      AccountNo = PatchField(fields.AccountNo, AccountNo);
      Description = PatchField(fields.Description, Description);
      StandardAccount = PatchField(fields.StandardAccountUID, StandardAccount);
      OrganizationalUnit = PatchField(fields.OrganizationalUnitUID, OrganizationalUnit);
      Project = PatchField(fields.ProjectUID, Project);
      _attributes = fields.GetAtttributesToJson();
      _financialData = fields.GetFinancialDataToJson();
      _tags = string.Join(" ", fields.Tags);
    }

    #endregion Methods

  } // class FinancialAccount

} // namespace Empiria.Financial
