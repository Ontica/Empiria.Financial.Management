/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                        Component : Test cases                             *
*  Assembly : Financial.Integration.Core.Tests.dll        Pattern   : Unit tests                             *
*  Type     : ExternalCreditSystemServicesTests           License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Unit tests for ExternalCreditSystemServices interface.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;
using Empiria.Financial.Adapters;

namespace Empiria.Tests.Financial.Integation {

  /// <summary>Unit tests for ExternalCreditSystemServices interface.</summary>
  public class ExternalCreditSystemServicesTests {

    #region Facts

    [Fact]
    public void Should_Execute_TryGetCreditAccountData() {

      var service = new ExternalCreditSystemServices();

      ICreditAccountData sut = service.TryGetCreditWithAccountNo("12345");

      Assert.NotNull(sut);
      Assert.NotEmpty(sut.AccountNo);
      Assert.NotEmpty(sut.Borrower);
      Assert.NotEmpty(sut.SubledgerAccountNo);
    }

    #endregion Facts

  }  // class ExternalCreditSystemServicesTests

}  // namespace Empiria.Tests.Financial.Integation
