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

using Empiria.Financial.Concepts;
using Empiria.Financial.Concepts.Data;

using Empiria.Financial.Rules;
using Empiria.Financial.Rules.Data;

namespace Empiria.Tests.Financial {

  /// <summary>Financial data cleaners.</summary>
  public class DataCleaners {

    #region Facts

    [Fact]
    public void Clean_Financial_Concepts() {
      var groups = FinancialConceptGroup.GetList();

      foreach (var group in groups) {
        var concepts = group.GetConcepts();

        foreach (var parent in concepts) {
          var children = parent.GetAllChildren();
          foreach (var concept in children) {
            FinancialConceptsData.CleanConcept(concept, parent);
          }
        }
      }
    }


    [Fact]
    public void Clean_Financial_Rules() {
      var rules = BaseObject.GetFullList<FinancialRule>("RULE_CATEGORY_ID > 0");

      foreach (var rule in rules) {
        FinancialRulesData.CleanFinancialRule(rule);
      }
    }

    #endregion Facts

  }  // class DataCleaners

}  // namespace Empiria.Tests.Financial
