/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillDto                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return bill data.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Billing.Adapters {

  /// <summary>Output DTO used to return bill data.</summary>
  public class BillHolderDto {

    public BillDto Bill {
      get; internal set;
    }

    public FixedList<BillConceptDto> Concepts {
      get; internal set;
    }

    public FixedList<BillRelatedBillDto> BillRelatedBills {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }


  } // class BillHolderDto


  /// <summary>Output DTO used to return bill entry data.</summary>
  public class BillDto {

    public string UID {
      get; internal set;
    }


    public string BillNo {
      get; internal set;
    }


    public NamedEntityDto Category {
      get; internal set;
    }


    public NamedEntityDto BillType {
      get; internal set;
    }


    public NamedEntityDto ManagedBy {
      get; internal set;
    }


    public DateTime IssueDate {
      get; internal set;
    }


    public NamedEntityDto IssuedBy {
      get; internal set;
    }


    public NamedEntityDto IssuedTo {
      get; internal set;
    }


    public string CurrencyCode {
      get; internal set;
    }


    public decimal Subtotal {
      get; internal set;
    }


    public decimal Discount {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }


    public NamedEntityDto Status {
      get; internal set;
    }

  } // Class BillEntryDto


  /// <summary>Output DTO used to return bill concept entry data.</summary>
  public class BillWithConceptsDto {

    public BillDto Bill {
      get; internal set;
    }


    public FixedList<BillConceptDto> Concepts {
      get; internal set;
    } = new FixedList<BillConceptDto>();


    public FixedList<BillRelatedBillDto> BillRelatedBills {
      get; internal set;
    }

  } // class BillWithConceptsDto


  /// <summary>Output DTO used to return bill entry data.</summary>
  public class BillConceptDto {


    public string UID {
      get; set;
    }


    //public string BillUID {
    //  get; set;
    //}


    public NamedEntityDto Product {
      get; internal set;
    }
    

    public string Description {
      get; internal set;
    }


    public decimal Quantity {
      get; internal set;
    }


    public decimal UnitPrice {
      get; set;
    }


    public decimal Subtotal {
      get; internal set;
    }


    public decimal Discount {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }


    public FixedList<BillTaxEntryDto> TaxEntries {
      get; internal set;
    } = new FixedList<BillTaxEntryDto>();



  } // class BillConceptDto


  /// <summary>Output DTO used to return bill related bill entry data.</summary>
  public class BillRelatedBillDto {

    public string UID {
      get; internal set;
    }


    public string RelatedDocument {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }


    public FixedList<BillTaxEntryDto> TaxEntries {
      get; internal set;
    } = new FixedList<BillTaxEntryDto>();
  }


  /// <summary>Output DTO used to return bill tax entry data.</summary>
  public class BillTaxEntryDto {


    public string UID {
      get; set;
    }


    public NamedEntityDto TaxMethod {
      get; internal set;
    }


    public NamedEntityDto TaxFactorType {
      get; internal set;
    }


    public decimal Factor {
      get; internal set;
    }


    public decimal BaseAmount {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public DateTime PostingTime {
      get; internal set;
    }


    public NamedEntityDto Status {
      get; internal set;
    }

  } // class BillTaxEntryDto


} // namespace Empiria.Billing.Adapters
