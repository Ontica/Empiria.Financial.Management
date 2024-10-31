/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractItemMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for ContractItem instances.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts.Adapters {

  /// <summary>Provides data mapping services for ContractItem instances.</summary>
  static internal class ContractItemMapper {

    static internal FixedList<ContractItemDto> Map(FixedList<ContractItem> contractsItem) {
      return contractsItem.Select(x => Map(x))
                      .ToFixedList();
    }


    static internal ContractItemDto Map(ContractItem contractItem) {
      return new ContractItemDto {
        UID = contractItem.UID,
        Contract = contractItem.Contract.MapToNamedEntity(),
        Supplier = contractItem.Supplier.MapToNamedEntity(),
        Product = contractItem.Product.MapToNamedEntity(),
        Description = contractItem.Description,
        Unit = contractItem.UnitMeasure.MapToNamedEntity(),
        FromQuantity = contractItem.FromQuantity,
        ToQuantity = contractItem.ToQuantity,
        UnitPrice = contractItem.UnitPrice,
        Project = contractItem.Project.MapToNamedEntity(),
        Periodicity = contractItem.PaymentsPeriodicity.MapToNamedEntity(),
        BudgetAccount = contractItem.BudgetAccount.MapToNamedEntity()
      };

    }

  }  // class ContractItemMapper

}  // namespace Empiria.Contracts.Adapters
