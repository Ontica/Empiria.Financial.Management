/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : StandardAccount                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents an standard account.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Concepts;

namespace Empiria.Financial {

  /// <summary>Partitioned type that represents an standard account.</summary>
  [PartitionedType(typeof(StandardAccountType))]
  public class StandardAccount : BaseObject, INamedEntity {

    #region Fields

    static readonly private FixedList<StandardAccount> _cache;

    #endregion Fields

    #region Constructors and parsers

    static StandardAccount() {
      _cache = GetFullList<StandardAccount>("STD_ACCT_STATUS <> 'X'");
    }

    protected StandardAccount(StandardAccountType powertype) : base(powertype) {
      // Require by Empiria FrameWork
    }

    static public StandardAccount Parse(int id) => ParseId<StandardAccount>(id);

    static public StandardAccount Parse(string uid) => ParseKey<StandardAccount>(uid);

    static internal StandardAccount TryParseAccountNo(string stdAcctNo) {
      return _cache.Find(x => x.StdAcctNo == stdAcctNo);
    }

    static public StandardAccount Empty => ParseEmpty<StandardAccount>();

    #endregion Constructors and parsers

    #region Properties

    public StandardAccountType StandardAccountType {
      get {
        return (StandardAccountType) base.GetEmpiriaType();
      }
    }


    [DataField("STD_ACCT_CATEGORY_ID")]
    public StandardAccountCategory Category {
      get; private set;
    }


    [DataField("STD_ACCT_CHART_OF_ACCOUNTS_ID")]
    public ChartOfAccounts ChartOfAccounts {
      get; private set;
    }


    [DataField("STD_ACCT_NUMBER")]
    public string StdAcctNo {
      get; private set;
    }


    public string[] StdAcctSegments {
      get {
        return StdAcctNo.Split('.');
      }
    }


    [DataField("STD_ACCT_DESCRIPTION")]
    public string Description {
      get; private set;
    }

    public string Name {
      get {
        if (StdAcctNo.Length != 0) {
          return $"({StdAcctNo}) {Description}";
        }
        return Description;
      }
    }


    public string FullName {
      get {
        if (this.IsEmptyInstance) {
          return string.Empty;
        }
        if (Parent.FullName.Length == 0) {
          return Description;
        }
        return $"{Description} - {Parent.FullName}";
      }
    }


    [DataField("STD_ACCT_IDENTIFICATORS")]
    public string Identifiers {
      get; private set;
    }


    [DataField("STD_ACCT_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("STD_ACCT_ROLE_TYPE", Default = AccountRoleType.Undefined)]
    public AccountRoleType RoleType {
      get; private set;
    }


    [DataField("STD_ACCT_DEBTOR_CREDITOR_TYPE", Default = DebtorCreditorType.Undefined)]
    public DebtorCreditorType DebtorCreditorType {
      get; private set;
    }


    [DataField("STD_ACCT_RECORDING_RULE")]
    public string RecordingRule {
      get; private set;
    }


    [DataField("STD_ACCT_EXT_DATA")]
    public JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(StdAcctNo, Description, Identifiers, Tags, Category.Name);
      }
    }


    [DataField("STD_ACCT_PARENT_ID")]
    private int _parentId = -1;

    public StandardAccount Parent {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }
        return Parse(_parentId);
      }
      private set {
        if (this.IsEmptyInstance) {
          return;
        }
        _parentId = value.Id;
      }
    }


    [DataField("STD_ACCT_START_DATE", Default = "ExecutionServer.DateMinValue")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("STD_ACCT_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("STD_ACCT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("STD_ACCT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("STD_ACCT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }


    public int Level {
      get {
        if (this.IsEmptyInstance || Parent.IsEmptyInstance) {
          return 1;
        }
        return Parent.Level + 1;
      }
    }


    public bool IsLastLevel {
      get {
        return RoleType != AccountRoleType.Sumaria;
      }
    }

    [DataField("STD_ACCT_MAIN_CLASSIFICATION_ID")]
    public FinancialConcept MainClassification {
      get; private set;
    }

    #endregion Properties

    #region Methods

    private List<StandardAccount> _allChildren = null;
    internal FixedList<StandardAccount> GetAllChildren() {
      if (_allChildren != null) {
        return _allChildren.ToFixedList();
      }

      _allChildren = new List<StandardAccount>();
      foreach (var child in GetChildren()) {
        _allChildren.Add(child);
        _allChildren.AddRange(child.GetAllChildren());
      }

      return _allChildren.ToFixedList();
    }


    private FixedList<StandardAccount> _children = null;
    internal FixedList<StandardAccount> GetChildren() {
      if (this.IsEmptyInstance) {
        return new FixedList<StandardAccount>();
      }
      if (_children == null) {
        _children = ChartOfAccounts.GetStandardAccounts()
                                   .FindAll(x => x.Parent.Equals(this))
                                   .Sort((x, y) => x.StdAcctNo.CompareTo(y.StdAcctNo));
      }
      return _children;
    }


    internal FixedList<FinancialAccount> GetNonOperationAccounts() {
      return FinancialAccount.GetList()
                             .FindAll(x => x.StandardAccount.StdAcctNo.StartsWith(StdAcctNo) &&
                                          !x.IsOperationAccount)
                             .Sort((x, y) => $"{x.Project.ProjectNo}|{x.AccountNo}".CompareTo($"{y.Project.ProjectNo}|{y.AccountNo}"));
    }

    #endregion Methods

  } // class StandardAccount

} // namespace Empiria.Financial
