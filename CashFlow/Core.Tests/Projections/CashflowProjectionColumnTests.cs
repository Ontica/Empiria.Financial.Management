/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Test cases                              *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashflowProjectionColumnTests              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashflowProjectionColumn type.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashflowProjectionColumn type.</summary>
  public class CashflowProjectionColumnTests {

    #region Facts

    [Fact]
    public void Should_Get_All_CashFlow_Projection_Columns() {
      var sut = BaseObject.GetFullList<CashFlowProjectionColumn>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashFlowProjectionColumn() {
      var sut = CashFlowProjectionColumn.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashFlowProjectionColumn.Parse("Empty"), sut);
    }


    [Fact]
    public void Should_Parse_All_CashFlow_Projection_Columns() {
      var list = CashFlowProjectionCategory.GetList();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.ProjectionType);
      }
    }

    #endregion Facts

  }  // class CashflowProjectionColumnTests

}  // namespace Empiria.Tests.CashFlow.Projections
