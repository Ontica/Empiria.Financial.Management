/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Use cases tests                         *
*  Type     : PayableUseCasesTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for payable objects use cases.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

using Empiria.Payments.Payables.UseCases;
using Empiria.Payments.Payables.Adapters;

namespace Empiria.Tests.Payments.Payables.UseCases {

  /// <summary>Test cases for payable objects use cases.</summary>
  public class PayableUseCasesTests {

    #region Use cases initialization

    private readonly PayableUseCases _usecases;

    public PayableUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = PayableUseCases.UseCaseInteractor();
    }

    ~PayableUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Add_PayableItem() {
      var fields = new PayableItemFields {
        PayableUID = "713b2755-aee1-44af-9f3c-1f46caebca1c",
        ProductUID = "",
        UnitUID = "",
        Description = "Agregado desde las pruebas",
        Quantity = 3m,
        UnitPrice = 10,
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        ExchangeRate = 1m,
        BudgetAccountUID = "ebc50e45-071d-4da4-8c63-0c54c10cfe0a"
      };

      var payableUID = "713b2755-aee1-44af-9f3c-1f46caebca1c";
      var sut = _usecases.AddPayableItem(payableUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Create_Payable() {
      var fields = new PayableFields {
        PayableTypeUID = "ObjectTypeInfo.Payable.ContractMilestone",
        PayableEntityUID = "0f2a2a72-0d6c-4dcb-84d1-98534ebb1713",
        Description = "Payable de Prueba",
        OrganizationalUnitUID = "d4b9aae9-cc6e-4fbc-9589-639dec5dab9f",
        PayToUID = "cea608fb-c327-4ba2-8cc1-ecc6cc482636",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        BudgetTypeUID = "ObjectTypeInfo.Budget.ProgramaFinanciero",
        DueTime = DateTime.Today,
    };

      var sut = _usecases.CreatePayable(fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Delete_Payable() {

      _usecases.DeletePayable("79cda212-9676-429b-b242-407614eb01db");
    }


    [Fact]
    public void Should_Delete_PayableItem() {

      _usecases.RemovePayableItem("713b2755-aee1-44af-9f3c-1f46caebca1c", "456e7c4d-a5b3-4f8a-bd8b-ad1d05d11240");
    }


    [Fact]
    public void Should_Get_Payable() {

      var sut = _usecases.GetPayable("79cda212-9676-429b-b242-407614eb01db");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_PayableItems() {

      var sut = _usecases.GetPayableItems("713b2755-aee1-44af-9f3c-1f46caebca1c");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_Payable_Types() {

      var sut = _usecases.GetPayableTypes();

     Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Search_Payables() {
      var query = new PayablesQuery {
        Keywords = "00005",
        FromDate = new DateTime(2024, 01, 01),
        ToDate = new DateTime(2028, 01, 01)
      };

      var sut = _usecases.SearchPayables(query);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Update_Payable() {
      var fields = new PayableFields {
        Description = "Actualziado desde las pruebas",
        OrganizationalUnitUID = "d4b9aae9-cc6e-4fbc-9589-639dec5dab9f",
        PayToUID = "c6278424-d1ff-492f-b5fe-410b4258292c",
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        BudgetTypeUID = "ObjectTypeInfo.Budget.ProgramaFinanciero",
        DueTime = DateTime.Today,
      };

      var payableUID = "abdc27b9-5fb1-4386-aa87-f5ad5ec66fea";

      var sut = _usecases.UpdatePayable(payableUID, fields);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Update_Payable_Payment() {
      var fields = new PayablePaymentFields {
        PaymentMethodUID = "ff779080-f58c-41ac-a48d-c1a00a2c5232", //"591e657c-229d-4d0b-9988-ce6b9489725e",
        PaymentAccountUID = ""
      };

      var payableUID = "abdc27b9-5fb1-4386-aa87-f5ad5ec66fea";

      var sut = _usecases.UpdatePayablePayment(payableUID, fields);

      Assert.NotNull(sut);
    }

    [Fact]
    public void Should_Update_PayableItem() {
      var fields = new PayableItemFields {
        PayableUID = "713b2755-aee1-44af-9f3c-1f46caebca1c",
        UID = "0785cf8e-fd06-4e41-ac1b-b2e3b09d12c2",
        ProductUID = "",
        UnitUID = "",
        Description = "Actualizando Items desde las pruebas",
        Quantity = 3m,
        UnitPrice = 10,
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        ExchangeRate = 1m,
        BudgetAccountUID = "ebc50e45-071d-4da4-8c63-0c54c10cfe0a"
      };

      var payableUID = "713b2755-aee1-44af-9f3c-1f46caebca1c";
      var payableItemUID = "0785cf8e-fd06-4e41-ac1b-b2e3b09d12c2";
      var sut = _usecases.UpdatePayableItem(payableUID,payableItemUID, fields);

      Assert.NotNull(sut);
    }

    #region PayableDataStructure

    [Fact]
    public void Should_Get_PayableData() {

      var sut = _usecases.GetPayableData("713b2755-aee1-44af-9f3c-1f46caebca1c");

      Assert.NotNull(sut);
    }

    #endregion PayableDataStructure

    #endregion Facts

  }  //  class PayableUseCasesTests

}  // namespace Empiria.Tests.Payments.Payables.UseCases
