/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : StandardAccountsCatalogueTests             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for StandardAccountsCatalogue type.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for StandardAccountsCatalogue type.</summary>
  public class StandardAccountsCatalogueTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Standard_Acccount_Categories() {
      var catalogues = StandardAccountsCatalogue.GetList();

      foreach (var sut in catalogues) {
        Assert.NotEmpty(sut.Name);
        Assert.NotEmpty(sut.GetStandardAccounts());
      }
    }


    [Fact]
    public void Should_Read_All_Standard_Accounts_Catalogues() {
      var sut = BaseObject.GetFullList<StandardAccountsCatalogue>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_StandardAccountsCatalogue() {
      var sut = StandardAccountsCatalogue.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(StandardAccountsCatalogue.Parse("Empty"), sut);
      Assert.Empty(sut.GetStandardAccounts());
    }

    #endregion Facts

  }  // class StandardAccountsCatalogueTests

}  // namespace Empiria.Tests.Financial.Accounts
