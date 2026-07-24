/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial cost object                      Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : FinancialCostObjects                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial cost objects.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

using Empiria.Financial.CostObject.Data;
using Empiria.Json;
using Empiria.Parties;

namespace Empiria.Financial {

  /// <summary>Represents a financial cost objects.</summary>
  public class FinancialCostObject : BaseObject {

    #region Constructors and parsers

    private FinancialCostObject() {
      // Require by Empiria FrameWork
    }

    static public FinancialCostObject Parse(int id) => ParseId<FinancialCostObject>(id);

    static public FinancialCostObject Parse(string uid) => ParseKey<FinancialCostObject>(uid);

    static public FinancialCostObject Empty => ParseEmpty<FinancialCostObject>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("COBJ_TYPE_ID")]
    public FinancialCostObjectType CostType {
      get; private set;
    }

    [DataField("COBJ_EXTERNAL_NO")]
    public string ExternalCode {
      get; private set;
    }

    [DataField("COBJ_DESCRIPTION")]
    public string Description {
      get; private set;
    }

    [DataField("COBJ_EXT_DATA")]
    internal JsonObject ExtData {
      get; private set;
    }

    public string Keywords {
      get {
        return  EmpiriaString.BuildKeywords(ExternalCode, Description);
      }
    }

    [DataField("COBJ_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }

    [DataField("COBJ_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }

    [DataField("COBJ_HISTORIC_ID")]
    internal int HistoricId {
      get; private set;
    }

    [DataField("COBJ_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }

    [DataField("COBJ_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }

    [DataField("COBJ_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    static public FinancialCostObject Create(FinancialCostObjectEntry entry) {
      Assertion.Require(entry, nameof(entry));

      var costObject = new FinancialCostObject {
        CostType = FinancialCostObjectType.Parse(entry.CostObjectTypeId),
        ExternalCode = entry.ExternalCode,
        Description = entry.Description,
        StartDate = entry.StartDate,
        EndDate = ExecutionServer.DateMaxValue
      };

      return costObject;
    }

    internal void Update(FinancialCostObjectEntry entry) {

      Description = entry.Description;
      EndDate = entry.EndDate ?? EndDate;
    }

    internal void Delete() {
      Status = EntityStatus.Deleted;
      EndDate = DateTime.Today;
    }

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      FinancialCostObjectData.Write(this);
    }

    #endregion Methods

  } // class FinancialCostObjects

} // namespace Empiria.Financial.CostObjects
