/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Service tests                           *
*  Type     : PaymentServiceTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for payment services.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System.Threading.Tasks;

using Empiria.Payments.Processor;

namespace Empiria.Tests.Payments {

  /// <summary>Unit tests for payment services.</summary>
  public class PaymentServiceTests {

    #region Use cases initialization

    private readonly PaymentService _services;

    public PaymentServiceTests() {
      TestsCommonMethods.Authenticate();

      _services = PaymentService.ServiceInteractor();
    }


    ~PaymentServiceTests() {
      _services.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public async Task Should_Send_Payment_Order_To_Pay() {
      var sut = await _services.SendPaymentOrderToPay("fa3697b3-a42c-4753-b4bc-8d5f27737c4f");

      Assert.NotNull(sut);
    }


    [Fact]
    public async Task Should_Validate_Is_Payment_Order_Is_Payed() {
      var sut = await _services.ValidatePaymentOrderIsPayed("65dccef4-cd85-4b5b-8a99-b6ca255c8ac3");

      Assert.NotNull(sut);
    }


    [Fact]
    public async Task Should_Validate_Is_Payments_Orders_Are_Payed() {
      var sut = await _services.ValidatePayment();

      Assert.NotEqual(0, sut);
    }

    #endregion Facts

  }  // class PaymentServiceTests

}  // namespace Empiria.Tests.Payments
