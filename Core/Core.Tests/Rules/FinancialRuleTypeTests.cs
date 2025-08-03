/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialRuleTypeTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialRuleType power type.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Rules;

namespace Empiria.Tests.Financial.Rules {

  /// <summary>Unit tests for FinancialRuleType power type.</summary>
  public class FinancialRuleTypeTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Financial_Rule_Types() {
      var sut = FinancialRuleType.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Financial_Rule_Types() {
      var rules = FinancialRuleType.GetList();

      foreach (var sut in rules) {
        Assert.NotNull(sut.DisplayName);
      }
    }


    [Fact]
    public void Should_Parse_Empty_FinancialRuleType() {
      var sut = FinancialRuleType.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialRuleTypeTests

}  // namespace Empiria.Tests.Financial.Rules
