/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : PaymentAccountTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for PaymentAccount instances.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Payments;

namespace Empiria.Tests.Payments {

  /// <summary>Unit tests for PaymentAccount instances.</summary>
  public class PaymentAccountTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Payment_Accounts() {
      var sut = BaseObject.GetList<PaymentAccount>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_Empty_PaymentAccount() {
      var sut = PaymentAccount.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class PaymentAccountTests

}  // namespace Empiria.Tests.Payments
