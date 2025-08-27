/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : StandardAccountSegmentTests                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for StandardAccountSegment type.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for StandardAccountSegment type.</summary>
  public class StandardAccountSegmentTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Standard_Acccount_Segments() {
      var segments = StandardAccountSegment.GetList();

      foreach (var sut in segments) {
        Assert.NotEmpty(sut.Name);
        Assert.NotEmpty(sut.Code);
        Assert.NotNull(sut.Category);
      }
    }


    [Fact]
    public void Should_Read_All_Standard_Acccount_Segments() {
      var sut = StandardAccountSegment.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_StandardAccountSegment() {
      var sut = StandardAccountSegment.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(StandardAccountSegment.Parse("Empty"), sut);
      Assert.Equal(string.Empty, sut.Code);
      Assert.Equal(StandardAccountCategory.Empty, sut.Category);
    }

    #endregion Facts

  }  // class StandardAccountSegmentTests

}  // namespace Empiria.Tests.Financial.Accounts
