﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : StandardAccountCategoryTests               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for StandardAccountCategory type.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for StandardAccountCategory type.</summary>
  public class StandardAccountCategoryTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Standard_Acccount_Categories() {
      var categories = StandardAccountCategory.GetList();

      foreach (var sut in categories) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.Catalogue);
        Assert.NotEqual(StandardAccountsCatalogue.Empty, sut.Catalogue);
        Assert.NotNull(sut.Parent);
        Assert.NotEmpty(sut.GetStandardAccounts());
      }
    }


    [Fact]
    public void Should_Read_All_Standard_Acccount_Categories() {
      var sut = StandardAccountCategory.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_StandardAccountCategory() {
      var sut = StandardAccountCategory.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(StandardAccountCategory.Parse("Empty"), sut);
      Assert.Equal(StandardAccountCategory.Empty, sut.Parent);
      Assert.Equal(StandardAccountsCatalogue.Empty, sut.Catalogue);
      Assert.Empty(sut.GetStandardAccounts());
    }

    #endregion Facts

  }  // class StandardAccountCategoryTests

}  // namespace Empiria.Tests.Financial.Accounts
