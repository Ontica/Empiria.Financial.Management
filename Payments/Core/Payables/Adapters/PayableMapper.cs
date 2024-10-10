/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PayableMapper                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable objects and related types.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable objects and related types.</summary>
  internal class PayableMapper {

    static internal PayableDto Map(Payable payable) {
      return new PayableDto {
        UID = payable.UID,
        PayableNo = payable.payableNo,
        PayableType = new NamedEntityDto(payable.PayableType.UID, payable.PayableType.DisplayName),
        Description = payable.Description,
        PayTo = payable.PayTo.MapToNamedEntity(),
        RequestedBy = new NamedEntityDto(payable.OrganizationalUnit.UID, payable.OrganizationalUnit.Name),
        BudgetType = new NamedEntityDto(payable.BudgetType.UID, payable.BudgetType.Name),
        Total = payable.Total,
        Currency = payable.Currency.Name,
        RequestedTime = payable.RequestedTime,
        DueTime = payable.DueTime,
        Status = new NamedEntityDto(payable.Status.ToString(), payable.Status.GetName())
      };

    }

    static internal ContractMilestoneDto Map(ContractMilestone contractMilestone) {
      return new ContractMilestoneDto {
        UID = contractMilestone.UID,
        Contract = contractMilestone.Contract.MapToNamedEntity(),
        PayableNo = contractMilestone.payableNo,
        PayableType = new NamedEntityDto(contractMilestone.PayableType.UID, contractMilestone.PayableType.DisplayName),
        Description = contractMilestone.Description,
        PayTo = contractMilestone.PayTo.MapToNamedEntity(),
        RequestedBy = new NamedEntityDto(contractMilestone.OrganizationalUnit.UID, contractMilestone.OrganizationalUnit.Name),
        BudgetType = new NamedEntityDto(contractMilestone.BudgetType.UID, contractMilestone.BudgetType.Name),
        Total = contractMilestone.Total,
        Currency = contractMilestone.Currency.Name,
        RequestedTime = contractMilestone.RequestedTime,
        DueTime = contractMilestone.DueTime,
        Status = new NamedEntityDto(contractMilestone.Status.ToString(), contractMilestone.Status.GetName())
      };

    }

    static internal FixedList<PayableDescriptor> MapToDescriptor(FixedList<Payable> orders) {
      return orders.Select(x => MapToDescriptor(x)).ToFixedList();
    }


    static internal PayableDescriptor MapToDescriptor(Payable payable) {
      return new PayableDescriptor {
        UID = payable.UID,
        PayableNo = payable.payableNo,
        PayableTypeName = payable.PayableType.DisplayName,
        BudgetTypeName = payable.BudgetType.DisplayName,
        ContractNo = payable is ContractMilestone ? ((ContractMilestone) payable).Contract.ContractNo : "ND",
        PayTo = payable.PayTo.Name,
        Total = payable.Total,
        CurrencyCode = payable.Currency.ISOCode,
        DueTime = payable.DueTime,
        RequestedTime = payable.RequestedTime,
        RequestedBy = payable.OrganizationalUnit.Name,
        StatusName = payable.Status.GetName()
      };
    }

  } // class PayableMapper

} // namespace Empiria.Payments.Payables.Adapters