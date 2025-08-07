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

namespace Empiria.Financial.Rules.Data {

  /// <summary>Provides data access services for financial rules.</summary>
  static internal class TempRulesData {

    #region Methods

    static internal void CleanTempRule(TempRule rule) {

      string account = FormatAccountNumber(rule.CuentaContable);

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

    #region Helpers

    static private string FormatAccountNumber(string accountNumber) {
      string temp = EmpiriaString.TrimSpacesAndControl(accountNumber);

      if (temp.Length == 0) {
        return temp;
      }

      char separator = '.';
      string pattern = "0.00.00.00.00.00.00.00.00.00.00";

      temp = temp.Replace(separator.ToString(), string.Empty);

      temp = temp.TrimEnd('0');

      if (temp.Length > EmpiriaString.CountOccurences(pattern, '0')) {
        Assertion.RequireFail($"Number of placeholders in pattern ({pattern}) is less than the " +
                              $"number of characters in the input string ({accountNumber}).");
      } else {
        temp = temp.PadRight(EmpiriaString.CountOccurences(pattern, '0'), '0');
      }

      for (int i = 0; i < pattern.Length; i++) {
        if (pattern[i] == separator) {
          temp = temp.Insert(i, separator.ToString());
        }
      }

      while (true) {
        if (temp.EndsWith($"{separator}0000")) {
          temp = temp.Remove(temp.Length - 5);

        } else if (temp.EndsWith($"{separator}000")) {
          temp = temp.Remove(temp.Length - 4);

        } else if (temp.EndsWith($"{separator}00")) {
          temp = temp.Remove(temp.Length - 3);

        } else if (temp.EndsWith($"{separator}0")) {
          temp = temp.Remove(temp.Length - 2);

        } else {
          break;
        }
      }

      return temp;
    }

    #endregion Helpers

  }  // class FinancialRulesData

}  // namespace Empiria.Financial.Rules.Data
