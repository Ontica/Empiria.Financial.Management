/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : PayableItemMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable items.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable items.</summary>
  internal class PayableItemMapper {

    #region Methods

    static internal FixedList<PayableItemDto> Map(FixedList<IPayableEntityItem> items) {
      return items.Select(x => Map(x)).ToFixedList();
    }

    static internal PayableItemDto Map(IPayableEntityItem payableItem) {
      return new PayableItemDto {
        UID = payableItem.UID,
        Subtotal = payableItem.Subtotal,
        Currency = payableItem.Currency.MapToNamedEntity()
      };
    }


    #endregion Methods

  } // class PayableItemMapper

} // namespace Empiria.Payments.Payables.Adapters