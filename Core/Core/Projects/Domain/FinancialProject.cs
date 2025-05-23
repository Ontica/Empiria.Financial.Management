/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information holder                      *
*  Type     : FinancialProject                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial project.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.StateEnums;
using Empiria.Parties;

using Empiria.Financial.Projects.Data;

namespace Empiria.Financial.Projects {

  /// <summary>Represents a financial project.</summary>
  public class FinancialProject : BaseObject, INamedEntity {

    #region Constructors and parsers

    private FinancialProject() {
      // Required by Empiria Framework
    }

    internal FinancialProject(OrganizationalUnit orgUnit, string name) {
      Assertion.Require(orgUnit, nameof(orgUnit));
      Assertion.Require(name, nameof(name));
      Assertion.Require(!orgUnit.IsEmptyInstance,
                        "orgUnit can not be the empty instance.");

      this.OrganizationalUnit = orgUnit;
      this.Name = name;
      this.ProjectNo = "N/D";
      this.StartDate = DateTime.Today;
    }

    static public FinancialProject Parse(int id) => ParseId<FinancialProject>(id);

    static public FinancialProject Parse(string uid) => ParseKey<FinancialProject>(uid);

    static public FinancialProject Empty => ParseEmpty<FinancialProject>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PRJ_STD_ACCT_ID")]
    public StandardAccount StandardAccount {
      get; private set;
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
        if (ProjectNo.Length != 0) {
          return $"({ProjectNo}) {Name} [{Program}]";
        }
        return $"{Name} [{Program}]";
      }
    }


    public string Program {
      get {
        return StandardAccount.Parse(StandardAccount.ParentId).Name;
      }
    }


    [DataField("PRJ_ORG_UNIT_ID")]
    public OrganizationalUnit OrganizationalUnit {
      get; private set;
    }


    [DataField("PRJ_IDENTIFIERS")]
    public string Identifiers {
      get; protected set;
    }


    [DataField("PRJ_TAGS")]
    public string Tags {
      get; protected set;
    }


    [DataField("PRJ_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(ProjectNo, Name, Identifiers, Tags, Program,
                                           OrganizationalUnit.Keywords, Category.Keywords,
                                           StandardAccount.Keywords);
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

    #endregion Properties

    #region Methods

    internal void SetParent(FinancialProject parent) {
      Assertion.Require(parent, nameof(parent));

      this.Parent = parent;
    }


    internal void Suspend() {
      Assertion.Require(Status == EntityStatus.Active,
                        $"Can not suspend project. Its status is {Status.GetName()}.");
      this.Status = EntityStatus.Suspended;
    }


    internal void Delete() {
      Assertion.Require(Status == EntityStatus.Pending,
                  $"Can not delete project. Its status is {Status.GetName()}.");
      this.Status = EntityStatus.Deleted;
    }


    internal static FixedList<FinancialProject> SearchProjects(string filter, string sort) {

      return FinancialProjectDataService.SearchProjects(filter, sort);
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.StartDate = ExecutionServer.DateMinValue;
        this.EndDate = ExecutionServer.DateMaxValue;
        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      FinancialProjectDataService.WriteProject(this, this.ExtData.ToString());
    }


    internal void Update(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.Name = PatchField(fields.Name, this.Name);
      this.ProjectNo = PatchField(fields.ProjectNo, this.ProjectNo);
      this.Category = PatchField(fields.CategoryUID, this.Category);
      this.StandardAccount = PatchField(fields.StandardAccountUID, this.StandardAccount);
      this.OrganizationalUnit = PatchField(fields.OrganizationUnitUID, this.OrganizationalUnit);
    }

    #endregion Methods

  } // class FinancialProject

} // namespace Empiria.Financial.Projects
