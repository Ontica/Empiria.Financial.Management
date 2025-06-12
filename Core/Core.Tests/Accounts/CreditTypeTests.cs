/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : CreditTypeTests                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CreditTypeTests type.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for CreditTypeTests type.</summary>
  public class CreditTypeTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Credit_Types() {
      var creditTypes = CreditType.GetList();

      foreach (var sut in creditTypes) {
        Assert.NotEmpty(sut.Name);
      }
    }


    [Fact]
    public void Should_Read_All_Credit_Types() {
      var sut = CreditType.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_CreditType() {
      var sut = CreditType.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CreditType.Parse("Empty"), sut);
    }

    #endregion Facts

  }  // class CreditTypeTests

}  // namespace Empiria.Tests.Financial.Accounts
