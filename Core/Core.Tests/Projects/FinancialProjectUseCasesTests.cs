/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects Management                        Component : Test cases                              *
*  Assembly : Empiria.Projects.Core.Tests.dll            Pattern   : Use cases tests                         *
*  Type     : FinancialProjectUseCasesTests              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for financial projects use cases.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Projects.Adapters;
using Empiria.Financial.Projects.UseCases;

namespace Empiria.Tests.Financial.Projects {

  /// <summary>Test cases for financial projects use cases.</summary>
  public class FinancialProjectUseCasesTests {

    #region Use cases initialization

    private readonly FinancialProjectUseCases _usecases;

    public FinancialProjectUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = FinancialProjectUseCases.UseCaseInteractor();
    }

    ~FinancialProjectUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Create_FinancialProject() {

      var fields = new FinancialProjectFields {
        StandardAccountUID = "1d87bbf6-a36a-4a53-8134-674ad323b335",
        CategoryUID = "f0f98712-54da-40e8-bb7e-dac99350d9ab",
        ProjectNo = "90210",
        Name = "Proyecto de Prueba",
        OrganizationUnitUID = "e166a051-f848-4cbf-82df-e2f9a266b005"
      };

      var sut = _usecases.CreateProject(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Delete_FinancialProject() {
      var UID = "2b11c01c-50d0-4ae3-a3cd-bf89c7a8779a";
      _usecases.DeleteProject(UID);
    }


    [Fact]
    public void Should_GetFinacialProjectCategories() {
      var sut = _usecases.GetFinancialQueryCategories();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_GetFinacialProjectCategory() {
      var sut = _usecases.GetFinancialQueryCategory("bdf57f96-27d4-4007-a38c-30c567298803");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Projects() {
      string keywords = "agua potable";

      var sut = _usecases.SearchProjects(keywords);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Search_ProjectsByQuery() {
      var query = new FinancialProjectQuery {
        Keywords = "Agua",
        OrganizationUnitUID = "",
        Status = StateEnums.EntityStatus.Active
      };

      var sut = _usecases.SearchProjects(query);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }
       

    [Fact]
    public void Should_Update_FinancialProject() {
      var fields = new FinancialProjectFields {
        StandardAccountUID = "1d87bbf6-a36a-4a53-8134-674ad323b335",
        CategoryUID = "f0f98712-54da-40e8-bb7e-dac99350d9ab",
        ProjectNo = "0920",
        Name = "Test 3i3i3i",
        OrganizationUnitUID = "e166a051-f848-4cbf-82df-e2f9a266b005"
      };

      var UID = "2b11c01c-50d0-4ae3-a3cd-bf89c7a8779a";
      var sut = _usecases.UpdateProject(UID, fields);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialProjectUseCasesTests

}  // namespace Empiria.Tests.Financial.Projects
