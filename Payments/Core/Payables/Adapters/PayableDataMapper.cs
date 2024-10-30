/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : Payable}DataMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable data objects and related types.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable data objects and related types.</summary>
  internal class PayableDataMapper {

    static internal PayableHolderDto Map(Payable payable) {

      return new PayableHolderDto {
        Payable = PayableMapper.Map(payable),
        PayableEntity = MapPayableEntity(payable.PayableEntity),
        Items = MapPayableItems(payable),
        Documents = MapDocuments(),
        History = MapHistory(),
        Actions = MapActions(),
        Bills = MapBills()
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
        Cancel = true,
        Update = true
      };

    }


    static private ShowDto MapShowDto() {

      return new ShowDto {
        PayablesData = true,
        PaymentsData = false
      };

    }


    static private FixedList<BillDto> MapBills() {
      var bill = new BillDto {
        UID = "e0f6a221-bf23-429a-a503-bfd63eb581fa",
        Name = "LA VÍA ONTICA SC",
        Date = DateTime.Today,
        CFDIGUID = "-bf23-429a-a503-bfd63eb581",
        Items = MapBillItems()
      };

      List<BillDto> bills = new List<BillDto>();
      bills.Add(bill);

      return bills.ToFixedList();
    }


    static private FixedList<BillItemDto> MapBillItems() {
      var item =  new BillItemDto {
        UID = "aa4ca009-a204-4732-8f5c-c1bf3a25019f",
        Name = "345 Puntos de función COSMIC",
        Unit = "Puntos de función CFP2",
        Quantity = 1,
        Total = 4004.898m
      };

      List<BillItemDto> items = new List<BillItemDto>();
      items.Add(item);

      return items.ToFixedList();
    }


    static private FixedList<DocumentDto> MapDocuments() {

     var document =  new DocumentDto {
        UID = "e0f6a221-bf23-429a-a503-bfd63eb581fa",
        Type = "",
        Name = "Reporte de del Mes de enero",
        Description = "Reporte correspondiente al mes de enero",
        UploadedDate = DateTime.Today,
        UploadedBy = "Raul lopez"
      };

      List<DocumentDto> documents = new List<DocumentDto>();
      documents.Add(document);

      return documents.ToFixedList();
    }

    static private FixedList<HistoryDto> MapHistory() {

      var history = new HistoryDto {
        UID = "e0f6a221-bf23-429a-a503-bfd63eb581fa",
        Type = "",
        Description = "Reporte correspondiente al mes de enero",
        Time = DateTime.Today,
        Party = new NamedEntityDto("758ebaf6-cb80-24a6-b175-e0158cb5be4d", "Rene Astudillo Ramirez")
      };

      List<HistoryDto> histories = new List<HistoryDto>();
      histories.Add(history);

      return histories.ToFixedList();

    }

    static private FixedList<PayableDataItemDto> MapPayableItems(Payable payable) {
      var payableItems = payable.GetItems();

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

    static private PayableEntityDto MapPayableEntity(IPayableEntity payableEntity) {

      return new PayableEntityDto {
        UID = payableEntity.UID,
        Type = payableEntity.Type.MapToNamedEntity(),
        EntityNo = payableEntity.EntityNo,
        Name = payableEntity.Name,
        Description = payableEntity.Description,

        Items = MapPayableEntityItems(payableEntity.Items)
      };

    }


    static private FixedList<PayableEntityItemDto> MapPayableEntityItems(IEnumerable<IPayableEntityItem> items) {
      return items.Select(x => MapPayableEntityItem(x))
                  .ToFixedList();
    }


    static private PayableEntityItemDto MapPayableEntityItem(IPayableEntityItem item) {
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

    #endregion Private Methods


  } // class PayableMapper

} // namespace Empiria.Payments.Payables.Adapters