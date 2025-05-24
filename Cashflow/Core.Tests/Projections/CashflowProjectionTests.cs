/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Test cases                              *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashFlowProjectionTests                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashFlowProjection instances.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowProjection instances.</summary>
  public class CashFlowProjectionTests {

    #region Facts

    [Fact]
    public void Should_Get_All_CashFlow_Projections() {
      var sut = BaseObject.GetFullList<CashFlowProjection>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashFlowProjection() {
      var sut = CashFlowProjection.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashFlowProjection.Parse("Empty"), sut);
      Assert.Equal(sut, sut.AdjustmentOf);
    }


    [Fact]
    public void Should_Parse_All_CashFlow_Projections() {
      var list = BaseObject.GetFullList<CashFlowProjection>();

      foreach (var sut in list) {
        Assert.NotNull(sut.AdjustmentOf);
        Assert.NotNull(sut.AppliedBy);
        Assert.NotNull(sut.AttributesData);
        Assert.NotNull(sut.AuthorizedBy);
        Assert.NotNull(sut.BaseAccount);
        Assert.NotNull(sut.BaseParty);
        Assert.NotNull(sut.BaseProject);
        Assert.NotNull(sut.Category);
        Assert.NotNull(sut.ConfigData);
        Assert.NotNull(sut.Description);
        Assert.NotNull(sut.FinancialData);
        Assert.NotNull(sut.Identificators);
        Assert.NotNull(sut.Justification);
        Assert.NotEmpty(sut.Keywords);
        Assert.NotNull(sut.Plan);
        Assert.NotNull(sut.PostedBy);
        Assert.NotEmpty(sut.ProjectionNo);
        Assert.NotNull(sut.ProjectionType);
        Assert.NotNull(sut.Rules);
        Assert.NotNull(sut.OperationSource);
        Assert.NotNull(sut.Tags);
      }
    }

    #endregion Facts

  }  // class CashFlowProjectionTests

}  // namespace Empiria.Tests.CashFlow.Projections
