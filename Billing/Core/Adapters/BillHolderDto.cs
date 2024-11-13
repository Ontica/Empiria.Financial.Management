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

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }


  } // class BillDto


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
    } = new NamedEntityDto("", "");


    public NamedEntityDto BillType {
      get; internal set;
    } = new NamedEntityDto("", "");


    public NamedEntityDto ManagedBy {
      get; set;
    } = new NamedEntityDto("", "");


    public DateTime IssueDate {
      get; internal set;
    }


    public NamedEntityDto IssuedBy {
      get; internal set;
    } = new NamedEntityDto("","");


    public NamedEntityDto IssuedTo {
      get; internal set;
    } = new NamedEntityDto("", "");


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
    } = new NamedEntityDto("", "");


    public DateTime PostingTime {
      get; internal set;
    }


    public NamedEntityDto Status {
      get; internal set;
    } = new NamedEntityDto("", "");

  } // Class BillEntryDto


  public class BillWithConceptsDto {

    public BillDto Bill {
      get; internal set;
    }


    public FixedList<BillConceptDto> Concepts {
      get; internal set;
    } = new FixedList<BillConceptDto>();

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
    } = new NamedEntityDto("", "");
    

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


    public FixedList<BillTaxEntryDto> TaxEntries {
      get; set;
    } = new FixedList<BillTaxEntryDto>();



  } // class BillConceptDto


  /// <summary>Output DTO used to return bill tax entry data.</summary>
  public class BillTaxEntryDto {


    public string UID {
      get; set;
    }


    //public string BillUID {
    //  get; set;
    //}


    //public string BillConceptUID {
    //  get; set;
    //}


    public NamedEntityDto TaxMethod {
      get; set;
    } = new NamedEntityDto("", "");


    public NamedEntityDto TaxFactorType {
      get; set;
    } = new NamedEntityDto("", "");


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


    public NamedEntityDto Status {
      get; internal set;
    } = new NamedEntityDto("", "");

  } // class BillTaxEntryDto


} // namespace Empiria.Billing.Adapters
