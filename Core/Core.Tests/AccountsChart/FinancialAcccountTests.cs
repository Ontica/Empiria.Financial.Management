/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : FinancialAcccountTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for financial accounts objects.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;


namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for financial accounts objects.</summary>
  public class FinancialAcccountTests {

    #region Facts

    [Fact]
    public void Should_Read_All_FinancialAccount() {
      var sut = BaseObject.GetFullList<FinancialAccount>();

      Assert.NotNull(sut);
      Assert.True(sut.Count > 0);
    }


    [Fact]
    public void Should_Read_Empty_FinancialAccount() {
      var sut = FinancialAccount.Empty;

      Assert.NotNull(sut);
    }  

    #endregion Facts

  }  // class FinancialAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
