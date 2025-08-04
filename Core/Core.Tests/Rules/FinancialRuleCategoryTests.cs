/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialRuleCategoryTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for inancialRuleCategory type.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System;

using Empiria.StateEnums;

using Empiria.Financial.Rules;

namespace Empiria.Tests.Financial.Rules {

  /// <summary>Unit tests for inancialRuleCategory type.</summary>
  public class FinancialRuleCategoryTests {

    #region Facts

    [Fact]
    public void Should_Get_FinancialRuleCategory_Financial_Rules() {
      FinancialRuleCategory category = TestsObjects.TryGetObject<FinancialRuleCategory>();

      var sut = category.GetFinancialRules();

      Assert.NotEmpty(sut);
      Assert.All(sut, x => Assert.True(x.Status != EntityStatus.Deleted));
    }


    [Fact]
    public void Should_Get_FinancialRuleCategory_Financial_Rules_In_Date() {
      FinancialRuleCategory category = TestsObjects.TryGetObject<FinancialRuleCategory>();

      var date = DateTime.Today;

      var sut = category.GetFinancialRules(date);

      Assert.NotEmpty(sut);
      Assert.All(sut, x => Assert.True(x.StartDate <= date && date <= x.EndDate));
      Assert.All(sut, x => Assert.True(x.Status != EntityStatus.Deleted));
    }


    [Fact]
    public void Should_Parse_All_Financial_Rules_Categories() {
      var categories = FinancialRuleCategory.GetList();

      foreach (var sut in categories) {
        Assert.NotEmpty(sut.NamedKey);
      }
    }


    [Fact]
    public void Should_Parse_FinancialRuleCategory_With_NamedKey() {
      var category = TestsObjects.TryGetObject<FinancialRuleCategory>();

      var sut = FinancialRuleCategory.ParseNamedKey(category.NamedKey);

      Assert.Equal(category, sut);
    }


    [Fact]
    public void Should_Read_All_Financial_Rules_Categories() {
      var sut = FinancialRuleCategory.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FinancialRuleCategory() {
      var sut = FinancialRuleCategory.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(FinancialRuleCategory.Parse("Empty"), sut);
    }

    #endregion Facts

  }  // class FinancialRuleCategoryTests

}  // namespace Empiria.Tests.Financial.Rules
