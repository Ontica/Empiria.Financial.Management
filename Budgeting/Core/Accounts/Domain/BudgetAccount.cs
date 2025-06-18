/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : BudgetAccount                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a budget account.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting.Data;

namespace Empiria.Budgeting {

  /// <summary>Partitioned type that represents a budget account.</summary>
  [PartitionedType(typeof(BudgetAccountType))]
  public class BudgetAccount : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected BudgetAccount(BudgetAccountType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    public BudgetAccount(BudgetAccountType accountType,
                         BudgetAccountSegment baseSegment,
                         OrganizationalUnit orgUnit) : this(accountType) {
      Assertion.Require(accountType, nameof(accountType));
      Assertion.Require(baseSegment, nameof(baseSegment));
      Assertion.Require(orgUnit, nameof(orgUnit));

      this.BudgetType = baseSegment.BudgetSegmentType.BudgetType;
      this.BaseSegment = baseSegment;
      this.Code = baseSegment.Code;
      this.Organization = Organization.Primary;
      this.OrganizationalUnit = orgUnit;
      this.StartDate = DateTime.Today.Date;
    }


    static public BudgetAccount Parse(int id) => ParseId<BudgetAccount>(id);

    static public BudgetAccount Parse(string uid) => ParseKey<BudgetAccount>(uid);

    static public BudgetAccount Empty => ParseEmpty<BudgetAccount>();

    #endregion Constructors and parsers

    #region Properties

    public BudgetAccountType BudgetAccountType {
      get {
        return (BudgetAccountType) base.GetEmpiriaType();
      }
    }


    [DataField("BDG_ACCT_BUDGET_TYPE_ID")]
    public BudgetType BudgetType {
      get; private set;
    }


    [DataField("BDG_ACCT_CODE")]
    public string Code {
      get; private set;
    }


    [DataField("BDG_ACCT_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    public string Name {
      get {
        return $"{BaseSegment.Code} - {BaseSegment.Name}" +
               (Status == EntityStatus.Pending ? " (Autorización pendiente)" : string.Empty);
      }
    }


    [DataField("BDG_ACCT_ORG_ID")]
    public Organization Organization {
      get; private set;
    }


    [DataField("BDG_ACCT_ORG_UNIT_ID")]
    public OrganizationalUnit OrganizationalUnit {
      get; private set;
    }


    [DataField("BDG_ACCT_BASE_SEGMENT_ID")]
    public BudgetAccountSegment BaseSegment {
      get; private set;
    }

    public string BudgetProgram {
      get {
        return ExtData.Get("budgetProgram", "N/D");
      }
      private set {
        ExtData.SetIfValue("budgetProgram", value);
      }
    }

    [DataField("BDG_ACCT_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("BDG_ACCT_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }

    [DataField("BDG_ACCT_EXT_DATA")]
    protected JsonObject ExtData {
      get; set;
    } = new JsonObject();


    [DataField("BDG_ACCT_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("BDG_ACCT_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.BaseSegment.Keywords,
                                           this.BudgetProgram, this.OrganizationalUnit.Keywords);
      }
    }


    [DataField("BDG_ACCT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BDG_ACCT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BDG_ACCT_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      BudgetAccountDataService.WriteBudgetAccount(this, ExtData.ToString());
    }


    internal void SetStatus(EntityStatus newStatus) {

      if (Status == EntityStatus.Pending && newStatus == EntityStatus.OnReview) {
        BudgetProgram = OrganizationalUnit.ExtendedData.Get("budgetProgram", "N/A");

      } else if (Status == EntityStatus.OnReview && newStatus == EntityStatus.Pending) {
        BudgetProgram = "N/D";

      }

      Status = newStatus;
    }

    #endregion Methods

  } // class BudgetAccount

} // namespace Empiria.Budgeting
