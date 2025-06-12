/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : CreditStageTests                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CreditStage type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for CreditStage type.</summary>
  public class CreditStageTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Credit_Types() {
      var creditStages = CreditStage.GetList();

      foreach (var sut in creditStages) {
        Assert.NotEmpty(sut.Name);
      }
    }


    [Fact]
    public void Should_Read_All_Credit_Types() {
      var sut = CreditStage.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_CreditType() {
      var sut = CreditStage.Empty;

      Assert.NotNull(sut);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(CreditStage.Parse("Empty"), sut);
    }

    #endregion Facts

  }  // class CreditStageTests

}  // namespace Empiria.Tests.Financial.Accounts
