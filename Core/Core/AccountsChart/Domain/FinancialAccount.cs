/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : FinancialAccount                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial account.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;
using Empiria.Json;
using Empiria.Parties;

using Empiria.Financial.Projects;

using Empiria.Financial.Data;

using Empiria.Financial.Accounts.Adapters;

namespace Empiria.Financial {

  /// <summary>Represents a financial account.</summary>
  public class FinancialAccount : BaseObject, INamedEntity {

    #region Constructors and parsers

    private FinancialAccount() {
      // Require by Empiria FrameWork
    }

    public FinancialAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));
      Update(fields);
    }

    static public FinancialAccount Parse(int id) => ParseId<FinancialAccount>(id);

    static public FinancialAccount Parse(string uid) => ParseKey<FinancialAccount>(uid);

    static public FinancialAccount Empty => ParseEmpty<FinancialAccount>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("ACCT_STD_ACCT_ID")]
    public StandardAccount StandardAccount {
      get; private set;
    }


    [DataField("ACCT_ORG_ID")]
    public Party Organization {
      get; private set;
    }


    [DataField("ACCT_ORG_UNIT_ID")]
    public Party OrganizationUnit {
      get; private set;
    }


    [DataField("ACCT_PARTY_ID")]
    public Party Party {
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
    public string AcctNo {
      get; private set;
    }


    [DataField("ACCT_DESCRIPTION")]
    public string Name {
      get; private set;
    }

    [DataField("ACCT_IDENTIFIERS")]
    public string Identifiers {
      get; protected set;
    }


    [DataField("ACCT_TAGS")]
    public string Tags {
      get; protected set;
    }


    [DataField("ACCT_ATTRIBUTES")]
    public JsonObject Attributes {
      get; protected set;
    }


    [DataField("ACCT_FINANCIAL_DATA")]
    public JsonObject FinancialData {
      get; protected set;
    }


    [DataField("ACCT_CONFIG_DATA")]
    public JsonObject ConfigData {
      get; protected set;
    }


    [DataField("ACCT_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(AcctNo, Name);
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


    [DataField("ACCT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Active;

    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = EntityStatus.Deleted;

    }


    internal static FixedList<FinancialAccount> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      return FinancialAccountDataService.SearchAccount(keywords);
    }


    internal static FixedList<FinancialAccount> SearchAccount(string filter, string sort) {
      return FinancialAccountDataService.SearchAccount(filter, sort);
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

      this.StandardAccount = StandardAccount.Parse(fields.StandarAccountUID);
      this.Organization = Party.Parse(fields.OrganizationUID);
      this.OrganizationUnit = Party.Parse(fields.OrganizationUnitUID);
      this.Party = Party.Parse(fields.PartyUID);
      this.Project = FinancialProject.Parse(fields.ProjectUID);
      this.LedgerId = fields.LedgerId;
      this.AcctNo = fields.AcctNo;
      this.Name = fields.Description;
      this.Identifiers = fields.Identifiers;
      this.Tags = fields.Tags;
    }

    #endregion Methods

  } // class FinancialAccount

} // namespace Empiria.Financial
