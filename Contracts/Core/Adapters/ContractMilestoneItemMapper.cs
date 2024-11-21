/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractMilestoneItemMapper                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for ContractMilestoneItem instances.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Contracts.Adapters {

  /// <summary>Provides data mapping services for ContractMilestoneItem instances.</summary>
  static internal class ContractMilestoneItemMapper {

    static internal FixedList<ContractMilestoneItemDto> Map(FixedList<ContractMilestoneItem> milestoneItem) {
      return milestoneItem.Select(x => Map(x))
                      .ToFixedList();
    }


    static internal ContractMilestoneItemDto Map(ContractMilestoneItem milestoneItem) {
      return new ContractMilestoneItemDto {
        UID = milestoneItem.UID,
        ContractMilestone = milestoneItem.ContractMilestone.MapToNamedEntity(),
        ContractItem = milestoneItem.ContractItem.MapToNamedEntity(),
        Description = milestoneItem.Description,
        Quantity = milestoneItem.Quantity,
        ProductUnit = milestoneItem.ProductUnit.MapToNamedEntity(),
        Product = milestoneItem.Product.MapToNamedEntity(),
        UnitPrice = milestoneItem.UnitPrice,
        BudgetAccount = milestoneItem.BudgetAccount.MapToNamedEntity(),
        Status = milestoneItem.Status.MapToDto()
      };

    }

  }  // class ContractItemMapper

}  // namespace Empiria.Contracts.Adapters
