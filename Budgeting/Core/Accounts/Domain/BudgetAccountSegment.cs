﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : BudgetAccountSegment                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type of BudgetAccountSegmentType that holds data for a budget account segment.     *
*             Budget account segments can be functional or administrative areas, development programs,       *
*             projects, budget concepts or units, activities, financial sources, geographic regions, etc.    *
*             BudgetAccountSegment instances are the constitutive parts of budget accounts.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting.Data;

namespace Empiria.Budgeting {

  /// <summary>Partitioned type of BudgetAccountSegmentType that holds data for a budget account segment.
  /// Budget account segments can be functional or administrative areas, development programs, projects,
  /// budget concepts or units, activities, financial sources, geographic regions, etcetera.
  /// BudgetAccountSegment instances are the constitutive parts of budget accounts.</summary>
  [PartitionedType(typeof(BudgetAccountSegmentType))]
  public class BudgetAccountSegment : BaseObject, INamedEntity {

    #region Fields

    private Lazy<FixedList<BudgetAccountSegment>> _children;

    #endregion Fields

    #region Constructors and parsers

    protected BudgetAccountSegment(BudgetAccountSegmentType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public BudgetAccountSegment Parse(int id) => ParseId<BudgetAccountSegment>(id);

    static public BudgetAccountSegment Parse(string uid) => ParseKey<BudgetAccountSegment>(uid);

    static public BudgetAccountSegment Empty => ParseEmpty<BudgetAccountSegment>();

    #endregion Constructors and parsers

    #region Properties

    public BudgetAccountSegmentType BudgetSegmentType {
      get {
        return (BudgetAccountSegmentType) base.GetEmpiriaType();
      }
    }


    [DataField("BDG_ACCT_SEGMENT_CODE")]
    public string Code {
      get; protected set;
    }


    [DataField("BDG_ACCT_SEGMENT_NAME")]
    public string Name {
      get; protected set;
    }

    string INamedEntity.Name {
      get {
        return FullName;
      }
    }

    public string FullName {
      get {
        return $"{Code} - {Name}";
      }
    }

    [DataField("BDG_ACCT_SEGMENT_DESCRIPTION")]
    public string Description {
      get; protected set;
    }


    [DataField("BDG_ACCT_SEGMENT_PARENT_ID", Default = -1)]
    private int ParentId {
      get; set;
    } = -1;


    public BudgetAccountSegment Parent {
      get {
        if (this.IsEmptyInstance || this.ParentId == this.Id) {
          return this;
        }
        return BudgetAccountSegment.Parse(this.ParentId);
      }
    }


    public bool HasParent {
      get {
        return !Parent.IsEmptyInstance && Parent.Distinct(this);
      }
    }


    [DataField("BDG_ACCT_SEGMENT_EXT_DATA")]
    internal JsonObject ExtensionData {
      get; set;
    } = new JsonObject();


    [DataField("BDG_ACCT_SEGMENT_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("BDG_ACCT_SEGMENT_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("BDG_ACCT_SEGMENT_EXT_OBJECT_ID")]
    public int ExternalObjectReferenceId {
      get; private set;
    }


    [DataField("BDG_ACCT_SEGMENT_HISTORIC_ID")]
    public int HistoryId {
      get; private set;
    }


    internal protected virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.Description);
      }
    }


    [DataField("BDG_ACCT_SEGMENT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BDG_ACCT_SEGMENT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BDG_ACCT_SEGMENT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    public FixedList<BudgetAccountSegment> Children {
      get {
        return _children.Value;
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnLoad() {
      if (this.BudgetSegmentType.HasChildrenSegmentType) {
        _children = new Lazy<FixedList<BudgetAccountSegment>>(() => BudgetAccountSegmentDataService.BudgetAccountChildrenSegments(this));
      } else {
        _children = new Lazy<FixedList<BudgetAccountSegment>>(() => new FixedList<BudgetAccountSegment>());
      }
    }

    protected override void OnSave() {
      BudgetAccountSegmentDataService.Write(this);
    }

    #endregion Methods

  } // class BudgetAccountSegment

} // namespace Empiria.Budgeting
