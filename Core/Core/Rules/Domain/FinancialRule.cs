/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FinancialRule                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial rule.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Financial.Rules {

  /// <summary>Represents a financial rule.</summary>
  [PartitionedType(typeof(FinancialRuleType))]
  public class FinancialRule : BaseObject {

    #region Constructors and parsers

    protected FinancialRule(FinancialRuleType powertype) : base(powertype) {
      // Required by Empiria Framework
    }

    internal FinancialRule(FinancialRuleCategory category) : base(FinancialRuleType.Empty) {

    }

    static public FinancialRule Parse(int id) => ParseId<FinancialRule>(id);

    static public FinancialRule Parse(string uid) => ParseKey<FinancialRule>(uid);

    #endregion Constructors and parsers

    #region Properties

    public FinancialRuleType FinancialRuleType {
      get {
        return (FinancialRuleType) base.GetEmpiriaType();
      }
    }


    [DataField("RULE_CATEGORY_ID")]
    public FinancialRuleCategory Category {
      get; private set;
    }


    [DataField("RULE_GROUP_ID")]
    public int GroupId {
      get; private set;
    }


    [DataField("RULE_DEBIT_ACCOUNT")]
    public string DebitAccount {
      get; private set;
    }


    [DataField("RULE_DEBIT_CURRENCY_ID")]
    public Currency DebitCurrency {
      get; private set;
    }


    [DataField("RULE_CREDIT_ACCOUNT")]
    public string CreditAccount {
      get; private set;
    }


    [DataField("RULE_CREDIT_CURRENCY_ID")]
    public Currency CreditCurrency {
      get; private set;
    }


    [DataField("RULE_CONDITIONS")]
    protected internal JsonObject Conditions {
      get; private set;
    }


    [DataField("RULE_EXCEPTIONS")]
    protected internal JsonObject Exceptions {
      get; private set;
    }


    [DataField("RULE_EXT_DATA")]
    protected internal JsonObject ExtData {
      get; private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(DebitAccount, CreditAccount, FinancialRuleType.DisplayName, Category.Keywords);
      }
    }


    [DataField("RULE_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("RULE_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("RULE_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("RULE_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("RULE_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

  } // class FinancialRule

} // namespace Empiria.Financial.Rules
