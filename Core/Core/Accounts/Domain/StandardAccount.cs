/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : AccountsChart                              Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : StandardAccount                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents an standard account.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Financial {

  /// <summary>Partitioned type that represents an standard account.</summary>
  [PartitionedType(typeof(StandardAccountType))]
  public class StandardAccount : BaseObject, INamedEntity {
    //private ProjectFields fields;

    #region Constructors and parsers

    protected StandardAccount(StandardAccountType powertype) : base(powertype) {
      // Require by Empiria FrameWork
    }

    static public StandardAccount Parse(int id) => ParseId<StandardAccount>(id);

    static public StandardAccount Parse(string uid) => ParseKey<StandardAccount>(uid);

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


    [DataField("STD_ACCT_CATALOGUE_ID")]
    public StandardAccountsCatalogue Catalogue {
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
      get; protected set;
    }


    [DataField("STD_ACCT_TAGS")]
    public string Tags {
      get; protected set;
    }


    [DataField("STD_ACCT_ROLE")]
    public string Role {
      get; protected set;
    }


    [DataField("STD_ACCT_DEBTOR")]
    public string Debtor {
      get; protected set;
    }


    [DataField("STD_ACCT_RECORDING_RULE")]
    public string RecordingRule {
      get; protected set;
    }


    [DataField("STD_ACCT_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
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
      get; protected set;
    }


    [DataField("STD_ACCT_END_DATE")]
    public DateTime EndDate {
      get; protected set;
    }


    [DataField("STD_ACCT_POSTED_BY_ID")]
    public Contact PostedBy {
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

    private FixedList<StandardAccount> _allChildren = null;
    internal FixedList<StandardAccount> GetAllChildren() {
      if (_allChildren == null) {
        _allChildren = GetFullList<StandardAccount>()
                      .ToFixedList()
                      .FindAll(x => x.StdAcctNo.StartsWith($"{this.StdAcctNo}.") &&
                                    x.Catalogue.Equals(Catalogue) &&
                                   !x.IsEmptyInstance)
                                   .Sort((x, y) => x.StdAcctNo.CompareTo(y.StdAcctNo)
                      );
      }
      return _allChildren;
    }


    internal FixedList<StandardAccount> GetChildren() {
      return GetAllChildren()
            .FindAll(x => x.Parent.Equals(this));
    }

    #endregion Properties

  } // class StandardAccount

} // namespace Empiria.Financial
