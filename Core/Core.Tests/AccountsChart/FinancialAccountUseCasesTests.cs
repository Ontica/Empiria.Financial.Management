/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Use cases tests                         *
*  Type     : FinancialAccountUseCasesTests              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for financial accounts use cases.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Accounts.Adapters;
using Empiria.Financial.Accounts.UseCases;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Test cases for financial accounts use cases.</summary>
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


    [Fact]
    public void Should_Search_AccountsByQuery() {
      var query = new FinancialAccountQuery {
        Keywords = "Agua",
        OrganizationUnitUID = "",
        Status = StateEnums.EntityStatus.Active
      };

      var sut = _usecases.SearchAccount(query);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class FinancialAccountUseCasesTests

}  // namespace Empiria.Tests.Financial.Accounts
