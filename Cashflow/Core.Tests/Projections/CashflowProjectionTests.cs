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

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;

using Empiria.CashFlow.Projections;

namespace Empiria.Tests.CashFlow.Projections {

  /// <summary>Unit tests for CashFlowProjection instances.</summary>
  public class CashFlowProjectionTests {

    #region Facts

    [Fact]
    public void Should_Create_CashFlowProjection() {

      var plan = TestsObjects.TryGetObject<CashFlowPlan>(x => x.Status == OpenCloseStatus.Opened);
      var category = TestsObjects.TryGetObject<CashFlowProjectionCategory>();
      var baseAccount = ExternalVariables.FinancialAccounts.Find(x =>
                                 x.FinancialAccountType.PlaysRole(CashFlowProjection.BASE_ACCOUNT_ROLE) &&
                                 x.OrganizationalUnit.PlaysRole(CashFlowProjectionRules.CASH_FLOW_ROLE)
                               );

      var sut = new CashFlowProjection(plan, category, baseAccount);

      Assert.Equal(plan, sut.Plan);
      Assert.Equal(category, sut.Category);
      Assert.Equal(category.ProjectionType, sut.ProjectionType);
      Assert.Equal(baseAccount, sut.BaseAccount);
      Assert.Equal(baseAccount.Project, sut.BaseProject);
      Assert.Equal(baseAccount.OrganizationalUnit, sut.BaseParty);
      Assert.False(sut.HasProjectionNo);
      Assert.Equal(CashFlowProjection.TO_ASSIGN_PROJECTION_NO, sut.ProjectionNo);
      Assert.Equal(OperationSource.Default, sut.OperationSource);
      Assert.Equal(CashFlowProjection.Empty, sut.AdjustmentOf);
      Assert.Equal(TransactionStatus.Pending, sut.Status);
      Assert.Empty(sut.Description);
      Assert.Empty(sut.Justification);
      Assert.True(sut.Rules.CanUpdate);

    }


    [Fact]
    public void Should_Cancel_CashFlowProjection() {

      TestsCommonMethods.Authenticate();

      CashFlowProjection sut = TestsObjects.TryGetObject<CashFlowProjection>(
                                              x => x.Rules.CanDelete && x.HasProjectionNo
                                            );

      string projectionNo = sut.ProjectionNo;

      sut.DeleteOrCancel();

      Assert.Equal(TransactionStatus.Canceled, sut.Status);
      Assert.Equal(projectionNo, sut.ProjectionNo);
    }


    [Fact]
    public void Should_Delete_CashFlowProjection() {

      TestsCommonMethods.Authenticate();

      CashFlowProjection sut = TestsObjects.TryGetObject<CashFlowProjection>(
                                              x => x.Rules.CanDelete && !x.HasProjectionNo
                                            );

      sut.DeleteOrCancel();

      Assert.Equal(TransactionStatus.Deleted, sut.Status);
      Assert.Equal(CashFlowProjection.DELETED_PROJECTION_NO, sut.ProjectionNo);
    }


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


    [Fact]
    public void Should_Update_CashFlowProjection() {

      TestsCommonMethods.Authenticate();

      CashFlowProjection sut = TestsObjects.TryGetObject<CashFlowProjection>(
                                              x => x.Rules.CanUpdate
                                            );

      var updatedAccount = ExternalVariables.FinancialAccounts.Find(x =>
                                 x.Project.Distinct(sut.BaseProject) &&
                                 x.FinancialAccountType.PlaysRole(CashFlowProjection.BASE_ACCOUNT_ROLE) &&
                                 x.OrganizationalUnit.PlaysRole(CashFlowProjectionRules.CASH_FLOW_ROLE)
                              );

      Assert.True(updatedAccount != null, nameof(updatedAccount));

      var fields = new CashFlowProjectionFields {
        PartyUID = updatedAccount.OrganizationalUnit.UID,
        ProjectUID = updatedAccount.Project.UID,
        AccountUID = updatedAccount.UID,
        Justification = "Updated justification",
        Description = "Updated description"
      };

      sut.Update(fields);

      Assert.Equal(fields.PartyUID, sut.BaseParty.UID);
      Assert.Equal(fields.ProjectUID, sut.BaseProject.UID);
      Assert.Equal(fields.AccountUID, sut.BaseAccount.UID);
      Assert.Equal(fields.Description, sut.Description);
      Assert.Equal(fields.Justification, sut.Justification);
    }

    #endregion Facts

  }  // class CashFlowProjectionTests

}  // namespace Empiria.Tests.CashFlow.Projections
