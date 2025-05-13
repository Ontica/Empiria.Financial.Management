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
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Data;


namespace Empiria.Financial {

  /// <summary>Represents a financial project.</summary>
  public class Project : BaseObject, INamedEntity {

    #region Constructors and parsers

    static public Project Parse(int id) => ParseId<Project>(id);

    static public Project Parse(string uid) => ParseKey<Project>(uid);   

    static public Project Empty => ParseEmpty<Project>();

    #endregion Constructors and parsers

    #region Properties       

    [DataField("PRJ_TYPE_ID")]
    public int ProjectTypeId {
      get; private set;
    }


    [DataField("PRJ_STD_ACCT_ID")]
    public int StandarAccount {
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
    private int ParentId {
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
    private int HistoricId {
      get; set;
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

    internal static FixedList<Project> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

       return ProjectDataService.SearchProjects(keywords);
    }

    internal static FixedList<Project> SearchProjects(string filter, string sort) {

      return ProjectDataService.SearchProjects(filter, sort);
    }

    #endregion Methods

  } // class Project

} // namespace Empiria.Financial
