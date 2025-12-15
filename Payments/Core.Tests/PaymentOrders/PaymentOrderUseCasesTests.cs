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

using Empiria.Payments;
using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

namespace Empiria.Tests.Payments {

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
        Description = string.Empty,
        DueTime = DateTime.Today,
        RequestedByUID = "6bebca32-c14f-4996-8300-77ac86513a59",
        RequestedTime = DateTime.Now,
        ReferenceNumber = "777-333-77"
      };

      var sut = _usecases.CreatePaymentOrder(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Cancel_Payment_Order() {
      _usecases.CancelPaymentOrder("3c97430d-41e2-445e-81d2-2b6f9ef75207");
    }


    [Fact]
    public void Should_Update_Payment_Order() {
      var fields = new PaymentOrderFields {
        PayToUID = "cea608fb-c327-4ba2-8cc1-ecc6cc482636",
        PaymentMethodUID = "b7784ef7-0d58-43df-a128-9b35e2da678e",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        PaymentAccountUID = "b5a5081a-7945-49da-9913-7c278880ba43",
        Description = "Updated by test",
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

    #endregion Facts

  }  // class PaymentOrderUseCasesTests

}  // namespace Empiria.Tests.Payments
