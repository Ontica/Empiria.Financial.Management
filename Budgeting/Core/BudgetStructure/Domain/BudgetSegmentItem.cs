/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Partitioned Type | Information Holder   *
*  Type     : BudgetSegmentItem                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type of BudgetSegmentType that holds data for a budget segment item.               *
*             Budget segment items can be functional or administrative areas, development programs,          *
*             projects, budget concepts or units, activities, financial sources, geographic regions, etc.    *
*             BudgetSegmentItem instances are the constitutive parts of budget accounts.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

using Empiria.Budgeting.Data;

namespace Empiria.Budgeting {

  /// <summary>Partitioned type of BudgetSegmentType that holds data for a budget segment item.
  /// Budget segment items can be functional or administrative areas, development programs, projects,
  /// budget concepts or units, activities, financial sources, geographic regions, etcetera.
  /// BudgetSegmentItem instances are the constitutive parts of budget accounts.</summary>
  [PartitionedType(typeof(BudgetSegmentType))]
  public class BudgetSegmentItem : BaseObject, INamedEntity {

    #region Fields

    private Lazy<FixedList<BudgetSegmentItem>> _children;

    #endregion Fields

    #region Constructors and parsers

    protected BudgetSegmentItem(BudgetSegmentType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public BudgetSegmentItem Parse(int id) => BaseObject.ParseId<BudgetSegmentItem>(id);

    static public BudgetSegmentItem Parse(string uid) => BaseObject.ParseKey<BudgetSegmentItem>(uid);

    static public BudgetSegmentItem Empty => BaseObject.ParseEmpty<BudgetSegmentItem>();

    #endregion Constructors and parsers

    #region Properties

    public BudgetSegmentType BudgetSegmentType {
      get {
        return (BudgetSegmentType) base.GetEmpiriaType();
      }
    }


    [DataField("BDG_SEG_ITEM_CODE")]
    public string Code {
      get; protected set;
    }


    [DataField("BDG_SEG_ITEM_NAME")]
    public string Name {
      get; protected set;
    }


    [DataField("BDG_SEG_ITEM_DESCRIPTION")]
    public string Description {
      get; protected set;
    }


    [DataField("BDG_SEG_ITEM_PARENT_ID", Default = -1)]
    private int ParentId {
      get; set;
    } = -1;


    public BudgetSegmentItem Parent {
      get {
        if (this.IsEmptyInstance || this.ParentId == this.Id) {
          return this;
        }
        return BudgetSegmentItem.Parse(this.ParentId);
      }
    }


    public bool HasParent {
      get {
        return !Parent.IsEmptyInstance && Parent.Distinct(this);
      }
    }


    [DataField("BDG_SEG_ITEM_EXT_DATA")]
    internal JsonObject ExtensionData {
      get; set;
    } = new JsonObject();


    [DataField("BDG_SEG_ITEM_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("BDG_SEG_ITEM_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("BDG_SEG_ITEM_EXT_OBJECT_ID")]
    public int ExternalObjectReferenceId {
      get; private set;
    }

    [DataField("BDG_SEG_ITEM_HISTORIC_ID")]
    public int HistoryId {
      get; private set;
    }

    internal protected virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.Description);
      }
    }


    [DataField("BDG_SEG_ITEM_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("BDG_SEG_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BDG_SEG_ITEM_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    public FixedList<BudgetSegmentItem> Children {
      get {
        return _children.Value;
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnLoad() {
      if (this.BudgetSegmentType.HasChildrenSegmentType) {
        _children = new Lazy<FixedList<BudgetSegmentItem>>(() => BudgetSegmentItemsDataService.SegmentItemChildren(this));
      } else {
        _children = new Lazy<FixedList<BudgetSegmentItem>>(() => new FixedList<BudgetSegmentItem>());
      }
    }

    protected override void OnSave() {
      BudgetSegmentItemsDataService.Write(this);
    }

    #endregion Methods

  } // class BudgetSegmentItem

} // namespace Empiria.Budgeting
