/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Core                             Component : Data Cleaners                           *
*  Assembly : Empiria.Tests.Financial.dll                Pattern   : Tests used as data cleaners             *
*  Type     : FinancialAcccountTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Financial data cleaners.                                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;
using Empiria.Financial.Data;

using Empiria.Financial.Projects;
using Empiria.Financial.Projects.Data;

using Empiria.Financial.Rules;
using Empiria.Financial.Rules.Data;

namespace Empiria.Tests.Financial {

  /// <summary>Financial data cleaners.</summary>
  public class DataCleaners {

    #region Facts

    [Fact]
    public void Clean_Financial_Accounts() {
      var accounts = BaseObject.GetFullList<FinancialAccount>();

      foreach (var account in accounts) {
        FinancialAccountDataService.CleanAccount(account);
      }
    }


    [Fact]
    public void Clean_Financial_Projects() {
      var projects = BaseObject.GetFullList<FinancialProject>();

      foreach (var project in projects) {
        FinancialProjectDataService.CleanProject(project);
      }
    }


    [Fact]
    public void Clean_Financial_Rules() {
      var rules = BaseObject.GetFullList<FinancialRule>();

      foreach (var rule in rules) {
        FinancialRulesData.CleanFinancialRule(rule);
      }
    }


    [Fact]
    public void Clean_Standard_Accounts() {
      var stdAccounts = BaseObject.GetFullList<StandardAccount>();

      foreach (var stdAccount in stdAccounts) {
        StandardAccountDataService.CleanStandardAccount(stdAccount);
      }
    }

    #endregion Facts

  }  // class DataCleaners

}  // namespace Empiria.Tests.Financial
