/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone Management             Component : Test cases                              *
*  Assembly : Empiria.Contracts.Core.Tests.dll           Pattern   : Use cases tests                         *
*  Type     : ContractMilestoneUseCasesTests             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving accounts from the accounts chart.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;
using Empiria.Contracts.Adapters;
using Empiria.Contracts.UseCases;
using Empiria.Contracts;


namespace Empiria.Tests.Contracts
{

    /// <summary>Test cases contract milestone and contract milestone items.</summary>
    public class ContractMilestoneUseCasesTests
    {

        #region Use cases initialization

        private readonly ContractMilestoneUseCases _usecases;


        public ContractMilestoneUseCasesTests()
        {
            TestsCommonMethods.Authenticate();

            _usecases = ContractMilestoneUseCases.UseCaseInteractor();
        }

        ~ContractMilestoneUseCasesTests()
        {
            _usecases.Dispose();
        }

        #endregion Use cases initialization

        #region Facts

        [Fact]
        public void Should_Create_A_Contract_Milestone()
        {
            var fields = new ContractMilestoneFields
            {

                ContractUID = TestingConstants.CONTRACT_UID,
                MilestoneNo = "XXXX-XXX-XXXXX",
                Name = "Soporte del sistema SIAL 2024",
                Description = "Servicio unico soporte y mantenimiento del sistema SIAL, de acuerdo al contrato LIC/022/2024 Lobo Software Inc.",
                ManagedByOrgUnitUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
                SupplierUID = TestingConstants.SUPPLIER_UID,

            };

            ContractMilestoneDto sut = _usecases.CreateContractMilestone(fields);

            Assert.NotNull(sut);
            Assert.NotNull(sut.Contract.UID);

        }


        [Fact]
        public void Should_Remove_A_Contract_Milestone()
        {

            _usecases.RemoveContractMilestone(TestingConstants.CONTRACT_MILESTONE_UID);

        }


        [Fact]
        public void Should_Update_A_Contract_Milestone()
        {

            var fields = new ContractMilestoneFields
            {
                ContractUID = "d13fccb0-a5d0-419e-9204-777f57b6959d",
                Name = "BANOBRAS-2024-O-QQQQQQQ",
                Description = "Servicios de soporte técnico y mantenimiento al Sistema Fiduciario que opera en Banobras YATLA",
                MilestoneNo = "Soporte anio 2025",
                ManagedByOrgUnitUID = TestingConstants.MANAGED_BY_ORG_UNIT_UID,
                SupplierUID = TestingConstants.SUPPLIER_UID,
            };

            ContractMilestoneDto sut = _usecases.UpdateContractMilestone(TestingConstants.CONTRACT_MILESTONE_UID, fields);

            Assert.NotNull(sut);
            Assert.NotNull(sut.Contract.UID);
            Assert.Equal(fields.MilestoneNo, sut.MilestoneNo);
            Assert.Equal(fields.Name, sut.Name);
            Assert.Equal(fields.Description, sut.Description);

        }


        public ContractMilestoneDto ReadContractMilestone(string milestoneUID)
        {
            Assertion.Require(milestoneUID, nameof(milestoneUID));

            var milestone = ContractMilestone.Parse(milestoneUID);

            return ContractMilestoneMapper.Map(milestone);
        }


        [Fact]
        public void Should_Search_Contracts()
        {

            var query = new ContractMilestoneQuery
            {
                Keywords = "test",
            };

            //var sut = _usecases.SearchMileContracts(query);

            // Assert.NotNull(sut);

        }

        #endregion Facts

    }  // class ContractMilestoneUseCasesTests

}  // namespace Empiria.Tests.Contracts
