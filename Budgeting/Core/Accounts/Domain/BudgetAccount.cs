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
        return $"[{Segment_2.Code}] - {Segment_2.Name}";
      }
    }

    [DataField("BDG_ACCT_SEGMENT_01_ID")]
    private BudgetAccountSegment Segment_1 {
      get; set;
    }

    [DataField("BDG_ACCT_SEGMENT_02_ID")]
    private BudgetAccountSegment Segment_2 {
      get; set;
    }

    [DataField("BDG_ACCT_SEGMENT_03_ID")]
    private BudgetAccountSegment Segment_3 {
      get; set;
    }

    [DataField("BDG_ACCT_SEGMENT_04_ID")]
    private BudgetAccountSegment Segment_4 {
      get; set;
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
