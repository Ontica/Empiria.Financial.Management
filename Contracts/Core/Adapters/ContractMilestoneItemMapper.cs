﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractMilestoneItemMapper                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for ContractMilestoneItem instances.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts.Adapters {

  /// <summary>Provides data mapping services for ContractMilestoneItem instances.</summary>
  static internal class ContractMilestoneItemMapper {

    static internal FixedList<ContractMilestoneItemDto> Map(FixedList<ContractMilestoneItem> contractsItem) {
      return contractsItem.Select(x => Map(x))
                      .ToFixedList();
    }


    static internal ContractMilestoneItemDto Map(ContractMilestoneItem milestoneItem) {
      return new ContractMilestoneItemDto {
        UID = milestoneItem.UID,
        ContractMilestone = new NamedEntityDto(milestoneItem.ContractMilestone.UID, milestoneItem.ContractMilestone.Name),
        ContractItem = new NamedEntityDto(milestoneItem.ContractItem.UID, milestoneItem.ContractItem.Description),
        Description = milestoneItem.Description,
        Product = milestoneItem.Product.MapToNamedEntity(),
        ProductUnit = milestoneItem.ProductUnit.MapToNamedEntity(),
        Quantity = milestoneItem.Quantity,
        UnitPrice = milestoneItem.UnitPrice,
        BudgetAccount = new NamedEntityDto(milestoneItem.BudgetAccount.UID, milestoneItem.BudgetAccount.Code),
      };

    }

  }  // class ContractItemMapper

}  // namespace Empiria.Contracts.Adapters