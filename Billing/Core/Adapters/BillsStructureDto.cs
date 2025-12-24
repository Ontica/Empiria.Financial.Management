/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Output DTO                              *
*  Type     : BillsStructureDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with a list of bills with the sums of subtotal and total, and a taxes list.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

namespace Empiria.Billing.Adapters {

  ///<summary>Output DTO with a list of bills with the sums of subtotal and total, and a taxes list.</summary>
  public class BillsStructureDto {

    public FixedList<BillDto> Bills {
      get; internal set;
    }

    public decimal Subtotal {
      get; internal set;
    }

    public FixedList<BillsStructureTaxEntryDto> Taxes {
      get; internal set;
    }

    public decimal Total {
      get {
        return Subtotal + Taxes.Sum(x => x.Total);
      }
    }

  }  // class BillsStructureDto



  /// <summary>Output DTO with a tax type grouped sum for all the bills in a BillsStructureDto.</summary>
  public class BillsStructureTaxEntryDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto TaxType {
      get; internal set;
    }

    public decimal BaseAmount {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

  }  // class BillsStructureTaxEntryDto

} // namespace Empiria.Billing.Adapters
