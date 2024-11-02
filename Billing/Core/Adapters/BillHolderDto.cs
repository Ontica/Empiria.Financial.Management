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
using Empiria.Documents;
using Empiria.History.Services.Adapters;
using Empiria.StateEnums;
using Empiria.Storage;

namespace Empiria.Billing.Adapters {


  /// <summary>Output DTO used to return bill data.</summary>
  public class BillHolderDto {

    public BillDto Bill {
      get; set;
    } = new BillDto();


    public FixedList<BillConceptDto> Concepts {
      get; set;
    } = new FixedList<BillConceptDto>();


    public FixedList<Document> Documents {
      get; internal set;
    } = new FixedList<Document>();
    

    public FixedList<HistoryDto> History {
      get; set;
    } = new FixedList<HistoryDto>();


  } // class BillDto


  /// <summary>Output DTO used to return bill entry data.</summary>
  public class BillDto {

    public string UID {
      get; set;
    }


    public string BillNo {
      get; set;
    }


    public NamedEntityDto BillType {
      get; set;
    } = new NamedEntityDto("", "");


    public DateTime IssueDate {
      get; set;
    }


    public NamedEntityDto IssuedBy {
      get; set;
    } = new NamedEntityDto("","");


    public NamedEntityDto IssuedTo {
      get; set;
    } = new NamedEntityDto("", "");


    public NamedEntityDto ManagedBy {
      get; set;
    } = new NamedEntityDto("", "");


    public string CurrencyCode {
      get; set;
    }


    public decimal Subtotal {
      get; set;
    }


    public decimal Discount {
      get; set;
    }


    public decimal Total {
      get; set;
    }


    public NamedEntityDto PostedBy {
      get; set;
    } = new NamedEntityDto("", "");


    public DateTime PostingTime {
      get; set;
    }


    public NamedEntityDto Status {
      get; set;
    } = new NamedEntityDto("", "");


    public FixedList<BillConceptDto> Concepts {
      get; set;
    } = new FixedList<BillConceptDto>();
    
  } // Class BillEntryDto


  /// <summary>Output DTO used to return bill entry data.</summary>
  public class BillConceptDto {


    public string UID {
      get; set;
    }


    public string BillUID {
      get; set;
    }


    public string ProductUID {
      get; set;
    }


    public string Description {
      get; set;
    }


    public decimal Quantity {
      get; set;
    }


    public decimal UnitPrice {
      get; set;
    }


    public decimal Subtotal {
      get; set;
    }


    public decimal Discount {
      get; set;
    }


    public NamedEntityDto PostedBy {
      get; set;
    } = new NamedEntityDto("", "");


    public DateTime PostingTime {
      get; set;
    }


    public FixedList<BillTaxEntryDto> TaxEntriesDto {
      get; set;
    } = new FixedList<BillTaxEntryDto>();



  } // class BillConceptDto


  /// <summary>Output DTO used to return bill tax entry data.</summary>
  public class BillTaxEntryDto {


    public string UID {
      get; set;
    }


    public string BillUID {
      get; set;
    }


    public string BillConceptUID {
      get; set;
    }


    public BillTaxMethod TaxMethod {
      get; set;
    }


    public BillTaxFactorType TaxFactorType {
      get; set;
    }


    public decimal Factor {
      get; set;
    }


    public decimal BaseAmount {
      get; set;
    }


    public decimal Total {
      get; set;
    }


    public NamedEntityDto PostedBy {
      get; set;
    } = new NamedEntityDto("", "");


    public DateTime PostingTime {
      get; set;
    }


    public EntityStatus Status {
      get;
      internal set;
    }

  } // class BillTaxEntryDto


} // namespace Empiria.Billing.Adapters
