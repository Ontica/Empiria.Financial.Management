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

using Empiria.Documents.Services;
using Empiria.History.Services;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable data objects and related types.</summary>
  internal class PayableHolderMapper {

    static internal PayableHolderDto Map(Payable payable) {

      return new PayableHolderDto {
        Payable = PayableMapper.Map(payable),
        PayableEntity = PayableEntityMapper.Map(payable.PayableEntity),
        Items = MapPayableItems(payable.GetItems()),
        Bills = ExternalServices.GetPayableBills(payable),
        Documents = DocumentServices.GetEntityDocuments(payable),
        History = HistoryServices.GetEntityHistory(payable),
        Actions = MapActions(),
      };

    }

    #region Private Methods

    static private PayableActions MapActions() {
      return new PayableActions {
        CanDelete = true,
        CanUpdate = true,
        CanEditDocuments = true,
        CanGeneratePaymentOrder = true,
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

      string billConceptName = "No se ha anexado la factura";

      if (payableItem.HasBillConcept) {
        billConceptName = payableItem.BillConcept.Description;
      }

      return new PayableDataItemDto {
        UID = payableItem.UID,
        Name = payableItem.Description,
        BudgetAccount = payableItem.BudgetAccount.MapToNamedEntity(),
        BillConcept = billConceptName,
        PayableEntityItemUID = payableItem.PayableEntityItemId.ToString(),
        Quantity = payableItem.Quantity,
        Unit  = payableItem.Unit.Name,
        Total = payableItem.Total
      };
    }

    #endregion Private Methods

  } // class PayableMapper

} // namespace Empiria.Payments.Payables.Adapters
