/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     :ExternalAccountsTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for ExternalAccounts type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.UseCases;
using Xunit;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for ExternalAccounts type.</summary>
  public class ExternalAccountsTests {

    #region Facts

    [Fact]
    public void Should_RefreshAccount_From_CreditSystem() {

      var usecases = ExternalAccountsUseCases.UseCaseInteractor();

      string accountUID = "a6d887d5-3d99-4066-8e12-ebac47cbeadc";

      var sut = usecases.RefreshAccountFromCreditSystem(accountUID);

      Assert.NotNull(sut);
    }


    #endregion Facts


  }  // class ExternalAccountsTests

}  // namespace Empiria.Tests.Financial.Accounts
