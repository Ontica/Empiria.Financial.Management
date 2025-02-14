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
        PayableUID = "1537c110-e3f1-4ab8-b822-c00733ac3dd5",
        PayToUID = "c6278424-d1ff-492f-b5fe-410b4258292c",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
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
      _usecases.DeletePaymentOrder("ed7879fd-79df-4ea9-b83a-b131642b5ca2");
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
        PayableUID = "1537c110-e3f1-4ab8-b822-c00733ac3dd5",
        PayToUID = "c6278424-d1ff-492f-b5fe-410b4258292c",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
        Notes = "updated by test",
        Total = 21.89m,
        DueTime = DateTime.Today,
        RequestedByUID = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now

      };

      var sut = _usecases.UpdatePaymentOrder("4954fdf9-b947-48f4-b8fa-610f559448d8", fields);

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
      var sut = _usecases.SendPaymentOrderToPay("65dccef4-cd85-4b5b-8a99-b6ca255c8ac3");

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
