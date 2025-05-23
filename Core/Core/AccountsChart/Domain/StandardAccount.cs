/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : AccountsChart                              Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : StandardAccount                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a standard account.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.StateEnums;

namespace Empiria.Financial {

  /// <summary>Represents a standard account.</summary>
  public class StandardAccount : BaseObject, INamedEntity {
    //private ProjectFields fields;

    #region Constructors and parsers

    public StandardAccount() {
      // Require by Empiria FrameWork
    }

    static public StandardAccount Parse(int id) => ParseId<StandardAccount>(id);

    static public StandardAccount Parse(string uid) => ParseKey<StandardAccount>(uid);

    static public StandardAccount Empty => ParseEmpty<StandardAccount>();

    #endregion Constructors and parsers

    #region Properties

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
    public int ParentId {
      get; set;
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

    #endregion Properties

    #region Methods

    #endregion Methods

  } // class StandardAccount

} // namespace Empiria.Financial
