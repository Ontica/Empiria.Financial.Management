/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Use cases tests                         *
*  Type     : PaymentOrderUseCasesTests                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for payment order use cases.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

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
        ControlNo = "494844",
        PayToUID = "cea608fb-c327-4ba2-8cc1-ecc6cc482636",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
        Notes = "",
        Total = 1234567.89m,
        DueTime = DateTime.Today,
        RequestedByUID = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now,
        ReferenceNumber = "1929393-393838"
      };

      var sut = _usecases.CreatePaymentOrder(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Delete_Payment_Order() {
      _usecases.DeletePaymentOrder("3c97430d-41e2-445e-81d2-2b6f9ef75207");
    }


    [Fact]
    public void Should_Get_Payment_Order_Types() {

      var sut = _usecases.GetPaymentOrderTypes();

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Update_Payment_Order() {
      var fields = new PaymentOrderFields {
        ControlNo = "4945",
        PayToUID = "cea608fb-c327-4ba2-8cc1-ecc6cc482636",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
        Notes = "Updated by test",
        Total = 6000m,
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
      var sut = _usecases.SendPaymentOrderToPay("438263cd-7f86-4f86-bcc0-cf82cff5eb71");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Validate_Is_Payment_Order_Is_Payed() {
      var sut = _usecases.ValidatePaymentOrderIsPayed("65dccef4-cd85-4b5b-8a99-b6ca255c8ac3");

      Assert.NotNull(sut);
    }

   
    #endregion Facts

  }  // class PaymentOrderUseCasesTests

}  // namespace Empiria.Tests.Payments.Orders
