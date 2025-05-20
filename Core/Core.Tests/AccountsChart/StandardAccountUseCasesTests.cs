/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Use cases tests                         *
*  Type     : StandardAccountUseCasesTests               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for standard accounts use cases.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Accounts.UseCases;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Test cases for standard accounts use cases.</summary>
  public class StandardAccountUseCasesTests {

    #region Use cases initialization

    private readonly StandardAccountUseCases _usecases;

    public StandardAccountUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = StandardAccountUseCases.UseCaseInteractor();
    }

    ~StandardAccountUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Get_StandardAcccountCategories() {
      var sut = _usecases.GetStandardAccountCategories();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_StandardAcccounts() {
      var sut = _usecases.GetStandardAccount("207181");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Parse_StandardAcccountCategory() {
      var sut = _usecases.GetStandardAccountCategory("7418d18f-04f9-486b-9c93-a2d52b0be246");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Parse_StandardAcccountBytCategory() {
      var sut = _usecases.GetStandardAccountByCategory(632);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialAccountUseCasesTests

}  // namespace Empiria.Tests.Financial.Accounts
