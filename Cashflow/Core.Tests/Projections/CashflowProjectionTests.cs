/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Test cases                              *
*  Assembly : Empiria.Cashflow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashflowProjectionTests                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashflowProjection instances.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Cashflow.Projections;

namespace Empiria.Tests.Cashflow.Projections {

  /// <summary>Unit tests for CashflowProjection instances.</summary>
  public class CashflowProjectionTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Cashflow_Projections() {
      var sut = BaseObject.GetFullList<CashflowProjection>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashflowProjection() {
      var sut = CashflowProjection.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashflowProjection.Parse("Empty"), sut);
      Assert.Equal(sut, sut.AdjustmentOf);
    }


    [Fact]
    public void Should_Parse_All_Cashflow_Projections() {
      var list = BaseObject.GetFullList<CashflowProjection>();

      foreach (var sut in list) {
        Assert.NotNull(sut.AdjustmentOf);
        Assert.NotNull(sut.AppliedBy);
        Assert.NotNull(sut.AttributesData);
        Assert.NotNull(sut.AuthorizedBy);
        Assert.NotNull(sut.BaseAccount);
        Assert.NotNull(sut.BaseParty);
        Assert.NotNull(sut.BaseProject);
        Assert.NotNull(sut.Category);
        Assert.NotNull(sut.Classification);
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

  }  // class CashflowProjectionTests

}  // namespace Empiria.Tests.Cashflow.Projections
