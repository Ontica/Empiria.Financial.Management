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
    public void Should_Read_All_Standard_Accounts_Catalogues() {
      var sut = BaseObject.GetFullList<StandardAccountsCatalogue>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_StandardAccountsCatalogue() {
      var sut = StandardAccountsCatalogue.Empty;

      Assert.NotNull(sut);
      Assert.Equal(StandardAccountsCatalogue.Parse("Empty"), sut);
      Assert.Equal(-1, sut .Id);
    }

    #endregion Facts

  }  // class StandardAccountsCatalogueTests

}  // namespace Empiria.Tests.Financial.Accounts
