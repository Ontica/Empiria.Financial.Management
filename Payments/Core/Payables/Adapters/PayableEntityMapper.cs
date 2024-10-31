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

using Empiria.Financial;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable entities and their items.</summary>
  internal class PayableEntityMapper {

    static internal FixedList<PayableEntityDto> Map(IEnumerable<IPayableEntity> entities) {
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
