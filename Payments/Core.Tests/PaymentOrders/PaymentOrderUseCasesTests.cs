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
        PaymentOrderTypeUID = "32e1b307-676b-4488-b26f-1cbc03878875",
        PayableUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PayToUID = "c6278424-d1ff-492f-b5fe-410b4258292c",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "-32e1b307-676b-4488-b26f-1cbc03878875",
        Notes = "Sin notas",
        Total = 1234567.89m,
        DueTime = DateTime.Today,
        RequestedByUID  = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now

    };

      var sut = _usecases.CreatePaymentOrder(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Delete_Payment_Order() {
      _usecases.DeletePaymentOrder("b7a4b215-2ad8-4a5f-9349-21cce0e1a8e9");
    }


    [Fact]
    public void Should_Get_Payment_Methods() {

      var sut = _usecases.GetPaymentMethods();

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_Payment_Order_Types() {

      var sut = _usecases.GetPaymentOrderTypes();

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Update_Payment_Order() {
      var fields = new PaymentOrderFields {
        PaymentOrderTypeUID = "32e1b307-676b-4488-b26f-1cbc03878875",
        PayableUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PayToUID = "c6278424-d1ff-492f-b5fe-410b4258292c",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "-32e1b307-676b-4488-b26f-1cbc03878875",
        Notes = "updated by test",
        Total = 21.89m,
        DueTime = DateTime.Today,
        RequestedByUID = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now

      };

      var sut = _usecases.UpdatePaymentOrder("5426f403-2417-49de-b17d-ed18c37cbe06", fields);

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


    #endregion Facts

  }  // class PaymentOrderUseCasesTests

}  // namespace Empiria.Tests.Payments.Orders
