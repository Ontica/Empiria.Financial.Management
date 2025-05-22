/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Test cases                              *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashFlowProjectionCategoryTests            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashFlowProjectionCategory instances.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowProjectionCategory instances.</summary>
  public class CashFlowProjectionCategoryTests {

    #region Facts

    [Fact]
    public void Should_Get_All_CashFlow_Projection_Categories() {
      var sut = BaseObject.GetFullList<CashFlowProjectionCategory>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashFlowProjectionCategory() {
      var sut = CashFlowProjectionCategory.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashFlowProjectionCategory.Parse("Empty"), sut);
      Assert.NotNull(sut.ProjectionType);
    }


    [Fact]
    public void Should_Parse_All_CashFlow_Projection_Categories() {
      var list = CashFlowProjectionCategory.GetList();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.ProjectionType);
      }
    }

    #endregion Facts

  }  // class CashFlowProjectionCategoryTests

}  // namespace Empiria.Tests.CashFlow.Projections
