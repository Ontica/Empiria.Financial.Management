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

using Empiria.Financial.Projects.Adapters;
using System.CodeDom;

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


    [Fact]
    public void Should_Search_ProjectsByQuery() {
      var query = new ProjectQuery {
        Keywords = "Agua",
        OrganizationUnitUID = "",
        Status = StateEnums.EntityStatus.Active
      };

      var sut = _usecases.SearchProjects(query);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    [Fact]
    public void Create() {

      var fields = new ProjectFields {
        TypeId = 3281,
        StandarAccountUID = "206400",
        CategoryId = 501,
        PrjNo = "00",
        Name = "Name",
        OrganizationUnitUID = "e166a051-f848-4cbf-82df-e2f9a266b005"
      };


      var sut = _usecases.CreateProject(fields);

      Assert.NotNull(sut);
    }

    [Fact]
    public void Update() {
      var fields = new ProjectFields {
        TypeId = 3281,
        StandarAccountUID = "206102",
        CategoryId = 502,
        PrjNo = "0900",
        Name = "Otra prueba",
        OrganizationUnitUID = "e166a051-f848-4cbf-82df-e2f9a266b005"
      };

      var UID = "2b11c01c-50d0-4ae3-a3cd-bf89c7a8779a";
      var sut = _usecases.UpdateProject(UID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Delete() {
      var UID = "2b11c01c-50d0-4ae3-a3cd-bf89c7a8779a";
      _usecases.DeleteProject(UID);
    }

    #endregion Facts

  }  // class ProjectUseCasesTests

}  // namespace Empiria.Tests.Financial.Projects
