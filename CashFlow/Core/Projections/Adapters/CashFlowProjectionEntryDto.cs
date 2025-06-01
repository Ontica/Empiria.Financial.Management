/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Output DTOs                             *
*  Type     : CashFlowProjectionEntryDto                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return cash flow projection entries.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Output DTO used to return cash flow projection entries.</summary>
  public class CashFlowProjectionEntryDto {

    public string UID {
      get; internal set;
    }

    public CashFlowProjectionEntryDtoType EntryType {
      get;
    } = CashFlowProjectionEntryDtoType.Monthly;


    public string ProjectionUID {
      get; internal set;
    }

    public NamedEntityDto ProjectionColumn {
      get; internal set;
    }

    public NamedEntityDto CashFlowAccount {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public NamedEntityDto ProductUnit {
      get; internal set;
    }

    public decimal ProductQty {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public NamedEntityDto Month {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public decimal OriginalAmount {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

    public decimal ExchangeRate {
      get; internal set;
    } = 1m;

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // CashFlowProjectionEntryDto



  /// <summary>Output DTO used to return cash flow projection's entries for lists.</summary>
  public class CashFlowProjectionEntryDescriptor {

    public string UID {
      get; internal set;
    }

    public string CashFlowAccountCode {
      get; internal set;
    }

    public string CashFlowAccountName {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public string MonthName {
      get; internal set;
    }

    public string ProjectionColumn {
      get; internal set;
    }

    public decimal InflowAmount {
      get; internal set;
    }

    public decimal OutflowAmount {
      get; internal set;
    }

    public string ItemType {
      get; internal set;
    }

  }  // CashFlowProjectionEntryDescriptor

}  // namespace Empiria.CashFlow.Projections.Adapters
