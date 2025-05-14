/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Base Object                             *
*  Type     : FinancialProject                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial project.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.StateEnums;
using Empiria.Parties;

using Empiria.Financial.Projects.Adapters;
using Empiria.Financial.Projects.Data;

namespace Empiria.Financial {

  /// <summary>Represents a financial project.</summary>
  public class FinancialProject : BaseObject, INamedEntity {
    //private ProjectFields fields;

    #region Constructors and parsers

    public FinancialProject() {
      // Require by Empiria FrameWork
    }

    static public FinancialProject Parse(int id) => ParseId<FinancialProject>(id);

    static public FinancialProject Parse(string uid) => ParseKey<FinancialProject>(uid);

    static public FinancialProject Empty => ParseEmpty<FinancialProject>();

    public FinancialProject(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));
      Update(fields);
    }

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


    [DataField("PRJ_ORG_UNIT_ID")]
    public Party OrganizationUnit {
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
        return EmpiriaString.BuildKeywords(ProjectNo, Name);
      }
    }


    [DataField("PRJ_PARENT_ID")]
    public int ParentId {
      get; set;
    } = -1;


    [DataField("PRJ_START_DATE", Default = "ExecutionServer.DateMinValue")]
    public DateTime StartDate {
      get; protected set;
    }


    [DataField("PRJ_END_DATE")]
    public DateTime EndDate {
      get; protected set;
    }


    [DataField("PRJ_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("PRJ_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("PRJ_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    internal static FixedList<FinancialProject> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      return FinancialProjectDataService.SearchProjects(keywords);
    }


    internal static FixedList<FinancialProject> SearchProjects(string filter, string sort) {

      return FinancialProjectDataService.SearchProjects(filter, sort);
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.StartDate = ExecutionServer.DateMinValue;
        this.EndDate = ExecutionServer.DateMaxValue;

        this.ParentId = -1;

        this.PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        this.PostingTime = DateTime.Now;
      }

      FinancialProjectDataService.WriteProject(this, this.ExtData.ToString());
    }


    internal void Update(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();
     
      this.Category = FinancialProjectCategory.Parse(fields.CategoryUID);
      this.StandardAccount = StandardAccount.Parse(fields.StandardAccountUID);
      this.ProjectNo = fields.ProjectNo;
      this.Name = fields.Name;
      this.OrganizationUnit = Party.Parse(fields.OrganizationUnitUID);
    }

    #endregion Methods

  } // class FinancialProject

} // namespace Empiria.Financial
