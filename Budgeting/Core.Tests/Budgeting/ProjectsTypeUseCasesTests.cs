/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Test cases                              *
*  Assembly : Empiria.FinancialAccounting.Tests.dll      Pattern   : Use cases tests                         *
*  Type     : AccountsChartUseCasesTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving accounts from the accounts chart.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Xunit;

using Empiria.Projects.UseCases;
using Empiria.Projects.Adapters;
using Empiria.Budgeting.Tests;

namespace Empiria.Tests.Budgeting.Projects {

  /// <summary>Test cases for retrieving accounts from the accounts chart.</summary>
  public class ProjectTypesUseCasesTests {

    #region Use cases initialization

    private readonly ProjectTypesUseCases _usecases;


    public ProjectTypesUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = ProjectTypesUseCases.UseCaseInteractor();
    }

    ~ProjectTypesUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Read_A_Project_Type() {

      var sut = _usecases.ProjectTypesList();
      Assert.NotNull(sut);

    }

    [Fact]
    public void Should_Add_A_Project_Type() {

      var fields = new ProjectTypeFields {

        Name = "Servicios de desarrollo de software 1000",
        Code = "190000",

      };

      ProjectTypeDto sut = _usecases.AddProjectType(fields);

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Update_A_Project_Type() {

      var fields = new ProjectTypeFields {

        Name = "10001 - Servicios de desarrollo de software 10001",
        Code = "180000",

      };

      ProjectTypeDto sut = _usecases.UpdateProjectType("63e0d06b-fd6c-43cc-b35f-7cb569dadcb9", fields);

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Delete_A_Project_Type() {

      ProjectTypeDto sut = _usecases.DeleteProjectType("63e0d06b-fd6c-43cc-b35f-7cb569dadcb9");

      Assert.NotNull(sut);

    }


    #endregion Facts

  }  // class ProjectTypetUseCasesTests

}  // namespace Empiria.Tests.Budgeting.Projects
