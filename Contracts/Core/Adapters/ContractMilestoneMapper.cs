/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone Management             Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractMilestoneMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract milestone related types.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

using Empiria.Documents.Services;
using Empiria.History.Services;

namespace Empiria.Contracts.Adapters {

  /// <summary>Provides data mapping services for contract milestone related types.</summary>
  static internal class ContractMilestoneMapper {

    static internal FixedList<ContractMilestoneDto> Map(FixedList<ContractMilestone> milestones) {
      return milestones.Select(x => Map(x))
                      .ToFixedList();
    }


    static internal ContractMilestoneDto Map(ContractMilestone milestone) {
      return new ContractMilestoneDto {
        UID = milestone.UID,
        Contract = milestone.Contract.MapToNamedEntity(),
        MilestoneNo = milestone.MilestoneNo,
        Name = milestone.Name,
        ManagedByOrgUnit = milestone.ManagedByOrgUnit.MapToNamedEntity(),
        Description = milestone.Description,
        Supplier = milestone.Supplier.MapToNamedEntity(),
        Total = milestone.GetTotal(),
        Status = milestone.Status.MapToDto(),
        Items = ContractMilestoneItemMapper.Map(milestone.GetItems()),
        Documents = DocumentServices.GetEntityDocuments(milestone),
        History = HistoryServices.GetEntityHistory(milestone),
        Actions = MapActions()
      };
    }


    static internal FixedList<ContractMilestoneDescriptor> MapToDescriptor(FixedList<ContractMilestone> milestone) {
      return milestone.Select(ContractMilestone => MapToDescriptor(ContractMilestone))
                .ToFixedList();

    }

    #region Helpers

    static private BaseActions MapActions() {
      return new BaseActions {
        CanEditDocuments = true
      };
    }


    static private ContractMilestoneDescriptor MapToDescriptor(ContractMilestone milestone) {
      return new ContractMilestoneDescriptor {
        UID = milestone.UID,
        ContractUID = milestone.Contract.UID,
        MilestoneNo = milestone.MilestoneNo,
        Name = milestone.Name,
        Description = milestone.Description,
        Supplier = milestone.Supplier.Id,
        StatusName = EntityStatusEnumExtensions.GetName(milestone.Status)
      };
    }

    #endregion Helpers

  }  // class ContractMilestoneMapper

}  // namespace Empiria.Contracts.Adapters
