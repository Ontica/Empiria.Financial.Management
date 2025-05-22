/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Test cases                              *
*  Assembly : Empiria.Cashflow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashflowProjectionTypeTests                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashflowProjectionType instances.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Cashflow.Projections;

namespace Empiria.Tests.Cashflow.Projections {

  /// <summary>Unit tests for CashflowProjectionType instances.</summary>
  public class CashflowProjectionTypeTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Cashflow_Projection_Types() {
      var sut = CashflowProjectionType.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashflowProjectionType() {
      var sut = CashflowProjectionType.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Prefix);
      Assert.NotNull(sut.RelatedDocumentTypes);
      Assert.NotNull(sut.Sources);
    }


    [Fact]
    public void Should_Parse_All_Cashflow_Projection_Types() {
      var list = CashflowProjectionType.GetList();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.Prefix);
        Assert.NotNull(sut.RelatedDocumentTypes);
        Assert.NotNull(sut.Sources);
      }
    }

    #endregion Facts

  }  // class CashflowProjectionTypeTests

}  // namespace Empiria.Tests.Cashflow.Projections
