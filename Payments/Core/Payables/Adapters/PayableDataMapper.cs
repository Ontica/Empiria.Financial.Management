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
using System.Security.Cryptography;
using System.Xml.Linq;
using Empiria.Measurement;
using Empiria.Services;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable data objects and related types.</summary>
  internal class PayableDataMapper {

    static internal PayableDataDto Map(Payable payable) {

      return new PayableDataDto {
        Payable = PayableMapper.Map(payable),
        PayableEntity = MapPayableEntity(),
        Items = MapPayableItems(payable)
,       Documents = MapDocuments(),
        History = MapHistory(), 
        Actions = MapActions()
      };

    }

    #region Private Methods

    static private ActionsDto MapActions() {

      return new ActionsDto {
        CanObject = MapCanDto(),
        ShowObject = MapShowDto()
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

    static private FixedList<HistoryDto>MapHistory() {

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
        BudgetAccount = payableItem.BudgetAccount.BudgetAccountType.Name,
        BillConcept = "Pago de servicios de software",
        PayableEntityItemUID = "35051c46-536d-42c5-a5d4-aa3021d5018a",
        Quantity = 1 ,
        Unit  = "Puntos de función CFP2",
        Total = 4004.898m
      };
    }

    static private PayableEntityDto MapPayableEntity() {

      return new PayableEntityDto {
        UID = "d13fccb0-a5d0-419e-9204-777f57b6959d",
        Type = "Contrato de prestación de servicios",
        Name = "Servicios de desarrollo de software 1",
        Description = "",
        Items = MapPayebleEntityItems()
      };

    }


    static private FixedList<PayableEntityItemDto> MapPayebleEntityItems() {

      var item =  new PayableEntityItemDto {
        UID = "35051c46-536d-42c5-a5d4-aa3021d5018a",
        Name = "Servicio c1 del mes de enero",
        Quantity = 343,
        Unit = "Puntos de función CFP",
        Total = 4004.898m
      };

      List<PayableEntityItemDto> payableEntityItems = new List<PayableEntityItemDto>();
      payableEntityItems.Add(item);

      return payableEntityItems.ToFixedList();
    }

    #endregion Private Methods     


  } // class PayableMapper

} // namespace Empiria.Payments.Payables.Adapters