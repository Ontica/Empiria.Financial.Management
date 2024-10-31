/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract milestones Management             Component : Use cases Layer                         *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : ContractMilestoneUseCases                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract milestone management.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Contracts.Data;
using Empiria.Contracts.Adapters;

namespace Empiria.Contracts.UseCases {

  /// <summary>Use cases for contract milestone management.</summary>
  public class ContractMilestoneUseCases : UseCase {

    #region Constructors and parsers

    protected ContractMilestoneUseCases() {
      // no-op
    }

    static public ContractMilestoneUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ContractMilestoneUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractMilestoneDto AddContractMlestone(ContractMilestoneFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contractMilestone = new ContractMilestone();

      contractMilestone.Load(fields);

      contractMilestone.Save();

      return ContractMilestoneMapper.Map(contractMilestone);
    }

    
    public ContractMilestoneDto GetContractMilestone(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contractMilestone = ContractMilestone.Parse(contractUID);

      return ContractMilestoneMapper.Map(contractMilestone);
    }


    public FixedList<ContractMilestoneItemDto> GetContracMilestonetItems(string contractMilestoneUID) {
      Assertion.Require(contractMilestoneUID, nameof(contractMilestoneUID));

      var contractMilestone = ContractMilestone.Parse(contractMilestoneUID);

      FixedList<ContractMilestoneItem> items = contractMilestone.GetItems();

      return ContractMilestoneItemMapper.Map(items);
    }


    public FixedList<ContractMilestoneDescriptor> SearchContractMilestone(ContractMilestoneQuery query) {
      Assertion.Require(query, nameof(query));

      /*
      string condition = query.MapToFilterString();
      string orderBy = query.MapToSortString();
      */
     
      FixedList<ContractMilestone> contractMilestone = new FixedList<ContractMilestone>();
      
      //ContractMilestoneData.GetContractMilestone(condition, orderBy);

      return ContractMilestoneMapper.MapToDescriptor(contractMilestone);
    }
    
    public ContractMilestoneDto UpdateContractMilestone(string ContractUID,
                                               ContractMilestoneFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contractMilestone = ContractMilestone.Parse(ContractUID);

      contractMilestone.Load(fields);

      contractMilestone.Save();

      return ContractMilestoneMapper.Map(contractMilestone);
    }


    #endregion Use cases

  }  // class ContractMilestoneUseCases

}  // namespace Empiria.Contracts.UseCases
