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

    internal FinancialProject(FinancialProjectCategory category, OrganizationalUnit baseOrgUnit,
                              INamedEntity subprogram, string name) {
      Assertion.Require(category, nameof(category));
      Assertion.Require(!category.IsEmptyInstance, nameof(category));
      Assertion.Require(baseOrgUnit, nameof(baseOrgUnit));
      Assertion.Require(!baseOrgUnit.IsEmptyInstance, nameof(baseOrgUnit));
      Assertion.Require(subprogram, nameof(subprogram));
      Assertion.Require(name, nameof(name));

      Category = category;
      BaseOrgUnit = baseOrgUnit;
      _subprogram = StandardAccount.Parse(subprogram.UID);

      Name = EmpiriaString.Clean(name);
      ProjectNo = "N/D";
      StartDate = DateTime.Today;
    }

    static public FinancialProject Parse(int id) => ParseId<FinancialProject>(id);

    static public FinancialProject Parse(string uid) => ParseKey<FinancialProject>(uid);

    static public FinancialProject Empty => ParseEmpty<FinancialProject>();


    static internal FixedList<INamedEntity> GetClassificationList(FinancialProjectClassificator classificator) {
      var category = CommonStorage.ParseNamedKey<StandardAccountCategory>(classificator.ToString());

      return category.GetStandardAccounts()
                     .Select(x => (INamedEntity) x)
                     .ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties


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
          return $"({ProjectNo}) {Name} [{Program.Name}]";
        }
        return $"{Name} [{Program.Name}]";
      }
    }


    public INamedEntity Program {
      get {
        return StandardAccount.Parse(_subprogram.ParentId);
      }
    }


    [DataField("PRJ_STD_ACCT_ID")]
    private StandardAccount _subprogram = StandardAccount.Empty;

    public INamedEntity Subprogram {
      get {
        return _subprogram;
      }
    }


    [DataField("PRJ_BASE_ORG_UNIT_ID")]
    public Party BaseOrgUnit {
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
        return EmpiriaString.BuildKeywords(ProjectNo, Name, Identifiers, Tags, Program.Name,
                                           BaseOrgUnit.Keywords, Category.Keywords,
                                           _subprogram.Keywords);
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

    internal FinancialProjectRules Rules {
      get {
        return new FinancialProjectRules(this);
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      Assertion.Require(Status == EntityStatus.Pending,
                        $"Can not delete project. Its status is {Status.GetName()}.");
      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        StartDate = ExecutionServer.DateMinValue;
        EndDate = ExecutionServer.DateMaxValue;
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      FinancialProjectDataService.WriteProject(this, _subprogram, this.ExtData.ToString());
    }


    internal void SetParent(FinancialProject parent) {
      Assertion.Require(parent, nameof(parent));

      Parent = parent;
    }


    internal void Suspend() {
      Assertion.Require(Status == EntityStatus.Active,
                        $"Can not suspend project. Its status is {Status.GetName()}.");
      this.Status = EntityStatus.Suspended;
    }


    internal void Update(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Category = PatchField(fields.CategoryUID, this.Category);
      _subprogram = PatchField(fields.SubprogramUID, _subprogram);
      Name = PatchField(fields.Name, this.Name);
      ProjectNo = PatchField(fields.ProjectNo, this.ProjectNo);
      BaseOrgUnit = PatchField(fields.BaseOrgUnitUID, this.BaseOrgUnit);
    }

    #endregion Methods

  } // class FinancialProject

} // namespace Empiria.Financial.Projects
