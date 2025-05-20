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
    public void Should_Create_FinancialAccount() {

      var fields = new FinancialAccountFields {
        StandarAccountUID = "1d87bbf6-a36a-4a53-8134-674ad323b335",
        OrganizationUID = "e166a051-f848-4cbf-82df-e2f9a266b005",
        OrganizationUnitUID = "e166a051-f848-4cbf-82df-e2f9a266b005",
        PartyUID = "8382a99e-92e7-4d57-978c-d7d7ef8d3e33",
        ProjectUID = "17903a85-4018-4ee1-8218-cc497dc688ee",
        AcctNo = "0000001",
        Description = "Test del 000001",
      };

      var sut = _usecases.CreateAccount(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Delete_FinancialAccount() {
      var UID = "989fafcc-01e0-48d7-b2c0-3375b860874b";
      _usecases.DeleteAccount(UID);
    }


    [Fact]
    public void Should_Search_Acccount() {
      var sut = _usecases.SearchAccounts("eva");

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


    [Fact]
    public void Should_Update_FinancialAccount() {
      var fields = new FinancialAccountFields {
        StandarAccountUID = "1d87bbf6-a36a-4a53-8134-674ad323b335",
        OrganizationUID = "e166a051-f848-4cbf-82df-e2f9a266b005",
        OrganizationUnitUID = "e166a051-f848-4cbf-82df-e2f9a266b005",
        PartyUID = "e99b4454-1e0e-47b9-a746-9798b015094a",
        ProjectUID = "0eca8e61-66c1-4801-9b0d-05e6587841ce",
        AcctNo = "0000001",
        Description = "Test Update",
      };

      var UID = "989fafcc-01e0-48d7-b2c0-3375b860874b";
      var sut = _usecases.UpdateAccount(UID, fields);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialAccountUseCasesTests

}  // namespace Empiria.Tests.Financial.Accounts
