/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone Management             Component : Test cases                              *
*  Assembly : Empiria.Contracts.Core.Tests.dll           Pattern   : Use cases tests                         *
*  Type     : ContractMilestoneUseCasesTests             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving accounts from the accounts chart.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Contracts.Adapters;
using Empiria.Contracts.UseCases;
using System.Security.Cryptography;

namespace Empiria.Tests.Contracts {

  /// <summary>Test cases contract milestone and contract milestone items.</summary>
  public class ContractMilestoneUseCasesTests {

    #region Use cases initialization

    private readonly ContractMilestoneUseCases _usecases;
    private readonly ContractMilestoneItemUseCases _itemusecases;


    public ContractMilestoneUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = ContractMilestoneUseCases.UseCaseInteractor();
      _itemusecases = ContractMilestoneItemUseCases.UseCaseInteractor();
    }

    ~ContractMilestoneUseCasesTests() {
      _usecases.Dispose();
      _itemusecases.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Add_A_Contract_Milestone() {
      var fields = new ContractMilestoneFields {

        ContractUID = TestingConstants.CONTRACT_UID, 
        MilestoneNo = "Soporte del sistema LOBO 2024",
        Name = "Soporte del sistema SIAL", 
        Description = "Servicio unico soporte y mantenimiento del sistema SIAL, de acuerdo al contrato LIC/022/2024 Lobo Software Inc.",
        ManagedByOrgUnitUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        SupplierUID = TestingConstants.SUPPLIER_UID,

      };

      ContractMilestoneDto sut = _usecases.AddContractMilestone(fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.Contract.UID);

    }

    [Fact]
    public void Should_Update_A_Contract_Milestone() {
      var fields = new ContractMilestoneFields {
        Name = "BANOBRAS-2024-O-XXXXXX",
        Description = "Servicios de soporte técnico y mantenimiento al Sistema Fiduciario que opera en Banobras YATLA",
        ManagedByOrgUnitUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
        SupplierUID = TestingConstants.SUPPLIER_UID,
      };

      /*
            ContractHolderDto sut = _usecases.UpdateContract(TestingConstants.CONTRACT_UID, fields);

      Assert.NotNull(sut);
      Assert.NotNull(sut.Contract.UID);
      Assert.Equal(fields.ContractNo, sut.Contract.ContractNo);
      Assert.Equal(fields.Name, sut.Contract.Name);
      Assert.Equal(fields.Total, sut.Contract.Total);
      */
    }


    [Fact]
    public void Should_Add_A_Contract_Milestone_Item() {

            var fields = new ContractMilestoneItemFields {

                MilestoneUID = TestingConstants.CONTRACT_MILESTONE_UID,
                ContractItemUID = TestingConstants.CONTRACT_ITEM_UID,
                Description = "Pago unico",
                Quantity = 120,
                ProductUnitUID = TestingConstants.CONTRACT_MILESTONE_ITEM_UNIT_UID,
                ProductUID = TestingConstants.CONTRACT_ITEM_PRODUCT_UID,
                UnitPrice = 1800,
                BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,
            };

            ContractMilestoneItemDto sut = _itemusecases.AddContractMilestoneItem(TestingConstants.CONTRACT_MILESTONE_UID, fields);

            Assert.NotNull(sut);
            Assert.NotNull(sut.UID);
    }


    [Fact]
    public void Should_Read_A_Contract() {

    //  ContractHolderDto sut = _usecases.GetContractMilestone(TestingConstants.CONTRACT_UID);

    //  Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Read_A_Contract_Milestone_Item() {

      ContractMilestoneItemDto sut = _itemusecases.GetContractMilestoneItem(TestingConstants.CONTRACT_MILESTONE_ITEM_UID);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_A_Contract_Items() {

    //  FixedList<ContractItemDto> sut = _usecases.GetContractItems(TestingConstants.CONTRACT_UID);

    //  Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Remove_A_Contract_Item() {

     // _itemusecases.DeleteContractMilestoneItem(TestingConstants.CONTRACT_MILESTONE_ITEM_UID);

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

      // ContractItemDto sut = _itemusecases.UpdateContractMilestoneItem(TestingConstants.CONTRACT_MILESTONE_ITEM_UID, fields);

      // Assert.NotNull(sut);
      // Assert.NotNull(sut.UID);
    }


    [Fact]
    public void Should_Search_Contracts() {

      var query = new ContractQuery {
        Keywords = "test",
      };

      //var sut = _usecases.SearchContracts(query);

      // Assert.NotNull(sut);

    }

    #endregion Facts

  }  // class ContractMilestoneUseCasesTests

}  // namespace Empiria.Tests.Contracts
