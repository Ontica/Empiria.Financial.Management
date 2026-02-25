/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Documents;
using Empiria.History;
using Empiria.StateEnums;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill.</summary>
  static public class BillMapper {

    #region Public methods

    static internal BillHolderDto Map(Bill bill) {

      return new BillHolderDto() {
        Bill = MapToBillDto(bill),
        Concepts = MapBillConcepts(bill.Concepts),
        BillRelatedBills = MapBillRelatedBills(bill.BillRelatedBills),
        Documents = DocumentServices.GetAllEntityDocuments(bill),
        History = HistoryServices.GetEntityHistory(bill),
        Actions = MapActions()
      };
    }


    static public FixedList<BillDto> MapToBillDto(FixedList<Bill> bills) {
      return bills.Select((x) => MapToBillDto(x))
                  .ToFixedList();

    }

    static public BillsStructureDto MapToBillStructure(FixedList<Bill> bills) {
      var billsTotals = new BillsTotals(bills);

      return new BillsStructureDto {
        Bills = MapToBillDto(bills),
        Subtotal = billsTotals.Subtotal,
        Discounts = billsTotals.Discounts,
        Taxes = MapStructureTaxEntries(billsTotals.TaxItems),
        Total = billsTotals.Total
      };
    }


    static public BillDto MapToBillDto(Bill bill) {

      var files = DocumentServices.GetAllEntityDocuments(bill);

      return new BillDto {
        UID = bill.UID,
        BillNo = bill.BillNo,
        Name = bill.Name,
        Category = bill.BillCategory.MapToNamedEntity(),
        BillType = bill.BillCategory.MapToNamedEntity(),
        ManagedBy = bill.ManagedBy.MapToNamedEntity(),
        IssuedBy = bill.IssuedBy.MapToNamedEntity(),
        IssuedTo = bill.IssuedTo.MapToNamedEntity(),
        CurrencyCode = bill.Currency.ISOCode,
        Subtotal = bill.Subtotal,
        Discount = bill.Discount,
        Taxes = Math.Round(bill.Taxes, 2, MidpointRounding.AwayFromZero),
        Total = bill.Subtotal - bill.Discount + bill.Taxes,
        IssueDate = bill.IssueDate,
        PostedBy = bill.PostedBy.MapToNamedEntity(),
        PostingTime = bill.PostingTime,
        Status = bill.Status.MapToDto(),
        Files = files.Select(x => x.File)
                     .ToFixedList(),
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


    static private FixedList<BillRelatedBillDto> MapBillRelatedBills(
                    FixedList<BillRelatedBill> billRelatedBills) {

      return billRelatedBills.Select((x) => MapToBillRelatedBillsDto(x))
                         .ToFixedList();
    }


    static private FixedList<BillsStructureTaxEntryDto> MapStructureTaxEntries(FixedList<BillTaxItemTotal> taxItemsTotals) {
      return taxItemsTotals.Select(x => MapStructureTaxEntry(x))
                           .ToFixedList();
    }


    static private BillsStructureTaxEntryDto MapStructureTaxEntry(BillTaxItemTotal taxItemTotal) {
      return new BillsStructureTaxEntryDto {
        UID = taxItemTotal.UID,
        TaxType = new NamedEntityDto(taxItemTotal.TaxType.UID, taxItemTotal.TaxName),
        BaseAmount = taxItemTotal.BaseAmount,
        Total = taxItemTotal.Total
      };
    }


    static private FixedList<BillTaxEntryDto> MapBillTaxes(FixedList<BillTaxEntry> taxEntries) {
      return taxEntries.Select((x) => MapToBillTaxesDto(x))
                       .ToFixedList();
    }


    static private BillConceptDto MapToBillConceptsDto(BillConcept billConcept) {
      
      return new BillConceptDto {
        UID = billConcept.UID,
        TypeName = billConcept.BillConceptType.DisplayName,
        Product = billConcept.Product.MapToNamedEntity(),
        Description = billConcept.Description,
        Quantity = billConcept.Quantity,
        UnitPrice = billConcept.UnitPrice,
        Subtotal = billConcept.Subtotal,
        Discount = billConcept.Discount,
        PostedBy = billConcept.PostedBy.MapToNamedEntity(),
        PostingTime = billConcept.PostingTime,
        TaxEntries = MapBillTaxes(billConcept.TaxEntries)
      };
    }


    static private BillDescriptorDto MapToBillDescriptorDto(Bill bill) {
      return new BillDescriptorDto() {
        UID = bill.UID,
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


    static private BillRelatedBillDto MapToBillRelatedBillsDto(BillRelatedBill x) {

      return new BillRelatedBillDto {
        UID = x.UID,
        RelatedDocument = x.RelatedDocument,
        PostedBy = x.PostedBy.MapToNamedEntity(),
        PostingTime = x.PostingTime,
        TaxEntries = MapBillTaxes(x.TaxEntries)
      };
    }


    static private BillTaxEntryDto MapToBillTaxesDto(BillTaxEntry taxEntry) {
      return new BillTaxEntryDto {
        UID = taxEntry.UID,
        TaxMethod = taxEntry.TaxMethod.MapToDto(),
        TaxFactorType = taxEntry.TaxFactorType.MapToDto(),
        Factor = taxEntry.Factor,
        BaseAmount = taxEntry.BaseAmount,
        Total = taxEntry.Total,
        PostedBy = taxEntry.PostedBy.MapToNamedEntity(),
        PostingTime = taxEntry.PostingTime,
        Status = EntityStatusEnumExtensions.MapToDto(taxEntry.Status)
      };
    }

    #endregion Helpers

  } // class BillMapper

} // namespace Empiria.Billing.Adapters
