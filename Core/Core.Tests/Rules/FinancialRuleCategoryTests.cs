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

using Empiria.Financial.Rules;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for inancialRuleCategory type.</summary>
  public class FinancialRuleCategoryTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Financial_Rules_Categories() {
      var categories = FinancialRuleCategory.GetList();

      foreach (var sut in categories) {
        Assert.NotEmpty(sut.NamedKey);
      }
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

}  // namespace Empiria.Tests.Financial.Accounts
