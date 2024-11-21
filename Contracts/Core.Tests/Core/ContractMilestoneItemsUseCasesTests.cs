/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone items Management       Component : Test cases                              *
*  Assembly : Empiria.Contracts.Core.Tests.dll           Pattern   : Use cases tests                         *
*  Type     : ContractMilestoneItemsUseCasesTests        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving milestones items.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;
using Empiria.Contracts.Adapters;
using Empiria.Contracts.UseCases;

namespace Empiria.Tests.Contracts
{

    /// <summary>Test cases contract milestone items.</summary>
    public class ContractMilestoneItemUseCasesTests
    {

        #region Use cases initialization

        private readonly ContractMilestoneItemUseCases _usecases;


        public ContractMilestoneItemUseCasesTests()
        {
            TestsCommonMethods.Authenticate();

            _usecases = ContractMilestoneItemUseCases.UseCaseInteractor();
        }

        ~ContractMilestoneItemUseCasesTests()
        {
            _usecases.Dispose();
        }

        #endregion Use cases initialization

        #region Facts

        [Fact]
        public void Should_Create_A_Contract_Milestone_Items()
        {
            var fields = new ContractMilestoneItemFields
            {
                UID = TestingConstants.CONTRACT_MILESTONE_ITEM_UID,
                MilestoneUID = TestingConstants.CONTRACT_MILESTONE_UID,
                ContractItemUID = TestingConstants.CONTRACT_ITEM_UID,
                Description = "Servicio unico soporte y mantenimiento del sistema SIAL, de acuerdo al contrato LIC/022/2025 Lobo Software Inc.",
                Quantity = 100,
                ProductUnitUID = TestingConstants.CONTRACT_PRODUCT_UNIT_UID,
                ProductUID = TestingConstants.CONTRACT_MILESTONE_PRODUCT_UID,
                UnitPrice = 2024,
                BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,

            };

            ContractMilestoneItemDto sut = _usecases.CreateContractMilestoneItem(fields);

            Assert.NotNull(sut);
            Assert.NotNull(sut.Description);
            Assert.NotNull(sut.Product);

        } // Should_Create_A_Contract_Milestone_Items


        [Fact]
        public void Should_Update_A_Contract_Milestone_Item()
        {

            var fields = new ContractMilestoneItemFields
            {

                MilestoneUID = TestingConstants.CONTRACT_MILESTONE_UID,
                ContractItemUID = TestingConstants.CONTRACT_ITEM_UID,
                Description = "Servicio de soporte y mantenimiento del sistema SIAL, contrato LIC/022/2025 Lobo Software Inc.",
                Quantity = 180,
                ProductUnitUID = TestingConstants.CONTRACT_PRODUCT_UNIT_UID,
                ProductUID = TestingConstants.CONTRACT_MILESTONE_PRODUCT_UID,
                UnitPrice = 10900,
                BudgetAccountUID = TestingConstants.CONTRACT_BUDGET_ACCOUNT_UID,
            };

            ContractMilestoneItemDto sut = _usecases.UpdateContractMilestoneItem(TestingConstants.CONTRACT_MILESTONE_ITEM_UID, fields);

            Assert.NotNull(sut);
            Assert.NotNull(sut.UID);

        } // Should_Update_A_Contract_Milestone_Item


        [Fact]
        public void Should_Remove_A_Contract_Milestone_Item()
        {

            _usecases.RemoveContracMilestonetItem(TestingConstants.CONTRACT_MILESTONE_ITEM_UID);

        } // Should_Remove_A_Contract_Milestone_Item


        #endregion Facts


    } // class ContractMilestoneItemUseCasesTests

} // namespace Empiria.Tests.Contracts