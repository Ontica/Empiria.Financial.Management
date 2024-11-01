/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : Payable}DataMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable data objects and related types.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable data objects and related types.</summary>
  internal class PayableHolderMapper {

    static internal PayableHolderDto Map(Payable payable) {

      return new PayableHolderDto {
        Payable = PayableMapper.Map(payable),
        PayableEntity = PayableEntityMapper.Map(payable.PayableEntity),
        Items = MapPayableItems(payable.GetItems()),
        Bills = ExternalServices.GetPayableBills(payable),
        Documents = ExternalServices.GetEntityDocuments(payable),
        History = ExternalServices.GetEntityHistory(payable),
        Actions = MapActions(),
      };

    }


    #region Private Methods

    static private ActionsDto MapActions() {

      return new ActionsDto {
        Can = MapCanDto(),
        Show = MapShowDto()
      };

    }


    static private CanDto MapCanDto() {

      return new CanDto {
        Delete = true,
        Update = true
      };

    }


    static private ShowDto MapShowDto() {

      return new ShowDto {
        PayablesData = true,
        PaymentsData = false
      };

    }

    static private FixedList<PayableDataItemDto> MapPayableItems(FixedList<PayableItem> payableItems) {

      List<PayableDataItemDto> Items = new List<PayableDataItemDto>();

      foreach (var payableItem in payableItems) {
        Items.Add(MapPayableItem(payableItem));
      }

      return Items.ToFixedList();
    }

    static private PayableDataItemDto MapPayableItem(PayableItem payableItem) {

      return new PayableDataItemDto {
        UID = payableItem.UID,
        Name = "Servicio c1 del mes de enero",
        BudgetAccount = new NamedEntityDto  (payableItem.BudgetAccount.UID, payableItem.Description),
        BillConcept = "Pago de servicios de software",
        PayableEntityItemUID = "35051c46-536d-42c5-a5d4-aa3021d5018a",
        Quantity = 1 ,
        Unit  = "Puntos de función CFP2",
        Total = 4004.898m
      };
    }

    #endregion Private Methods

  } // class PayableMapper

} // namespace Empiria.Payments.Payables.Adapters