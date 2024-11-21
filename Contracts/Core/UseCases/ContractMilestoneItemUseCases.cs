/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestones Management             Component : Use cases Layer                         *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : ContractMilestoneItemUseCases              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract milestone items management.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Contracts.Adapters;

namespace Empiria.Contracts.UseCases
{

    /// <summary>Use cases for contract milestone items management.</summary>
    public class ContractMilestoneItemUseCases : UseCase
    {

        #region Constructors and parsers

        protected ContractMilestoneItemUseCases()
        {
            // no-op
        }

        static public ContractMilestoneItemUseCases UseCaseInteractor()
        {
            return UseCase.CreateInstance<ContractMilestoneItemUseCases>();
        }

        #endregion Constructors and parsers

        #region Use cases

        public ContractMilestoneItemDto CreateContractMilestoneItem(ContractMilestoneItemFields fields)
        {

            Assertion.Require(fields, nameof(fields));

            fields.EnsureValid();

            var milestoneItem = new ContractMilestoneItem(fields);

            milestoneItem.Load(fields);

            milestoneItem.Save();

            return ContractMilestoneItemMapper.Map(milestoneItem);
        }


        public void RemoveContracMilestonetItem(string milestoneItemUID)
        {

            Assertion.Require(milestoneItemUID, nameof(milestoneItemUID));

            var milestoneItem = ContractMilestoneItem.Parse(milestoneItemUID);

            milestoneItem.Delete();

            milestoneItem.Save();
        }


        public ContractMilestoneItemDto GetContractMilestoneItem(string milestoneItemUID)
        {

            Assertion.Require(milestoneItemUID, nameof(milestoneItemUID));

            var milestoneItem = ContractMilestoneItem.Parse(milestoneItemUID);

            return ContractMilestoneItemMapper.Map(milestoneItem);
        }


        public ContractMilestoneItemDto UpdateContractMilestoneItem(string milestoneItemUID,
                                                                    ContractMilestoneItemFields fields)
        {

            Assertion.Require(fields, nameof(fields));

            fields.EnsureValid();

            var milestoneItem = ContractMilestoneItem.Parse(milestoneItemUID);

            milestoneItem.Load(fields);

            milestoneItem.Save();

            return ContractMilestoneItemMapper.Map(milestoneItem);
        }

        #endregion Use cases

    }  // class ContracMilestonetUseCases

}  // namespace Empiria.Contracts.UseCases
