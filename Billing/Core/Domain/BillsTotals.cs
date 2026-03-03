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
                              FixedList<Bill> bills, decimal totalTax) {

      UID = taxType.UID;
      TaxType = taxType;
      TaxName = taxType.Name;
      TaxMethod = taxMethod;

      if (taxMethod == BillTaxMethod.Retencion) {
        TaxName = $"{taxType.Name} Retenido";
      }

      BaseAmount = Math.Round(bills.Sum(x => x.Subtotal), 2, MidpointRounding.AwayFromZero);

      var taxes = Math.Round(totalTax, 2, MidpointRounding.AwayFromZero);

      Total = taxes;
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
        
        var taxTotal = new BillTaxItemTotal(taxTypeGroup.Key.TaxMethod,
                                            taxTypeGroup.Key.TaxType, taxTypeGroup.ToFixedList());

        taxItemsByBills.Add(taxTotal);
      }

      return taxItemsByBills;
    }


    static private FixedList<BillTaxItemTotal> GroupTaxesByType(
                                                FixedList<BillTaxItemTotal> billTaxItemTotals) {

      var returnedTaxItems = new List<BillTaxItemTotal>();

      foreach (var taxItem in billTaxItemTotals) {

        var existTaxItemType = returnedTaxItems.Find(x => x.TaxType == taxItem.TaxType &&
                                                          x.TaxName == taxItem.TaxName);

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

    #region Methods Tax Totals from BillSchemaData

    internal FixedList<BillTaxItemTotal> BuildTaxItemsFromBillSchemaData() {

      List<BillTaxItemTotal> taxTypesGroupsByBills = new List<BillTaxItemTotal>();

      GetRetencionISR(taxTypesGroupsByBills);
      GetRetencionIVA(taxTypesGroupsByBills);
      GetRetencionIEPS(taxTypesGroupsByBills);
      GetTrasladoIVA(taxTypesGroupsByBills);
      GetTrasladoIEPS(taxTypesGroupsByBills);

      return taxTypesGroupsByBills.ToFixedList();
    }


    private void GetRetencionIEPS(List<BillTaxItemTotal> taxTypesGroupsByBills) {
      var billsRetencionesIEPS = _bills.FindAll(x => x.SchemaData.RetencionIEPS > 0);

      var iepsRetenciones = billsRetencionesIEPS.FindAll(x => !x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.RetencionIEPS * -1) +
                              billsRetencionesIEPS.FindAll(x => x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.RetencionIEPS);
      if (iepsRetenciones != 0) {

        taxTypesGroupsByBills.Add(new BillTaxItemTotal(BillTaxMethod.Retencion, TaxType.Parse(103),
                                  billsRetencionesIEPS.ToFixedList(), iepsRetenciones));
      }
    }


    private void GetRetencionISR(List<BillTaxItemTotal> taxTypesGroupsByBills) {
      var billsRetencionesISR = _bills.FindAll(x => x.SchemaData.RetencionISR > 0);

      var isrRetenciones = billsRetencionesISR.FindAll(x => !x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.RetencionISR * -1) +
                              billsRetencionesISR.FindAll(x => x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.RetencionISR);
      if (isrRetenciones != 0) {
        taxTypesGroupsByBills.Add(new BillTaxItemTotal(BillTaxMethod.Retencion, TaxType.Parse(102),
                                  billsRetencionesISR.ToFixedList(), isrRetenciones));
      }
    }


    private void GetRetencionIVA(List<BillTaxItemTotal> taxTypesGroupsByBills) {

      var billsRetencionesIVA = _bills.FindAll(x => x.SchemaData.RetencionIVA > 0);

      var ivaRetenciones = billsRetencionesIVA.FindAll(x => !x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.RetencionIVA * -1) +
                              billsRetencionesIVA.FindAll(x => x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.RetencionIVA);
      if (ivaRetenciones != 0) {
        taxTypesGroupsByBills.Add(new BillTaxItemTotal(BillTaxMethod.Retencion, TaxType.Parse(101),
                                  billsRetencionesIVA.ToFixedList(), ivaRetenciones));
      }
    }


    private void GetTrasladoIEPS(List<BillTaxItemTotal> taxTypesGroupsByBills) {

      var billsTrasladosIEPS = _bills.FindAll(x => x.SchemaData.TrasladoIEPS > 0);

      var iepsTraslados = billsTrasladosIEPS.FindAll(x => !x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.TrasladoIEPS) +
                           billsTrasladosIEPS.FindAll(x => x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.TrasladoIEPS * -1);
      if (iepsTraslados != 0) {
        taxTypesGroupsByBills.Add(new BillTaxItemTotal(BillTaxMethod.Traslado, TaxType.Parse(103),
                                  billsTrasladosIEPS.ToFixedList(), iepsTraslados));
      }
    }


    private void GetTrasladoIVA(List<BillTaxItemTotal> taxTypesGroupsByBills) {

      var billsTrasladosIVA = _bills.FindAll(x => x.SchemaData.TrasladoIVA > 0);

      var ivaTraslados = billsTrasladosIVA.FindAll(x => !x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.TrasladoIVA) +
                           billsTrasladosIVA.FindAll(x => x.BillType.IsCreditNote)
                              .Sum(x => x.SchemaData.TrasladoIVA * -1);
      if (ivaTraslados != 0) {
        taxTypesGroupsByBills.Add(new BillTaxItemTotal(BillTaxMethod.Traslado, TaxType.Parse(101),
                                  billsTrasladosIVA.ToFixedList(), ivaTraslados));
      }
    }

    #endregion Methods Tax Totals from BillSchemaData

  }  // class BillsTotals

}  // namespace Empiria.Billing
