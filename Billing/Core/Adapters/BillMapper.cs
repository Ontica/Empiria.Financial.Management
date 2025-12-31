/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Documents;
using Empiria.History;

using Empiria.Financial;
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

      var subTotalBilled = bills.Sum(x => x.BillType.Name.Contains("CreditNote") ?
                                                            -1 * x.Subtotal : x.Subtotal);

      return new BillsStructureDto {
        Bills = MapToBillDto(bills),
        Subtotal = subTotalBilled,
        Taxes = MapStructureTaxEntries(bills),
      };
    }


    static public BillDto MapToBillDto(Bill bill) {

      var files = DocumentServices.GetAllEntityDocuments(bill);

      var concepts = bill.Concepts;

      string name;

      if (bill.BillType.Name.Contains("Voucher")) {
        name = bill.Description;
      } else if (concepts.Count != 0) {
        name = concepts[0].Description;
      } else {
        name = "La factura no tiene conceptos";
      }

      return new BillDto {
        UID = bill.UID,
        BillNo = bill.BillNo,
        Name = name,
        Category = bill.BillCategory.MapToNamedEntity(),
        BillType = bill.BillCategory.MapToNamedEntity(),
        ManagedBy = bill.ManagedBy.MapToNamedEntity(),
        IssuedBy = bill.IssuedBy.MapToNamedEntity(),
        IssuedTo = bill.IssuedTo.MapToNamedEntity(),
        CurrencyCode = bill.Currency.ISOCode,
        Subtotal = bill.Subtotal,
        Discount = bill.Discount,
        Taxes = bill.Taxes,
        Total = bill.Subtotal - bill.Discount,
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


    static internal BillWithConceptsDto MapToBillWithConcepts(Bill bill) {
      return new BillWithConceptsDto {
        Bill = MapToBillDto(bill),
        Concepts = MapBillConcepts(bill.Concepts),
        BillRelatedBills = MapBillRelatedBills(bill.BillRelatedBills)
      };
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


    static private FixedList<BillsStructureTaxEntryDto> MapStructureTaxEntries(FixedList<Bill> bills) {
      FixedList<BillTaxEntry> taxEntries = bills.SelectFlat(x => x.Concepts)
                                                .SelectFlat(x => x.TaxEntries);

      var taxTypeGroup = taxEntries.GroupBy(x => x.TaxType);

      return taxTypeGroup.Select(x => MapStructureTaxEntry(x))
                         .ToFixedList();
    }


    static private BillsStructureTaxEntryDto MapStructureTaxEntry(IGrouping<TaxType, BillTaxEntry> taxEntry) {

      var taxes = taxEntry.ToFixedList()
                          .FindAll(x => !x.Bill.BillType.Name.Contains("CreditNote"))
                          .Sum(x => x.Total);

      var creditNoteTaxes = taxEntry.ToFixedList()
                                    .FindAll(x => x.Bill.BillType.Name.Contains("CreditNote"))
                                    .Sum(x => x.Total);

      return new BillsStructureTaxEntryDto {
        UID = taxEntry.Key.UID,
        TaxType = taxEntry.Key.MapToNamedEntity(),
        BaseAmount = taxEntry.Sum(x => x.BaseAmount),
        Total = taxes - creditNoteTaxes
      };
    }


    static private FixedList<BillTaxEntryDto> MapBillTaxes(FixedList<BillTaxEntry> taxEntries) {
      return taxEntries.Select((x) => MapToBillTaxesDto(x))
                       .ToFixedList();
    }


    static private BillConceptDto MapToBillConceptsDto(BillConcept billConcept) {
      return new BillConceptDto {
        UID = billConcept.UID,
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
        UID = x.BillRelatedBillUID,
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
