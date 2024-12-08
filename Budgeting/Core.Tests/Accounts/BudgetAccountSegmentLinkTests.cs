/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : BudgetAccountSegmentLinkTests              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BudgetAccountSegmentLink type.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting;
using Empiria.Products;

namespace Empiria.Tests.Budgeting {

  /// <summary>Unit tests for BudgetAccountSegmentLink type.</summary>
  public class BudgetAccountSegmentLinkTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Budget_Accounts_Segment_Links() {
      var segments = BaseObject.GetList<BudgetAccountSegmentLink>();

      foreach (var sut in segments) {
        Assert.NotNull(sut.BaseObjectLinkType);
        Assert.NotNull(sut.BudgetAccountSegment);
        Assert.NotNull(sut.GetLinkedObject<BaseObject>());
        Assert.NotNull(sut.LinkedObjectRole);
      }
    }


    [Fact]
    public void Should_Read_All_Budget_Accounts_Segment_Links() {
      var sut = BaseObject.GetList<BudgetAccountSegmentLink>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_All_Budget_Accounts_Segment_Products_Links() {
      var sut = BaseObjectLink.GetList<BudgetAccountSegmentLink>(BudgetAccountSegmentLink.ProcurementProductLink);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Budget_Account_Segments_For_A_Product() {
      var sut = BaseObjectLink.GetBaseObjectsFor<BudgetAccountSegment>(BudgetAccountSegmentLink.ProcurementProductLink,
                                                                       Product.Parse(4));

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_All_Products_For_A_Budget_Account_Segment() {
      var sut = BaseObjectLink.GetLinkedObjectsFor<Product>(BudgetAccountSegmentLink.ProcurementProductLink,
                                                            BudgetAccountSegment.Parse(2205));

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class BudgetAccountSegmentLinkTests

}  // namespace Empiria.Tests.Budgeting
