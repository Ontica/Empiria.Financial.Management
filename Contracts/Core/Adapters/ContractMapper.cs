/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract related types.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts.Adapters {

  /// <summary>Provides data mapping services for Contract related types.</summary>
  static internal class ContractMapper {

    static internal FixedList<ContractDto> Map(FixedList<Contract> contracts) {
      return contracts.Select(x => Map(x))
                      .ToFixedList();
    }


    static internal ContractDto Map(Contract contract) {
      return new ContractDto {
        UID = contract.UID,
        ContractNo = contract.ContractNo,
        Name = contract.Name,
        SignDate = contract.SignDate,
        Total = contract.Total,
      };
    }


    static internal FixedList<ContractDescriptor> MapToDescriptor(FixedList<Contract> contracts) {
      return contracts.Select(contract => MapToDescriptor(contract))
                .ToFixedList();

    }


    private static ContractDescriptor MapToDescriptor(Contract contract) {
      return new ContractDescriptor {
        UID = contract.UID,
        ContractNo = contract.ContractNo,
        Name = contract.Name,
        Description = contract.Description,
        SignDate = contract.SignDate,
        Supplier = contract.Supplier.Name,
        BudgetType = contract.BudgetType.DisplayName,
        ManagedByOrgUnit = contract.ManagedByOrgUnit.FullName,
        ContractType = contract.ContractType.Name,
        Currency = contract.Currency.Name,
        ToDate = contract.ToDate,
        FromDate = contract.ToDate,
      };

    }

  }  // class ContractMapper

}  // namespace Empiria.Contracts.Adapters
