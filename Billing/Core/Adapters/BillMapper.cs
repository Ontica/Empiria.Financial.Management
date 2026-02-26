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

    #endregion Public methods

    #region Helpers

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

    #endregion Helpers

  } // class BillMapper

} // namespace Empiria.Billing.Adapters
