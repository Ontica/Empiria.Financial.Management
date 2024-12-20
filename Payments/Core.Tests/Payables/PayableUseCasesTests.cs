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
using Empiria.Payments.Payables;

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
        Description = "Prueba del dia 1 Noviembre",
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
        PayableTypeUID = "ObjectTypeInfo.Payable.ContractOrder",
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

      _usecases.RemovePayableItem("713b2755-aee1-44af-9f3c-1f46caebca1c", "675f4977-d3cd-4114-953f-8c46f8a3ace3");
    }


    [Fact]
    public void Should_Generate_Payable_Items() {
      Payable payable = Payable.Parse("c62a8523-a9a9-4202-afe1-3b08ca871daf");

      PayableItemGenerator generator = new PayableItemGenerator(payable);
      var sut = generator.Generate();

      Assert.NotNull(sut);
      Assert.True(sut.Count > 0);
    }


    [Fact]
    public void Should_Get_Payable() {

      var sut = _usecases.GetPayable("f578ae68-8570-4e86-b4d1-6f2fcc6995f2");

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_PayableItems() {

      var sut = _usecases.GetPayableItems("1537c110-e3f1-4ab8-b822-c00733ac3dd5");

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
    public void Should_Set_Payment_Instruction() {
      var payableUID = "abdc27b9-5fb1-4386-aa87-f5ad5ec66fea";

      var sut = _usecases.SetPaymentInstruction(payableUID);

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
    public void Should_Update_PayableItem() {
      var fields = new PayableItemFields {
        UID = "675f4977-d3cd-4114-953f-8c46f8a3ace3",
        PayableUID = "713b2755-aee1-44af-9f3c-1f46caebca1c",
        ProductUID = "675f4977-d3cd-4114-953f-8c46f8a3ace3",
        UnitUID = "",
        Description = "Items actualizados desde las pruebas el 1 Nov",
        Quantity = 3m,
        UnitPrice = 10,
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e",
        ExchangeRate = 1m,
        BudgetAccountUID = "ebc50e45-071d-4da4-8c63-0c54c10cfe0a"
      };

      var payableUID = "713b2755-aee1-44af-9f3c-1f46caebca1c";
      var payableItemUID = "675f4977-d3cd-4114-953f-8c46f8a3ace3";
      var sut = _usecases.UpdatePayableItem(payableUID,payableItemUID, fields);

      Assert.NotNull(sut);
    }

    #region PayableDataStructure



    #endregion PayableDataStructure

    #endregion Facts

  }  //  class PayableUseCasesTests

}  // namespace Empiria.Tests.Payments.Payables.UseCases
