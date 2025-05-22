/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Test cases                              *
*  Assembly : Empiria.Cashflow.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : CashflowPlanTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashflowPlan instances.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Cashflow.Projections;

namespace Empiria.Tests.Cashflow.Projections {

  /// <summary>Unit tests for CashflowPlan instances.</summary>
  public class CashflowPlanTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Cashflow_Plans() {
      var sut = BaseObject.GetFullList<CashflowPlan>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_CashflowPlan() {
      var sut = CashflowPlan.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CashflowPlan.Parse("Empty"), sut);
      Assert.NotNull(sut.AvailableCategories);
      Assert.NotEmpty(sut.Prefix);
      Assert.NotEmpty(sut.Years);
    }


    [Fact]
    public void Should_Parse_All_Cashflow_Plans() {
      var list = CashflowPlan.GetList();

      foreach (var sut in list) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.AvailableCategories);
        Assert.NotEmpty(sut.Prefix);
        Assert.NotEmpty(sut.Years);
      }
    }

    #endregion Facts

  }  // class CashflowPlanTests

}  // namespace Empiria.Tests.Cashflow.Projections
