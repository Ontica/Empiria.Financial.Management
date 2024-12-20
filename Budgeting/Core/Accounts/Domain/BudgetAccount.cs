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

namespace Empiria.Budgeting {

  /// <summary>Partitioned type that represents a budget account.</summary>
  [PartitionedType(typeof(BudgetAccountType))]
  public class BudgetAccount : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected BudgetAccount(BudgetAccountType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
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
      get;
      private set;
    }


    [DataField("BDG_ACCT_CODE")]
    public string Code {
      get;
      private set;
    }


    public string Name {
      get {
        return $"[{BaseSegment.Code}] - {BaseSegment.Name} ({OrganizationalUnit.Code})";
      }
    }

    [DataField("BDG_ACCT_ORG_ID")]
    internal Organization Organization {
      get; private set;
    }


    [DataField("BDG_ACCT_ORG_UNIT_ID")]
    internal OrganizationalUnit OrganizationalUnit {
      get; private set;
    }


    [DataField("BDG_ACCT_BASE_SEGMENT_ID")]
    internal BudgetAccountSegment BaseSegment {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_01_ID")]
    internal BudgetAccountSegment ClassSegment_1 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_02_ID")]
    internal BudgetAccountSegment ClassSegment_2 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_03_ID")]
    internal BudgetAccountSegment ClassSegment_3 {
      get; private set;
    }

    [DataField("BDG_ACCT_CLASS_SEGMENT_04_ID")]
    internal BudgetAccountSegment ClassSegment_4 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_05_ID")]
    internal BudgetAccountSegment ClassSegment_5 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_06_ID")]
    internal BudgetAccountSegment ClassSegment_6 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_07_ID")]
    internal BudgetAccountSegment ClassSegment_7 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_08_ID")]
    internal BudgetAccountSegment ClassSegment_8 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_09_ID")]
    internal BudgetAccountSegment ClassSegment_9 {
      get; private set;
    }

    [DataField("BDG_ACCT_CLASS_SEGMENT_10_ID")]
    internal BudgetAccountSegment ClassSegment_10 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_11_ID")]
    internal BudgetAccountSegment ClassSegment_11 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_12_ID")]
    internal BudgetAccountSegment ClassSegment_12 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_13_ID")]
    internal BudgetAccountSegment ClassSegment_13 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_14_ID")]
    internal BudgetAccountSegment ClassSegment_14 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_15_ID")]
    internal BudgetAccountSegment ClassSegment_15 {
      get; private set;
    }


    [DataField("BDG_ACCT_CLASS_SEGMENT_16_ID")]
    internal BudgetAccountSegment ClassSegment_16 {
      get; private set;
    }


    [DataField("BDG_ACCT_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return _identificators.Split(' ').ToFixedList();
      }
    }


    [DataField("BDG_ACCT_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return _tags.Split(' ').ToFixedList();
      }
    }

    [DataField("BDG_ACCT_EXT_DATA")]
    private JsonObject ExtData {
      get;
      set;
    } = new JsonObject();


    [DataField("BDG_ACCT_START_DATE")]
    public DateTime StartDate {
      get;
      private set;
    }

    [DataField("BDG_ACCT_END_DATE")]
    public DateTime EndDate {
      get;
      private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name);
      }
    }


    [DataField("BDG_ACCT_POSTED_BY_ID")]
    public Party PostedById {
      get;
      private set;
    }


    [DataField("BDG_ACCT_POSTING_TIME")]
    public DateTime PostingTime {
      get;
      private set;
    }


    [DataField("BDG_ACCT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      private set;
    }

    #endregion Properties

  } // class BudgetAccount

} // namespace Empiria.Budgeting
