/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Aggregate root                          *
*  Type     : FinancialRuleCategory                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial rule category that serves as an aggregate root for FinancialRules.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.Financial.Rules.Data;

namespace Empiria.Financial.Rules {

  /// <summary>Represents a financial rule category that serves as an aggregate root for FinancialRules.</summary>
  public class FinancialRuleCategory : CommonStorage {

    #region Fields

    private Lazy<List<FinancialRule>> _rules = new Lazy<List<FinancialRule>>();

    #endregion Fields

    #region Constructors and parsers

    protected FinancialRuleCategory() {
      // Required by Empiria Framework
    }

    static public FinancialRuleCategory Parse(int id) => ParseId<FinancialRuleCategory>(id);

    static public FinancialRuleCategory Parse(string uid) => ParseKey<FinancialRuleCategory>(uid);

    static public FinancialRuleCategory ParseNamedKey(string namedKey) => ParseNamedKey<FinancialRuleCategory>(namedKey);

    static public FinancialRuleCategory Empty => ParseEmpty<FinancialRuleCategory>();

    static public FixedList<FinancialRuleCategory> GetList() {
      return GetStorageObjects<FinancialRuleCategory>();
    }

    protected override void OnLoad() {
      Refesh();
    }

    #endregion Constructors and parsers

    #region Properties

    public FinancialRuleType FinancialRuleType {
      get {
        int id = base.ExtData.Get<int>("ruleTypeId");

        return FinancialRuleType.Parse(id);
      }
    }


    public bool IsSingleEntry {
      get {
        return base.ExtData.Get("isSingleEntry", false);
      }
    }


    public new string NamedKey {
      get {
        return base.NamedKey;
      }
    }

    #endregion Properties

    #region Methods

    internal FinancialRule AddRule(FinancialRuleFields fields) {
      Assertion.Require(fields, nameof(fields));

      var rule = new FinancialRule(this);

      rule.Update(fields);

      _rules.Value.Add(rule);

      return rule;
    }


    public FixedList<FinancialRule> GetFinancialRules() {
      return _rules.Value.ToFixedList();
    }


    public FixedList<FinancialRule> GetFinancialRules(DateTime date) {
      return _rules.Value.FindAll(x => x.StartDate <= date && date <= x.EndDate)
                         .ToFixedList();
    }


    internal void RemoveRule(FinancialRule rule) {
      Assertion.Require(rule, nameof(rule));
      Assertion.Require(_rules.Value.Contains(rule), "rule does not belong to this category.");

      rule.Delete();

      _rules.Value.Remove(rule);
    }


    internal void UpdateRule(FinancialRule rule, FinancialRuleFields fields) {
      Assertion.Require(rule, nameof(rule));
      Assertion.Require(_rules.Value.Contains(rule), "rule does not belong to this category.");
      Assertion.Require(fields, nameof(fields));

      rule.Update(fields);
    }

    #endregion Methods

    #region Helpers

    internal FixedList<DataTableColumn> GetDataColumns() {
      return ExtData.GetFixedList<DataTableColumn>("dataColumns", false);
    }

    private void Refesh() {
      _rules = new Lazy<List<FinancialRule>>(() => FinancialRulesData.GetFinancialRules(this));
    }

    #endregion Helpers

  } // class FinancialRuleCategory

} // namespace Empiria.Financial.Rules
