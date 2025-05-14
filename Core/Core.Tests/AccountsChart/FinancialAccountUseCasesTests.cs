/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects Management                        Component : Test cases                              *
*  Assembly : Empiria.Projects.Core.Tests.dll            Pattern   : Use cases tests                         *
*  Type     : ProjectUseCasesTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for financial projects use cases.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Accounts.UseCases;

namespace Empiria.Tests.Financial.Projects {

  /// <summary>Test cases for financial projects use cases.</summary>
  public class FinancialAccountUseCasesTests {

    #region Use cases initialization

    private readonly FinancialAccountUseCases _usecases;

    public FinancialAccountUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = FinancialAccountUseCases.UseCaseInteractor();
    }

    ~FinancialAccountUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Parse_StandardAcccounts() {
      var sut = _usecases.GetStandardAccount("207181");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Acccount() {
      var sut = _usecases.SearchAccounts("eva");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_StandardAcccountCategories() {
      var sut = _usecases.GetStandardAccountCategories();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_StandardAcccountCategory() {
      var sut = _usecases.GetStandardAccountCategory("7418d18f-04f9-486b-9c93-a2d52b0be246");

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class ProjectUseCasesTests

}  // namespace Empiria.Tests.Financial.Projects
