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

using Empiria.StateEnums;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowPlan instances.</summary>
  public class CashFlowPlanTests {

    #region Facts

    [Fact]
    public void Should_Add_CashFlowProjection() {

      var sut = TestsObjects.TryGetObject<CashFlowPlan>(x => x.Status == OpenCloseStatus.Opened);
      var category = TestsObjects.TryGetObject<CashFlowProjectionCategory>();
      var baseAccount = ExternalVariables.FinancialAccounts.Find(x =>
                                    x.OrganizationalUnit.PlaysRole(CashFlowProjectionRules.CASH_FLOW_ROLE)
                                  );

      var count = sut.Projections.Count;

      CashFlowProjection projection = sut.AddProjection(category, baseAccount);

      Assert.Equal(projection.Plan, sut);
      Assert.Equal(projection.Category, category);
      Assert.Equal(projection.BaseAccount, baseAccount);
      Assert.Equal(count + 1, sut.Projections.Count);
    }


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


    [Fact]
    public void Should_Remove_CashFlowProjection() {

      TestsCommonMethods.Authenticate();

      var sut = TestsObjects.TryGetObject<CashFlowPlan>(x => x.Status == OpenCloseStatus.Opened);

      var projection = sut.Projections.Find(x => x.Status == TransactionStatus.Pending);

      var count = sut.Projections.Count;

      sut.RemoveProjection(projection);

      Assert.True(projection.Status == TransactionStatus.Deleted ||
                  projection.Status == TransactionStatus.Canceled);

      Assert.Equal(count - 1, sut.Projections.Count);
    }

    #endregion Facts

  }  // class CashFlowPlanTests

}  // namespace Empiria.Tests.CashFlow.Projections
