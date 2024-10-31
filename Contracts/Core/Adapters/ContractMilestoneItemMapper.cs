/* Empiria Financial *****************************************************************************************
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
        ContractMilestone = milestoneItem.ContractMilestone.MapToNamedEntity(),
        ContractItem = milestoneItem.ContractItem.MapToNamedEntity(),
        Description = milestoneItem.Description,
        Product = milestoneItem.Product.MapToNamedEntity(),
        ProductUnit = milestoneItem.ProductUnit.MapToNamedEntity(),
        Quantity = milestoneItem.Quantity,
        UnitPrice = milestoneItem.UnitPrice,
        BudgetAccount = milestoneItem.BudgetAccount.MapToNamedEntity(),
      };

    }

  }  // class ContractItemMapper

}  // namespace Empiria.Contracts.Adapters
