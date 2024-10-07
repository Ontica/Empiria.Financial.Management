/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project                                    Component : Test cases                              *
*  Assembly : Empiria.FinancialAccounting.Tests.dll      Pattern   : Use cases tests                         *
*  Type     : AccountsChartUseCasesTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving accounts from the accounts chart.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Projects.UseCases;
using Empiria.Projects.Adapters;

namespace Empiria.Tests.Budgeting.Projects {

  /// <summary>Test cases for retrieving accounts from the accounts chart.</summary>
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
    public void Should_Add_A_Project() {

      var fields = new ProjectFields {

        Name = "Servicios de desarrollo de software 10001",
        Description = "Definicion del proyecto 10001",
        Code = "S2-2024-06-00001"

      };

      ProjectDto sut = _usecases.AddProject(fields);

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Read_A_Project() {

      var sut = _usecases.ProjectList();
      Assert.NotNull(sut);

    }

    #endregion Facts

  }  // class ContractUseCasesTests

}  // namespace Empiria.Tests.Payments.Contracts
