﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone Management             Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractMilestoneMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract milestone related types.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

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
        Contract = milestone.Contract,
        MilestoneNo = milestone.MilestoneNo,  
        Name = milestone.Name,
        Description = milestone.Description,
        Supplier = milestone.Supplier,
        //PaymentExtData = milestone.PaymentData.,
      };
    }


    static internal FixedList<ContractMilestoneDescriptor> MapToDescriptor(FixedList<ContractMilestone> milestone) {
      return milestone.Select(ContractMilestone => MapToDescriptor(ContractMilestone))
                .ToFixedList();

    }


    private static ContractMilestoneDescriptor MapToDescriptor(ContractMilestone milestone) {
      return new ContractMilestoneDescriptor {
        UID = milestone.UID,
        Contract = milestone.Contract.Id,
        MilestoneNo = milestone.MilestoneNo,
        Name = milestone.Name,
        Description = milestone.Description,
        Supplier = milestone.Supplier.Id,
        statusName = EntityStatusEnumExtensions.GetName(milestone.Status)
      };

    }

  }  // class ContractMilestoneMapper

}  // namespace Empiria.Contracts.Adapters
