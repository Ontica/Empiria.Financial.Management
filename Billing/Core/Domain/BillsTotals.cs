/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : BillsTotals                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Calculates taxes and discount totals for a set of bills.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;

namespace Empiria.Billing {

  /// <summary>Calculates taxes and discount totals for a set of bills.</summary>
  public class BillTaxItemTotal {

    internal BillTaxItemTotal() {

    }

    internal BillTaxItemTotal(BillTaxMethod taxMethod, TaxType taxType,
                              FixedList<BillTaxEntry> billTaxEntries) {
      UID = taxType.UID;
      TaxType = taxType;
      TaxName = taxType.Name;
      TaxMethod = taxMethod;

      if (taxMethod == BillTaxMethod.Retencion) {
        TaxName = $"{taxType.Name} Retenido";
      }

      BaseAmount = Math.Round(billTaxEntries.Sum(x => x.BaseAmount), 2, MidpointRounding.AwayFromZero);

      var taxes = Math.Round(billTaxEntries.FindAll(x => !x.Bill.BillType.IsCreditNote)
                              .Sum(x => x.TaxMethod == BillTaxMethod.Retencion ? -1 * x.Total : x.Total),
                             2, MidpointRounding.AwayFromZero);

      var creditNoteTaxes = Math.Round(billTaxEntries.ToFixedList()
                                        .FindAll(x => x.Bill.BillType.IsCreditNote)
                                        .Sum(x => x.TaxMethod == BillTaxMethod.Retencion ? x.Total : -1 * x.Total)
                                       , 2, MidpointRounding.AwayFromZero);

      Total = taxes + creditNoteTaxes;
    }

    public string UID {
      get; internal set;
    }

    public TaxType TaxType {
      get; internal set;
    }

    public BillTaxMethod TaxMethod {
      get; internal set;
    }

    public string TaxName {
      get; internal set;
    }

    public decimal BaseAmount {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }


    internal void SumTotals(BillTaxItemTotal taxItem) {

      this.BaseAmount += taxItem.BaseAmount;
      this.Total += taxItem.Total;
    }

  }  // class BillTaxItemTotal


  /// <summary>Calculate taxes and discount totals for a set of bills.</summary>
  public class BillsTotals {

    private readonly FixedList<Bill> _bills;

    #region Constructors and parsers

    public BillsTotals(FixedList<Bill> bills) {
      Assertion.Require(bills, nameof(bills));

      _bills = bills;
      TaxItems = BuildTaxItems();
    }

    #endregion Constructors and parsers

    #region Properties

    public decimal Subtotal {
      get {
        return _bills.Sum(x => x.BillType.IsCreditNote ?
                               -1 * x.Subtotal : x.Subtotal) +
               _bills.SelectFlat(x => x.Concepts.Where(a => a.SchemaData.IsBonusConcept))
                     .ToFixedList().Sum(x => x.Subtotal);
      }
    }


    public decimal Discounts {
      get {
        var discount = _bills.Sum(x => x.BillType.IsCreditNote ? -1 * x.Discount : x.Discount);
        return _bills.Sum(x => x.BillType.IsCreditNote ? -1 * x.Discount : x.Discount);
      }
    }


    public FixedList<BillTaxItemTotal> TaxItems {
      get;
    }


    public decimal Total {
      get {
        return Subtotal - Discounts + TaxItems.Sum(x => x.Total);
      }
    }


    public decimal GetTotal() {
      return _bills.Sum(x => x.BillType.IsCreditNote ? -1 * x.Total : x.Total);
    }


    public decimal BudgetableTaxesTotal {
      get {
        return TaxItems.FindAll(x => x.TaxType.IsBudgetable)
                       .Sum(x => x.Total);
      }
    }

    #endregion Properties

    #region Methods

    internal FixedList<BillTaxItemTotal> BuildTaxItems() {

      var taxTypesGroupsByBills = new List<BillTaxItemTotal>();

      foreach (var bill in _bills) {

        taxTypesGroupsByBills.AddRange(GetTaxItemsByBills(bill));
      }

      return GroupTaxesByType(taxTypesGroupsByBills.ToFixedList());
    }


    private List<BillTaxItemTotal> GetTaxItemsByBills(Bill bill) {

      var taxItemsByBills = new List<BillTaxItemTotal>();

      var taxTypesByBill = bill.BillTaxes.GroupBy(x => new { x.TaxType, x.TaxMethod });

      foreach (var taxTypeGroup in taxTypesByBill) {

        var taxTotal = new BillTaxItemTotal(taxTypeGroup.Key.TaxMethod, taxTypeGroup.Key.TaxType, taxTypeGroup.ToFixedList());

        taxItemsByBills.Add(taxTotal);
      }

      return taxItemsByBills;
    }


    static private FixedList<BillTaxItemTotal> GroupTaxesByType(FixedList<BillTaxItemTotal> billTaxItemTotals) {

      var returnedTaxItems = new List<BillTaxItemTotal>();

      foreach (var taxItem in billTaxItemTotals) {

        var existTaxItemType = returnedTaxItems.Find(x => x.TaxType == taxItem.TaxType && x.TaxName == taxItem.TaxName);

        if (existTaxItemType != null) {
          existTaxItemType.SumTotals(taxItem);
        } else {

          BillTaxItemTotal taxItemTotal = new BillTaxItemTotal {
            UID = taxItem.UID,
            TaxType = taxItem.TaxType,
            TaxMethod = taxItem.TaxMethod,
            TaxName = taxItem.TaxName,
            BaseAmount = taxItem.BaseAmount,
            Total = taxItem.Total,
          };

          returnedTaxItems.Add(taxItemTotal);
        }
      }
      return returnedTaxItems.ToFixedList();
    }

    #endregion Methods

  }  // class BillsTotals

}  // namespace Empiria.Billing
