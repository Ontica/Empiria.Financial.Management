/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts                                   Component : Domain Layer                            *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Partitioned Type                        *
*  Type     : Account                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial account.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Financial.Data;
using Empiria.Contacts;
using Empiria.Parties;


namespace Empiria.Financial {

  /// <summary>Represents a financial account.</summary>
  public class Account : BaseObject, INamedEntity {

    #region Constructors and parsers

    public Account() {
      // Require by Empiria FrameWork
    }

    static public Account Parse(int id) => ParseId<Account>(id);

    static public Account Parse(string uid) => ParseKey<Account>(uid);

    static public Account Empty => ParseEmpty<Account>();


    #endregion Constructors and parsers

    #region Properties       

    [DataField("ACCT_ID")]
    public int AcctId {
      get; private set;
    }


    [DataField("ACCT_TYPE_ID")]
    public int TypeId {
      get; private set;
    }


    [DataField("ACCT_STD_ACCT_ID")]
    public int StdAcctId {
      get; private set;
    }


    [DataField("ACCT_ORG_ID")]
    public Contact OrganizationUnit {
      get; private set;
    }


    [DataField("ACCT_PARTY_ID")]
    public Party Party {
      get; private set;
    }


    [DataField("ACCT_PROJECT_ID")]
    public FinancialProject Project {
      get; private set;
    }

    
    [DataField("ACCT_LEDGER_ID")]
    public int LedgerId {
      get; private set;
    }

    [DataField("ACCT_NUMBER")]
    public string AcctNo {
      get; private set;
    }

    
    [DataField("ACCT_DESCRIPTION")]
    public string Name {
      get; private set;
    }

    [DataField("ACCT_IDENTIFIERS")]
    public string Identifiers {
      get; protected set;
    }


    [DataField("ACCT_TAGS")]
    public string Tags {
      get; protected set;
    }

  
    [DataField("ACCT_ATTRIBUTES")]
    public string Attributes {
      get; protected set;
    }


    [DataField("ACCT_FINANCIAL_DATA")]
    public string FinancialData {
      get; protected set;
    }


    [DataField("ACCT_CONFIG_DATA")]
    public string ConfigData {
      get; protected set;
    }

    
    [DataField("ACCT_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(AcctNo, Name);
      }
    }


    [DataField("ACCT_PARENT_ID")]
    public int ParentId {
      get; set;
    }


    [DataField("ACCT_START_DATE", Default = "ExecutionServer.DateMinValue")]
    public DateTime StartDate {
      get; protected set;
    }


    [DataField("ACCT_END_DATE")]
    public DateTime EndDate {
      get; protected set;
    }


    [DataField("ACCT_HISTORIC_ID")]
    public int HistoricId {
      get; set;
    }


    [DataField("ACCT_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("ACCT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("ACCT_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }


    #endregion Properties

    #region Methods

    internal static FixedList<Account> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      return AccountDataService.SearchAccount(keywords);
    }

    internal static FixedList<Account> SearchAccount(string filter, string sort) {

      return AccountDataService.SearchAccount(filter, sort);
    }


    #endregion Methods

  } // class Account

} // namespace Empiria.Financial
