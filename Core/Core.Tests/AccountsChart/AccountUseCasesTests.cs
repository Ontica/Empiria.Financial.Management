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
  public class AccountUseCasesTests {

    #region Use cases initialization

    private readonly AccountUseCases _usecases;

    public AccountUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = AccountUseCases.UseCaseInteractor();
    }

    ~AccountUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Search_StandardAcccounts() {

      string keywords = "servico";

      var sut = _usecases.SearchStandardAccounts(keywords);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


   
    #endregion Facts

  }  // class ProjectUseCasesTests

}  // namespace Empiria.Tests.Financial.Projects
