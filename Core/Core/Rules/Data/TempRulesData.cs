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
  static internal class TempRulesData {

    #region Methods

    static internal void CleanTempRule(TempRule rule) {

      string account = IntegrationLibrary.FormatAccountNumber(rule.CuentaContable);

      var sql = "UPDATE FMS_RULES_2 " +
                $"SET CUENTA_CONTABLE = '{account}' " +
                $"WHERE ID_RULE = {rule.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal FixedList<TempRule> GetTempRules() {
      var sql = "SELECT * FROM FMS_RULES_2";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<TempRule>(op);
    }


    #endregion Methods

  }  // class TempRulesData

}  // namespace Empiria.Financial.Rules.Data
