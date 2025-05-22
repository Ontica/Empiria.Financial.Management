/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Test cases                              *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashFlowProjectionTypeTests                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashFlowProjectionType instances.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowProjectionType instances.</summary>
  public class CashFlowProjectionTypeTests {

    #region Facts

    [Fact]
    public void Should_Get_All_CashFlow_Projection_Types() {
      var sut = CashFlowProjectionType.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashFlowProjectionType() {
      var sut = CashFlowProjectionType.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Prefix);
      Assert.NotNull(sut.RelatedDocumentTypes);
      Assert.NotNull(sut.Sources);
    }


    [Fact]
    public void Should_Parse_All_CashFlow_Projection_Types() {
      var list = CashFlowProjectionType.GetList();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.Prefix);
        Assert.NotNull(sut.RelatedDocumentTypes);
        Assert.NotNull(sut.Sources);
      }
    }

    #endregion Facts

  }  // class CashFlowProjectionTypeTests

}  // namespace Empiria.Tests.CashFlow.Projections
