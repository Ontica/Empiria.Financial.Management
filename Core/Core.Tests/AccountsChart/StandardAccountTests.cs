/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : StandardAccountTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for StandardAccount type.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for StandardAccount type.</summary>
  public class StandardAccountTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Standard_Accounts() {
      var sut = BaseObject.GetFullList<StandardAccount>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_StandardAccount() {
      var sut = StandardAccount.Empty;

      Assert.NotNull(sut);
      Assert.Equal(StandardAccount.Parse("Empty"), sut);
      Assert.Equal(-1, sut .Id);
    }

    #endregion Facts

  }  // class StandardAccountTests

}  // namespace Empiria.Tests.Financial.Accounts
