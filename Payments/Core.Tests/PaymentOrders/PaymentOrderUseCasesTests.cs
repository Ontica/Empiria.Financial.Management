/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Use cases tests                         *
*  Type     : PaymentOrderUseCasesTests                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for payment order use cases.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System;

using Empiria.Payments.Orders;
using Empiria.Payments.Orders.Adapters;
using Empiria.Payments.Orders.UseCases;


namespace Empiria.Tests.Payments.Orders {

  /// <summary>Test cases for payment order use cases.</summary>
  public class PaymentOrderUseCasesTests {

    #region Use cases initialization

    private readonly PaymentOrderUseCases _usecases;

    public PaymentOrderUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = PaymentOrderUseCases.UseCaseInteractor();
    }

    ~PaymentOrderUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Add_Payment_Order() {
      var fields = new PaymentOrderFields {
        PaymentOrderTypeUID = "fe85b014-9929-4339-b56f-5e650d3bd42c",
        PayToUID = "cea608fb-c327-4ba2-8cc1-ecc6cc482636",
        PaymentMethodUID = "ff779080-f58c-41ac-a48d-c1a00a2c5232",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
        Notes = "",
        DueTime = DateTime.Today,
        RequestedByUID = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now,
        ReferenceNumber = "777-333-77"
      };

      var sut = _usecases.CreatePaymentOrder(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Delete_Payment_Order() {
      _usecases.DeletePaymentOrder("3c97430d-41e2-445e-81d2-2b6f9ef75207");
    }

    [Fact]
    public void Should_Update_Payment_Order() {
      var fields = new PaymentOrderFields {
        PayToUID = "cea608fb-c327-4ba2-8cc1-ecc6cc482636",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
        Notes = "Updated by test",
        DueTime = DateTime.Today,
        RequestedByUID = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now,
        ReferenceNumber = "9999"
      };

      var sut = _usecases.UpdatePaymentOrder("abe5bd58-91fc-4e0f-b96b-bed19953940e", fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Payment_Orders() {
      var query = new PaymentOrdersQuery {
        Keywords = "",
        FromDate = new DateTime(2024, 01, 01),
        ToDate = new DateTime(2024, 01, 01)
      };

      var sut = _usecases.SearchPaymentOrders(query);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Send_Payment_Order_To_Pay() {
      var sut = _usecases.SendPaymentOrderToPay("fa3697b3-a42c-4753-b4bc-8d5f27737c4f");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Validate_Is_Payment_Order_Is_Payed() {
      var sut = _usecases.ValidatePaymentOrderIsPayed("65dccef4-cd85-4b5b-8a99-b6ca255c8ac3");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Validate_Is_Payments_Orders_Are_Payed() {
      var sut = _usecases.ValidatePayment();

      Assert.NotEqual(0, sut);
    }

    #endregion Facts

  }  // class PaymentOrderUseCasesTests

}  // namespace Empiria.Tests.Payments.Orders
