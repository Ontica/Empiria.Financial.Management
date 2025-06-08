/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : ChartOfAccountsTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for ChartOfAccounts type.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for ChartOfAccounts type.</summary>
  public class ChartOfAccountsTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Chart_Of_Accounts() {
      var sut = ChartOfAccounts.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_ChartOfAccounts() {
      var sut = ChartOfAccounts.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(ChartOfAccounts.Parse("Empty"), sut);
      Assert.Empty(sut.GetStandardAccounts());
    }


    [Fact]
    public void Should_Parse_All_Charts_Of_Accounts() {
      var chartOfAccounts = ChartOfAccounts.GetList();

      foreach (var sut in chartOfAccounts) {
        Assert.NotEmpty(sut.Name);
        Assert.NotEmpty(sut.GetStandardAccounts());
      }
    }

    #endregion Facts

  }  // class ChartOfAccountsTests

}  // namespace Empiria.Tests.Financial.Accounts
