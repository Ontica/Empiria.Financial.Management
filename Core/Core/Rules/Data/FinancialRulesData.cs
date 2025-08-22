/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Data Layer                              *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Data Service                            *
*  Type     : FinancialRulesData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial rules.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

using Empiria.Financial.Integration;

namespace Empiria.Financial.Rules.Data {

  /// <summary>Provides data access services for financial rules.</summary>
  static internal class FinancialRulesData {

    #region Methods

    static internal void CleanFinancialRule(FinancialRule rule) {
      if (rule.IsEmptyInstance) {
        return;
      }

      string debitAccount = IntegrationLibrary.FormatAccountNumber(rule.DebitAccount);
      string creditAccount = IntegrationLibrary.FormatAccountNumber(rule.CreditAccount);

      var sql = "UPDATE FMS_RULES " +
                $"SET RULE_UID = '{System.Guid.NewGuid().ToString()}', " +
                $"RULE_DEBIT_ACCOUNT = '{debitAccount}', " +
                $"RULE_CREDIT_ACCOUNT = '{creditAccount}', " +
                $"RULE_KEYWORDS = '{rule.Keywords}' " +
                $"WHERE RULE_ID = {rule.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal FixedList<FinancialRule> GetFinancialRules(FinancialRuleCategory category) {
      var sql = "SELECT * FROM FMS_RULES " +
                $"WHERE RULE_CATEGORY_ID = {category.Id} AND RULE_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialRule>(op);
    }

    #endregion Methods

  }  // class FinancialRulesData

}  // namespace Empiria.Financial.Rules.Data
