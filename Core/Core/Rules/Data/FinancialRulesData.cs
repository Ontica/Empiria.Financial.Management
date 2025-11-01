/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Data Layer                              *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Data Service                            *
*  Type     : FinancialRulesData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial rules.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

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
                $"RULE_DEBIT_CONCEPT = '{EmpiriaString.Clean(rule.DebitConcept)}', " +
                $"RULE_CREDIT_CONCEPT = '{EmpiriaString.Clean(rule.CreditConcept)}', " +
                $"RULE_KEYWORDS = '{rule.Keywords}' " +
                $"WHERE RULE_ID = {rule.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal List<FinancialRule> GetFinancialRules(FinancialRuleCategory category) {
      var sql = "SELECT * FROM FMS_RULES " +
                $"WHERE RULE_CATEGORY_ID = {category.Id} AND RULE_STATUS <> 'X' " +
                $"ORDER BY RULE_GROUP_ID, RULE_DEBIT_ACCOUNT DESC, RULE_CREDIT_ACCOUNT DESC";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<FinancialRule>(op);
    }


    static internal FixedList<FinancialRule> SearchFinancialRules(string filter) {
      Assertion.Require(filter, nameof(filter));

      var sql = "SELECT * FROM FMS_RULES " +
                $"WHERE {filter} " +
                $"ORDER BY RULE_DEBIT_ACCOUNT, RULE_CREDIT_ACCOUNT, " +
                $"RULE_DEBIT_CONCEPT, RULE_CREDIT_CONCEPT";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialRule>(op);
    }


    static internal void WriteFinancialRule(FinancialRule o) {
      var op = DataOperation.Parse("write_FMS_Rule", o.Id, o.UID, o.FinancialRuleType.Id,
                  o.Category.Id, o.GroupId, o.DebitAccount, o.DebitConcept, o.DebitCurrency.Id,
                  o.CreditAccount, o.CreditConcept, o.CreditCurrency.Id, o.Description,
                  o.Conditions.ToString(), o.Exceptions.ToString(), o.ExtData.ToString(),
                  o.Keywords, o.StartDate, o.EndDate, o.PostingTime, o.PostedBy.Id, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class FinancialRulesData

}  // namespace Empiria.Financial.Rules.Data
