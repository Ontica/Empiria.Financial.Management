/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : BudgetAccountSegmentTests                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BudgetAccountSegment type.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting;
using Empiria.Budgeting.Data;

namespace Empiria.Tests.Budgeting {

  /// <summary>Unit tests for BudgetAccountSegment type.</summary>
  public class BudgetAccountSegmentTests {

    #region Facts

    [Fact]
    public void Clean_Segments() {
      var accountSegments = BaseObject.GetFullList<FormerBudgetAcctSegment>();

      foreach (var segment in accountSegments) {
        FormerBudgetAcctSegmentData.CleanSegment(segment);
      }
    }


    [Fact]
    public void Should_Parse_All_Budget_Accounts_Segments() {
      var segments = BaseObject.GetList<FormerBudgetAcctSegment>();

      foreach (var sut in segments) {
        Assert.NotEmpty(sut.Code);
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.Parent);
        Assert.NotNull(sut.Children);
      }
    }


    [Fact]
    public void Should_Read_Empty_BudgetAccountSegment() {
      var sut = FormerBudgetAcctSegment.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.Parent);
      Assert.Empty(sut.Children);
    }


    [Fact]
    public void Should_Read_All_Budget_Accounts_Segments() {
      var sut = BaseObject.GetList<FormerBudgetAcctSegment>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class BudgetAccountSegmentTests

}  // namespace Empiria.Tests.Budgeting
