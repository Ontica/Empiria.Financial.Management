/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder, Aggregate root      *
*  Type     : FinancialRuleCategory                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial rule category that serves as an aggregate root for FinancialRules.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.DynamicData;

using Empiria.Financial.Rules.Data;

namespace Empiria.Financial.Rules {

  /// <summary>Represents a financial rule category that serves as an aggregate root for FinancialRules.</summary>
  public class FinancialRuleCategory : CommonStorage {

    #region Fields

    private Lazy<FixedList<FinancialRule>> _rules = new Lazy<FixedList<FinancialRule>>();

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

    public FixedList<FinancialRule> GetFinancialRules() {
      return _rules.Value;
    }


    public FixedList<FinancialRule> GetFinancialRules(DateTime date) {
      return _rules.Value.FindAll(x => x.StartDate <= date && date <= x.EndDate);
    }

    #endregion Methods

    #region Helpers

    internal FixedList<DataTableColumn> GetDataColumns() {
      return ExtData.GetFixedList<DataTableColumn>("dataColumns", false);
    }

    private void Refesh() {
      _rules = new Lazy<FixedList<FinancialRule>>(() => FinancialRulesData.GetFinancialRules(this));
    }

    #endregion Helpers

  } // class FinancialRuleCategory

} // namespace Empiria.Financial.Rules
