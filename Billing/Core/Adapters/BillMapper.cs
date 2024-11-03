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
using Empiria.Data;
using Empiria.Storage;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill.</summary>
  static internal class BillMapper {

    #region Public methods


    static internal BillHolderDto Map(Bill bill) {

      return new BillHolderDto() {
        Bill = MapToBillDto(bill),
        Concepts = MapBillConcepts(bill.Concepts),
        Documents = ExternalServices.GetEntityDocuments(bill),
        History = ExternalServices.GetEntityHistory(bill),
      };

    }


    static internal BillDto MapToBillDto(Bill bill) {

      BillDto dto = new BillDto();
      dto.UID = bill.UID;
      dto.BillNo = bill.BillNo;
      dto.BillType = new NamedEntityDto(bill.BillType.UID, bill.BillType.DisplayName);
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
      dto.Status = new NamedEntityDto(bill.Status.ToString(), bill.Status.GetName());
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



    #endregion Public methods

    #region Private methods

    static private FixedList<BillConceptDto> MapBillConcepts(FixedList<BillConcept> billConcepts) {
      if (billConcepts.Count == 0) {
        return new FixedList<BillConceptDto>();
      }

      var conceptsDto = billConcepts.Select((x) => MapToBillConceptsDto(x));

      return new FixedList<BillConceptDto>(conceptsDto);
    }


    static private FixedList<BillTaxEntryDto> MapBillTaxes(FixedList<BillTaxEntry> taxEntries) {
      if (taxEntries.Count == 0) {
        return new FixedList<BillTaxEntryDto>();
      }
      var taxes = taxEntries.Select((x) => MapToBillTaxesDto(x));

      return new FixedList<BillTaxEntryDto>(taxes);
    }


    static private BillConceptDto MapToBillConceptsDto(BillConcept billConcept) {

      BillConceptDto conceptDto = new BillConceptDto();
      conceptDto.BillUID = billConcept.Bill.UID;
      conceptDto.UID = billConcept.BillConceptUID;
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


    static private BillTaxEntryDto MapToBillTaxesDto(BillTaxEntry x) {
      BillTaxEntryDto dto = new BillTaxEntryDto();
      dto.BillUID = x.Bill.UID;
      dto.BillConceptUID = x.BillConcept.UID;
      dto.UID = x.BillTaxUID;
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
