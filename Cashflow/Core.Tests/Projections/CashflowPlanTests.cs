/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Test cases                              *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashFlowPlanTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashFlowPlan instances.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowPlan instances.</summary>
  public class CashFlowPlanTests {

    #region Facts

    [Fact]
    public void Should_Get_All_CashFlow_Plans() {
      var sut = BaseObject.GetFullList<CashFlowPlan>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashFlowPlan() {
      var sut = CashFlowPlan.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashFlowPlan.Parse("Empty"), sut);
      Assert.NotNull(sut.AvailableCategories);
      Assert.NotEmpty(sut.Prefix);
      Assert.NotEmpty(sut.Years);
    }


    [Fact]
    public void Should_Parse_All_CashFlow_Plans() {
      var list = CashFlowPlan.GetList();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.AvailableCategories);
        Assert.NotEmpty(sut.Prefix);
        Assert.NotEmpty(sut.Years);
      }
    }

    #endregion Facts

  }  // class CashFlowPlanTests

}  // namespace Empiria.Tests.CashFlow.Projections
