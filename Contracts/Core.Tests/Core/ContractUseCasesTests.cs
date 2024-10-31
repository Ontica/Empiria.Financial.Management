/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Contracts.Core.Tests.dll           Pattern   : Use cases tests                         *
*  Type     : ContractUseCasesTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving accounts from the accounts chart.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Contracts.Adapters;
using Empiria.Contracts.UseCases;

namespace Empiria.Tests.Contracts {

  /// <summary>Test cases for retrieving accounts from the accounts chart.</summary>
  public class ContractUseCasesTests {

    #region Use cases initialization

    private readonly ContractUseCases _usecases;
    private readonly ContractItemUseCases _itemusecases;


    public ContractUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = ContractUseCases.UseCaseInteractor();
      _itemusecases = ContractItemUseCases.UseCaseInteractor();
    }

    ~ContractUseCasesTests() {
      _usecases.Dispose();
      _itemusecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Add_A_Contract() {
      var fields = new ContractFields {
        ContractTypeUID = TestingConstants.CONTRACT_TYPE_UID,
        ContractNo = "DAGA/146/2023",
        Name = "BANOBRAS-2023-O-00ABCD",
        Description = "Fábrica de Software 2023-2024",
        CurrencyUID = TestingConstants.CONTRACT_CURRENCY_UID,
        FromDate = new DateTime(2023, 11, 07),
        ToDate = new DateTime(2024, 12, 31),
        SignDate = new DateTime(2023, 11, 07),
        ManagedByOrgUnitUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        BudgetTypeUID = TestingConstants.CONTRACT_BUDGET_TYPE_UID,
        SupplierUID = TestingConstants.SUPPLIER_UID,
        ParentUID = "Empty",
        Total = 237762005.00M,
      };

      ContractDto sut = _usecases.AddContract(fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.UID);
      Assert.Equal(fields.ContractNo, sut.ContractNo);
      Assert.Equal(fields.Name, sut.Name);
      Assert.Equal(fields.Total, sut.Total);
    }

    [Fact]
    public void Should_Update_A_Contract() {
      var fields = new ContractFields {
        ContractTypeUID = TestingConstants.CONTRACT_TYPE_UID,
        ContractNo = "DAGA/031/2022",
        Name = "BANOBRAS-2024-O-XXXXXX",
        Description = "Servicios de soporte técnico y mantenimiento al Sistema Fiduciario que opera en Banobras YATLA",
        CurrencyUID = TestingConstants.CONTRACT_CURRENCY_UID,
        FromDate = new DateTime(2022, 09, 01),
        ToDate = new DateTime(2024, 08, 31),
        SignDate = new DateTime(2022, 09, 01),
        ManagedByOrgUnitUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        BudgetTypeUID = TestingConstants.CONTRACT_BUDGET_TYPE_UID,
        SupplierUID = TestingConstants.SUPPLIER_UID,
        ParentUID = "Empty",
        Total = 9300402.32M,
      };

      ContractDto sut = _usecases.UpdateContract(TestingConstants.CONTRACT_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.UID);
      Assert.Equal(fields.ContractNo, sut.ContractNo);
      Assert.Equal(fields.Name, sut.Name);
      Assert.Equal(fields.Total, sut.Total);
    }


    [Fact]
    public void Should_Add_A_Contract_Item() {

      var fields = new ContractItemFields {

        ContractUID = TestingConstants.CONTRACT_UID,
        ProductUID = TestingConstants.CONTRACT_ITEM_PRODUCT_UID,
        Description = "Prueba contract items  2000",
        UnitMeasureUID = TestingConstants.CONTRACT_ITEM_UNIT_UID,
        ToQuantity = 5,
        FromQuantity = 2,
        UnitPrice = 20,
        BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,
        ProjectUID = TestingConstants.CONTRACT_ITEM_PROJECT_UID,
        PaymentPeriodicityUID = TestingConstants.CONTRACT_ITEM_PYM_PER_UID
      };

      ContractItemDto sut = _itemusecases.AddContractItem(TestingConstants.CONTRACT_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.UID);
    }


    [Fact]
    public void Should_Read_A_Contract() {

      ContractDto sut = _usecases.GetContract(TestingConstants.CONTRACT_UID);

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Read_A_Contract_Item() {

      ContractItemDto sut = _itemusecases.GetContractItem(TestingConstants.CONTRACT_ITEM_UID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_A_Contract_Items() {

      FixedList<ContractItemDto> sut = _usecases.GetContractItems(TestingConstants.CONTRACT_UID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Remove_A_Contract_Item() {

      _itemusecases.DeleteContractItem(TestingConstants.CONTRACT_ITEM_UID);

    }


    [Fact]
    public void Should_Update_A_Contract_Item() {

      var fields = new ContractItemFields {

        ContractUID = TestingConstants.CONTRACT_UID,
        ProductUID = TestingConstants.CONTRACT_ITEM_PRODUCT_UID,
        Description = "Prueba contract item modificar en test",
        UnitMeasureUID = TestingConstants.CONTRACT_ITEM_UNIT_UID,
        ToQuantity = 10,
        FromQuantity = 5,
        UnitPrice = 20,
        BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,
        ProjectUID = TestingConstants.CONTRACT_ITEM_PROJECT_UID,
        PaymentPeriodicityUID = TestingConstants.CONTRACT_ITEM_PYM_PER_UID
      };

      ContractItemDto sut = _itemusecases.UpdateContractItem(TestingConstants.CONTRACT_ITEM_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.UID);
    }


    [Fact]
    public void Should_Search_Contracts() {

      var query = new ContractQuery {
        Keywords = "test",
      };

      var sut = _usecases.SearchContracts(query);

      Assert.NotNull(sut);

    }

    #endregion Facts

  }  // class ContractUseCasesTests

}  // namespace Empiria.Tests.Contracts
