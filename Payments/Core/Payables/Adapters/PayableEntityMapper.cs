/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PayableEntityMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable entities and their items.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;
using Empiria.Budgeting;
using Empiria.Financial;
using Empiria.Financial.Services;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable entities and their items.</summary>
  public class PayableEntityMapper {

    static public FixedList<PayableEntityDto> Map(IEnumerable<IPayableEntity> entities) {
      return entities.Select(x => Map(x))
                     .ToFixedList();
    }


    static internal PayableEntityDto Map(IPayableEntity payableEntity) {
      return new PayableEntityDto {
        UID = payableEntity.UID,
        Type = payableEntity.Type.MapToNamedEntity(),
        EntityNo = payableEntity.EntityNo,
        Name = payableEntity.Name,
        Description = payableEntity.Description,
        Budget = Budget.Parse(1).MapToNamedEntity(),
        Currency = Currency.Parse(600).MapToNamedEntity(),
        PayTo = payableEntity.PayTo.MapToNamedEntity(),
        PaymentAccounts = PaymentAccountServices.GetPaymentAccounts(payableEntity.PayTo.UID),
        Items = MapItems(payableEntity.Items)
      };
    }

    #region Helpers

    static private FixedList<PayableEntityItemDto> MapItems(IEnumerable<IPayableEntityItem> items) {
      return items.Select(x => MapItem(x))
                  .ToFixedList();
    }


    static private PayableEntityItemDto MapItem(IPayableEntityItem item) {
      return new PayableEntityItemDto {
        UID = item.UID,
        Quantity = item.Quantity,
        Unit = item.Unit.MapToNamedEntity(),
        Product = item.Product.MapToNamedEntity(),
        Description = item.Description,
        UnitPrice = item.UnitPrice,
        Total = item.Total,
        BudgetAccount = item.BudgetAccount.MapToNamedEntity(),
      };
    }

    #endregion Helpers

  } // class PayableEntityMapper

} // namespace Empiria.Payments.Payables.Adapters
