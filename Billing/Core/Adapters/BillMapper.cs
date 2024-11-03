/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill.</summary>
  static internal class BillMapper {

    #region Public methods

    static internal BillHolderDto Map(Bill bill) {

      return new BillHolderDto() {
        Bill = MapToBillDto(bill),
        Concepts = MapBillConcepts(bill.Concepts),
        Documents = DocumentServices.GetEntityDocuments(bill),
        History = HistoryServices.GetEntityHistory(bill),
        Actions = MapActions()
      };

    }


    static internal BillDto MapToBillDto(Bill bill) {
      return new BillDto {
        UID = bill.UID,
        BillNo = bill.BillNo,
        BillType = bill.BillType.MapToNamedEntity(),
        IssueDate = bill.IssueDate,
        IssuedBy = bill.IssuedBy.MapToNamedEntity(),
        IssuedTo = bill.IssuedTo.MapToNamedEntity(),
        ManagedBy = bill.ManagedBy.MapToNamedEntity(),
        CurrencyCode = bill.Currency.ISOCode,
        Subtotal = bill.Subtotal,
        Discount = bill.Discount,
        Total = bill.Total,
        PostedBy = bill.PostedBy.MapToNamedEntity(),
        PostingTime = bill.PostingTime,
        Status = bill.Status.MapToDto(),
        Concepts = MapBillConcepts(bill.Concepts)
      };
    }


    static internal FixedList<BillDescriptorDto> MapToBillListDto(FixedList<Bill> bills) {
      return bills.Select((x) => MapToBillDescriptorDto(x))
                  .ToFixedList();
    }

    #endregion Public methods

    #region Helpers

    static private BaseActions MapActions() {
      return new BaseActions {
        CanEditDocuments = true
      };
    }

    static private FixedList<BillConceptDto> MapBillConcepts(FixedList<BillConcept> billConcepts) {
      return billConcepts.Select((x) => MapToBillConceptsDto(x))
                         .ToFixedList();
    }


    static private FixedList<BillTaxEntryDto> MapBillTaxes(FixedList<BillTaxEntry> taxEntries) {
      return taxEntries.Select((x) => MapToBillTaxesDto(x))
                       .ToFixedList();
    }


    static private BillConceptDto MapToBillConceptsDto(BillConcept billConcept) {
      return new BillConceptDto {
        BillUID = billConcept.Bill.UID,
        UID = billConcept.BillConceptUID,
        ProductUID = billConcept.Product.UID,
        Description = billConcept.Description,
        Quantity = billConcept.Quantity,
        UnitPrice = billConcept.UnitPrice,
        Subtotal = billConcept.Subtotal,
        Discount = billConcept.Discount,
        PostedBy = billConcept.PostedBy.MapToNamedEntity(),
        PostingTime = billConcept.PostingTime,
        TaxEntriesDto = MapBillTaxes(billConcept.TaxEntries)
      };
    }


    static private BillDescriptorDto MapToBillDescriptorDto(Bill bill) {
      return new BillDescriptorDto() {
        UID = bill.BillUID,
        BillNo = bill.BillNo,
        BillTypeName = bill.BillType.DisplayName,
        IssuedByName = bill.IssuedBy.Name,
        IssuedToName = bill.IssuedTo.Name,
        CategoryName = bill.BillCategory.Name,
        Total = bill.Total,
        IssueDate = bill.IssueDate,
        StatusName = bill.Status.GetName()
      };
    }


    static private BillTaxEntryDto MapToBillTaxesDto(BillTaxEntry taxEntry) {
      return new BillTaxEntryDto {
        BillUID = taxEntry.Bill.UID,
        BillConceptUID = taxEntry.BillConcept.UID,
        UID = taxEntry.BillTaxUID,
        TaxMethod = taxEntry.TaxMethod,
        TaxFactorType = taxEntry.TaxFactorType,
        Factor = taxEntry.Factor,
        BaseAmount = taxEntry.BaseAmount,
        Total = taxEntry.Total,
        PostedBy = taxEntry.PostedBy.MapToNamedEntity(),
        PostingTime = taxEntry.PostingTime,
        Status = taxEntry.Status,
      };
    }

    #endregion Helpers

  } // class BillMapper

} // namespace Empiria.Billing.Adapters
