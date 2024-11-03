/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Mapper                                  *
*  Type     : ContractMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for Contract related types.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

using Empiria.StateEnums;

namespace Empiria.Contracts.Adapters {

  /// <summary>Provides data mapping services for contract milestone related types.</summary>
  static internal class ContractMapper {

    static internal ContractHolderDto Map(Contract contract) {
      return new ContractHolderDto {
        Contract = MapContract(contract),
        Items = ContractItemMapper.Map(contract.GetItems()),
        Milestones = ContractMilestoneMapper.Map(contract.GetMilestones()),
        Documents = DocumentServices.GetEntityDocuments(contract),
        History = HistoryServices.GetEntityHistory(contract),
      };
    }

    static internal ContractDto MapContract(Contract contract) {
      return new ContractDto {
        UID = contract.UID,
        ContractType = contract.ContractType.MapToNamedEntity(),
        ContractNo = contract.ContractNo,
        Name = contract.Name,
        Description = contract.Description,
        Supplier = contract.Supplier.MapToNamedEntity(),
        ManagedByOrgUnit = contract.ManagedByOrgUnit.MapToNamedEntity(),
        BudgetType = contract.BudgetType.MapToNamedEntity(),
        FromDate = contract.FromDate,
        ToDate = contract.ToDate,
        SignDate = contract.SignDate,
        Currency = contract.Currency.MapToNamedEntity(),
        Total = contract.Total,
        Status = contract.Status.MapToDto()
      };
    }


    static internal FixedList<ContractDescriptor> MapToDescriptor(FixedList<Contract> contracts) {
      return contracts.Select(contract => MapToDescriptor(contract))
                       .ToFixedList();
    }


    static private ContractDescriptor MapToDescriptor(Contract contract) {
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
        FromDate = contract.FromDate,
        StatusName = contract.Status.GetName(),
        Total = contract.Total,
      };
    }

  }  // class ContractMapper

}  // namespace Empiria.Contracts.Adapters
