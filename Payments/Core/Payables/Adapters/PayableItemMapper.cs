/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PayableItemMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable items.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable items.</summary>
  internal class PayableItemMapper {

    #region Methods

    static internal FixedList<PayableItemDto> Map(FixedList<PayableItem> payableItems) {
      return payableItems.Select(x => Map(x)).ToFixedList();
    }

    static internal PayableItemDto Map(PayableItem payableItem) {
      return new PayableItemDto {
        UID = payableItem.UID,
        OutputTotal = payableItem.OutputTotal,
        InputTotal = payableItem.InputTotal,
        Currency = payableItem.Currency.MapToNamedEntity(),
        ExchangeRate = payableItem.ExchangeRate,
        Status = payableItem.Status.MapToDto()
      };
    }


    #endregion Methods

  } // class PayableItemMapper

} // namespace Empiria.Payments.Payables.Adapters