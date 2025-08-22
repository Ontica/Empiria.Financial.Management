/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                          Component : Domain Layer                         *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Static service library               *
*  Type     : IntegrationLibrary                            License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Static library with general purpose methods.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Integration {

  /// <summary>Static library with general purpose methods.</summary>
  static public class IntegrationLibrary {

    static public string FormatAccountNumber(string accountNumber) {
      string temp = EmpiriaString.TrimSpacesAndControl(accountNumber);

      if (temp.Length == 0) {
        return temp;
      }

      if (temp.Contains("*") || temp.Contains("]") || temp.Contains("^") || temp.Contains("~")) {
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

  }  // class IntegrationLibrary

}  // namespace Empiria.Financial.Integration
