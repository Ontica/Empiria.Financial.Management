﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PayableMapper                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable objects and related types.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Financial.Adapters;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable objects and related types.</summary>
  internal class PayableMapper {

    static internal PayableDto Map(Payable payable) {
      return new PayableDto {
        UID = payable.UID,
        PayableNo = payable.PayableNo,
        PayableType = payable.PayableType.MapToNamedEntity(),
        Description = payable.Description,
        PayTo = payable.PayTo.MapToNamedEntity(),
        RequestedBy = payable.OrganizationalUnit.MapToNamedEntity(),
        BudgetType = payable.Budget.BudgetType.MapToNamedEntity(),
        Budget = payable.Budget.MapToNamedEntity(),
        // ExchangeRateType = payable.ExchangeRateType,
        ExchangeRate = payable.ExchangeRate,
        Total = payable.Total,
        Currency = payable.Currency.MapToNamedEntity(),
        PaymentMethod = PaymentMethodMapper.Map(payable.PaymentMethod),
        PaymentAccount = payable.PaymentAccount.MapToNamedEntity(),

        RequestedTime = payable.RequestedTime,
        DueTime = payable.DueTime,
        Status = payable.Status.MapToNamedEntity()
      };

    }


    static internal FixedList<PayableDescriptor> MapToDescriptor(FixedList<Payable> orders) {
      return orders.Select(x => MapToDescriptor(x)).ToFixedList();
    }


    static internal PayableDescriptor MapToDescriptor(Payable payable) {
      return new PayableDescriptor {
        UID = payable.UID,
        PayableNo = payable.PayableNo,
        PayableTypeName = payable.PayableType.DisplayName,
        BudgetTypeName = payable.Budget.BudgetType.DisplayName,
        BudgetName = payable.Budget.Name,
        ContractNo = payable.PayableEntity.EntityNo,
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