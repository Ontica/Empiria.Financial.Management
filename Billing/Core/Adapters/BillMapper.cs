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
using Empiria.Billing.SATMexicoImporter;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill.</summary>
  static internal class BillMapper {

    #region Public methods

    static internal BillEntryDto MapToBillDto(Bill bill) {

      BillEntryDto dto = new BillEntryDto();
      dto.Bill_UID = bill.UID;
      dto.BillNo = bill.BillNo;
      dto.IssueDate = bill.IssueDate;
      dto.IssuedBy = new NamedEntityDto(bill.IssuedBy.UID, bill.IssuedBy.Name);
      dto.IssuedTo = new NamedEntityDto(bill.IssuedTo.UID, bill.IssuedTo.Name);
      dto.ManagedBy = new NamedEntityDto(bill.ManagedBy.UID, bill.ManagedBy.Name);
      dto.CurrencyCode = bill.Currency.ISOCode;
      dto.Subtotal = bill.Subtotal;
      dto.Discount = bill.Discount;
      dto.Total = bill.Total;
      dto.PostedBy = new NamedEntityDto(bill.PostedBy.UID, bill.PostedBy.Name);
      dto.PostingTime = bill.PostingTime;
      dto.Status = bill.Status;
      dto.Concepts = MapBillConcepts(bill.Concepts);
      return dto;
    }


    static internal FixedList<BillDescriptorDto> MapToBillListDto(FixedList<Bill> bills) {
      if (bills.Count == 0) {
        return new FixedList<BillDescriptorDto>();
      }

      var billDto = bills.Select((x) => MapToBillDescriptorDto(x));

      return new FixedList<BillDescriptorDto>(billDto);
    }


    static private BillDescriptorDto MapToBillDescriptorDto(Bill x) {

      return new BillDescriptorDto() {
        Bill_UID = x.UID,
        BillNo = x.BillNo,
        IssuedBy= x.IssuedBy.Name,
        IssuedTo = x.IssuedTo.Name,
        Category = x.BillCategory.Name,
        Total = x.Total,
        IssueDate = x.IssueDate,
        Status = x.Status.GetName()
      };
    }

    #endregion Public methods


    #region Private methods


    static private FixedList<BillConceptDto> MapBillConcepts(FixedList<BillConcept> billConcepts) {
      if (billConcepts.Count == 0) {
        return new FixedList<BillConceptDto>();
      }

      var conceptsDto = billConcepts.Select((x) => MapToBillConceptsDto(x));

      return new FixedList<BillConceptDto>(conceptsDto);
    }


    private static BillConceptDto MapToBillConceptsDto(BillConcept billConcept) {

      BillConceptDto conceptDto = new BillConceptDto();
      conceptDto.BillUID = billConcept.Bill.UID;
      conceptDto.BillConceptUID = billConcept.UID;
      conceptDto.ProductUID = billConcept.Product.UID;
      conceptDto.Description = billConcept.Description;
      conceptDto.Quantity = billConcept.Quantity;
      conceptDto.UnitPrice = billConcept.UnitPrice;
      conceptDto.Subtotal = billConcept.Subtotal;
      conceptDto.Discount = billConcept.Discount;
      conceptDto.PostedBy = new NamedEntityDto(billConcept.PostedBy.UID, billConcept.PostedBy.Name);
      conceptDto.PostingTime = billConcept.PostingTime;
      conceptDto.TaxEntriesDto = MapBillTaxes(billConcept.TaxEntries);

      return conceptDto;
    }


    static private FixedList<BillTaxEntryDto> MapBillTaxes(FixedList<BillTaxEntry> taxEntries) {
      if (taxEntries.Count==0) {
        return new FixedList<BillTaxEntryDto>();
      }
      var taxes = taxEntries.Select((x) => MapToBillTaxesDto(x));

      return new FixedList<BillTaxEntryDto>(taxes);
    }


    static private BillTaxEntryDto MapToBillTaxesDto(BillTaxEntry x) {
      BillTaxEntryDto dto = new BillTaxEntryDto();
      dto.BillUID = x.Bill.UID;
      dto.BillConceptUID = x.BillConcept.UID;
      dto.BillTaxEntryUID = x.UID;
      dto.TaxMethod = x.TaxMethod;
      dto.TaxFactorType = x.TaxFactorType;
      dto.Factor = x.Factor;
      dto.BaseAmount = x.BaseAmount;
      dto.Total = x.Total;
      dto.PostedBy = new NamedEntityDto(x.PostedBy.UID, x.PostedBy.Name);
      dto.PostingTime = x.PostingTime;
      dto.Status = x.Status;
      return dto;
    }

    #endregion Private methods

  } // class BillMapper

} // namespace Empiria.Billing.Adapters
