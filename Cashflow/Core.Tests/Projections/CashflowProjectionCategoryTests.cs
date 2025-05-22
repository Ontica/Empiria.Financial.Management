/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Test cases                              *
*  Assembly : Empiria.Cashflow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashflowProjectionCategoryTests            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashflowProjectionCategory instances.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Cashflow.Projections;

namespace Empiria.Tests.Cashflow.Projections {

  /// <summary>Unit tests for CashflowProjectionCategory instances.</summary>
  public class CashflowProjectionCategoryTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Cashflow_Projection_Categories() {
      var sut = BaseObject.GetFullList<CashflowProjectionCategory>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashflowProjectionCategory() {
      var sut = CashflowProjectionCategory.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashflowProjectionCategory.Parse("Empty"), sut);
      Assert.NotNull(sut.ProjectionType);
    }


    [Fact]
    public void Should_Parse_All_Cashflow_Projection_Categories() {
      var list = BaseObject.GetFullList<CashflowProjectionCategory>();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.ProjectionType);
      }
    }

    #endregion Facts

  }  // class CashflowProjectionCategoryTests

}  // namespace Empiria.Tests.Cashflow.Projections
