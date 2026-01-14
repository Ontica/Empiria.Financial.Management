/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : BillsTotals                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Calculates taxes and discount totals for a set of bills.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;

namespace Empiria.Billing {

  /// <summary>Calculates taxes and discount totals for a set of bills.</summary>
  public class BillTaxItemTotal {

    internal BillTaxItemTotal(TaxType taxType,
                              BillTaxMethod taxMethod,
                              FixedList<BillTaxEntry> billTaxEntries) {
      UID = taxType.UID;
      TaxType = taxType;
      TaxName = taxType.Name;

      if (taxMethod == BillTaxMethod.Retencion) {
        TaxName = $"{taxType.Name} Retenido";
      }

      BaseAmount = billTaxEntries.Sum(x => x.BaseAmount);

      var taxes = billTaxEntries.FindAll(x => !x.Bill.BillType.IsCreditNote)
                                .Sum(x => x.TaxMethod == BillTaxMethod.Retencion ? -1 * x.Total : x.Total);

      var creditNoteTaxes = billTaxEntries.ToFixedList()
                                          .FindAll(x => x.Bill.BillType.IsCreditNote)
                                          .Sum(x => x.TaxMethod == BillTaxMethod.Retencion ? x.Total : -1 * x.Total);

      Total = taxes + creditNoteTaxes;
    }

    public string UID {
      get;
    }

    public TaxType TaxType {
      get;
    }

    public string TaxName {
      get;
    }

    public decimal BaseAmount {
      get;
    }

    public decimal Total {
      get;
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
                               -1 * x.Subtotal : x.Subtotal);
      }
    }


    public decimal Discounts {
      get {
        return _bills.SelectFlat(x => x.Concepts)
                     .ToFixedList().Sum(x => x.Bill.BillType.IsCreditNote ? -1 * x.Discount : x.Discount);
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

    #endregion Properties

    #region Methods

    private FixedList<BillTaxItemTotal> BuildTaxItems() {
      FixedList<BillTaxEntry> taxEntries = _bills.SelectFlat(x => x.Concepts)
                                                 .SelectFlat(x => x.TaxEntries);

      var taxTypeGroups = taxEntries.GroupBy(x => new { x.TaxType, x.TaxMethod });

      var taxItems = new List<BillTaxItemTotal>(taxTypeGroups.Count());

      foreach (var taxTypeGroup in taxTypeGroups) {
        var taxTotal = new BillTaxItemTotal(taxTypeGroup.Key.TaxType,
                                            taxTypeGroup.Key.TaxMethod,
                                            taxTypeGroup.ToFixedList());
        taxItems.Add(taxTotal);
      }

      return taxItems.ToFixedList();
    }

    #endregion Methods

  }  // class BillsTotals

}  // namespace Empiria.Billing
