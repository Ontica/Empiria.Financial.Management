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

using Empiria.Financial.Projects.UseCases;

namespace Empiria.Tests.Financial.Projects {

  /// <summary>Test cases for financial projects use cases.</summary>
  public class ProjectUseCasesTests {

    #region Use cases initialization

    private readonly ProjectUseCases _usecases;

    public ProjectUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = ProjectUseCases.UseCaseInteractor();
    }

    ~ProjectUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Search_Projects() {

      string keywords = "agua potable";

      var sut = _usecases.SearchProjects(keywords);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class ProjectUseCasesTests

}  // namespace Empiria.Tests.Financial.Projects
