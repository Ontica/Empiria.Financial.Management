/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialRuleTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialRule type.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Rules;
using Empiria.Financial.Rules.Data;

namespace Empiria.Tests.Financial.Rules {

  /// <summary>Unit tests for FinancialRule type.</summary>
  public class FinancialRuleTests {

    #region Facts

    [Fact]
    internal void Clean_Financial_Rules() {
      var rules = BaseObject.GetFullList<FinancialRule>();

      foreach (var rule in rules) {
        FinancialRulesData.CleanFinancialRule(rule);
      }
    }


    [Fact]
    public void Should_Parse_All_Financial_Rules() {
      var rules = BaseObject.GetFullList<FinancialRule>();

      foreach (var sut in rules) {
        Assert.NotNull(sut.Category);
        Assert.NotNull(sut.DebitAccount);
        Assert.NotNull(sut.DebitCurrency);
        Assert.NotNull(sut.CreditCurrency);
        Assert.NotNull(sut.Conditions);
        Assert.NotNull(sut.Exceptions);
        Assert.NotNull(sut.ExtData);
      }
    }


    [Fact]
    public void Should_Read_All_Financial_Rules() {
      var sut = BaseObject.GetFullList<FinancialRule>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FinancialRule() {
      var sut = FinancialRule.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(FinancialRule.Parse("Empty"), sut);
    }

    #endregion Facts

  }  // class FinancialRuleTests

}  // namespace Empiria.Tests.Financial.Rules
