/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects                                   Component : Domain Layer                            *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Partitioned Type                        *
*  Type     : Project                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial project.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Financial.Data;
using Empiria.Financial.Projects.Adapters;
using Empiria.Contacts;
using Empiria.Parties;


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

    public FinancialProject(ProjectFields fields) {
      Assertion.Require(fields, nameof(fields));
      Update(fields);
    }

    #endregion Constructors and parsers

    #region Properties       

    [DataField("PRJ_TYPE_ID")]
    public int ProjectTypeId {
      get; private set;
    }


    [DataField("PRJ_STD_ACCT_ID")]
    public StandardAccount StandarAccount {
      get; private set;
    }


    [DataField("PRJ_CATEGORY_ID")]
    public int CategoryId {
      get; private set;
    }


    [DataField("PRJ_NO")]
    public string PrjNo {
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
        return EmpiriaString.BuildKeywords(PrjNo, Name);
      }
    }


    [DataField("PRJ_PARENT_ID")]
    public int ParentId {
      get; set;
    }


    [DataField("PRJ_START_DATE", Default = "ExecutionServer.DateMinValue")]
    public DateTime StartDate {
      get; protected set;
    }


    [DataField("PRJ_END_DATE")]
    public DateTime EndDate {
      get; protected set;
    }


    [DataField("PRJ_HISTORIC_ID")]
    public int HistoricId {
      get; set;
    }


    [DataField("PRJ_POSTED_BY_ID")]
    public Contact PostedBy {
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

    internal static FixedList<FinancialProject> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      return ProjectDataService.SearchProjects(keywords);
    }

    internal static FixedList<FinancialProject> SearchProjects(string filter, string sort) {

      return ProjectDataService.SearchProjects(filter, sort);
    }

    internal void Update(ProjectFields fields) {
      Assertion.Require(fields, nameof(fields));
      fields.EnsureValid();

      this.ProjectTypeId = fields.TypeId;
      this.CategoryId = fields.CategoryId;
      this.StandarAccount = StandardAccount.Parse(fields.StandarAccountUID);
      this.PrjNo = fields.PrjNo;
      this.Name = fields.Name;
      this.OrganizationUnit = Party.Parse(fields.OrganizationUnitUID);
      this.Identifiers = string.Empty;
      this.Tags = string.Empty;
      this.ExtData = JsonObject.Empty;
      this.ParentId = -1;
      this.StartDate = ExecutionServer.DateMinValue;
      this.EndDate = ExecutionServer.DateMaxValue;
      this.HistoricId = 1;
      this.PostedBy = ExecutionServer.CurrentContact;
      this.PostingTime = DateTime.Now;
      this.Status = fields.Status;
    }

    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }

      ProjectDataService.WriteProject(this, this.ExtData.ToString());
    }

    internal void Delete() {
      this.Status = EntityStatus.Deleted;
    }

    #endregion Methods

  } // class Project

} // namespace Empiria.Financial
